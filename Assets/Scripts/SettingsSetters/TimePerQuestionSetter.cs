using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimePerQuestionSetter : MonoBehaviour
{
    [SerializeField]
    private Slider _timePerQuestionSlider;

    [SerializeField]
    private TMP_Text _sliderText;

    [SerializeField]
    private float _infinitySymbolFontSize;

    [SerializeField]
    private float _maxSliderValue = 61;

    private float _originalFontSize;



    private void Awake()
    {
        _timePerQuestionSlider.onValueChanged.AddListener(SetTimeSettingAndText);
        _originalFontSize = _sliderText.fontSize;
        _sliderText.text = _timePerQuestionSlider.value.ToString();
    }
    private void OnEnable()
    {
        GameManager.Instance.Settings.TimePerQuestion = _timePerQuestionSlider.value;   
        print(GameManager.Instance.Settings.TimePerQuestion.ToString());
    }

   
    private void SetTimeSettingAndText(float value)
    {
        
        if (value == _maxSliderValue)
        {
            _sliderText.fontStyle = FontStyles.Normal;
            _sliderText.fontSize = _infinitySymbolFontSize;
            _sliderText.alignment = TextAlignmentOptions.Bottom;
            _sliderText.text = "\u221E";
            GameManager.Instance.Settings.TimePerQuestion = -1f;
            print(GameManager.Instance.Settings.TimePerQuestion.ToString());
            return;

        }
        
        _sliderText.fontStyle = FontStyles.Bold;
        _sliderText.alignment = TextAlignmentOptions.Center;
        _sliderText.fontSize = _originalFontSize;
        _sliderText.text = value.ToString();
        GameManager.Instance.Settings.TimePerQuestion = value;
        print(GameManager.Instance.Settings.TimePerQuestion.ToString());



    }
}
