using UnityEngine;
using TMPro;

public class CategoryTextSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _categoryText;

    private void OnEnable()
    {
        _categoryText.text = SettingsManager.Instance.Settings.Category.Name;
    }
}
