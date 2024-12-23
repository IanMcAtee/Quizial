using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class to provide methods for controlling Left/Right Shift Button Groups <br/>
/// Utilized on question and difficulty option buttons
/// </summary>
public class LeftRightShiftButtonGroup : LeftRightButtonGroup
{
    [SerializeField, Tooltip("The starting index for the center objects")]
    private int _startingObjectIndex = 0;
    [SerializeField, Tooltip("Array of center objects")]
    private GameObject[] _centerObjects;

    [Tooltip("Array of events that will be fired on center object activation." +
        "Note, event index must match that of corresponding center object")]
    public UnityEvent[] OnCenterObjectActivate;

    public int CurrentObjectIndex { get; private set; }

    protected override void Awake()
    {
        // Check if number of center objects and number of events match
        if (OnCenterObjectActivate.Length != 0 && OnCenterObjectActivate.Length != _centerObjects.Length)
        {
            Debug.LogError("Number of events must be zero or match number of center objects");
        }

        // Call the base awake method to assign button's AddListener events
        base.Awake();
    }

    private void Start()
    {
        // Set the desired intial center object active
        CurrentObjectIndex = _startingObjectIndex;
        SetCenterObjectActive(CurrentObjectIndex);
    }

    /// <summary>
    /// Public method to shift the button group left and invoke corresponding event. <br/>
    /// Also fires event defined in parent LeftRightButtonGroup.Left_OnClick()
    /// </summary>
    public override void Left_OnClick()
    {
        ShiftCenterObject(-1);
        base.Left_OnClick();
    }

    /// <summary>
    /// Public method to shift the button group right and invoke corresponding event. <br/>
    /// Also fires event defined in parent LeftRightButtonGroup.Right_OnClick()
    /// </summary>
    public override void Right_OnClick()
    {
        ShiftCenterObject(1);
        base.Right_OnClick();
    }


    /// <summary>
    /// Private method to shift center object by shiftAmount and invoke associated event
    /// </summary>
    /// <param name="shiftAmount"></param>
    private void ShiftCenterObject(int shiftAmount)
    {
        // Only shift if center index will still be inbounds after shift
        if (CurrentObjectIndex + shiftAmount >= 0 && 
            CurrentObjectIndex + shiftAmount < _centerObjects.Length)
        {
            // Shift index, set new center object active, and invoke event
            CurrentObjectIndex += shiftAmount;
            SetCenterObjectActive(CurrentObjectIndex);
            // Only invoke associated event if there are events defined in inspector
            if (OnCenterObjectActivate.Length > 0)
            {
                OnCenterObjectActivate[CurrentObjectIndex]?.Invoke();
            }
        }
    }

    /// <summary>
    /// Private method set a single center object active by index
    /// </summary>
    /// <param name="index"></param>
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
