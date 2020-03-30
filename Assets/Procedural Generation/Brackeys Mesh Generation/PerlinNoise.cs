using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 20.0f;

    public float xScroll = 0.00f;
    public float yScroll = 0.00f;

    public float xOffset = 100.0f;
    public float yOffset = 100.0f;

    public bool randomOffset = false;
    void Update()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(width, height);

        if (randomOffset)
        {
            xOffset = Random.Range(0, 999999);
            yOffset = Random.Range(0, 999999);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                tex.SetPixel(x, y, color);
            }
        }

        xOffset += xScroll;
        yOffset += yScroll;
        
        tex.Apply();

        return tex;
    }

    Color CalculateColor(int x, int y)
    {
        var xNormalized = (float) x / width  * scale + xOffset;
        var yNormalized = (float) y / height  * scale + yOffset;
        float sample = Mathf.PerlinNoise(xNormalized, yNormalized);
        return new Color(sample, sample, sample);
    }

}
