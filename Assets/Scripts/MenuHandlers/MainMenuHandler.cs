using UnityEngine;
using TMPro;

/// <summary>
/// Class to handle behaviour on the main menu UI
/// </summary>
public class MainMenuHandler : MenuElement
{
    [SerializeField]
    private TMP_Text _settingsText;
    [SerializeField]
    private float _settingsTextLineHeight = 60f;
    [SerializeField]
    private GameObject _optionsUIObject;
    [SerializeField]
    private ParticleSystem _mainMenuParticleSystem;

    private void OnEnable()
    {
        // On each enable, set the current settings text
        FormatMainMenuSettingsText(GameManager.Instance.Settings);
        // Activate the particle system
        _mainMenuParticleSystem.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // Deactivate the particle system
        if (_mainMenuParticleSystem != null)
        {
            _mainMenuParticleSystem.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Method to start game from button press
    /// </summary>
    public void Start_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
    
    /// <summary>
    /// Method to activate the settings UI from button press
    /// </summary>
    public void Settings_OnClick()
    {
        gameObject.SetActive(false);
        _optionsUIObject.SetActive(true);
    }

    /// <summary>
    /// Method to format and display settings text
    /// </summary>
    /// <param name="settings"></param>
    private void FormatMainMenuSettingsText(GameSettings settings)
    {
        string text = $"<line-height={_settingsTextLineHeight}>"
            + $"<b>Number of Questions:</b> {settings.NumQuestions}\n"
            + $"<b>Time per Question:</b> ";
        if (settings.TimePerQuestion <= 0)
        {
            text += $"Unlimited\n";
        }
        else
        {
            text += $"{settings.TimePerQuestion} (s)\n";
        }
        text += $"<b>Category:</b> {settings.Category.Name}\n"
            + $"<b>Difficulty:</b> {settings.Difficulty}"; 

        _settingsText.text = text;  
    }
}
