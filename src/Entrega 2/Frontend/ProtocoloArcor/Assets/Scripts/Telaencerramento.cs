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
    public string cenaPrimeiraFase = "Estacao7Belo";

    void Start()
    {
        Time.timeScale = 1f;                    // garante que o jogo não está pausado
        Cursor.visible = true;                  // mostra o mouse
        Cursor.lockState = CursorLockMode.None;

        float total = 0f;

        // Fase 1
        if (Tempos.passouFase1)
        {
            textoFase1.text = "Fase 1: " + Formatar(Tempos.tempoFase1);
            total += Tempos.tempoFase1;
        }
        else
        {
            textoFase1.text = "Fase 1: não concluída";
        }

        // Fase 2
        if (Tempos.passouFase2)
        {
            textoFase2.text = "Fase 2: " + Formatar(Tempos.tempoFase2);
            total += Tempos.tempoFase2;
        }
        else
        {
            textoFase2.text = "Fase 2: não concluída";
        }

        // Fase 3 (Central Bon o Bon)
        if (Tempos.passouFase3)
        {
            textoFase3.text = "Fase 3: " + Formatar(Tempos.tempoFase3);
            total += Tempos.tempoFase3;
        }
        else
        {
            textoFase3.text = "Fase 3: não concluída";
        }

        // Fase 4
        if (Tempos.passouFase4)
        {
            textoFase4.text = "Fase 4: " + Formatar(Tempos.tempoFase4);
            total += Tempos.tempoFase4;
        }
        else
        {
            textoFase4.text = "Fase 4: não concluída";
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