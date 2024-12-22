using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LeftRightButtonGroup : MonoBehaviour
{
    [SerializeField]
    protected Button
        _leftButton,
        _rightButton;

    [SerializeField]
    protected UnityEvent
        _onLeftButtonClick = null,
        _onRightButtonClick = null;


    private void Awake()
    {
        _leftButton.onClick.AddListener(Left_OnClick);
        _rightButton.onClick.AddListener(Right_OnClick);

    }

    public virtual void Left_OnClick()
    {
        _onLeftButtonClick?.Invoke();
    }

    public virtual void Right_OnClick()
    {
        _onRightButtonClick?.Invoke();
    }

}
