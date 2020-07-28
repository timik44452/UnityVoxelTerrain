using UnityEngine;
using System.Collections.Generic;

public class MarchingTerrain : MonoBehaviour
{
    public int Width
    {
        get
        {
            if (terrainConfiguration == null)
                return 0;

            return terrainConfiguration.Width;
        }
    }
    public int Height
    {
        get
        {
            if (terrainConfiguration == null)
                return 0;

            return terrainConfiguration.Height;
        }
    }
    public int Length
    {
        get
        {
            if (terrainConfiguration == null)
                return 0;

            return terrainConfiguration.Length;
        }
    }
    public int Level
    {
        get => m_mainLevel;
        set
        {
            int tempLevel = Mathf.Clamp(value, 1, m_chunkSize / 2);

            if(m_mainLevel != tempLevel)
            {
                m_mainLevel = tempLevel;
            }
        }
    }

    public Material material;

    public TerrainConfiguration terrainConfiguration;

    public const int m_chunkSize = 32;

    [SerializeField]
    private int m_mainLevel = 1;
    [SerializeField]
    private int m_chunkCountWidth = 0;
    [SerializeField]
    private int m_chunkCountHeight = 0;
    [SerializeField]
    private int m_chunkCountLength = 0;

    [SerializeField]
    private TerrainChunk[] chunks = new TerrainChunk[0];


    #region Processing
    public IEnumerable<ProcessingData> SetLevel(int level)
    {
        Level = level;

        return Rebuild();
    }

    public IEnumerable<ProcessingData> Rebuild()
    {
        foreach (var clearing_processing in Clear())
        {
            yield return clearing_processing;
        }

        foreach (var marching_processing in Build())
        {
            yield return marching_processing;
        }
    }

    public IEnumerable<ProcessingData> Clear()
    {
        ProcessingData processing = new ProcessingData("Destroying chunks", 0);

        var chunks = gameObject.GetComponentsInChildren<TerrainChunk>();

        for (int i = 0; i < chunks.Length; i++)
        {
            DestroyImmediate(chunks[i].gameObject);

            processing.progress = i / (float)chunks.Length;

            yield return processing;
        }

        RecreateChunkBuffer();
    }

    public IEnumerable<ProcessingData> Build()
    {
        RecreateChunkBuffer();

        ProcessingData processing = new ProcessingData("Marching", 0);

        if (terrainConfiguration == null)
        {
            yield break;
        }

        for (int x = 0; x < m_chunkCountWidth; x++)
            for (int y = 0; y < m_chunkCountHeight; y++)
                for (int z = 0; z < m_chunkCountLength; z++)
                {
                    MarchingRegion region = new MarchingRegion(x * m_chunkSize, y * m_chunkSize, z * m_chunkSize, m_chunkSize, m_chunkSize, m_chunkSize);
                    TerrainChunk currentChunk = TerrainChunk.CreateChunk(region, this, material, false);

                    chunks[x + z * m_chunkCountWidth + y * m_chunkCountWidth * m_chunkCountLength] = currentChunk;
                    currentChunk.Generate();

                    currentChunk.transform.parent = transform;

                    processing.progress = x / (float)m_chunkCountWidth;

                    yield return processing;
                }
    }

    public IEnumerable<ProcessingData> BuildMesh()
    {
        Mesh mesh = new Mesh();
        ProcessingData processing = new ProcessingData("Mesh building", 0, mesh);

        List<CombineInstance> instances = new List<CombineInstance>();

        if (terrainConfiguration == null)
        {
            yield break;
        }

        for (int x = 0; x < m_chunkCountWidth; x++)
            for (int y = 0; y < m_chunkCountHeight; y++)
                for (int z = 0; z < m_chunkCountLength; z++)
                {
                    MarchingRegion region = new MarchingRegion(x * m_chunkSize, y * m_chunkSize, z * m_chunkSize, m_chunkSize, m_chunkSize, m_chunkSize);
                    TerrainChunk chunk = chunks[x + z * m_chunkCountWidth + y * m_chunkCountWidth * m_chunkCountLength];

                    if (chunk == null || chunk.mesh == null || chunk.mesh.vertexCount == 0)
                        continue;

                    CombineInstance combineInstance = new CombineInstance();

                    combineInstance.mesh = chunk.mesh;
                    combineInstance.mesh.name = chunk.name;
                    combineInstance.transform = chunk.transform.localToWorldMatrix;

                    instances.Add(combineInstance);

                    processing.progress = x / (float)m_chunkCountWidth;

                    yield return processing;
                }
        
        mesh.CombineMeshes(instances.ToArray(), true);

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        processing.progress = 1F;

        yield return processing;
    }
    #endregion

    public TerrainChunk GetChunk(int x, int y, int z)
    {
        if (x >= 0 && x < m_chunkCountWidth && y >= 0 && y < m_chunkCountHeight && z >= 0 && z <= m_chunkCountLength)
        {
            return chunks[x + z * m_chunkCountWidth + y * m_chunkCountWidth * m_chunkCountLength];
        }
        else
        {
            return null;
        }
    }

    private void RecreateChunkBuffer()
    {
        m_chunkCountWidth = Width / m_chunkSize;
        m_chunkCountHeight = Height / m_chunkSize;
        m_chunkCountLength = Length / m_chunkSize;

        chunks = new TerrainChunk[m_chunkCountWidth * m_chunkCountHeight * m_chunkCountLength];
    }
}
