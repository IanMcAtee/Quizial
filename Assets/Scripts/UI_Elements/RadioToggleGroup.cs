using UnityEngine;

/// <summary>
/// Allows children toggle buttons to act as radio switches (i.e. only one can be toggled at a time)
/// </summary>
public class RadioToggleGroup : MonoBehaviour
{
    // KNOWN ISSUE: On toggling _allowDeselect at runtime
    [SerializeField]
    private bool _allowDeselect = true;

    private ToggleButton[] _toggleButtons;
    private ToggleButton _selectedToggleButton = null;

    private void Start()
    {
        // Find all children toggle buttons, and add RadioToggle to their OnClick() event
        _toggleButtons = GetComponentsInChildren<ToggleButton>();  
        foreach (ToggleButton toggleButton in _toggleButtons)
        {
            toggleButton.onClick.AddListener(RadioToggle);
        }
    }

    /// <summary>
    /// Method to iterate through all toggle buttons and toggle only the one that was just clicked. <br/>
    /// All other toggle buttons are toggled off. 
    /// </summary>
    private void RadioToggle()
    {
        int selectedIndex = 0;

        for (int i = 0; i < _toggleButtons.Length; i++)
        {
            if (_toggleButtons[i].IsToggled && _toggleButtons[i] != _selectedToggleButton)
            {
                selectedIndex = i;
            }
            else if (_toggleButtons[i].IsToggled)
            {
                if (!_allowDeselect)
                {
                    _toggleButtons[i].interactable = true;
                }
                _toggleButtons[i].Toggle();
            } 
        }

        _selectedToggleButton = _toggleButtons[selectedIndex];

        if (!_allowDeselect)
        {
            _selectedToggleButton.interactable = false;
        }
    }
}
