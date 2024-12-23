using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Base class for button groups with a left and right button. <br/>
/// Parent to PlusMinusButtonGroup.cs and LeftRightShiftButtonGroup.
/// </summary>
public class LeftRightButtonGroup : MonoBehaviour
{
    [SerializeField]
    protected Button
        _leftButton,
        _rightButton;

    public UnityEvent
        OnLeftButtonClick = null,
        OnRightButtonClick = null;


    protected virtual void Awake()
    {
        // On awake, add the Left/Right events to the button's onClick events
        _leftButton.onClick.AddListener(Left_OnClick);
        _rightButton.onClick.AddListener(Right_OnClick);
    }

    /// <summary>
    /// Overridable method for left click behaviour
    /// </summary>
    public virtual void Left_OnClick()
    {
        OnLeftButtonClick?.Invoke();
    }

    /// <summary>
    /// Overridable method for right click behaviour
    /// </summary>
    public virtual void Right_OnClick()
    {
        OnRightButtonClick?.Invoke();
    }
}
