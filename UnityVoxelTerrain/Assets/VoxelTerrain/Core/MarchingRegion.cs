using UnityEngine;

[System.Serializable]
public struct MarchingRegion
{
    public int X
    {
        get => m_x;
        set
        {
            m_x = value;
            recalculate();
        }
    }
    public int Y
    {
        get => m_y;
        set
        {
            m_y = value;
            recalculate();
        }
    }
    public int Z
    {
        get => m_z;
        set
        {
            m_z = value;
            recalculate();
        }
    }

    public int Width
    {
        get => m_width;
        set
        {
            m_width = value;
            recalculate();
        }
    }
    public int Height
    {
        get => m_height;
        set
        {
            m_height = value;
            recalculate();
        }
    }
    public int Length
    {
        get => m_length;
        set
        {
            m_length = value;
            recalculate();
        }
    }

    public int Right => m_right;
    public int Top => m_top;
    public int Forward => m_forward;

    [SerializeField]
    private int m_x;
    [SerializeField]
    private int m_y;
    [SerializeField]
    private int m_z;

    [SerializeField]
    private int m_width;
    [SerializeField]
    private int m_height;
    [SerializeField]
    private int m_length;

    [SerializeField]
    private int m_right;
    [SerializeField]
    private int m_top;
    [SerializeField]
    private int m_forward;


    public MarchingRegion(int x, int y, int z, int width, int height, int length)
    {
        m_x = x;
        m_y = y;
        m_z = z;

        m_width = width;
        m_height = height;
        m_length = length;

        m_right = 0;
        m_top = 0;
        m_forward = 0;

        recalculate();
    }

    private void recalculate()
    {
        m_right = m_x + m_width;
        m_top = m_y + m_height;
        m_forward = m_z + m_length;
    }
}