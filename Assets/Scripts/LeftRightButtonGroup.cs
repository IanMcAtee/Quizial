using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LeftRightButtonGroup : MonoBehaviour
{
    [SerializeField]
    private Button 
        _leftButton,
        _rightButton;

    [SerializeField]
    private int _startingObjectIndex = 0;

    [SerializeField]
    private GameObject[] _centerObjects;

    [SerializeField]
    private UnityEvent _onValueChanged;

    public int CurrentObjectIndex { get; private set; }


    private void Start()
    {
        _leftButton.onClick.AddListener(ShiftLeft);
        _rightButton.onClick.AddListener(ShiftRight);
        CurrentObjectIndex = _startingObjectIndex;
        SetObjectActiveAndInvoke(CurrentObjectIndex);
    }    

    public void ShiftLeft()
    {
        if (CurrentObjectIndex > 0)
        {
            CurrentObjectIndex--;
            print(CurrentObjectIndex);
            SetObjectActiveAndInvoke(CurrentObjectIndex);
        }
    }

    public void ShiftRight()
    {
        if (CurrentObjectIndex < _centerObjects.Length - 1)
        {
            CurrentObjectIndex++;
            print(CurrentObjectIndex);
            SetObjectActiveAndInvoke(CurrentObjectIndex);
        }
    }

    private void SetObjectActiveAndInvoke(int index)
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
        _onValueChanged?.Invoke();
    }




}
