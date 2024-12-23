using UnityEngine;
using TMPro;

/// <summary>
/// Handles the behavior of the category button upon instantiation
/// </summary>
[RequireComponent(typeof(ToggleButton))]
public class CategoryButton : MonoBehaviour
{

    [SerializeField]
    private ToggleButton _toggleButton;

    [SerializeField]
    private TMP_Text _buttonText;

    [SerializeField]
    private Color 
        _normalTextColor,
        _toggledTextColor;

    public TriviaCategory AssociatedCategory { get; private set; }

    private void Start()
    {
        // Add the category selection to the OnToggleSelect/Deselect events
        _toggleButton.OnToggleSelect.AddListener(OnSelectCategory);
        _toggleButton.OnToggleDeselect.AddListener(OnDeselectCategory);
    }

    /// <summary>
    /// Public method to initialize the button with associated category and text <br/>
    /// </summary>
    /// <param name="category"></param>
    public void InitializeCategoryButton(TriviaCategory category)
    {
        AssociatedCategory = category;  
        _buttonText.text = category.Name;
    }

    /// <summary>
    /// Private method to set the category and adjust button color/text on select
    /// </summary>
    private void OnSelectCategory()
    {
        GameManager.Instance.Settings.Category = AssociatedCategory;
        _toggleButton.image.fillCenter = true;
        _buttonText.color = _toggledTextColor;
    }

    /// <summary>
    /// Private method adjust button/text color on button deselect
    /// </summary>
    private void OnDeselectCategory()
    {
        _toggleButton.image.fillCenter = false;
        _buttonText.color = _normalTextColor;
    }
}
