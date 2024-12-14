using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    public TMP_Text DescritionText;
    public void UpdateText(string text)
    {
        DescritionText.text = text;
    }
}
