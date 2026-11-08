
using UnityEngine;
using System.Collections.Generic;

public class GeradorLabirinto : MonoBehaviour
{
    public int colunas = 5;
    public int linhas = 5;
    public float tamanhoCelula = 2f;
    public float alturaParede = 3f;
    public float espessuraParede = 0.2f;

    private bool[,] visitado;
    private bool[,,] paredes;

    void Start()
    {
        GerarLabirinto();
    }

    void GerarLabirinto()
    {
        visitado = new bool[colunas, linhas];
        paredes = new bool[colunas, linhas, 4];

        for (int x = 0; x < colunas; x++)
        {
            for (int z = 0; z < linhas; z++)
            {
                for (int d = 0; d < 4; d++)
                    paredes[x, z, d] = true;
            }
        }

        CriarCaminhos(2, 0);

        for (int x = 0; x < colunas; x++)
        {
            for (int z = 0; z < linhas; z++)
            {
                float centroX = (x - 2) * tamanhoCelula;
                float centroZ = (z - 2) * tamanhoCelula;

                if (paredes[x, z, 0])
                    CriarParede(centroX, centroZ + tamanhoCelula / 2, true);

                if (paredes[x, z, 1])
                    CriarParede(centroX + tamanhoCelula / 2, centroZ, false);
            }
        }
    }

    void CriarCaminhos(int x, int z)
    {
        visitado[x, z] = true;

        List<int> direcoes = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < direcoes.Count; i++)
        {
            int aleatorio = Random.Range(i, direcoes.Count);
            int temp = direcoes[i];
            direcoes[i] = direcoes[aleatorio];
            direcoes[aleatorio] = temp;
        }

        foreach (int d in direcoes)
        {
            int nx = x;
            int nz = z;

            if (d == 0) nz++;
            if (d == 1) nx++;
            if (d == 2) nz--;
            if (d == 3) nx--;

            if (nx < 0 || nx >= colunas ||
                nz < 0 || nz >= linhas)
                continue;

            if (visitado[nx, nz])
                continue;

            paredes[x, z, d] = false;
            paredes[nx, nz, (d + 2) % 4] = false;

            CriarCaminhos(nx, nz);
        }
    }

    void CriarParede(float x, float z, bool horizontal)
    {
        GameObject parede = GameObject.CreatePrimitive(PrimitiveType.Cube);

        parede.name = "ParedeGerada";
        parede.transform.SetParent(transform);

        parede.transform.position = new Vector3(x, alturaParede / 2, z);

        if (horizontal)
        {
            parede.transform.localScale = new Vector3(
                tamanhoCelula + espessuraParede,
                alturaParede,
                espessuraParede
            );
        }
        else
        {
            parede.transform.localScale = new Vector3(
                espessuraParede,
                alturaParede,
                tamanhoCelula + espessuraParede
            );
        }
    }
}
