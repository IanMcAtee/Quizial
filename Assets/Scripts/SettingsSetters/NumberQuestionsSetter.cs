using UnityEngine;


public class NumberQuestionsSetter : MonoBehaviour
{
    [SerializeField]
    private PlusMinusButtonGroup _plusMinusButtonGroup;

    public void SetNumberOfQuestions()
    {
        SettingsManager.Instance.Settings.NumQuestions = _plusMinusButtonGroup.Value;
        print(SettingsManager.Instance.Settings.NumQuestions);
    }
}
