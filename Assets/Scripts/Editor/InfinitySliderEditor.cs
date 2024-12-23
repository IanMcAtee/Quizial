using UnityEditor;

/// <summary>
/// Custom editor for InfinitySlider type
/// </summary>
[CustomEditor(typeof(InfinitySlider))]
public class InfinitySliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();
    }
}
