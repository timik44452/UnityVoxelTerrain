using System.IO;
using UnityEditor;
using UnityEngine;

public static class ContentUtility
{
    private static string s_rootContentPath = "Assets";
    private static string s_contentPath = "Assets/VoxelTerrain/Content/";

    public static T LoadContent<T>(string name, bool useRootPath = false) where T : Object
    {
        return AssetDatabase.LoadAssetAtPath<T>(GetPath(name, useRootPath));
    }

    public static T LoadOrCreateScriptableObject<T>(string name, bool useRootPath = false) where T : ScriptableObject
    {
        var tempObejct = LoadContent<T>(name, useRootPath);

        return tempObejct == null ? CreateScriptableObject<T>(name, useRootPath) : tempObejct;
    }

    public static T CreateScriptableObject<T>(string name, bool useRootPath = false) where T : ScriptableObject
    {
        var path = GetPath(name, useRootPath);
        var scriptableObject = ScriptableObject.CreateInstance<T>();
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.CreateAsset(scriptableObject, path);

        return scriptableObject;
    }

    private static string GetPath(string assetName, bool useRootPath)
    {
        string path = Path.Combine(useRootPath ? s_rootContentPath : s_contentPath, assetName);

        return path;
    }
}
