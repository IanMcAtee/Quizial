using UnityEngine;
using TMPro;
/// <summary>
/// Class to handle behaviour on the Options UI
/// </summary>
public class OptionsMenuHandler : MonoBehaviour
{
    [SerializeField]
    private PlusMinusButtonGroup _numQuestionsButtonGroup;
    [SerializeField]
    private TMP_Text _categoryText;
    [SerializeField]
    private LeftRightShiftButtonGroup 
        _difficultyButtonGroup,
        _questionTypeButtonGroup;
    [SerializeField]
    private InfinitySlider _timePerQuestionSlider;
    [SerializeField]
    private GameObject
        _mainMenuUI,
        _categoriesUI;

    private void Awake()
    {
   
        
    }

    private void OnDestroy()
    {
        
    }

    private void OnEnable()
    {
        _categoryText.text = GameManager.Instance.Settings.Category.Name;
    }

    public void SetNumberQuestions(int numQuestions)
    {
        GameManager.Instance.Settings.NumQuestions = numQuestions;
        print(GameManager.Instance.Settings.NumQuestions);
    }

    public void SetDifficulty(int difficulty)
    {
        GameManager.Instance.Settings.Difficulty = (TriviaDifficulty)difficulty;
        print(GameManager.Instance.Settings.Difficulty);
    }

    public void SetQuestionType(int questionType)
    {
        GameManager.Instance.Settings.QuestionType = (TriviaQuestionType)questionType;
        print(GameManager.Instance.Settings.QuestionType);
    }

    public void SetTimePerQuestion(float timePerQuestion)
    {
        // Slider in infinity state, need to set time per question to -1
        if (timePerQuestion >= _timePerQuestionSlider.maxValue)
        {
            GameManager.Instance.Settings.TimePerQuestion = -1f;
            print(GameManager.Instance.Settings.TimePerQuestion);
            return;
        }
        // Otherwise, set to value of the slider
        GameManager.Instance.Settings.TimePerQuestion = timePerQuestion;
        print(GameManager.Instance.Settings.TimePerQuestion);
    }

    public void Back_OnClick()
    {
        gameObject.SetActive(false);
        _mainMenuUI.SetActive(true);
    }

    public void MoreCategories_OnClick()
    {
        gameObject.SetActive(false);
        _categoriesUI.SetActive(true);
    }
}
