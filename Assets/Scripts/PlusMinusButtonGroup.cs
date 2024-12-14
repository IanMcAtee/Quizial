using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class PlusMinusButtonGroup : MonoBehaviour
{
    [SerializeField]
    private int
        _initialValue = 10,
        _minValue = 1,
        _maxValue = 50;

    [SerializeField]
    private Button
        _plusButton,
        _minusButton;

    [SerializeField]
    private TMP_Text
        _displayText;

    [SerializeField]
    private float _delayDecayRate = 0.1f;

    [SerializeField]
    private AnimationCurve _holdDelayCurve;

    [SerializeField]
    private UnityEvent _onValueChanged;

    public int Value { get; private set; }

    private bool _isButtonPressed;  

    private void Start()
    {
        SetValue(_initialValue);
    }

    public void IncrementValue()
    {
        
        if (Value < _maxValue)
        {
            SetValue(Value + 1);
        }
    }

    public void DecrementValue()
    {
        if (Value > _minValue)
        {
            SetValue(Value - 1);
        }  
    }

    public void SetValue(int value)
    {
        Value = value;
        _displayText.text = Value.ToString();
        _onValueChanged?.Invoke();
    }

    public void OnButtonDown(bool isPlusButton)
    { 
        _isButtonPressed = true;
        StartCoroutine(OnButtonHoldRoutine(isPlusButton));
    }

    public void OnButtonUp()
    {
        _isButtonPressed = false;
        StopAllCoroutines();
    }
    
    private IEnumerator OnButtonHoldRoutine(bool isPlusButton)
    {
        if (isPlusButton) { IncrementValue(); }
        else { DecrementValue(); }
        
        float delayPoint = 0;
        float delay = _holdDelayCurve.Evaluate(delayPoint);
        yield return new WaitForSeconds(delay);    

        while (_isButtonPressed)
        {
            if (isPlusButton) { IncrementValue(); }
            else { DecrementValue(); }
            delayPoint += _delayDecayRate;
            delay = _holdDelayCurve.Evaluate(delayPoint);
            yield return new WaitForSeconds(delay);
        }
    }


}
