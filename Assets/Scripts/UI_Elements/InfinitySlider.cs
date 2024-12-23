using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Inherited slider class to set a TMP text to value of slider, and to infinity if slider is at max
/// </summary>
public class InfinitySlider : Slider
{
    [SerializeField]
    private TMP_Text _sliderText;

    [SerializeField]
    private float _infinitySymbolFontSize;

    private float _originalFontSize;

    protected override void Awake()
    {
        base.Awake();
        base.onValueChanged.AddListener(SetSliderText);
        _originalFontSize = _sliderText.fontSize;   
        SetSliderText(base.value);
    }

    /// <summary>
    /// Private method to set the text component of the slider to value or infinity if slider is at max. <br/>
    /// Note, some adjustments to font size, weight, and location are made to infinity symbol to make it <br/>
    /// look normal with Montserrat font. May need to remove these or adjust further if using different font.
    /// </summary>
    /// <param name="newValue"></param>
    private void SetSliderText(float newValue)
    {
        // If value is max, use infinity symbol
        if (newValue == base.maxValue)
        {
            // Set TMP parameters to avoid infinity symbol looking different from default text
            _sliderText.fontStyle = FontStyles.Normal;
            _sliderText.fontSize = _infinitySymbolFontSize;
            _sliderText.alignment = TextAlignmentOptions.Bottom;
            // Set infinity symbol with unicode
            _sliderText.text = "\u221E";
            return;
        }
        
        // Set the slider text to value, adjust TMP parameters back to normal
        _sliderText.fontStyle = FontStyles.Bold;
        _sliderText.alignment = TextAlignmentOptions.Center;
        _sliderText.fontSize = _originalFontSize;
        _sliderText.text = newValue.ToString();
    }
}
