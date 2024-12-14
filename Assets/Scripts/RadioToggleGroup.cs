using UnityEngine;

public class RadioToggleGroup : MonoBehaviour
{
    // KNOWN ISSUE ON TOGGLING _allowDeselct at runtime
    [SerializeField]
    private bool _allowDeselect = true;

    private ToggleButton[] _toggleButtons;

    private ToggleButton _selectedToggleButton = null;

    private void Start()
    {
        _toggleButtons = GetComponentsInChildren<ToggleButton>();  
        foreach (ToggleButton toggleButton in _toggleButtons)
        {
            toggleButton.onClick.AddListener(RadioToggle);
            
        }
    }


    private void RadioToggle()
    {
        int selectedIndex = 0;

        for (int i = 0; i < _toggleButtons.Length; i++)
        {
            if (_toggleButtons[i].IsToggled && _toggleButtons[i] != _selectedToggleButton)
            {
                selectedIndex = i;
                print(selectedIndex);
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

        print(_selectedToggleButton.name);
    }
}
