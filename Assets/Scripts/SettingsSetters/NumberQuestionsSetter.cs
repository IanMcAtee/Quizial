using UnityEngine;


public class NumberQuestionsSetter : MonoBehaviour
{
    [SerializeField]
    private PlusMinusButtonGroup _plusMinusButtonGroup;

    public void SetNumberOfQuestions()
    {
        GameManager.Instance.Settings.NumQuestions = _plusMinusButtonGroup.Value;
        print(GameManager.Instance.Settings.NumQuestions);
    }
}
