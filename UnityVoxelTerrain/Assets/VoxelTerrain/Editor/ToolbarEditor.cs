using UnityEditor;
using UnityEngine;

public class ToolbarEditor : EditorWindow
{
    public static ToolbarEditor CurrentToolbar
    {
        get => GetWindow<ToolbarEditor>();
    }

    [MenuItem("Tools/MarchingTerrain/Toolbar")]
    public static void OpenToolbar()
    {
        if (CurrentToolbar == null)
            CreateWindow<ToolbarEditor>();

        ToolbarEditor toolbarEditor = CurrentToolbar;

        toolbarEditor.Focus();
    }

    private void OnGUI()
    {
        Rect windowRect = new Rect(Vector2.zero, position.size);

        //ToolbarGUI.DrawToolbar(windowRect);
    }
}
