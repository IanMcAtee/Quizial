using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class CategoryButton1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectedBackPlate;
    [SerializeField]
    private GameObject _deselectedBackPlate;
    [SerializeField]
    private TMP_Text _buttonText;
    [SerializeField]
    private Color _selectedTextColor;
    [SerializeField] 
    private Color _deselectedTextColor;
    [SerializeField]
    private bool _isSelected = false;
    public TriviaCategory AssociatedCategory { get; set; }

    private Button _button;
    private ToggleButtonGroup _toggleGroup;



    private void Start()
    {
        _button = GetComponent<Button>();
        _toggleGroup = GetComponentInParent<ToggleButtonGroup>();

        if (_toggleGroup != null && _isSelected)
        {
            //_toggleGroup.SetAsActiveToggleButton(this);
        }
 
        _button.onClick.AddListener(CategoryButton_OnClick);
        SetButtonAppearance(_isSelected);
      
    }

    public void CategoryButton_OnClick()
    {
       
        SetToggleState(!_isSelected);
    }

    public void SetToggleState(bool toggleState)
    {
        _isSelected = toggleState;

        if (_toggleGroup != null && _isSelected)
        {
            //_toggleGroup.SetAsActiveToggleButton(this);
        }

        SetButtonAppearance(toggleState);
    }

    public void SetButtonText(string text)
    {
        _buttonText.text = text;
    }

  

    private void SetButtonAppearance(bool toggleState)
    {
        _selectedBackPlate.SetActive(toggleState);
        _deselectedBackPlate.SetActive(!toggleState);
        if (toggleState)
        {
            _buttonText.color = _selectedTextColor;
        }
        else
        {
            _buttonText.color = _deselectedTextColor;
        }
    }


}
