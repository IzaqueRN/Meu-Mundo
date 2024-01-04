using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Perlin : MonoBehaviour
{

    public RawImage image;
    Texture2D tex;
    public float zoom;
    public float alturaMaxima = 1;



    void Start()
    {
        tex = new Texture2D(100, 100);
        tex.filterMode = FilterMode.Point;
        tex.name = "tESTE";
        image.texture = tex;

        for (int y = 0; y < tex.height; y++)
        {

            for (int x = 0; x < tex.width; x++)
            {
                tex.SetPixel(x,y,Color.black);

            }

        }

        tex.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            for (int y = 0; y < tex.height; y++)
            {

                for (int x = 0; x < tex.width; x++)
                {
                    tex.SetPixel(x, y, testePerlin(x,y));

                }

            }

            tex.Apply();
        }
       // zoom += 0.0001f;
        for (int y = 0; y < tex.height; y++)
        {

            for (int x = 0; x < tex.width; x++)
            {
                tex.SetPixel(x, y, testePerlin(x, y));

            }

        }

        tex.Apply();

    }

    public Color testePerlin(int x,int y) {

        Color color = Color.black;
        color.g = alturaMaxima * Mathf.PerlinNoise(x * zoom, y * zoom) ;

        return color;
    }

}
