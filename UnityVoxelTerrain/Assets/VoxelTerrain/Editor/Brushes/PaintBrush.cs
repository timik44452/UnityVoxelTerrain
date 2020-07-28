using UnityEngine;
using System.Collections.Generic;

public static class PaintBrush
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

                    terrain.terrainConfiguration.terrainMap.SetColor(x, y, z, props.color);
                }
            }
        }

        chunks.ForEach(x => x?.Generate());
    }
}