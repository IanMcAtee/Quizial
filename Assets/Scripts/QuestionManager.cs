using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    private float _nextQuestionDelay = 3f;
    [SerializeField] 
    private float _timePerQuestion = 10f;

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
    private Animator _questionAnimator;
    

    [Header("Difficulty/Category/Question Number Properties")]
    [SerializeField]
    private TMP_Text _categoryDifficultyText;
    [SerializeField]
    private TMP_Text _questionNumberText;

    public static QuestionManager Instance { get; private set; }
    public TriviaQuestion[] QuestionSet { get; private set; }
    public int Score { get; private set; } = 0;

    private int _curQuestionIndex = 0;
    private int _correctAnswerIndex;
    private List<GameObject> _curAnswerButtonObjects = new List<GameObject>();
    private bool _hasAnswered = false;
    private float _totalAnimationTime = 0f;
    private WaitForSeconds _halfAnimationDelay = null;

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

    private void Start()
    {
        foreach (AnimationClip anim in _questionAnimator.runtimeAnimatorController.animationClips)
        {
            _totalAnimationTime += anim.length;
        }
        _halfAnimationDelay = new WaitForSeconds(_totalAnimationTime / 2f);
        print($"Total Animation Time: {_totalAnimationTime}");

    }

    
    public void StartQuestions()
    {
        // Reset the state of the question manager
        ResetQuestionManagerState();

        string QuestionSetURL = OpenTdbAPIHelper.FormURL(GameManager.Instance.Settings);
        OpenTdbAPIHelper.ResponseCode responseCode;
        QuestionSet = OpenTdbAPIHelper.GetQuestionSet(QuestionSetURL, out responseCode);
        if (responseCode != OpenTdbAPIHelper.ResponseCode.Success)
        {
            // Handle api error
            Debug.LogError($"API ERROR: {responseCode}");
            return;
        }

        _curQuestionIndex = 0;
        StartCoroutine(AnimateQuestionRoutine(QuestionSet[0]));
        
    }

    private void ResetQuestionManagerState()
    {
        ClearAnswers();
        StopAllCoroutines();
        _hasAnswered = false;
        _questionBorderImage.fillAmount = 1f;
    }



    public void NextQuestion()
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


    private void DisplayMultipleAnswers(TriviaQuestion question)
    {
        int numAnswers = question.IncorrectAnswers.Length + 1;
        _correctAnswerIndex = Random.Range(0, numAnswers);
        print($"Correct Answer Index: {_correctAnswerIndex}");
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

    private void ClearAnswers()
    {
        foreach (GameObject answerObject in _curAnswerButtonObjects)
        {
            Destroy(answerObject);
        }
        _curAnswerButtonObjects.Clear();
    }



    public void OnAnswerButtonPress(bool isCorrect)
    {
        _hasAnswered = true;
        DisableAnswerButtons();
        if (!isCorrect)
        {
            HighlightCorrectAnswer();
        }
        else
        {
            Score++;
        }
        StartCoroutine(NextQuestionDelayRoutine());
    }

    private void DisableAnswerButtons()
    {
        foreach (GameObject buttonObject in _curAnswerButtonObjects)
        {
            Button buttonComponent = buttonObject.GetComponent<Button>();
            buttonComponent.enabled = false;
        }
    }

    private void HighlightCorrectAnswer()
    {
        AnswerButton correctAnswerButton = _curAnswerButtonObjects[_correctAnswerIndex].GetComponent<AnswerButton>();
        correctAnswerButton.ChangeBorderStyle(true);
    }
    


    private void OnQuestionTimeOut()
    {
        // Disable Buttons and Highlight Correct Answer
        DisableAnswerButtons();
        HighlightCorrectAnswer();
        StartCoroutine(NextQuestionDelayRoutine());
    }


    private IEnumerator NextQuestionDelayRoutine()
    {
        yield return new WaitForSeconds(_nextQuestionDelay);
        NextQuestion();
    }

    private IEnumerator QuestionCountdownRoutine()
    {
        float timeRemaining = _timePerQuestion;

        while (timeRemaining > 0 && !_hasAnswered)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
            _questionBorderImage.fillAmount = timeRemaining/_timePerQuestion;
            
        }
        if (!_hasAnswered)
        {
            OnQuestionTimeOut();
        }
        yield return null;
    }

    private IEnumerator AnimateQuestionRoutine(TriviaQuestion question)
    {

        if (_curQuestionIndex == 0)
        {
            SetQuestionText(question);
            _questionAnimator.SetTrigger("Enter");
            print("ENTER TRIGGER");
            yield return _halfAnimationDelay;
        }
        else
        {
     
            _questionAnimator.SetTrigger("Exit");
       
            yield return _halfAnimationDelay;
            ClearAnswers();
            SetQuestionText(question);
            _questionAnimator.SetTrigger("Enter");
          
            yield return _halfAnimationDelay;
        }
        StartCoroutine(QuestionCountdownRoutine());
    }

    private void SetQuestionText(TriviaQuestion question)
    {
        _categoryDifficultyText.text = $"{question.Category} \u2022 {question.Difficulty}";
        _questionNumberText.text = $"{question.Number.ToString()} / {QuestionSet.Length.ToString()}";
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




}
