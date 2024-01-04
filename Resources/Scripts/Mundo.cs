using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Mundo: MonoBehaviour
{

    public GameObject ChunkPrefab;

    public Texture2D tex;
    List<Superficie> superficies = new List<Superficie>();

    public int totalSuperficies = 0;
    public int totalChunks = 0;
    public int totalChunksATT = 0;

    public Vector3Int ChunkSelecionada = new Vector3Int();
    void Start()
    {

        GerarTerreno(30,30);
        InstanciarChunks();

    }
    
    void GerarTerreno(int TamX, int TamZ) {


        Debug.LogWarning("Gerando placas de Superficie... ");

        for (int x = 0; x < TamX; x++)
        {
            for (int z = 0; z < TamX; z++)
            {

                superficies.Add(new Superficie());
                superficies[superficies.Count - 1].posicao = new Vector2Int(x, z);
                superficies[superficies.Count - 1].GerarSuperficie();

            }


        }

        Debug.LogWarning("Superficies Criada, Total superficies: " + superficies.Count);
    }

    void InstanciarChunks() {

        for (int s = 0; s < superficies.Count; s++) // percorre toda a superficies do Mundo 
        {

            for (int c = 0; c < superficies[s].chunksList.Count; c++) // percorre todas as chunks de altura dessa superficie
            {

                superficies[s].chunksList[c].InstanciarChunk(transform);

            }


        }
    
    
    }

    public Transform TransformMundo() {

        return transform;
    }
    public int AlteracaoSuperficie(int x, int z)
    {

        float altura = 8 * Mathf.PerlinNoise(x * 0.033f, z * 0.046f);

        int y = Mathf.FloorToInt(altura);

        if (y < 0)
            y = 0;
        return y;
    }

    public int AlteracaoSuperficie2(int x, int z)
    {

        float altura = 32 * Mathf.PerlinNoise(z * 0.0003f, x * 0.00033f);

        int y = Mathf.FloorToInt(altura);

        if (y < 6)
            y = 0;
        return y;
    }
    void Update()
    {

        totalSuperficies = superficies.Count;


        for (int s = 0; s < superficies.Count; s++)
        {

            for (int c = 0; c < superficies[s].chunksList.Count; c++) 
            {

                if (superficies[s].chunksList[c].atualizar) 
                {
                    superficies[s].chunksList[c].GerarMesh();
                    superficies[s].chunksList[c].AtualizarMesh();
                    superficies[s].chunksList[c].atualizar = false;
                    break;
                }
            
            }

        }


        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("Alterando Chunk da Superficie: " + ChunkSelecionada);

            foreach (var chunk in superficies)
            {
                for (int c = 0; c < chunk.chunksList.Count; c++)
                {
                    if (chunk.chunksList[c].posicao == ChunkSelecionada)
                    {
                        Debug.Log("Chunk da Superficie encontrada: " + ChunkSelecionada);

                        for (int x = 0; x < 16; x++)
                        {
                            for (int z = 0; z < 16; z++)
                            {
                                for (int y = 0; y < 16; y++)
                                {
                                    chunk.chunksList[c].blocos[x, y, z] = 2;
                                }
                            }

                        }

                        chunk.chunksList[c].GerarMesh();
                        chunk.chunksList[c].AtualizarMesh();
                        break;
                    }
                }

            }


        }

    }


}
