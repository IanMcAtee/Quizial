using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Singleton to manage the behaviour of the round of trivia
/// </summary>
public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    private float _nextQuestionDelay = 3f;

    [Header("Question Properties")]
    [SerializeField]
    private TMP_Text _questionText;
    [SerializeField]
    private Image _questionBorderImage;

    [Header("Answer Properties")]
    [SerializeField]
    private GameObject _answerButtonPrefab;
    [SerializeField]
    private Transform _answerGroup;

    [Header("Animation Properties")]
    [SerializeField]
    private QuestionAnimator _questionAnimator;
    

    [Header("Difficulty/Category/Question Number Properties")]
    [SerializeField]
    private TMP_Text _categoryDifficultyText;
    [SerializeField]
    private TMP_Text _questionNumberText;

    public static QuestionManager Instance { get; private set; }
    public TriviaQuestion[] QuestionSet { get; private set; }

    public OpenTdbAPIHelper.ResponseCode APIResponseCode
    {
        get { return _responseCode; }
    }

    private OpenTdbAPIHelper.ResponseCode _responseCode = OpenTdbAPIHelper.ResponseCode.None;
    private int _curQuestionIndex = 0;
    private int _correctAnswerIndex;
    private List<GameObject> _curAnswerButtonObjects = new List<GameObject>();
    private bool _hasAnswered = false;

    // Implement singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Starts a round of trivia questions based on current game settings
    /// </summary>
    public void StartQuestions()
    {
        // Reset the state of the question manager
        ResetQuestionManagerState();

        // Get a set of trivia questions from the API
        string QuestionSetURL = OpenTdbAPIHelper.FormURL(GameManager.Instance.Settings);
        QuestionSet = OpenTdbAPIHelper.GetQuestionSet(QuestionSetURL, out _responseCode);

        // If API call unsuccessful, go to error state
        if (_responseCode != OpenTdbAPIHelper.ResponseCode.Success)
        {
            GameManager.Instance.UpdateGameState(GameState.Error);
            //Debug.LogError($"API ERROR: {_responseCode}");
            return;
        }

        // Otherwise, display first question and start countdown
        _curQuestionIndex = 0;
        SetQuestionText(QuestionSet[0]);
        if (GameManager.Instance.Settings.TimePerQuestion > 0f)
        {
            StartCoroutine(QuestionCountdownRoutine());
        }
    }

    /// <summary>
    /// Resets the state of the question manager
    /// </summary>
    private void ResetQuestionManagerState()
    {
        _questionAnimator.ResetPosition();
        ClearAnswers();
        StopAllCoroutines();
        _hasAnswered = false;
        _questionBorderImage.fillAmount = 1f;
    }

    /// <summary>
    /// Sets the question text and displays the answers
    /// </summary>
    /// <param name="question"></param>
    private void SetQuestionText(TriviaQuestion question)
    {
        _categoryDifficultyText.text = $"{question.Category} \u2022 {question.Difficulty}";
        _questionNumberText.text = $"{_curQuestionIndex + 1} / {QuestionSet.Length}";
        _questionText.text = question.Question;

        if (question.IncorrectAnswers.Length == 1)
        {
            DisplayTrueFalseAnswers(question);
        }
        else
        {
            DisplayMultipleAnswers(question);
        }
    }

    /// <summary>
    /// Moves to the next trivia question
    /// </summary>
    private void NextQuestion()
    { 
        _curQuestionIndex++;
        if (_curQuestionIndex < QuestionSet.Length)
        {
            _hasAnswered = false;
            StartCoroutine(AnimateQuestionRoutine(QuestionSet[_curQuestionIndex]));
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

    /// <summary>
    /// Instantiates the true/false answer buttons and associates each with correct/incorrect answer
    /// </summary>
    /// <param name="question"></param>
    private void DisplayTrueFalseAnswers(TriviaQuestion question)
    {
        GameObject trueButtonObject = Instantiate(_answerButtonPrefab, _answerGroup);
        AnswerButton trueButton = trueButtonObject.GetComponent<AnswerButton>();
        trueButton.SetAnswerText("True");
        GameObject falseButtonObject = Instantiate(_answerButtonPrefab, _answerGroup);
        AnswerButton falseButton = falseButtonObject.GetComponent<AnswerButton>();
        falseButton.SetAnswerText("False"); 
        if (question.CorrectAnswer == "True")
        {
            trueButton.SetAsCorrect();
            _correctAnswerIndex = 0;
        }
        else
        {
            falseButton.SetAsCorrect();
            _correctAnswerIndex = 1;
        }
        _curAnswerButtonObjects.Add(trueButtonObject);
        _curAnswerButtonObjects.Add(falseButtonObject);
    }

    /// <summary>
    /// Instantiates the multiple choice answer buttons and associates each with correct/incorrect answer
    /// </summary>
    /// <param name="question"></param>
    private void DisplayMultipleAnswers(TriviaQuestion question)
    {
        int numAnswers = question.IncorrectAnswers.Length + 1;
        _correctAnswerIndex = Random.Range(0, numAnswers);
        print($"CORRECT ANSWER INDEX: {_correctAnswerIndex}");
        int curIncorrectIndex = 0;
        GameObject curButtonObject = null;
        AnswerButton curButton = null;

        for (int i = 0; i < numAnswers;  i++)
        {
            if (i == _correctAnswerIndex)
            {
                curButtonObject = Instantiate(_answerButtonPrefab, _answerGroup);
                curButton = curButtonObject.GetComponent<AnswerButton>();
                curButton.SetAnswerText(question.CorrectAnswer);
                curButton.SetAsCorrect();
                
            }
            else
            {
                curButtonObject = Instantiate(_answerButtonPrefab, _answerGroup);
                curButton = curButtonObject.GetComponent<AnswerButton>();
                curButton.SetAnswerText(question.IncorrectAnswers[curIncorrectIndex]);
                curIncorrectIndex++;
            }
            _curAnswerButtonObjects.Add(curButtonObject);
        }
    }

    /// <summary>
    /// Clears the answer buttons
    /// </summary>
    private void ClearAnswers()
    {
        foreach (GameObject answerObject in _curAnswerButtonObjects)
        {
            Destroy(answerObject);
        }
        _curAnswerButtonObjects.Clear();
    }


    /// <summary>
    /// Handles on answer button press event
    /// </summary>
    /// <param name="isCorrect"></param>
    public void OnAnswerButtonPress(bool isCorrect)
    {
        print("IN ON ANSWER BUTTON PRESS");
        _hasAnswered = true;
        DisableAnswerButtons();
        if (!isCorrect)
        {
            HighlightCorrectAnswer();
        }
        else
        {
            print("IN CORRECT CONDITIONAL");
            GameManager.Instance.UpdateScore(1);
        }
        StartCoroutine(NextQuestionDelayRoutine());
    }

    /// <summary>
    /// Disables answer buttons from further input
    /// </summary>
    private void DisableAnswerButtons()
    {
        foreach (GameObject buttonObject in _curAnswerButtonObjects)
        {
            Button buttonComponent = buttonObject.GetComponent<Button>();
            buttonComponent.enabled = false;
        }
    }

    /// <summary>
    /// Highlight correct answer
    /// </summary>
    private void HighlightCorrectAnswer()
    {
        AnswerButton correctAnswerButton = _curAnswerButtonObjects[_correctAnswerIndex].GetComponent<AnswerButton>();
        correctAnswerButton.ChangeBorderStyle(true);
    }
    

    /// <summary>
    /// Handles the time out event
    /// </summary>
    private void OnQuestionTimeOut()
    {
        // Disable Buttons and Highlight Correct Answer
        DisableAnswerButtons();
        HighlightCorrectAnswer();
        StartCoroutine(NextQuestionDelayRoutine());
    }


    /// <summary>
    /// Coroutine to handle the timer countdown
    /// </summary>
    /// <returns></returns>
    private IEnumerator QuestionCountdownRoutine()
    {
        float timeRemaining = GameManager.Instance.Settings.TimePerQuestion;

        while (timeRemaining > 0 && !_hasAnswered)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
            _questionBorderImage.fillAmount = timeRemaining / GameManager.Instance.Settings.TimePerQuestion;
        }
        if (!_hasAnswered)
        {
            OnQuestionTimeOut();
        }
        yield return null;
    }

    /// <summary>
    /// Coroutine to handle the delay before the next question
    /// </summary>
    /// <returns></returns>
    private IEnumerator NextQuestionDelayRoutine()
    {
        yield return new WaitForSeconds(_nextQuestionDelay);
        NextQuestion();
    }

    
    /// <summary>
    /// Corountine to handle the animation of questions in and out
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    private IEnumerator AnimateQuestionRoutine(TriviaQuestion question)
    {
        _questionAnimator.PlayExitAnimation();

        while (_questionAnimator.IsAnimating)
        {
            yield return null;
        }

        ClearAnswers();
        SetQuestionText(question);

        _questionAnimator.PlayEnterAnimation();

        while (_questionAnimator.IsAnimating)
        {
            yield return null;
        }

        if (GameManager.Instance.Settings.TimePerQuestion > 0f)
        {
            StartCoroutine(QuestionCountdownRoutine());
        }
    }
}
