using UnityEngine;

using UnityEditor;
using UnityEngine.UI;


[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ToggleButton targetToggleButton = (ToggleButton)target;

        EditorGUILayout.HelpBox("Toggle Button is experimental, may result in unexpected behavior", MessageType.Warning);

        if (targetToggleButton.transition == ToggleButton.Transition.Animation)
        {
            EditorGUILayout.HelpBox("Toggle Button does not work with animation transition", MessageType.Error);
        }

        // Show default inspector property editor
        DrawDefaultInspector();

        
        
    }
}

