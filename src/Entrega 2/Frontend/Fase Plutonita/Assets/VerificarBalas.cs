using UnityEngine;

public class VerificarBalas : MonoBehaviour
{
    [Tooltip("Arraste aqui as 4 balas da fase")]
    public GameObject[] balas;

    bool terminou = false;

    void Start()
    {
        Debug.Log("VerificarBalas: monitorando " + balas.Length + " balas");
    }

    void Update()
    {
        if (terminou || balas.Length == 0) return;

        // Conta quantas balas ainda estão na cena
        int restantes = 0;
        foreach (var bala in balas)
        {
            if (bala != null && bala.activeInHierarchy)
                restantes++;
        }

        if (restantes == 0)
        {
            terminou = true;

            if (GerenciadorFase.Instancia != null)
                GerenciadorFase.Instancia.Vitoria();
            else
                Debug.LogWarning("VerificarBalas: não há GerenciadorFase na cena.");
        }
    }
}