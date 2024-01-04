using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Perlin1 : MonoBehaviour
{

    public RawImage image;
    Texture2D tex;
    public float zoom1;
    public float zoom2;
    public float alturaMaxima = 1;
    public float valorMin = 1;


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
        //zoom1 += 0.0001f;
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
        color.b = 0;
        
        float v = alturaMaxima * Mathf.PerlinNoise(x * zoom1, y * zoom2) ;

        if(v >= valorMin)
            color.b = v;

        return color;
    }

}
