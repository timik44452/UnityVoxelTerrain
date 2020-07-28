using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TerrainEditorUtility
{
    public BrushProps currentProps
    {
        get
        {
            if (m_currentProps == null)
            {
                m_currentProps = CreateBrushProps();
            }

            return m_currentProps;
        }
    }

    [SerializeField]
    private BrushProps m_currentProps;

    private const string materialAssetName = "Source/MarchingTerrainMaterial.mat";
    private const string brushAssetName = "Temp/Brush properties.asset";
    private const string terrainConfigurationName = "Terrain Configuration.asset";


    [MenuItem("Tools/MarchingTerrain/Create terrain")]
    public static void CreateTerrain()
    {
        var terrain = CreateEmptyTerrain();
        var material = GetMaterial();
        var configuration = ContentUtility.LoadOrCreateScriptableObject<TerrainConfiguration>(terrainConfigurationName, true);
        
        terrain.material = material;
        terrain.terrainConfiguration = configuration;

        ProcessingUtility.StartProcessing(terrain.Build());
    }

    public static BrushProps CreateBrushProps()
    {
        BrushProps brushProperties = ContentUtility.LoadOrCreateScriptableObject<BrushProps>(brushAssetName, true);

        return brushProperties;
    }

    public static MarchingTerrain CreateEmptyTerrain()
    {
        var terrainGameObject = new GameObject("Marching terrain");
        var terrain = terrainGameObject.AddComponent<MarchingTerrain>();

        return terrain;
    }

    public static Material GetMaterial()
    {
        return ContentUtility.LoadContent<Material>(materialAssetName);
    }
}
