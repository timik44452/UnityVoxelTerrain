using UnityEditor;
using UnityEngine;

public static class TerrainRay
{
    public static bool TryRaycastChunk(out TerrainChunk chunk, out Vector3 point)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit raycastHit;

        bool chunkHitted = Physics.Raycast(ray, out raycastHit) && raycastHit.collider.GetComponent<TerrainChunk>();
        
        chunk = raycastHit.collider?.GetComponent<TerrainChunk>();
        point = raycastHit.point;

        return chunkHitted;
    }
}