using TMPro;
using UnityEngine;

public class MainMenuSettingsTextSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _mainMenuSettingsText;
    [SerializeField]
    private int _settingsTextLineHeight = 60;

    private void OnEnable()
    {
        _mainMenuSettingsText.text = $"<line-height={_settingsTextLineHeight}>" +
                                     $"Number of Questions: {SettingsManager.Instance.Settings.NumQuestions}\n" +
                                     $"Category: {SettingsManager.Instance.Settings.Category.Name}\n" +
                                     $"Difficulty: {SettingsManager.Instance.Settings.Difficulty}";
    }
}
