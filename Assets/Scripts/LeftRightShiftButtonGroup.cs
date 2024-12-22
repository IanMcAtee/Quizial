using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LeftRightShiftButtonGroup : LeftRightButtonGroup
{
    [SerializeField]
    private int _startingObjectIndex = 0;

    [SerializeField]
    private GameObject[] _centerObjects;

    [SerializeField]
    private UnityEvent _onCenterObjectChanged;

    public int CurrentObjectIndex { get; private set; }

    private void Start()
    {
        CurrentObjectIndex = _startingObjectIndex;
        SetCenterObjectActive(CurrentObjectIndex);
    }    

    public override void Left_OnClick()
    {
        if (CurrentObjectIndex > 0)
        {
            CurrentObjectIndex--;
            print(CurrentObjectIndex);
            SetCenterObjectActive(CurrentObjectIndex);
            _onCenterObjectChanged?.Invoke();
        }

        base.Left_OnClick();
    }

    public override void Right_OnClick()
    {
        if (CurrentObjectIndex < _centerObjects.Length - 1)
        {
            CurrentObjectIndex++;
            print(CurrentObjectIndex);
            SetCenterObjectActive(CurrentObjectIndex);
            _onCenterObjectChanged?.Invoke();
        }

        base.Right_OnClick();
    }

    private void SetCenterObjectActive(int index)
    {
        for (int i = 0;  i < _centerObjects.Length; i++)
        {
            if (i  == index)
            {
                _centerObjects[i].SetActive(true);
                continue;
            }
            _centerObjects[i].SetActive(false);
        }   
    }
}
