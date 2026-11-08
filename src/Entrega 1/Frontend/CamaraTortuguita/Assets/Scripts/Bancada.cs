using UnityEngine;

public class Bancada : MonoBehaviour
{
    [Header("Peças já colocadas")]
    public bool corpoColocado;
    public bool cabecaColocada;
    public bool pataColocada;
    public bool pata1Colocada;
    public bool pernaColocada;
    public bool perna1Colocada;

    [Header("Partes da tartaruga (desativadas)")]
    public GameObject corpo, cabeca, pata, pata1, perna, perna1;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter: " + other.gameObject.name);

        if (other.CompareTag("Corpo") && !corpoColocado)
        {
            corpoColocado = true;
            corpo.SetActive(true);
        }
        else if (other.CompareTag("Cabeca") && !cabecaColocada)
        {
            cabecaColocada = true;
            cabeca.SetActive(true);
        }
        else if (other.CompareTag("Pata") && !pataColocada)
        {
            pataColocada = true;
            pata1.SetActive(true);
        }
        else if (other.CompareTag("Pata1") && !pata1Colocada)
        {
            pataColocada = true;
            pata1.SetActive(true);
        }
        else if (other.CompareTag("Perna") && !pernaColocada)
        {
            pernaColocada = true;
            perna1.SetActive(true);
        }
        else if (other.CompareTag("Perna1") && !perna1Colocada)
        {
            pernaColocada = true;
            perna1.SetActive(true);
        }
        
    }

    void VerificarCompleto()
    {
        if (corpoColocado && cabecaColocada && pataColocada && pata1Colocada &&
            pernaColocada && perna1Colocada)
        {
            Debug.Log("Tartaruga completa! Você venceu!");
            // aqui: tocar som, mostrar UI de vitória, etc.
        }
    }
}

