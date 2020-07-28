using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    public int X => x;
    public int Y => y;
    public int Z => z;

    public Mesh mesh;
    public MarchingTerrain Terrain;

    [SerializeField]
    private Marching marching;
    [SerializeField]
    private MarchingRegion region;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int z;

    public static TerrainChunk CreateChunk(MarchingRegion region, MarchingTerrain terrain, Material material, bool generate = true)
    {
        var chunkGameObject = new GameObject($"chunk({region.X},{region.Y},{region.Z})");
        var chunk = chunkGameObject.AddComponent<TerrainChunk>();

        chunkGameObject.transform.position = new Vector3(region.X, region.Y, region.Z);

        chunk.x = region.X / region.Width;
        chunk.y = region.Y / region.Height;
        chunk.z = region.Z / region.Length;

        chunk.Terrain = terrain;

        chunk.region = region;
        chunk.marching = new Marching();

        chunk.meshFilter = chunk.AddComponentIfNotAdded<MeshFilter>();
        chunk.meshRenderer = chunk.AddComponentIfNotAdded<MeshRenderer>();

        chunk.meshRenderer.material = material;

        if (generate)
        {
            chunk.Generate();
        }

        return chunk;
    }

    public void Generate()
    {
        mesh = marching.Generate(Terrain.Level, region, Terrain.terrainConfiguration.terrainMap);

        meshFilter.sharedMesh = mesh;

        UpdateCollider(mesh);
    }

    private void UpdateCollider(Mesh mesh)
    {
        // Collider update
        if (GetComponent<MeshCollider>())
        {
            DestroyImmediate(GetComponent<MeshCollider>());
        }

        var collider = gameObject.AddComponent<MeshCollider>();

        collider.sharedMesh = mesh;
    }

    private T AddComponentIfNotAdded<T>() where T : Component
    {
        T component = GetComponent<T>();

        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}