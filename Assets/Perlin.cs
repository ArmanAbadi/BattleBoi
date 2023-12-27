using UnityEngine;

public static class Perlin
{
    public static float PerlinNoise(float x, float y, int length, float scale)
    {
        return Mathf.PerlinNoise((float)(x/ length)* scale, (float)y/ length* scale);
    }
}
