using UnityEngine;

public static class Perlin
{
    public static float PerlinNoise(float x, float y, int length, float scale, float OffsetX, float OffsetY)
    {
        return Mathf.PerlinNoise((float)(x/ length)* scale+OffsetX, (float)y/ length* scale+OffsetY);
    }
}
