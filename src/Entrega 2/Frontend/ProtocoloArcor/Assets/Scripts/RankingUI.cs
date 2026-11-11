using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;
using System.Text;

public class RankingUI : MonoBehaviour
{
    public TMP_InputField campoNome;
    public TMP_Text textoRanking;

    [Tooltip("Cena carregada depois que o jogador registra o nome")]
    public string proximaCena = "Encerramento";

    [Tooltip("Segundos mostrando o ranking antes de ir sozinho para a próxima cena")]
    public float tempoMostrandoRanking = 8f;

    private float tempoFinal;
    private bool registrado = false;

    // Recebe o tempo total da partida
    public void DefinirTempoFinal(float tempo)
    {
        tempoFinal = tempo;
    }

    // Soma o tempo de todas as fases concluídas (os mesmos tempos da tela de encerramento)
    float CalcularTempoTotal()
    {
        float total = 0f;
        if (Tempos.passouFase1) total += Tempos.tempoFase1;
        if (Tempos.passouFase2) total += Tempos.tempoFase2;
        if (Tempos.passouFase3) total += Tempos.tempoFase3;
        if (Tempos.passouFase4) total += Tempos.tempoFase4;
        return total;
    }

    // Registra o jogador no ranking
    public void RegistrarResultado()
    {
        if (registrado) return;

        string nome = campoNome.text.Trim();

        if (string.IsNullOrEmpty(nome))
        {
            textoRanking.text = "Digite seu nome!";
            return;
        }

        registrado = true;

        RankingManager.RegistrarJogador(nome, tempoFinal);

        campoNome.interactable = false;

        AtualizarRanking();

        textoRanking.text += "\nAperte Espaço para continuar";

        StartCoroutine(IrParaEncerramento());
    }

    IEnumerator IrParaEncerramento()
    {
        float inicio = Time.time;
        yield return null; // espera um frame pra não pegar o mesmo clique/tecla

        while (Time.time - inicio < tempoMostrandoRanking)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                break;
            yield return null;
        }

        SceneManager.LoadScene(proximaCena);
    }

    // Exibe os 10 melhores jogadores
    public void AtualizarRanking()
    {
        if (textoRanking == null)
        {
            Debug.LogError("TextoRanking não foi atribuído!");
            return;
        }

        var jogadores = RankingManager.CarregarRanking();

        if (jogadores == null)
        {
            Debug.LogError("Ranking não foi carregado!");
            return;
        }

        StringBuilder texto = new StringBuilder();

        texto.AppendLine("RANKING PROJETO ARCOR");
        texto.AppendLine();

        int limite = Mathf.Min(10, jogadores.Count);

        for (int i = 0; i < limite; i++)
        {
            float tempo = jogadores[i].tempo;

            int minutos = Mathf.FloorToInt(tempo / 60);
            int segundos = Mathf.FloorToInt(tempo % 60);

            string tempoFormatado = minutos.ToString("00") + ":" +
                                    segundos.ToString("00");

            texto.AppendLine(
                (i + 1) + " - " +
                jogadores[i].nome.PadRight(25) +
                tempoFormatado
            );
        }
        textoRanking.text = texto.ToString();
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        tempoFinal = CalcularTempoTotal();
        AtualizarRanking();
    }
}
