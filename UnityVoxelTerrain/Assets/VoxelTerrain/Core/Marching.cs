using UnityEngine;

[System.Serializable]
public class Marching
{
    [SerializeField]
    private int triangle_Index = 0;
    [SerializeField]
    private int vertice_Index = 0;
    [SerializeField]
    private int[] triangles = new int[0];
    [SerializeField]
    private Vector3[] vertices = new Vector3[0];
    [SerializeField]
    private Vector2[] uvs = new Vector2[0];
    
    private const float threshould = 0.5F;


    public Mesh Generate(int lodLevel, MarchingRegion region, TerrainMap map)
    {
        triangle_Index = 0;
        vertice_Index = 0;

        float[] cubeData = new float[8];

        for (int x = region.X; x < region.Right; x += lodLevel)
        {
            for (int y = region.Y; y < region.Top; y += lodLevel)
            {
                for (int z = region.Z; z < region.Forward; z += lodLevel)
                {
                    Color color = map.GetColor(x, y, z);

                    GetHue(color, out float u, out float v);

                    int flagIndex = 0;

                    Vector3 point = new Vector3(
                        x % region.Width,
                        y % region.Height,
                        z % region.Length);

                    for (int i = 0; i < 8; i++)
                    {
                        int ix = x + MarchingData.VertexOffset[i].x * lodLevel;
                        int iy = y + MarchingData.VertexOffset[i].y * lodLevel;
                        int iz = z + MarchingData.VertexOffset[i].z * lodLevel;

                        float surface = map.GetSurface(ix, iy, iz);

                        if (surface <= threshould)
                        {
                            flagIndex |= 1 << i;
                        }

                        cubeData[i] = surface;
                    }

                    March(lodLevel, point, u, v, flagIndex, cubeData);
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.SetVertices(vertices, 0, vertice_Index);
        mesh.SetTriangles(triangles, 0, triangle_Index, 0);
        mesh.SetUVs(0, uvs, 0, vertice_Index);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        
        return mesh;
    }

    private void March(int lodLevel, Vector3 point, float u, float v, int flagIndex, float[] marchingData)
    {
        int edgeFlags = MarchingData.CubeEdgeFlags[flagIndex];

        Vector3[] vertex = new Vector3[12];

        if (edgeFlags == 0)
        {
            return;
        }

        for (int i = 0; i < 12; i++)
        {
            if ((edgeFlags & (1 << i)) != 0)
            {
                int edge_connection0 = MarchingData.EdgeConnection[i, 0];
                float edgeOffset = GetOffset(marchingData[edge_connection0], marchingData[MarchingData.EdgeConnection[i, 1]]);
                Vector3 vertexOffset = (MarchingData.VertexOffset[edge_connection0] + MarchingData.EdgeDirection[i] * edgeOffset) * lodLevel;

                vertex[i] = point + vertexOffset;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (MarchingData.TriangleConnectionTable[flagIndex, 3 * i] < 0)
            {
                break;
            }

            if (vertice_Index + 3 >= vertices.Length)
            {
                ResizeBuffers();
            }

            int idx = vertice_Index;

            for (int j = 0; j < 3; j++)
            {
                int vert = MarchingData.TriangleConnectionTable[flagIndex, 3 * i + j];

                triangles[triangle_Index] = idx + j;
                vertices[vertice_Index] = vertex[vert];
                uvs[vertice_Index] = new Vector2(u, v);

                triangle_Index++;
                vertice_Index++;
            }
        }
    }

    private void ResizeBuffers()
    {
        int size = 16;

        var triangles_temp = triangles;
        var vertices_temp = vertices;
        var uvs_temp = uvs;

        triangles = new int[triangles_temp.Length + size * 3];
        vertices = new Vector3[vertices_temp.Length + size];
        uvs = new Vector2[uvs_temp.Length + size];

        triangles_temp.CopyTo(triangles, 0);
        vertices_temp.CopyTo(vertices, 0);
        uvs_temp.CopyTo(uvs, 0);
    }

    private float GetOffset(float v1, float v2)
    {
        return  (threshould - v1) / (v2 - v1);
    }

    private void GetHue(Color color, out float h, out float b)
    {
        float hue = 0;
        float max = Mathf.Max(color.r, color.g, color.b);
        float min = Mathf.Min(color.r, color.g, color.b);
        float d = max - min;

        if (d == 0)
        {
            h = b = 0;
            return;
        }

        if (max == color.r)
        {
            hue = (color.g - color.b) / d + (color.g < color.b ? 6 : 0);
        }
        else if (max == color.g)
        {
            hue = (color.b - color.r) / d + 2;
        }
        else if (max == color.b)
        {
            hue = (color.r - color.g) / d + 4;
        }

        h = hue * 0.33F;
        b = color.grayscale;
    }
}