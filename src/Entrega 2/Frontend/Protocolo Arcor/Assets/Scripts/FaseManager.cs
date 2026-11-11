using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseManager : MonoBehaviour
{
    [SerializeField] private TimerManagement timer;

    [Tooltip("Cena carregada quando a Tortuguita fica montada")]
    public string proximaCena = "RankingFinal";

    [Tooltip("Segundos de espera antes de trocar de cena (pra ver a Tortuguita montada)")]
    public float esperaAntesDeTrocar = 2f;

    bool concluida = false;

    public void ConcluirFase()
    {
        if (concluida) return;
        concluida = true;

        float tempoFase = 0f;

        if (timer != null)
        {
            // Impede que o cronômetro continue
            timer.StopTimer();

            // Obtém o tempo decorrido
            tempoFase = timer.GetElapsedTime();
        }

        // Registra o tempo da fase
        RankingManager.RegistrarTempoFase(tempoFase);

        // tela de encerramento: Câmara da Tartaruga concluída
        Tempos.passouFase4 = true;
        Tempos.tempoFase4 = tempoFase;

        Debug.Log("Tempo da fase: " + tempoFase);

        // Carrega a próxima cena (o ranking)
        StartCoroutine(TrocarDeCena());
    }

    IEnumerator TrocarDeCena()
    {
        yield return new WaitForSecondsRealtime(esperaAntesDeTrocar);

        // Libera o mouse pra digitar o nome no ranking
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(proximaCena);
    }
}
