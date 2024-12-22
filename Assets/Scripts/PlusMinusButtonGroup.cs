using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;

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

    [SerializeField]
    private UnityEvent _onValueChanged;

    public int Value { get; private set; } = 0;

    private bool _isButtonPressed = false;  

    private void Start()
    {
        SetValue(_initialValue);
    }

    public override void Left_OnClick()
    {
        if (!_useVelocity)
        {
            SetValue(Value - 1);
            base.Left_OnClick();
        }  
    }

    public override void Right_OnClick()
    {
        if (!_useVelocity)
        {
            SetValue(Value + 1);
            base.Right_OnClick();
        }       
    }

    public void SetValue(int value)
    {
        if (value >= _minValue && value <= _maxValue)
        {
            Value = value;
        }   
        _displayText.text = Value.ToString();
        _onValueChanged?.Invoke();
    }

    public void OnButtonDown(int step)
    {
        if (_useVelocity)
        {
            _isButtonPressed = true;
            StartCoroutine(OnButtonHoldRoutine(step));
        }
    }
    

    public void OnButtonUp()
    {
        if (_useVelocity)
        {
            _isButtonPressed = false;
            StopAllCoroutines();
        }
    }
    
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
