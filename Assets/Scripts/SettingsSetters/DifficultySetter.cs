using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    [SerializeField]
    private TriviaDifficulty _associatedDifficulty;
    private void OnEnable()
    {

        GameManager.Instance.Settings.Difficulty = _associatedDifficulty;
        print(GameManager.Instance.Settings.Difficulty.ToString());
    }
    
}
