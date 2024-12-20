using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        _toggleButton.OnToggleSelect.AddListener(OnSelectCategory);
        _toggleButton.OnToggleDeselect.AddListener(OnDeselectCategory);
    }
    public void InitializeCategoryButton(TriviaCategory category)
    {
        AssociatedCategory = category;  
        _buttonText.text = category.Name;

    }

    public void OnSelectCategory()
    {
        GameManager.Instance.Settings.Category = AssociatedCategory;
        print(GameManager.Instance.Settings.Category.Name);
        _toggleButton.image.fillCenter = true;
        _buttonText.color = _toggledTextColor;
    }

    private void OnDeselectCategory()
    {
        _toggleButton.image.fillCenter = false;
        _buttonText.color = _normalTextColor;
        
    }
}
