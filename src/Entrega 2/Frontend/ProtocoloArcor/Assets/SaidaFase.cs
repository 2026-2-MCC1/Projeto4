using UnityEngine;

public class SaidaFase : MonoBehaviour
{
    public GameObject textoConclusao;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textoConclusao.SetActive(true);
            Debug.Log("FASE CONCLUÍDA!");
        }
    }
}