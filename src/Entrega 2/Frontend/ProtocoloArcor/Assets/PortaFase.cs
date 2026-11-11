using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaFase : MonoBehaviour
{
    [Tooltip("aaaaaaaa")]
    public string proximaCena = "Fase2";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(proximaCena);
        }
    }
}
