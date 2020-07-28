using UnityEngine;

[CreateAssetMenu]
public class TerrainConfiguration : ScriptableObject
{
    public int Width
    {
        get => m_width;
        set
        {
            if (m_width != value)
            {
                m_width = value;
                terrainMap.Resize(m_width, m_height, m_length);
            }
        }
    }
    public int Height
    {
        get => m_height;
        set
        {
            if (m_height != value)
            {
                m_height = value;
                terrainMap.Resize(m_width, m_height, m_length);
            }
        }
    }
    public int Length
    {
        get => m_length;
        set
        {
            if (m_length != value)
            {
                m_length = value;
                terrainMap.Resize(m_width, m_height, m_length);
            }
        }
    }

    [SerializeField, HideInInspector]
    public TerrainMap terrainMap;

    [SerializeField]
    private int m_width = 512;
    [SerializeField]
    private int m_height = 32;
    [SerializeField]
    private int m_length = 512;


    private void Awake()
    {
        terrainMap = new TerrainMap(m_width, m_height, m_length);
    }

    public void Save()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    public void Resize(int width, int height, int length)
    {
        if (m_width == width && m_height == height && m_length == length)
            return;

        m_width = width;
        m_height = height;
        m_length = length;

        terrainMap?.Resize(width, height, length);
    }
}
