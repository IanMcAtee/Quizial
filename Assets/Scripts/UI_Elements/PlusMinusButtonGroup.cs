using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Class to provide methods for controlling Plus/Minus Button Groups. <br/>
/// Plus and Minus buttons control a central value. <br/>
/// Can use velocity to allow increment/decrement on button hold
/// </summary>
public class PlusMinusButtonGroup : LeftRightButtonGroup
{
    [SerializeField]
    private int
        _initialValue = 10,
        _minValue = 1,
        _maxValue = 50;
    [SerializeField]
    private TMP_Text
        _displayText;
    [SerializeField] 
    private bool _useVelocity = true;
    [SerializeField]
    private float _delayDecayRate = 0.1f;
    [SerializeField]
    private AnimationCurve _holdDelayCurve;

    public UnityEvent<int> OnValueChanged;
    public int Value { get; private set; } = 0;

    private bool _isButtonPressed = false;  

    protected override void Awake()
    {
        // Call parent's Awake method to assign button listeners
        base.Awake();
        SetValue(_initialValue);
    }

    /// <summary>
    /// Public OnClick method to decrement value and invoke OnValueChanged event <br/>
    /// Only works if not using velocity <br/>
    /// Also fires event defined in parent LeftRightButtonGroup.Left_OnClick()
    /// </summary>
    public override void Left_OnClick()
    {
        if (!_useVelocity)
        {
            SetValue(Value - 1);
            base.Left_OnClick();
        }  
    }

    /// <summary>
    /// Public OnClick method to increment value and invoke OnValueChanged event <br/>
    /// Only works if not using velocity <br/>
    /// Also fires event defined in parent LeftRightButtonGroup.Right_OnClick()
    /// </summary>
    public override void Right_OnClick()
    {
        if (!_useVelocity)
        {
            SetValue(Value + 1);
            base.Right_OnClick();
        }       
    }

    /// <summary>
    /// Method for setting the button value and invoking the OnValueChanged event
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value)
    {
        if (value >= _minValue && value <= _maxValue)
        {
            Value = value;
        }   
        _displayText.text = Value.ToString();
        OnValueChanged?.Invoke(Value);
    }


    /// <summary>
    /// Public method to start the velocity button press coroutine <br/>
    /// Called from the OnPointerDown event trigger on each button
    /// </summary>
    /// <param name="step"></param>
    public void OnButtonDown(int step)
    {
        if (_useVelocity)
        {
            _isButtonPressed = true;
            StartCoroutine(OnButtonHoldRoutine(step));
        }
    }

    /// <summary>
    /// Public method to end the velocity button press coroutine <br/>
    /// Called from the OnPointerUp event trigger on each button
    /// </summary>
    /// <param name="step"></param>
    public void OnButtonUp()
    {
        if (_useVelocity)
        {
            _isButtonPressed = false;
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// Coroutine to handle button increment/decrement with velocity. <br/>
    /// Uses _delayDecayRate and _holdDelayCurve to smoothly increment/decrement button along the curve while holding down button
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    private IEnumerator OnButtonHoldRoutine(int step)
    {
        SetValue(Value + step);
        float delayPoint = 0;
        float delay = _holdDelayCurve.Evaluate(delayPoint);
        yield return new WaitForSeconds(delay);    

        while (_isButtonPressed)
        {
            SetValue(Value+step);
            delayPoint += _delayDecayRate;
            delay = _holdDelayCurve.Evaluate(delayPoint);
            yield return new WaitForSeconds(delay);
        }
    }
}
