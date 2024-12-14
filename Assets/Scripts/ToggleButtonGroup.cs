using UnityEngine;

public class ToggleButtonGroup : MonoBehaviour
{
    private CategoryButton[] _toggleButtons;
    private CategoryButton _activeToggleButton;

    private void OnEnable()
    {
        _toggleButtons = GetComponentsInChildren<CategoryButton>();
    }

    public void SetAsActiveToggleButton(CategoryButton toggleButton)
    {
        if (_activeToggleButton == null)
        {
            _activeToggleButton = toggleButton;
        }
        else if (_activeToggleButton == toggleButton)
        {
            return;
        }
        else
        {
            //_activeToggleButton.SetToggleState(false);
            _activeToggleButton = toggleButton;
        }
        
        
    }
}
