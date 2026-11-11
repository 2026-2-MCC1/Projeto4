using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Coloque este script em um objeto vazio da cena de encerramento.
public class TelaEncerramento : MonoBehaviour
{
    // Textos da tela (arraste cada um no Inspector)
    public TMP_Text textoFase1;
    public TMP_Text textoFase2;
    public TMP_Text textoFase3;
    public TMP_Text textoFase4;
    public TMP_Text textoTotal;

    // Nomes das cenas (confira se estão iguais aos do seu projeto)
    public string cenaMenu = "MainMenu";
    public string cenaPrimeiraFase = "Fase1_Estacao7Belo";

    void Start()
    {
        Time.timeScale = 1f;                    // garante que o jogo não está pausado
        Cursor.visible = true;                  // mostra o mouse
        Cursor.lockState = CursorLockMode.None;

        float total = 0f;

        // Fase 1: Estação 7 Belo
        if (Tempos.passouFase1)
        {
            textoFase1.text = "Estação 7 Belo: " + Formatar(Tempos.tempoFase1);
            total += Tempos.tempoFase1;
        }
        else
        {
            textoFase1.text = "Estação 7 Belo: não concluída";
        }

        // Fase 2: Zona Plutônica
        if (Tempos.passouFase2)
        {
            textoFase2.text = "Zona Plutônica: " + Formatar(Tempos.tempoFase2);
            total += Tempos.tempoFase2;
        }
        else
        {
            textoFase2.text = "Zona Plutônica: não concluída";
        }

        // Fase bônus: Central Bon o Bon (opcional)
        if (Tempos.passouFase3)
        {
            textoFase3.text = "Central Bon o Bon (bônus): " + Formatar(Tempos.tempoFase3);
            total += Tempos.tempoFase3;
        }
        else
        {
            textoFase3.text = "Central Bon o Bon (bônus): não jogada";
        }

        // Fase 3: Câmara da Tartaruga
        if (Tempos.passouFase4)
        {
            textoFase4.text = "Câmara da Tartaruga: " + Formatar(Tempos.tempoFase4);
            total += Tempos.tempoFase4;
        }
        else
        {
            textoFase4.text = "Câmara da Tartaruga: não concluída";
        }

        textoTotal.text = "Tempo total: " + Formatar(total);
    }

    // Transforma segundos em texto "minutos:segundos". Ex.: 75 -> "01:15"
    string Formatar(float segundos)
    {
        int minutos = (int)(segundos / 60);
        int seg = (int)(segundos % 60);
        return minutos.ToString("00") + ":" + seg.ToString("00");
    }

    // ---- Funções dos botões ----

    public void JogarNovamente()
    {
        Tempos.Zerar();
        EstadoJogo.NovoJogo();
        RankingManager.ReiniciarPartida();
        SceneManager.LoadScene(cenaPrimeiraFase);
    }

    public void VoltarAoMenu()
    {
        SceneManager.LoadScene(cenaMenu);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}