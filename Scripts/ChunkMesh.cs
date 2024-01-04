using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

public class ChunkMesh : MonoBehaviour
{
    public int[,,] blocos = new int[16, 16, 16];
    public int[,] blocosAlturaSuperficie = new int[16, 16];
    public Vector3Int posicao = Vector3Int.zero;

    List<Vector3> VerticesMesh = new List<Vector3>();
    List<Vector2> UvsMesh = new List<Vector2>();
    List<int> TriangulosMesh = new List<int>();
    Mesh mesh;

    float offsetUv = 1/16f;

    public static float alturaMaxima = 10;
    public static float alturaMin = 0;

    public static float zoomX = 0.05f;
    public static float zoomY = 0.051f;

    public bool superficie = false;
    public bool atualizar = false;
    public bool preenchida = false;

    void Start()
    {
        transform.name = "Chunk (" +posicao.x+ ","+ posicao.y + ","+ posicao.z + ")";
        mesh = new Mesh();
        mesh.name = "Mesh Novo";
        gameObject.GetComponent<MeshFilter>().mesh = mesh;



         //preencher();
       // GerarSuperficie();
      //  GerarMesh();
    }

    public void AtualizarMesh()
    {
        mesh.Clear();
        mesh.vertices = VerticesMesh.ToArray();
        mesh.triangles = TriangulosMesh.ToArray();
        mesh.uv = UvsMesh.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }

    public void preencher()
    {


        for (int x = 0; x < blocosAlturaSuperficie.GetLongLength(0); x++)
        {
            for (int z = 0; z < blocosAlturaSuperficie.GetLongLength(1); z++)
            {
                for (int y = 0; y < blocosAlturaSuperficie[x, z]; y++)
                {
                    blocos[x, y, z] = 1;
                }

            }

        }

    }
    public void GerarSuperficie()
    {


        for (int x = 0; x < blocos.GetLongLength(0); x++)
        {
            for (int z = 0; z < blocos.GetLongLength(2); z++)
            {
                int altura = AlturaSuperficie((int)transform.position.x + x, (int)transform.position.z + z);

                if (altura <= 1)
                {
                    blocos[x, altura, z] = -1;
                }
                if (altura > 1)
                {

                    blocos[x, altura, z] = 1;
                }


                for (int y = altura; y < blocos.GetLongLength(1); y++)
                {

                    if (blocos[x, y, z] == 0 && y < 3)
                    {
                        blocos[x, y, z] = 2;
                    }

                }

            }


        }

    }

    public int AlturaSuperficie(int x,int z) {


        float altura = 16 * Mathf.PerlinNoise(x * zoomX, z * zoomY) ;

        int y = Mathf.FloorToInt(altura);

        if (y > 14)
            y = 15;


        return y;
    }
    public void GerarMesh()
    {
        VerticesMesh.Clear();
        UvsMesh.Clear();
        TriangulosMesh.Clear();
        Vector2 idUV = new Vector2(1,15);

        for (int x = 0; x < blocos.GetLongLength(0); x++)
        {
            for (int y = 0; y < blocos.GetLongLength(1); y++)
            {
                for (int z = 0; z < blocos.GetLongLength(2); z++)
                {

                    if (blocos[x, y, z] != 0)
                    {

                        switch (blocos[x, y, z])
                        {
                            case -1:

                                idUV = new Vector2(1, 14);
                                break;
                            case 1:
                                idUV = new Vector2(1, 15);
                                break;
                            case 2:
                                idUV = new Vector2(2, 15);
                                break;
                            case 3:
                                idUV = new Vector2(14, 15);
                                break;

                        }


                        // Face Baixo
                        if (y == 0)
                        {

                            GerarFaceBaixo(x, y, z, idUV);
                        }
                        else if (blocos[x, y - 1, z] == 0)
                        {
                            GerarFaceBaixo(x, y, z, idUV);
                        }
                        // Face Cima
                        if (y == blocos.GetLongLength(1) - 1)
                        {
                            GerarFaceCima(x, y, z, idUV);
                        }
                        else if (blocos[x, y + 1, z] == 0)
                        {
                            GerarFaceCima(x, y, z, idUV);
                        }

                        // Face Tras
                        if (z == 0)
                        {
                            GerarFaceTras(x, y, z, idUV);
                        }
                        else if (blocos[x, y , z - 1] == 0)
                        {
                            GerarFaceTras(x, y, z , idUV);
                        }

                        // Face Frente
                        if (z == blocos.GetLongLength(2) -1)
                        {
                            GerarFaceFrente(x, y, z , idUV);
                        }
                        else if (blocos[x, y, z + 1] == 0)
                        {
                            GerarFaceFrente(x, y, z , idUV);
                        }


                        // Face Direita
                        if (x == blocos.GetLongLength(0) - 1)
                        {
                            GerarFaceDireita(x, y, z , idUV);
                        }
                        else if (blocos[x + 1, y, z] == 0)
                        {
                            GerarFaceDireita(x, y, z , idUV);
                        }


                        // Face Esquerda
                        if (x == 0)
                        {
                            GerarFaceEsquerda(x, y, z , idUV);
                        }
                        else if (blocos[x - 1, y, z] == 0)
                        {
                            GerarFaceEsquerda(x, y, z , idUV);
                        }

                    }

                }

            }

        }

    }

    public void GerarFaceBaixo(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 3

        // 0,1,2
        // 0,2,3

        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 2);
        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 1);

        /*
        UvsMesh.Add(new Vector2(0, 1));
        UvsMesh.Add(new Vector2(1, 1));
        UvsMesh.Add(new Vector2(1, 0));
        UvsMesh.Add(new Vector2(0, 0));

        */

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));


    }
    public void GerarFaceCima(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 4
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 6
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 7


        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2);
        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));
    }

    public void GerarFaceEsquerda(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 7
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 3
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 4

        // 0,1,2
        // 0,2,3
        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));

    }

    public void GerarFaceDireita(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 6

        // 0,1,2
        // 0,2,3
        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);
       
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));

    }

    public void GerarFaceFrente(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, 0.5f + z)); // Vertice 3
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 7
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, 0.5f + z)); // Vertice 6

        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));

    }
    public void GerarFaceTras(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(0.5f + x, -0.5f + y, -0.5f + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(-0.5f + x, 0.5f + y, -0.5f + z)); // Vertice 4

        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv));
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y));
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y));

    }




}
