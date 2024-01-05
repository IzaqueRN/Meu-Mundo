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

    public Vector3Int posicao = Vector3Int.zero;

    List<Vector3> VerticesMesh = new List<Vector3>();
    List<Vector2> UvsMesh = new List<Vector2>();
    List<int> TriangulosMesh = new List<int>();
    public Mesh mesh;
    public GameObject corpo;

    float offsetUv = 1f/16f;
    float escala = 0.5f;


    public bool atualizar = false;
    public bool preenchida = false;

    public void AtualizarMesh()
    {
        mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = VerticesMesh.ToArray();
        mesh.triangles = TriangulosMesh.ToArray();
        mesh.uv = UvsMesh.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        corpo.transform.name = "Chunk (" + posicao.x + "," + posicao.y + "," + posicao.z + ")";
        mesh.name = "Mesh Chunk (" + posicao.x + "," + posicao.y + "," + posicao.z + ")";

        corpo.transform.position = new Vector3(posicao.x * 16, posicao.y * 16, posicao.z * 16);
        corpo.GetComponent<MeshFilter>().mesh = mesh;
       //Material mat = new Material(Shader.Find("Standard"));
       // mat.name = "Material Test";

        corpo.GetComponent<MeshRenderer>().material = Resources.Load("Texturas/Materials/texture", typeof(Material)) as Material;
    }

    public void InstanciarChunk(Transform mundo) {

        corpo = new GameObject();
        corpo.transform.parent = mundo;
        corpo.AddComponent<MeshFilter>();
        corpo.AddComponent<MeshRenderer>();

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

                                idUV = new Vector2(0, 0); // badrock
                                break;
                            case 1:
                                idUV = new Vector2(3, 15); // pedra
                                break;
                            case 2:
                                idUV = new Vector2(2, 15); // areia
                                break;
                            case 3:
                                idUV = new Vector2(0, 15); // grama
                                break;
                            case 4:
                                idUV = new Vector2(1, 15); // agua
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
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, -escala + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, -escala + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, escala + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, escala + z)); // Vertice 3

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

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior


    }
    public void GerarFaceCima(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, -escala + z)); // Vertice 4
        VerticesMesh.Add(new Vector3(escala + x, escala + y, -escala + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(escala + x, escala + y, escala + z)); // Vertice 6
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, escala + z)); // Vertice 7


        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2);
        TriangulosMesh.Add(VerticesMesh.Count - 4); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior

    }

    public void GerarFaceEsquerda(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, escala + z)); // Vertice 7
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, escala + z)); // Vertice 3
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, -escala + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, -escala + z)); // Vertice 4

        // 0,1,2
        // 0,2,3
        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior

    }

    public void GerarFaceDireita(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(escala + x, escala + y, -escala + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, -escala + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, escala + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(escala + x, escala + y, escala + z)); // Vertice 6

        // 0,1,2
        // 0,2,3
        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior

    }

    public void GerarFaceFrente(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, escala + z)); // Vertice 2
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, escala + z)); // Vertice 3
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, escala + z)); // Vertice 7
        VerticesMesh.Add(new Vector3(escala + x, escala + y, escala + z)); // Vertice 6

        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior

    }
    public void GerarFaceTras(int x, int y, int z, Vector2 idUV)
    {
        VerticesMesh.Add(new Vector3(-escala + x, -escala + y, -escala + z)); // Vertice 0
        VerticesMesh.Add(new Vector3(escala + x, -escala + y, -escala + z)); // Vertice 1
        VerticesMesh.Add(new Vector3(escala + x, escala + y, -escala + z)); // Vertice 5
        VerticesMesh.Add(new Vector3(-escala + x, escala + y, -escala + z)); // Vertice 4

        TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 3); TriangulosMesh.Add(VerticesMesh.Count - 4);
        TriangulosMesh.Add(VerticesMesh.Count - 1); TriangulosMesh.Add(VerticesMesh.Count - 2); TriangulosMesh.Add(VerticesMesh.Count - 4);

        UvsMesh.Add(new Vector2(offsetUv * idUV.x, (offsetUv * idUV.y) + offsetUv) + new Vector2(0.009f, -0.009f)); // quina esquerda inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, (offsetUv * idUV.y) + offsetUv) + new Vector2(-0.009f, -0.009f));  // quina direita inferior
        UvsMesh.Add(new Vector2((offsetUv * idUV.x) + offsetUv, offsetUv * idUV.y) + new Vector2(-0.009f, 0.009f)); // ?
        UvsMesh.Add(new Vector2(offsetUv * idUV.x, offsetUv * idUV.y) + new Vector2(0.009f, 0.009f)); // quina direita superior

    }




}
