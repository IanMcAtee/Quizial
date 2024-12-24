using UnityEngine;
using TMPro;

/// <summary>
/// Class to handle behaviour on the Options UI
/// </summary>
public class OptionsMenuHandler : MonoBehaviour 
{
    [Header("General Properties")]
    [SerializeField]
    private GameObject _mainMenuUI;
    [SerializeField]
    private GameObject _mainOptionsUI;
    [SerializeField]
    private GameObject _categoriesUI;

    [Header("Options Properties")]
    [SerializeField]
    private PlusMinusButtonGroup _numQuestionsButtonGroup;
    [SerializeField]
    private LeftRightShiftButtonGroup 
        _difficultyButtonGroup,
        _questionTypeButtonGroup;
    [SerializeField]
    private InfinitySlider _timePerQuestionSlider;
    

    [Header("Category Properties")]
    [SerializeField]
    private TMP_Text _categoryText;
    [SerializeField]
    private GameObject _categoryButtonPrefab;
    [SerializeField]
    private Transform _scrollViewContent;

    private void Awake()
    {
        // On awake, populate the category buttons 
        InstantiateCategoryButtons();
    }

    private void OnEnable()
    {
        // On enable, ensure the main options menu is active and the category name is updated
        _mainOptionsUI.SetActive(true);
        _categoriesUI.SetActive(false);
        _categoryText.text = GameManager.Instance.Settings.Category.Name;
    }

    /// <summary>
    /// Private method to iterate through available categories and instantiate a unique category button for each
    /// </summary>
    private void InstantiateCategoryButtons()
    {
        foreach (TriviaCategory category in GameManager.Instance.AvailableCategories)
        {
            GameObject buttonObject = Instantiate(_categoryButtonPrefab, _scrollViewContent);
            CategoryButton categoryButton = buttonObject.GetComponent<CategoryButton>();
            categoryButton.InitializeCategoryButton(category);
        }
    }

    /// <summary>
    /// Public method to set the number of questions setting. <br/>
    /// Called from PlusMinusButton.OnValueUpdate
    /// </summary>
    /// <param name="numQuestions"></param>
    public void SetNumberQuestions(int numQuestions)
    {
        GameManager.Instance.Settings.NumQuestions = numQuestions;
    }

    /// <summary>
    /// Public method to set the difficulty setting. <br/>
    /// Called from LeftRightShiftButtonGroup.OnCenterObjectActivate
    /// </summary>
    /// <param name="difficulty"></param>
    public void SetDifficulty(int difficulty)
    {
        GameManager.Instance.Settings.Difficulty = (TriviaDifficulty)difficulty;
    }

    /// <summary>
    /// Public method to set the question type setting. <br/>
    /// Called from LeftRightShiftButtonGroup.OnCenterObjectActivate
    /// </summary>
    /// <param name="questionType"></param>
    public void SetQuestionType(int questionType)
    {
        GameManager.Instance.Settings.QuestionType = (TriviaQuestionType)questionType;
    }

    /// <summary>
    /// Public method to set the time per question setting <br/>
    /// Called from InfinitySlider.OnValueUpdate
    /// </summary>
    /// <param name="timePerQuestion"></param>
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

    /// <summary>
    /// Public on click method to go back to main menu from main options menu. <br/>
    /// Called from Back button on main options menu
    /// </summary>
    public void BackToMainMenu_OnClick()
    {
        gameObject.SetActive(false);
        _mainMenuUI.SetActive(true);
    }

    /// <summary>
    /// Public on click method to go back to main options menu from category menu. <br/>
    /// Called from Back button on category menu
    /// </summary>
    public void BackToMainOptions_OnClick()
    {
        // Toggle the active menus, and set the category text to new category
        _categoriesUI.SetActive(false); 
        _mainOptionsUI.SetActive(true);
        _categoryText.text = GameManager.Instance.Settings.Category.Name;
    }

    /// <summary>
    /// Public on click method to go to categories menu from main options menu. <br/>
    /// Called from More Categories Button
    /// </summary>
    public void MoreCategories_OnClick()
    {
        // Toggle the active menus
        _mainOptionsUI.SetActive(false);
        _categoriesUI.SetActive(true);
    }
}
