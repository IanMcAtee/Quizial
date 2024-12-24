using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

    private void SetScoreText()
    {
        _finalScoreText.text = $"{GameManager.Instance.Score}/{GameManager.Instance.Settings.NumQuestions}";
    }

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
