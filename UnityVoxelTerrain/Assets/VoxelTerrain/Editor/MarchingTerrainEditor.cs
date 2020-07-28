using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingTerrain))]
public class MarchingTerrainEditor : Editor
{
    [SerializeField]
    private HotkeyUtility m_hotkeyUtility;
    [SerializeField]
    private TerrainEditorUtility m_terrainEditorUtility;

    #region Service
    [SerializeField]
    private int m_width;
    [SerializeField]
    private int m_height;
    [SerializeField]
    private int m_length;
    #endregion

    public override void OnInspectorGUI()
    {
        if (!IsReady())
        {
            InitializeEditor();
            return;
        }

        MarchingTerrain terrain = (MarchingTerrain)target;

        var configuration = (TerrainConfiguration)EditorGUILayout.ObjectField("Configuration", terrain.terrainConfiguration, typeof(TerrainConfiguration), false);

        if (terrain.terrainConfiguration != configuration)
        {
            terrain.terrainConfiguration = configuration;

            ProcessingUtility.StartProcessing(terrain.Rebuild());
        }

        if (terrain.terrainConfiguration != null)
        {
            var level = EditorGUILayout.IntField("Level", terrain.Level);

            m_width = EditorGUILayout.IntField("Width", m_width);
            m_height = EditorGUILayout.IntField("Height", m_height);
            m_length = EditorGUILayout.IntField("Length", m_length);

            m_terrainEditorUtility.currentProps.currentToolType = (ToolType)EditorGUILayout.EnumPopup("Tool", m_terrainEditorUtility.currentProps.currentToolType);
            m_terrainEditorUtility.currentProps.brushRadius = EditorGUILayout.Slider("Radius", m_terrainEditorUtility.currentProps.brushRadius, 1, 25);
            m_terrainEditorUtility.currentProps.brushOpacity = EditorGUILayout.Slider("Opacity", m_terrainEditorUtility.currentProps.brushOpacity, 0.001F, 1F);

            if (m_terrainEditorUtility.currentProps.currentToolType == ToolType.Paint)
            {
                m_terrainEditorUtility.currentProps.color = EditorGUILayout.ColorField("Color", m_terrainEditorUtility.currentProps.color);
            }

            if (GUILayout.Button("Fill"))
            {
                terrain.terrainConfiguration.terrainMap.Fill(0, 0, 0, terrain.Width, terrain.Height - 1, terrain.Length, 1);
                ProcessingUtility.StartProcessing(terrain.Rebuild());
            }

            level = Mathf.Clamp(level, 1, MarchingTerrain.m_chunkSize / 2);

            if (terrain.Level != level)
            {
                terrain.Level = level;

                ProcessingUtility.StartProcessing(terrain.SetLevel(level));
            }

            if (terrain.Width != m_width || terrain.Height != m_height || terrain.Length != m_length)
            {
                if (GUILayout.Button("Resize"))
                {
                    terrain.terrainConfiguration.Resize(m_width, m_height, m_length);
                    ProcessingUtility.StartProcessing(terrain.Rebuild());
                }
            }

            if (GUILayout.Button("Rebuild"))
            {
                ProcessingUtility.StartProcessing(terrain.Build());
            }

            if (GUILayout.Button("Export"))
            {
                Mesh mesh = ProcessingUtility.StartProcessing(terrain.BuildMesh()) as Mesh;

                if (mesh != null)
                {
                    string path = EditorUtility.SaveFilePanel("Exporting mesh", "Assets", "terrain", "obj");

                    MeshExporter.MeshToFile(mesh, path);
                }
            }

            if (GUILayout.Button("Save"))
            {
                terrain.terrainConfiguration.Save();
            }
        }
    }

    private void OnEnable()
    {
        InitializeEditor();
    }

    private void OnSceneGUI()
    {
        if (!IsReady())
        {
            return;
        }

        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<TerrainChunk>())
            Selection.activeObject = target;

        Event currentEvent = Event.current;

        switch (currentEvent.type)
        {
            case EventType.KeyDown:
                {
                    m_hotkeyUtility.KeyDown(currentEvent.keyCode);
                }
                break;
            case EventType.KeyUp:
                {
                    m_hotkeyUtility.KeyUp(currentEvent.keyCode);
                }
                break;
            case EventType.MouseUp:
                {
                    m_hotkeyUtility.UnlockInput();

                    if (TerrainRay.TryRaycastChunk(out TerrainChunk chunk, out m_terrainEditorUtility.currentProps.point))
                    {
                        OnTerrainUp(chunk.Terrain);
                    }
                }
                break;
            case EventType.MouseDown:
            case EventType.MouseDrag:
                {
                    if (TerrainRay.TryRaycastChunk(out TerrainChunk chunk, out m_terrainEditorUtility.currentProps.point))
                    {
                        if (chunk.Terrain == null)
                        {
                            break;
                        }

                        OnTerrainMove(chunk.Terrain);

                        if (currentEvent.button != 0)
                        {
                            break;
                        }

                        m_hotkeyUtility.LockInput();

                        if (currentEvent.type == EventType.MouseDown)
                        {
                            OnTerrainDown(chunk.Terrain);
                        }
                        else
                        {
                            OnTerrainDrag(chunk.Terrain);
                        }
                    }
                }
                break;
        }
    }

    private void InitializeEditor()
    {
        MarchingTerrain terrain = (MarchingTerrain)target;

        m_width = terrain.Width;
        m_height = terrain.Height;
        m_length = terrain.Length;

        if (m_hotkeyUtility == null)
        {
            m_hotkeyUtility = new HotkeyUtility();
        }

        if (m_terrainEditorUtility == null)
        {
            m_terrainEditorUtility = new TerrainEditorUtility();
        }
    }

    private void OnTerrainUp(MarchingTerrain terrain)
    {

    }

    private void OnTerrainMove(MarchingTerrain terrain)
    {

    }

    private void OnTerrainDrag(MarchingTerrain terrain)
    {
        UseTool(terrain);
    }

    private void OnTerrainDown(MarchingTerrain terrain)
    {
        UseTool(terrain);
    }

    private void UseTool(MarchingTerrain terrain)
    {
        if(!IsReady())
        {
            return;
        }

        switch (m_terrainEditorUtility.currentProps.currentToolType)
        {
            case ToolType.CircleBrush: CircleBrush.Execute(terrain, m_terrainEditorUtility.currentProps, m_hotkeyUtility.IsInvertedMode()); break;
            case ToolType.SphereBrush: SphereBrush.Execute(terrain, m_terrainEditorUtility.currentProps, m_hotkeyUtility.IsInvertedMode()); break;
            case ToolType.Smooth: SmoothBrush.Execute(terrain, m_terrainEditorUtility.currentProps, m_hotkeyUtility.IsInvertedMode()); break;
            case ToolType.Noise: NoiseBrush.Execute(terrain, m_terrainEditorUtility.currentProps, m_hotkeyUtility.IsInvertedMode()); break;
            case ToolType.Paint: PaintBrush.Execute(terrain, m_terrainEditorUtility.currentProps, m_hotkeyUtility.IsInvertedMode()); break;
        }
    }

    private bool IsReady()
    {
        return 
            m_hotkeyUtility != null && 
            m_terrainEditorUtility != null;
    }
    
}
