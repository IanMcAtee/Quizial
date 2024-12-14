using UnityEngine;

public class QuestionTypeSetter : MonoBehaviour
{
    [SerializeField]
    private TriviaQuestionType _associatedQuestionType;

    private void OnEnable()
    {
        SettingsManager.Instance.Settings.QuestionType = _associatedQuestionType;   
        print(SettingsManager.Instance.Settings.QuestionType.ToString());
    }
}
