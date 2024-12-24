using UnityEngine;

public class PausedMenuHandler : MenuElement
{
    public void Resume_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void MainMenu_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }
}
