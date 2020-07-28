using UnityEditor;

[CustomEditor(typeof(TerrainConfiguration))]
public class TerrianConfigurationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainConfiguration configuration = (TerrainConfiguration)target;

        configuration.Width = EditorGUILayout.IntField("Width", configuration.Width);
        configuration.Height = EditorGUILayout.IntField("Height", configuration.Height);
        configuration.Length = EditorGUILayout.IntField("Length", configuration.Length);
    }
}
