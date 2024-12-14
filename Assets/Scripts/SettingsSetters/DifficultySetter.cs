using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    [SerializeField]
    private TriviaDifficulty _associatedDifficulty;
    private void OnEnable()
    {
        
        SettingsManager.Instance.Settings.Difficulty = _associatedDifficulty;
        print(SettingsManager.Instance.Settings.Difficulty.ToString());
    }
    
}
