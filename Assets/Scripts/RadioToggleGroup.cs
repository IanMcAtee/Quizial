using UnityEngine;

public class RadioToggleGroup : MonoBehaviour
{
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
                _toggleButtons[i].Toggle();
            }
            
        }

        _selectedToggleButton = _toggleButtons[selectedIndex];
        print(_selectedToggleButton.name);
    }
}
