using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Superficie
{

    public int[,] mapAlturas = new int[16, 16];
    public Vector2Int posicao = Vector2Int.zero;

    public List<ChunkMesh> chunksList = new List<ChunkMesh>();

    public static float alturaMaxima = 20;
    public static float alturaMin = 4;

    public static float zoomX = 0.0097f;
    public static float zoomY = 0.0098f;

    public void GerarSuperficie()
    {

        Vector2Int posiChunkMundo = new Vector2Int(posicao.x * 16, posicao.y * 16);
        Vector3Int posicaoBlocoMundo = Vector3Int.zero;
        int posicaoYBlocoNaChunk = 0;
        int posicaoYChunkDoBloco = 0;
        bool temChunk = false;
        int idBloco = 0;

        for (int z = 0; z < 16; z++)
        {
            for (int x = 0; x < 16; x++)
            {

                posicaoBlocoMundo = new Vector3Int(posiChunkMundo.x + x, 0, posiChunkMundo.y + z);
                int altura = AlturaSuperficie(posicaoBlocoMundo.x, posicaoBlocoMundo.z);
                altura += AlteracaoSuperficie(posicaoBlocoMundo.x, posicaoBlocoMundo.z, altura);
                mapAlturas[x, z] = altura;
                posicaoBlocoMundo.y = altura;
                idBloco = IdBloco(altura);

                if (posicaoBlocoMundo.y <= 6) { idBloco = 4; posicaoBlocoMundo.y = 5; }

                posicaoYBlocoNaChunk = posicaoBlocoMundo.y % 16;
                posicaoYChunkDoBloco = posicaoBlocoMundo.y / 16;


                temChunk = false;
                // busca a chunk do bloco
                for (int c = 0; c < chunksList.Count; c++)
                {

                    //se achou a chunk do block
                    if (chunksList[c].posicao.y == posicaoYChunkDoBloco)
                    {
                        temChunk = true;
                        chunksList[c].blocos[x, posicaoYBlocoNaChunk, z] = idBloco; // add o bloco na chunk

                        break;
                    }

                }
                // não tem a chunk, entrao cria uma
                if (temChunk == false)
                {

                    ChunkMesh chunk = new ChunkMesh();

                    chunk.posicao.y = posicaoYChunkDoBloco;
                    chunk.posicao.x = posicao.x;
                    chunk.posicao.z = posicao.y;
                    chunk.blocos[x, posicaoYBlocoNaChunk, z] = idBloco;  //add o bloco na nova chunk
                    chunk.atualizar = true;

                    Debug.Log("Criando ChunMesh " + chunk.posicao);
                    chunksList.Add(chunk);

                }

            }


        }

    }

    public void preencherChunks()
    {


        for (int c = 0; c < chunksList.Count; c++)
        {
            

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                   
                    int y = 0;
                    while (y < 16) {

                        if (y + (16 * chunksList[c].posicao.y) > mapAlturas[x,z] - 1) { break;}

                        if (chunksList[c].blocos[x, y, z] == 0) {
                            chunksList[c].blocos[x, y, z] = 1;
                        }
                        else { break;}
                        y++;

                    }

                }

            }


        }


    }
    public int IdBloco(int altura) {

        int id = 0;

        if (altura == 0 || altura < 2) { id = -1;}
        if (altura > 1 || altura < 8 ) { id = 2; }
        if (altura > 8) { id = 3; }
        if (altura > 15) { id = 1; }


        return id;
    }

    public int AlturaSuperficie(int x, int z)
    {

        float altura = alturaMaxima * Mathf.PerlinNoise(x * zoomX, z * zoomY);

        int y = Mathf.FloorToInt(altura);

        if (y < alturaMin)
            y = 0;
        return y;
    }

    public int AlteracaoSuperficie(int x, int z, int alt)
    {

        float altura = (alt * 0.8f) * Mathf.PerlinNoise(x * 0.05f, z * 0.06f);

        int y = Mathf.FloorToInt(altura);

        if (y < 2)
            y = 0;
        return y;
    }




}

