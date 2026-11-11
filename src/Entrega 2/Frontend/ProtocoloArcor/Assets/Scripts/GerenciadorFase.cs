
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    // ---------- referências do mundo ----------
    public Jogador jogador;
    public Painel painel;
    public BlockMan blockMan;
    public Renderer telaPainel;
    public Light[] luzes;
    public Renderer[] janelas;

    // ---------- referências da interface ----------
    public GameObject hud;
    public TMP_Text textoRelogio;
    public TMP_Text textoTituloPistas;
    public TMP_Text[] textosPistas;
    public TMP_Text textoInteracao;
    public GameObject caixaInteracao;
    public TMP_Text textoAviso;
    public GameObject caixaAviso;

    public GameObject telaPainelCodigo;
    public TMP_Text textoVisor;
    public TMP_Text textoMensagemPainel;

    public GameObject telaVitoria;
    public TMP_Text textoResultado;
    public GameObject botaoProximaFase;

    public GameObject telaDerrota;

    // ---------- configurações ----------
    public float tempoTotal = 120f;

    // Nome exato da cena da Zona Plutônica
    public string proximaCena = "Fase 2 Plutônica";

    // Fase bônus
    public float tempoParaBonus = 60f;
    public string cenaBonus = "Fase_CentralBonOBon";

    public AudioClip somVitoria;
    public AudioClip somDerrota;

    // ---------- estado do jogo ----------
    float tempoRestante;
    string[] pistas = new string[5];
    int pistasEncontradas = 0;

    bool painelAberto = false;
    bool venceu = false;
    bool ganhouBonus = false;
    bool perdeu = false;

    int pontos = 0;
    float tempoGasto = 0f;
    float melhorTempo = 0f;

    string digitado = "";
    float tempoMsgPainel = 0f;
    float tempoAviso = 0f;

    bool deuDica1 = false;
    bool deuDica2 = false;

    // ---------- iluminação ----------
    float brilho = 0.3f;
    float[] intensidadeOriginal;

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
        EstadoJogo.NovoJogo();

        telaPainelCodigo.SetActive(false);
        telaVitoria.SetActive(false);
        telaDerrota.SetActive(false);
        caixaAviso.SetActive(false);

        textoMensagemPainel.text = "";

        textoTituloPistas.text = "Pistas 0/5";

        for (int i = 0; i < textosPistas.Length; i++)
        {
            textosPistas[i].text = (i + 1) + ". ???";
        }

        blockMan.Falar(
            "Olá, técnico Fúlvio! Um apagão travou a fábrica. Ache os bilhetes com as pistas e digite o código no painel antes que o tempo acabe!",
            9f
        );
    }

    void Update()
    {
        if (venceu == false && perdeu == false)
        {
            tempoRestante -= Time.deltaTime;

            if (tempoRestante <= 0)
            {
                tempoRestante = 0;
                Perder();
            }

            DarDicas();
        }

        if (painelAberto == true)
        {
            DigitarNoPainel();
        }

        if (venceu == true)
        {
            AcenderLuzes();
        }

        if ((venceu == true || perdeu == true) &&
            Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }

        AtualizarInterface();
    }

    // ---------- dicas ----------
    void DarDicas()
    {
        if (tempoRestante <= 90 && deuDica1 == false)
        {
            deuDica1 = true;

            if (pistasEncontradas < 2)
            {
                blockMan.Falar(
                    "Dica: procure balas 7 belo em cima das máquinas, caixas, esteira e ao redor do mapa. Cada um revela um dígito do código.",
                    7f
                );
            }
        }

        if (tempoRestante <= 45 && deuDica2 == false)
        {
            deuDica2 = true;

            blockMan.Falar(
                "Faltam 45 segundos! Cada dígito depende do anterior, use as pistas na ordem.",
                7f
            );
        }
    }

    // ---------- interface ----------
    void AtualizarInterface()
    {
        int segundos = Mathf.CeilToInt(tempoRestante);

        textoRelogio.text =
            (segundos / 60).ToString("00") + ":" +
            (segundos % 60).ToString("00");

        hud.SetActive(venceu == false && perdeu == false);

        bool mostrarInteracao =
            painelAberto == false &&
            jogador.textoInteracao != "";

        caixaInteracao.SetActive(mostrarInteracao);

        if (mostrarInteracao == true)
        {
            textoInteracao.text = jogador.textoInteracao;
        }

        caixaAviso.SetActive(Time.time < tempoAviso);

        if (Time.time >= tempoMsgPainel)
        {
            textoMensagemPainel.text = "";
        }
    }

    void MostrarAviso(string texto)
    {
        textoAviso.text = texto;
        tempoAviso = Time.time + 3f;
    }

    // ---------- pistas ----------
    public void LerPista(Pista pista)
    {
        int posicao = pista.numero - 1;

        if (pistas[posicao] == null)
        {
            pistas[posicao] = pista.texto;
            pistasEncontradas++;

            textosPistas[posicao].text =
                pista.numero + ". " + pista.texto;

            textoTituloPistas.text =
                "Pistas " + pistasEncontradas + "/5";

            MostrarAviso("Pista " + pista.numero + " encontrada!");

            if (pistasEncontradas == 5)
            {
                blockMan.Falar(
                    "Você achou todas as pistas! Corra até o painel de controle e digite o código.",
                    7f
                );
            }
            else if (pistasEncontradas == 1)
            {
                blockMan.Falar(
                    "Boa! Continue procurando os outros bilhetes pela sala.",
                    5f
                );
            }
        }
        else
        {
            MostrarAviso(
                "Pista " + pista.numero + ": " + pista.texto
            );
        }
    }

    // ---------- painel de código ----------
    public void AbrirPainel()
    {
        painelAberto = true;
        digitado = "";

        AtualizarVisor();

        telaPainelCodigo.SetActive(true);

        jogador.podeMover = false;
        jogador.TravarMouse(false);
    }

    public void FecharPainel()
    {
        painelAberto = false;

        telaPainelCodigo.SetActive(false);

        jogador.podeMover = true;
        jogador.TravarMouse(true);
    }

    public void ApertarTecla(string tecla)
    {
        if (tecla == "C")
        {
            digitado = "";
        }
        else if (tecla == "OK")
        {
            Confirmar();
        }
        else
        {
            AdicionarDigito(tecla);
        }

        AtualizarVisor();
    }

    void DigitarNoPainel()
    {
        foreach (char c in Input.inputString)
        {
            if (c >= '0' && c <= '9')
            {
                AdicionarDigito(c.ToString());
            }
            else if (c == '\b' && digitado.Length > 0)
            {
                digitado = digitado.Substring(
                    0,
                    digitado.Length - 1
                );
            }
            else if (c == '\n' || c == '\r')
            {
                Confirmar();
            }
        }

        AtualizarVisor();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FecharPainel();
        }
    }

    void AdicionarDigito(string digito)
    {
        if (digitado.Length < painel.codigoCorreto.Length)
        {
            digitado += digito;
        }
    }

    void AtualizarVisor()
    {
        string visor = "";

        for (int i = 0; i < painel.codigoCorreto.Length; i++)
        {
            if (i < digitado.Length)
            {
                visor += digitado[i] + " ";
            }
            else
            {
                visor += "_ ";
            }
        }

        textoVisor.text = visor;
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

            textoMensagemPainel.text = "Código incorreto!";
            tempoMsgPainel = Time.time + 1.5f;

            blockMan.Falar(
                "Código incorreto. Releia as pistas com calma!",
                5f
            );
        }
    }

    // ---------- fim de fase ----------
    void Vencer()
    {
        venceu = true;

        GerenciadorAudio.ParaMusica();
        GerenciadorAudio.Efeito(somVitoria);

        painelAberto = false;
        painel.resolvido = true;

        pontos = Mathf.RoundToInt(tempoRestante * 10);

        tempoGasto = tempoTotal - tempoRestante;

        // Libera bônus somente se concluir em até 60 segundos
        ganhouBonus = tempoGasto <= tempoParaBonus;

        melhorTempo = PlayerPrefs.GetFloat(
            "MelhorTempoFase1",
            9999f
        );

        if (tempoGasto < melhorTempo)
        {
            melhorTempo = tempoGasto;

            PlayerPrefs.SetFloat(
                "MelhorTempoFase1",
                melhorTempo
            );

            PlayerPrefs.Save();
        }

        jogador.podeMover = false;
        jogador.TravarMouse(false);

        telaPainel.material.color =
            new Color(0.2f, 1f, 0.5f);

        for (int i = 0; i < janelas.Length; i++)
        {
            janelas[i].material.color =
                new Color(0.75f, 0.88f, 1f);
        }

        RenderSettings.ambientLight =
            new Color(0.5f, 0.5f, 0.55f);

        telaPainelCodigo.SetActive(false);

        string extra = "";

        if (ganhouBonus)
        {
            extra = "\nTempo recorde! Fase bônus liberada!";
        }

        textoResultado.text =
            "Seu tempo: " + tempoGasto.ToString("0.0") + " s\n" +
            "Melhor tempo: " + melhorTempo.ToString("0.0") + " s\n" +
            "Pontuação: " + pontos + extra;

        botaoProximaFase.SetActive(true);
        telaVitoria.SetActive(true);

        if (ganhouBonus)
        {
            blockMan.Falar(
                "Fábrica reativada em tempo recorde! Você desbloqueou a fase bônus Central Bon o Bon!",
                10f
            );
        }
        else
        {
            blockMan.Falar(
                "Fábrica reativada! Siga para a Zona Plutônica.",
                10f
            );
        }
    }

    void Perder()
    {
        perdeu = true;

        GerenciadorAudio.ParaMusica();
        GerenciadorAudio.Efeito(somDerrota);

        painelAberto = false;

        jogador.podeMover = false;
        jogador.TravarMouse(false);

        telaPainelCodigo.SetActive(false);
        telaDerrota.SetActive(true);

        blockMan.Falar(
            "O tempo acabou... Tente de novo!",
            6f
        );
    }

    void AcenderLuzes()
    {
        brilho += Time.deltaTime / 3f;

        if (brilho > 1f)
        {
            brilho = 1f;
        }

        for (int i = 0; i < luzes.Length; i++)
        {
            luzes[i].intensity =
                intensidadeOriginal[i] * brilho;
        }
    }

    // ---------- botões das telas de fim ----------
    public void Reiniciar()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void IrParaProximaFase()
    {
        if (ganhouBonus)
        {
            SceneManager.LoadScene(cenaBonus);
        }
        else
        {
            SceneManager.LoadScene(proximaCena);
        }
    }
}
