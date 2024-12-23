using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    public void SetAnswerText(string text)
    {
        _answerText.text = text;
    }

    public void SetAsCorrect()
    {
        IsCorrect = true;  
    }

    public void CheckAnswer()
    {
        ChangeBorderStyle(IsCorrect);
        QuestionManager.Instance.OnAnswerButtonPress(IsCorrect);
        
    }

    public void ChangeBorderStyle(bool isCorrect)
    {
        _normalBorder.gameObject.SetActive(false);
        _correctBorder.gameObject.SetActive(isCorrect);
        _incorrectBorder.gameObject.SetActive(!isCorrect);
    }

    

}
