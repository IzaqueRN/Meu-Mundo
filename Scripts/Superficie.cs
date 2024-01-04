using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superficie
{

    public int[,] altura = new int[16, 16];
    public Vector2Int posicao = Vector2Int.zero;

    public static float alturaMaxima = 32;
    public static float alturaMin = 0;

    public static float zoomX = 0.0197f;
    public static float zoomY = 0.0198f;


    public int AlturaSuperficie(int x, int z)
    {

        float altura = alturaMaxima * Mathf.PerlinNoise(x * zoomX, z * zoomY);

        int y = Mathf.FloorToInt(altura);

        if (y < alturaMin)
            y = 0;
        return y;
    }

    public int AlteracaoSuperficie(int x, int z)
    {

        float altura = 8 * Mathf.PerlinNoise(x * 0.03f, z * 0.04f);

        int y = Mathf.FloorToInt(altura);

        if (y < 4)
            y = 0;
        return y;
    }

}

