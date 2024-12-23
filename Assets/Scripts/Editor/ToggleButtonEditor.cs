using UnityEditor;

/// <summary>
/// Custom editor for ToggleButton type
/// </summary>
[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the editor target
        ToggleButton targetToggleButton = (ToggleButton)target;

        // Warn user that ToggleButton is experimental
        EditorGUILayout.HelpBox("Toggle Button is experimental, may result in unexpected behavior", MessageType.Warning);

        // Warn user that ToggleButton does not work in button animation mode
        if (targetToggleButton.transition == ToggleButton.Transition.Animation)
        {
            EditorGUILayout.HelpBox("Toggle Button does not work with animation transition", MessageType.Error);
        }

        // Show default inspector property editor
        DrawDefaultInspector(); 
    }
}

