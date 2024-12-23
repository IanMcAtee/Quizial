using UnityEditor;

[CustomEditor(typeof(InfinitySlider))]
public class InfinitySliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
