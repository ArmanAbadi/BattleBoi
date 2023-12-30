using UnityEngine;

public static class Perlin
{
    static float OffsetX = 1233245.123f;
    static float OffsetY = 436872.345f;
    public static float PerlinNoise(float x, float y, int length, float scale)
    {
        return Mathf.PerlinNoise((float)(x/ length)* scale+OffsetX, (float)y/ length* scale+OffsetY);
    }
}
