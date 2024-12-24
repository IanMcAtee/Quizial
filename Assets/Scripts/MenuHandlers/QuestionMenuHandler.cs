using UnityEngine;

public class QuestionMenuHandler : MenuElement
{
    public void PauseButton_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }
}
