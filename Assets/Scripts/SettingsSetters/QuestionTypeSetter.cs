using UnityEngine;

public class QuestionTypeSetter : MonoBehaviour
{
    [SerializeField]
    private TriviaQuestionType _associatedQuestionType;

    private void OnEnable()
    {
        GameManager.Instance.Settings.QuestionType = _associatedQuestionType;   
        print(GameManager.Instance.Settings.QuestionType.ToString());
    }
}
