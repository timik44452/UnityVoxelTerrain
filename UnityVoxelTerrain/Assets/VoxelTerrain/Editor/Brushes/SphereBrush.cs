using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public static class SphereBrush
{
    public static void Execute(MarchingTerrain terrain, BrushProps props, bool invert)
    {
        Vector3 point = props.point;
        float radius = props.brushRadius;
        float opacity = props.brushOpacity;
        
        int start_x = Mathf.RoundToInt(point.x - radius);
        int start_y = Mathf.RoundToInt(point.y - radius);
        int start_z = Mathf.RoundToInt(point.z - radius);

        int diameter = Mathf.RoundToInt(radius * 2);

        List<TerrainChunk> chunks = new List<TerrainChunk>();

        for (int x = start_x; x < start_x + diameter; x++)
        {
            for (int y = start_y; y < start_y + diameter; y++)
            {
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

                    float power = Vector3.Distance(point, new Vector3(x, y, z)) / radius;

                    power = Mathf.Clamp01((1F - power) * opacity);

                    if (invert)
                    {
                        terrain.terrainConfiguration.terrainMap.AddSurface(x, y, z, -power);
                    }
                    else
                    {
                        terrain.terrainConfiguration.terrainMap.AddSurface(x, y, z, power);
                    }
                }
            }
        }

        chunks.ForEach(x => x?.Generate());
    }
}