using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetivoFase : MonoBehaviour
{
    [SerializeField] private TMP_Text textoObjetivo;

    [SerializeField] private int quantidadePartesnaFase = 7;
    private int totalPecas;
    private int pecasEncontradas;
    private int pecasMontadas;

    void Start()
    {
        ConfigurarFase(quantidadePartesnaFase);
    }
    // Define quantas peças existem na fase
    public void ConfigurarFase(int quantidadePecas)
    {
        totalPecas = quantidadePecas;
        pecasEncontradas = 0;
        pecasMontadas = 0;

        AtualizarObjetivo();
    }

    // Chamado quando uma peça é coletada
    public void RegistrarPecaEncontrada()
    {
        pecasEncontradas++;

        AtualizarObjetivo();
    }

    // Chamado quando o jogador começa a montagem
    public void IniciarMontagem()
    {
        textoObjetivo.text =
            "OBJETIVO DA FASE\n\n" +
            "Encaixe todas as partes da Tortuguita " +
            "corretamente na bancada!";
    }

    // Chamado quando uma peça é encaixada
    public void RegistrarPecaMontada()
    {
        pecasMontadas++;

        if (pecasMontadas >= totalPecas)
        {
            ConcluirFase();
        }
        else
        {
            textoObjetivo.text =
                "OBJETIVO DA FASE\n\n" +
                "Tortuguita: " + pecasMontadas +
                "/" + totalPecas +
                " partes montadas.\n\n" +
                "Continue encaixando as peças!";
        }
    }

    // Atualiza o objetivo de coleta
    private void AtualizarObjetivo()
    {
        if (pecasEncontradas < totalPecas)
        {
            textoObjetivo.text =
                "OBJETIVO DA FASE\n\n" +
                "Encontre todas as partes da Tortuguita!\n\n" +
                "Peças encontradas: " +
                pecasEncontradas + "/" + totalPecas;
        }
        else
        {
            textoObjetivo.text =
                "OBJETIVO DA FASE\n\n" +
                "Todas as peças foram encontradas!\n\n" +
                "Vá até a bancada e monte a Tortuguita.";
        }
    }

    // Mensagem final
    public void ConcluirFase()
    {
        textoObjetivo.text =
            "PARABÉNS!\n\n" +
            "Você montou a Tortuguita corretamente!\n\n" +
            "FASE CONCLUÍDA!";

        // Avisa o FaseManager: ele para o timer, registra o tempo e vai pro ranking
        FaseManager faseManager = FindFirstObjectByType<FaseManager>();
        if (faseManager != null)
        {
            faseManager.ConcluirFase();
        }
        else
        {
            // Se não tiver FaseManager na cena, vai direto pro ranking
            StartCoroutine(IrParaRanking());
        }
    }

    IEnumerator IrParaRanking()
    {
        yield return new WaitForSecondsRealtime(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("RankingFinal");
    }
}