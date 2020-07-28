using UnityEngine;
using System.Collections.Generic;

public static class SmoothBrush
{
    public static void Execute(MarchingTerrain terrain, BrushProps props, bool invert)
    {
        float radius = props.brushRadius;
        float opacity = props.brushOpacity;
        Vector3 point = props.point;

        int start_x = (int)(point.x - radius);
        int start_y = (int)(point.y - radius);
        int start_z = (int)(point.z - radius);

        int diameter = (int)(radius * 2);

        List<TerrainChunk> chunks = new List<TerrainChunk>();

        for (int x = start_x; x < start_x + diameter; x++)
        {
            for (int y = start_y; y < start_y + diameter; y++)
                for (int z = start_z; z < start_z + diameter; z++)
                {
                    int c_x = x / MarchingTerrain.m_chunkSize;
                    int c_y = y / MarchingTerrain.m_chunkSize;
                    int c_z = z / MarchingTerrain.m_chunkSize;

                    if (!chunks.Find(_chunk => _chunk.X == c_x && _chunk.Y == c_y && _chunk.Z == c_z))
                    {
                        var temp = terrain.GetChunk(c_x, c_y, c_z);

                        if (temp != null)
                        {
                            chunks.Add(temp);
                        }
                    }

                    SmoothCell(x, y, z, opacity, terrain);
                }
        }

        chunks.ForEach(x => x?.Generate());
    }

    private static void SmoothCell(int x, int y, int z, float power, MarchingTerrain terrain)
    {
        float s0 = terrain.terrainConfiguration.terrainMap.GetSurface(x, y, z);

        float s1 = terrain.terrainConfiguration.terrainMap.GetSurface(x + 1, y, z);
        float s2 = terrain.terrainConfiguration.terrainMap.GetSurface(x - 1, y, z);
        float s3 = terrain.terrainConfiguration.terrainMap.GetSurface(x, y, z + 1);
        float s4 = terrain.terrainConfiguration.terrainMap.GetSurface(x, y, z - 1);
        float s5 = terrain.terrainConfiguration.terrainMap.GetSurface(x, y + 1, z);
        float s6 = terrain.terrainConfiguration.terrainMap.GetSurface(x, y - 1, z);

        float surface = (s0 + s1 + s2 + s3 + s4 + s5 + s6) / 7;

        terrain.terrainConfiguration.terrainMap.SetSurface(x, y, z, Mathf.Lerp(s0, surface, power));
    }
}