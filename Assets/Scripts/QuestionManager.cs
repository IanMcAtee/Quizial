using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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
        //StartQuestions();
    }

    public void StartQuestions()
    {
        //QuestionSet = OpenTdbAPIHelper.GetQuestionSet("https://opentdb.com/api.php?amount=10");
        string QuestionSetURL = OpenTdbAPIHelper.FormURL(SettingsManager.Instance.Settings);
        QuestionSet = OpenTdbAPIHelper.GetQuestionSet(QuestionSetURL);
        _curQuestionIndex = 0;
        DisplayQuestion(QuestionSet[0]);
        
    }

    public void NextQuestion()
    { 
        _curQuestionIndex++;
        if (_curQuestionIndex < QuestionSet.Length)
        {
            ClearAnswers();
            _hasAnswered = false;
            DisplayQuestion(QuestionSet[_curQuestionIndex]);
        }
        else
        {
            //Round over
        }
    }

    private void DisplayQuestion(TriviaQuestion question)
    {
        print($"Correct Answer: {question.CorrectAnswer}");
        print("Incorrect Answers: ");
        foreach (string answer in question.IncorrectAnswers)
        {
            print(answer);
        }

        // Set question, category, difficulty, and number text
        _questionText.text = question.Question;
        _categoryDifficultyText.text = $"{question.Category} \u2022 {question.Difficulty}";
        _questionNumberText.text = $"{question.Number.ToString()} / {QuestionSet.Length.ToString()}";

        // Display answers based on if they're true/false or multiple choice types
        if (question.IncorrectAnswers.Length == 1)
        {
            DisplayTrueFalseAnswers(question);
        }
        else
        {
            DisplayMultipleAnswers(question);
        }

        StartCoroutine(QuestionCountdownRoutine());

        


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




}
