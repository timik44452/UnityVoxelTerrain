using UnityEngine;
using System.Collections.Generic;

public static class CircleBrush
{
    public static void Execute(MarchingTerrain terrain, BrushProps props, bool invert)
    {
        float radius = props.brushRadius;
        float opacity = props.brushOpacity;
        Vector3 point = props.point;

        int start_x = (int)(point.x - radius);
        int y = (int)(point.y + 0.5F);
        int start_z = (int)(point.z - radius);

        int diameter = (int)(radius * 2);

        List<TerrainChunk> chunks = new List<TerrainChunk>();

        for (int x = start_x; x < start_x + diameter; x++)
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

        chunks.ForEach(x => x?.Generate());
    }
}