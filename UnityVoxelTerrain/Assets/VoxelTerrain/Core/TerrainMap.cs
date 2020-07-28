using UnityEngine;

[System.Serializable]
public class TerrainMap
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private int length;
    [SerializeField]
    private float[] surface_data;
    [SerializeField]
    private Color[] texture_data;

    public TerrainMap(int Width, int Height, int Length, bool fill = true)
    {
        width = Width;
        height = Height;
        length = Length;

        surface_data = new float[Width * Height * Length];
        texture_data = new Color[Width * Height * Length];

        if (fill)
        {
            Fill(0, 0, 0, Width, 1, Length, 1);
        }
    }

    public void AddSurface(int x, int y, int z, float surface)
    {
        if (Contains(x, y, z))
        {
            int idx = x + z * width + y * width * length;

            surface_data[idx] = Mathf.Clamp(surface_data[idx] + surface, -1, 1);
        }
    }

    public void SetSurface(int x, int y, int z, float surface)
    {
        if (Contains(x, y, z))
        {
            int idx = x + z * width + y * width * length;

            surface_data[idx] = surface;
        }
    }

    public float GetSurface(int x, int y, int z)
    {
        if (Contains(x, y, z))
        {
            return surface_data[x + z * width + y * width * length];
        }

        return 0;
    }

    public void SetColor(int x, int y, int z, Color color)
    {
        if (Contains(x, y, z))
        {
            texture_data[x + z * width + y * width * length] = color;
        }
    }

    public Color GetColor(int x, int y, int z)
    {
        if (Contains(x, y, z))
        {
            return texture_data[x + z * width + y * width * length];
        }

        return Color.black;
    }

    public void Resize(int width, int height, int length)
    {
        if (this.width == width && this.height == height && this.length == length)
            return;

        int old_width = this.width;
        int old_height = this.height;
        int old_length = this.length;

        this.width = width;
        this.height = height;
        this.length = length;

        var tempSurface = surface_data;
        var tempTexture = texture_data;

        surface_data = new float[width * height * length];
        texture_data = new Color[width * height * length];

        Fill(0, 0, 0, width, 1, length, 1);

        for (int x = 0; x < Mathf.Min(old_width, width); x++)
            for (int y = 0; y < Mathf.Min(old_height, height); y++)
                for (int z = 0; z < Mathf.Min(old_length, length); z++)
                {
                    int idx = x + z * old_width + y * old_width * old_length;

                    SetSurface(x, y, z, tempSurface[idx]);
                    SetColor(x, y, z, tempTexture[idx]);
                }
    }

    public void Fill(int x, int y, int z, int width, int height, int length, float surface)
    {
        for (int _x = x; _x < x + width; _x++)
            for (int _y = y; _y < y + height; _y++)
                for (int _z = z; _z < z + length; _z++)
                {
                    SetSurface(_x, _y, _z, surface);
                }
    }

    private bool Contains(int x, int y, int z)
    {
        return
            x >= 0 && x < width &&
            y >= 0 && y < height &&
            z >= 0 && z < length;
    }
}