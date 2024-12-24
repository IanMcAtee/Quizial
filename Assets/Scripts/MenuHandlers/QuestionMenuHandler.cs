using UnityEngine;

/// <summary>
/// Class to handle the behaviour on the Question UI
/// </summary>
public class QuestionMenuHandler : MenuElement
{
    /// <summary>
    /// On click method to pause the game
    /// </summary>
    public void PauseButton_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }
}
