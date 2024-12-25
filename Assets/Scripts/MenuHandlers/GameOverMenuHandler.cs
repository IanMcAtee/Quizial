using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class to handle the behaviour on the Game Over UI
/// </summary>
public class GameOverMenuHandler : MenuElement
{
    [SerializeField]
    private TMP_Text _finalScoreText;

    [SerializeField]
    private Image _scoreBorderImage;

    [SerializeField]
    private AnimationCurve _scoreAnimationCurve;

    private void OnEnable()
    {
        SetScoreText();
        StartCoroutine(ScoreBorderFillRoutine());   
    }

    /// <summary>
    /// Sets the score text
    /// </summary>
    private void SetScoreText()
    {
        _finalScoreText.text = $"{GameManager.Instance.Score}/{GameManager.Instance.Settings.NumQuestions}";
    }

    /// <summary>
    /// On click method to return to the main menu
    /// </summary>
    public void MainMenu_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    /// <summary>
    /// On click method to restart the game 
    /// </summary>
    public void Restart_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    /// <summary>
    /// Coroutine to animate the score border image to score percentage
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScoreBorderFillRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _scoreAnimationCurve.keys[^1].time)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            float fillAmount = _scoreAnimationCurve.Evaluate(elapsedTime) *
                ((float)GameManager.Instance.Score / (float)GameManager.Instance.Settings.NumQuestions);
            print(fillAmount);
            _scoreBorderImage.fillAmount = fillAmount;
        }
    }
}
