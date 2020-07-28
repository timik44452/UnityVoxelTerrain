using UnityEngine;
using System.IO;
using System.Text;

public class MeshExporter : MonoBehaviour
{
    public static string MeshToString(Mesh mesh)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append($"g Group_{mesh.name}\n");

        foreach (Vector3 vertex in mesh.vertices)
        {
            stringBuilder.Append($"v {vertex.x} {vertex.y} {vertex.z}\n");
        }

        stringBuilder.Append("\n");

        foreach (Vector3 normal in mesh.normals)
        {
            stringBuilder.Append($"vn {normal.x} {normal.y} {normal.z}\n");
        }

        stringBuilder.Append("\n");

        foreach (Vector3 uv in mesh.uv)
        {
            stringBuilder.Append($"vt {uv.x} {uv.y}\n");
        }

        for (int material = 0; material < mesh.subMeshCount; material++)
        {
            stringBuilder.Append("\n");

            int[] triangles = mesh.GetTriangles(material);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }

        return stringBuilder.ToString().Replace(',','.');
    }

    public static void MeshToFile(Mesh[] meshes, string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var mesh in meshes)
            {
                writer.Write(MeshToString(mesh));
            }
        }
    }

    public static void MeshToFile(Mesh mesh, string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.Write(MeshToString(mesh));
        }
    }
}
