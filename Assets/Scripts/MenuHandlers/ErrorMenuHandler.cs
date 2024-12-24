using UnityEngine;
using TMPro;

/// <summary>
/// Class to handle the behaviour on the error menu
/// </summary>
public class ErrorMenuHandler : MenuElement
{
    [SerializeField]
    private TMP_Text 
        _errorDetailsText,
        _troubleshootingText;

    private void OnEnable()
    {
        OpenTdbAPIHelper.ResponseCode responseCode = QuestionManager.Instance.APIResponseCode;
        PrintResponseCode(responseCode);
        PrintTroubleShootingTip(responseCode);
    }

    public void MainMenu_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void Quit_OnClick()
    {
        GameManager.Instance.QuitApplication();
    }

    private void PrintResponseCode(OpenTdbAPIHelper.ResponseCode responseCode)
    {
        string responseCodeText = $"Response Code {(int)responseCode}: {responseCode}";
        _errorDetailsText.text = responseCodeText; 
    }

    private void PrintTroubleShootingTip(OpenTdbAPIHelper.ResponseCode responseCode)
    {
        string troubleshootText = "We're really sorry about this. Looks like something the developer " +
                    "messed up. We'll have a stern conversation with him. In the meantime, maybe try " +
                    "unplugging and plugging it back in?";

        switch (responseCode)
        {
            case OpenTdbAPIHelper.ResponseCode.Success:
                troubleshootText = "Hey, this isn't an error. How did we get here?";
                break;
            case OpenTdbAPIHelper.ResponseCode.NoResults:
                troubleshootText = "We're really sorry about this. Maybe try adjusting your " +
                    "question settings. It could be that the database doesn't contain " +
                    "enough questions that match your parameters";
                break;
            case OpenTdbAPIHelper.ResponseCode.InvalidParameter:
                break;
            case OpenTdbAPIHelper.ResponseCode.TokenNotFound:
                break;
            case OpenTdbAPIHelper.ResponseCode.TokenEmpty:
                break;
            case OpenTdbAPIHelper.ResponseCode.RateLimit:
                troubleshootText = "We're really sorry about this. Maybe try waiting a few moments " +
                    "before starting a new game";
                break;
        }

        _troubleshootingText.text = troubleshootText;
    }
}