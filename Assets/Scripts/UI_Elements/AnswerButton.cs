using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the behaviour of the answer button prefab
/// </summary>
public class AnswerButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _answerText;

    [SerializeField]
    private Image
        _normalBorder,
        _correctBorder,
        _incorrectBorder;

    [field: SerializeField]
    public bool IsCorrect { get; private set; }

    private void Awake()
    {
        IsCorrect = false;
    }

    /// <summary>
    /// Set the answer text
    /// </summary>
    /// <param name="text"></param>
    public void SetAnswerText(string text)
    {
        _answerText.text = text;
    }

    /// <summary>
    /// Associates this button with the correct answer
    /// </summary>
    public void SetAsCorrect()
    {
        IsCorrect = true;  
    }

    /// <summary>
    /// On click event of this button
    /// </summary>
    public void CheckAnswer()
    {
        ChangeBorderStyle(IsCorrect);
        QuestionManager.Instance.OnAnswerButtonPress(IsCorrect);
    }

    /// <summary>
    /// Sets the border color to green for correct, red for incorrect
    /// </summary>
    /// <param name="isCorrect"></param>
    public void ChangeBorderStyle(bool isCorrect)
    {
        _normalBorder.gameObject.SetActive(false);
        _correctBorder.gameObject.SetActive(isCorrect);
        _incorrectBorder.gameObject.SetActive(!isCorrect);
    }
}
