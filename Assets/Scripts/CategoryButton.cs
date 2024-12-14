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
        SettingsManager.Instance.Settings.Category = AssociatedCategory;
        print(SettingsManager.Instance.Settings.Category.Name);
        _toggleButton.image.fillCenter = true;
        _buttonText.color = _toggledTextColor;
    }

    private void OnDeselectCategory()
    {
        SettingsManager.Instance.Settings.Category = SettingsManager.Instance.AvailableCategories[0];
        print(SettingsManager.Instance.Settings.Category.Name);
        _toggleButton.image.fillCenter = false;
        _buttonText.color = _normalTextColor;
        //CategoryButton anyCategoryButton = 
    }
}
