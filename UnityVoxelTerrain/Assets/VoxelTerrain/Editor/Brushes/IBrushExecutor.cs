using UnityEngine;

public interface IBrushExecutor
{
    void Execute(bool invert, float radius, float opacity, Vector3 point, MarchingTerrain terrain);
}
