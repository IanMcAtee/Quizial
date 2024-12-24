using UnityEngine;

/// <summary>
/// Class to handle the behaviour of the Paused UI
/// </summary>
public class PausedMenuHandler : MenuElement
{
    /// <summary>
    /// On click method to resume game
    /// </summary>
    public void Resume_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    /// <summary>
    /// On click method to go to main menu
    /// </summary>
    public void MainMenu_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }
}
