using UnityEngine;

public enum ToolType
{
    SphereBrush,
    CircleBrush,
    Noise,
    Smooth,
    Paint
}

public class BrushProps : ScriptableObject
{
    public Color color;
    public Vector3 point;
    public ToolType currentToolType;

    public float brushRadius = 1.0F;
    public float brushOpacity = 1.0F;

    public void Save()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}
