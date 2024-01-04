using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo: MonoBehaviour
{

   public GameObject ChunkPrefab;
   List<GameObject> ChunkList = new List<GameObject>();


    float alturaMaxima = 32;
    float alturaMin = 0;

    float zoomX = 0.0292f;
    float zoomY = 0.0291f;

    public bool atualizouChunks = true;
    public long totalBlocks = 0;
    public Vector3Int addNovoBloco = Vector3Int.zero;
    public Vector3Int posicaoBlocoMundo = Vector3Int.zero;
    public Vector3Int posicaoBlocoNaChunk = Vector3Int.zero;
    public Vector3Int posicaoChunkDoBloco = Vector3Int.zero;

    void Start()
    {

        GerarSuperficie(10,10);

    }


    void GerarSuperficie(int TamX,int TamZ) {

        Debug.LogWarning("Gerando Superficie... ");
        int alturaBlocoSuperficie = 0;
        long totalBlocos = 0;
        int id = 1;
        for (int x = 0; x < 16 * TamX; x++)
        {
            for (int z = 0; z < 16 * TamX; z++)
            {
                alturaBlocoSuperficie = AlturaSuperficie( x, z);
                
                
               
                if(alturaBlocoSuperficie == 0) { id = -1; }
                if (alturaBlocoSuperficie > 0) { id = 1; }
                if (alturaBlocoSuperficie > 10 && alturaBlocoSuperficie  < 30) { id = 2; }

                ADDBloco(new Vector3Int(x,alturaBlocoSuperficie,z),id);
                totalBlocos++;
                totalBlocks++;
            }

        }

        Debug.LogWarning("Superficie Criada, Total Blocos: " + totalBlocos);
    }




    void ADDBloco(Vector3Int posicao, int id)
    {
        int indexChunk = 0;
        bool achouChunk= false;
        Vector3Int posicaoBlocoNaChunk = Vector3Int.zero;
        Vector3Int posicaoChunkDoBloco = Vector3Int.zero;

        posicaoChunkDoBloco.x = posicao.x / 16;
        posicaoChunkDoBloco.y = posicao.y / 16;
        posicaoChunkDoBloco.z = posicao.z / 16;

        posicaoBlocoNaChunk.x = posicao.x % 16;
        posicaoBlocoNaChunk.y = posicao.y % 16;
        posicaoBlocoNaChunk.z = posicao.z % 16;

        if (ChunkList.Count > 0) {
            foreach (var chunk in ChunkList)
            {
                if (chunk.GetComponent<ChunkMesh>().posicao == posicaoChunkDoBloco) {
                    Debug.Log("Chunk: Encontrada! " + posicaoChunkDoBloco);
                    achouChunk = true;
                    break;
                }
                indexChunk++;
            }
        }

        if (achouChunk == true) 
        {
            ChunkList[indexChunk].GetComponent<ChunkMesh>().blocos[posicaoBlocoNaChunk.x, posicaoBlocoNaChunk.y, posicaoBlocoNaChunk.z] = id;
            ChunkList[indexChunk].GetComponent<ChunkMesh>().atualizar = true;
        }
        else
        {
            Debug.Log("Criando Chunk: " + posicaoChunkDoBloco);
            ChunkList.Add(Instantiate(ChunkPrefab, new Vector3(16 * posicaoChunkDoBloco.x, 16 * posicaoChunkDoBloco.y, 16 * posicaoChunkDoBloco.z), Quaternion.identity, transform));
            ChunkList[ChunkList.Count - 1].GetComponent<ChunkMesh>().posicao = posicaoChunkDoBloco;
            ChunkList[ChunkList.Count - 1].GetComponent<ChunkMesh>().atualizar = true;
            ChunkList[ChunkList.Count - 1].GetComponent<ChunkMesh>().blocos[posicaoBlocoNaChunk.x, posicaoBlocoNaChunk.y, posicaoBlocoNaChunk.z] = id;
        }

    }

    public int AlturaSuperficie(int x, int z)
    {

        float altura = alturaMaxima * Mathf.PerlinNoise(x * zoomX, z * zoomY);

        int y = Mathf.FloorToInt(altura);

        if (y < alturaMin)
            y = 0;
        return y;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("ADD Bloco...");
            ADDBloco(addNovoBloco, -1);

        }
        atualizouChunks = true;
        for (int c = 0; c < ChunkList.Count; c++)
        {

            if (ChunkList[c].GetComponent<ChunkMesh>().atualizar)
            {
              
                ChunkList[c].GetComponent<ChunkMesh>().GerarMesh();
                ChunkList[c].GetComponent<ChunkMesh>().AtualizarMesh();
                ChunkList[c].GetComponent<ChunkMesh>().atualizar = false;
                atualizouChunks = false;
                break;
            }

        }



    }

  
}
