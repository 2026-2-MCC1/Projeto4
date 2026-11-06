using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a Fase 1: tempo, pistas, painel de código, vitória e derrota.
public class GerenciadorFase : MonoBehaviour
{
    // referências (arrastadas na cena)
    public Jogador jogador;
    public Painel painel;
    public BlockMan blockMan;
    public Renderer telaPainel;
    public Light[] luzes;
    public Renderer[] janelas;

    public float tempoTotal = 120f;

    // usado pela introdução da história: enquanto for true, o tempo não corre
    public bool emIntroducao = false;

    float tempoRestante;
    string[] pistas = new string[4];
    int pistasEncontradas = 0;

    bool painelAberto = false;
    bool venceu = false;
    bool perdeu = false;
    int pontos = 0;
    float tempoGasto = 0f;
    float melhorTempo = 0f;

    string digitado = "";
    string msgPainel = "";
    float tempoMsgPainel = 0f;

    string aviso = "";
    float tempoAviso = 0f;

    bool deuDica1 = false;
    bool deuDica2 = false;

    // as luzes começam fracas (apagão) e acendem quando vence
    float brilho = 0.3f;
    public float[] intensidadeOriginal;

    void Awake()
    {
        tempoRestante = tempoTotal;

        intensidadeOriginal = new float[luzes.Length];
        for (int i = 0; i < luzes.Length; i++)
        {
            intensidadeOriginal[i] = luzes[i].intensity;
            luzes[i].intensity = intensidadeOriginal[i] * brilho;
        }
    }

    void Start()
    {
        if (emIntroducao == false)
        {
            ComecarJogo();
        }
    }

    // chamado no começo assim que a introdução terminar
    public void ComecarJogo()
    {
        emIntroducao = false;
        blockMan.Falar("Olá, técnico Fúlvio! Um apagão travou a fábrica. Ache os bilhetes com as pistas e digite o código no painel antes que o tempo acabe!", 9f);
    }

    void Update()
    {
        if (emIntroducao == true)
        {
            return;
        }

        if (venceu == false && perdeu == false)
        {
            tempoRestante = tempoRestante - Time.deltaTime;

            if (tempoRestante <= 0)
            {
                tempoRestante = 0;
                Perder();
            }

            // dicas do Block Man
            if (tempoRestante <= 90 && deuDica1 == false)
            {
                deuDica1 = true;
                if (pistasEncontradas < 2)
                {
                    blockMan.Falar("Dica: procure bilhetes em cima das mesas, da caixa e da esteira. Cada um revela um dígito do código.", 7f);
                }
            }
            if (tempoRestante <= 45 && deuDica2 == false)
            {
                deuDica2 = true;
                blockMan.Falar("Faltam 45 segundos! Cada dígito depende do anterior, use as pistas na ordem.", 7f);
            }
        }

        if (painelAberto == true)
        {
            DigitarNoPainel();
        }

        if (venceu == true)
        {
            AcenderLuzes();
        }

        if ((venceu == true || perdeu == true) && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    // ---------- pistas ----------

    public void LerPista(Pista pista)
    {
        int posicao = pista.numero - 1;

        if (pistas[posicao] == null)
        {
            pistas[posicao] = pista.texto;
            pistasEncontradas = pistasEncontradas + 1;
            MostrarAviso("Pista " + pista.numero + " encontrada!");

            if (pistasEncontradas == 4)
            {
                blockMan.Falar("Você achou todas as pistas! Corra até o painel de controle e digite o código.", 7f);
            }
            else if (pistasEncontradas == 1)
            {
                blockMan.Falar("Boa! Continue procurando os outros bilhetes pela sala.", 5f);
            }
        }
        else
        {
            MostrarAviso("Pista " + pista.numero + ": " + pista.texto);
        }
    }

    void MostrarAviso(string texto)
    {
        aviso = texto;
        tempoAviso = Time.time + 3f;
    }

    // ---------- painel ----------

    public void AbrirPainel()
    {
        painelAberto = true;
        digitado = "";
        msgPainel = "";
        jogador.podeMover = false;
        jogador.TravarMouse(false);
    }

    void FecharPainel()
    {
        painelAberto = false;
        jogador.podeMover = true;
        jogador.TravarMouse(true);
    }

    void DigitarNoPainel()
    {
        // também dá para digitar no teclado
        foreach (char c in Input.inputString)
        {
            if (c >= '0' && c <= '9')
            {
                AdicionarDigito(c.ToString());
            }
            else if (c == '\b' && digitado.Length > 0)
            {
                digitado = digitado.Substring(0, digitado.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                Confirmar();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FecharPainel();
        }
    }

    void AdicionarDigito(string digito)
    {
        if (digitado.Length < painel.codigoCorreto.Length)
        {
            digitado = digitado + digito;
        }
    }

    void Confirmar()
    {
        if (digitado == painel.codigoCorreto)
        {
            Vencer();
        }
        else
        {
            digitado = "";
            msgPainel = "Código incorreto!";
            tempoMsgPainel = Time.time + 1.5f;
            blockMan.Falar("Código incorreto. Releia as pistas com calma!", 5f);
        }
    }

    // ---------- fim de fase ----------

    void Vencer()
    {
        venceu = true;
        painelAberto = false;
        painel.resolvido = true;
        pontos = Mathf.RoundToInt(tempoRestante * 10);

        // guarda o melhor tempo
        tempoGasto = tempoTotal - tempoRestante;
        melhorTempo = PlayerPrefs.GetFloat("MelhorTempoFase1", 9999f);
        if (tempoGasto < melhorTempo)
        {
            melhorTempo = tempoGasto;
            PlayerPrefs.SetFloat("MelhorTempoFase1", melhorTempo);
            PlayerPrefs.Save();
        }

        jogador.podeMover = false;
        jogador.TravarMouse(false);

        // a fábrica "amanhece"
        telaPainel.material.color = new Color(0.2f, 1f, 0.5f);
        for (int i = 0; i < janelas.Length; i++)
        {
            janelas[i].material.color = new Color(0.75f, 0.88f, 1f);
        }
        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);

        blockMan.Falar("Fábrica reativada! Na próxima parada, procure a fase bônus Central Bon o Bon para ganhar a Bala 7 Belo.", 10f);
    }

    void Perder()
    {
        perdeu = true;
        painelAberto = false;
        jogador.podeMover = false;
        jogador.TravarMouse(false);
        blockMan.Falar("O tempo acabou... Tente de novo!", 6f);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
