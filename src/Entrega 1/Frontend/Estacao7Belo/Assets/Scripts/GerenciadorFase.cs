using UnityEngine;
using UnityEngine.SceneManagement;

// Gerencia a fase: tempo, pistas, painel de código, vitória e derrota.
public class GerenciadorFase : MonoBehaviour
{
    public static GerenciadorFase instancia;

    // referências (arrastadas na cena)
    public Jogador jogador;
    public Painel painel;
    public BlockMan blockMan;
    public Renderer telaPainel;
    public Light[] luzes;
    public Renderer[] janelas;

    public float tempoTotal = 120f;

    float tempoRestante;
    string[] pistas = new string[4];
    int pistasEncontradas = 0;

    bool painelAberto = false;
    bool venceu = false;
    bool perdeu = false;
    int pontos = 0;

    string digitado = "";
    string msgPainel = "";
    float tempoMsgPainel = 0f;

    string aviso = "";
    float tempoAviso = 0f;

    bool deuDica1 = false;
    bool deuDica2 = false;

    // para o efeito de "amanhecer" quando vence
    float brilho = 0.3f;
    float[] intensidadeOriginal;
    Color corNoite;
    Color corDia = new Color(0.75f, 0.88f, 1f);

    void Awake()
    {
        instancia = this;
        tempoRestante = tempoTotal;

        // começa com as luzes fracas (apagão)
        intensidadeOriginal = new float[luzes.Length];
        for (int i = 0; i < luzes.Length; i++)
        {
            intensidadeOriginal[i] = luzes[i].intensity;
            luzes[i].intensity = intensidadeOriginal[i] * brilho;
        }
        if (janelas.Length > 0)
        {
            corNoite = janelas[0].material.color;
        }
    }

    void Start()
    {
        blockMan.Falar("Olá, técnico Fúlvio! Um apagão travou a fábrica. Ache os bilhetes com as pistas e digite o código no painel antes que o tempo acabe!", 9f);
    }

    void Update()
    {
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
        // aceita digitar no teclado também
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

        jogador.podeMover = false;
        jogador.TravarMouse(false);

        telaPainel.material.color = new Color(0.2f, 1f, 0.5f);
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

    void AcenderLuzes()
    {
        // brilho vai de 0.3 até 1 em uns 3 segundos
        brilho = Mathf.MoveTowards(brilho, 1f, Time.deltaTime / 3f);
        float t = (brilho - 0.3f) / 0.7f;

        for (int i = 0; i < luzes.Length; i++)
        {
            luzes[i].intensity = intensidadeOriginal[i] * brilho;
        }
        for (int i = 0; i < janelas.Length; i++)
        {
            janelas[i].material.color = Color.Lerp(corNoite, corDia, t);
        }
        RenderSettings.ambientLight = Color.Lerp(new Color(0.03f, 0.03f, 0.05f), new Color(0.5f, 0.5f, 0.55f), t);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ---------- tela (HUD) ----------

    void OnGUI()
    {
        GUI.skin.box.fontSize = 22;
        GUI.skin.button.fontSize = 24;
        GUI.skin.label.fontSize = 16;

        float meioX = Screen.width / 2f;
        float meioY = Screen.height / 2f;

        if (venceu == false && perdeu == false)
        {
            // cronômetro
            int segundos = Mathf.CeilToInt(tempoRestante);
            string relogio = (segundos / 60).ToString("00") + ":" + (segundos % 60).ToString("00");
            GUI.Box(new Rect(meioX - 80, 10, 160, 50), relogio);

            // lista de pistas
            GUI.Box(new Rect(10, 10, 390, 245), "Pistas " + pistasEncontradas + "/4");
            for (int i = 0; i < 4; i++)
            {
                string linha = (i + 1) + ". ???";
                if (pistas[i] != null)
                {
                    linha = (i + 1) + ". " + pistas[i];
                }
                GUI.Label(new Rect(20, 50 + i * 50, 370, 50), linha);
            }

            // mira e texto de interação
            if (painelAberto == false)
            {
                GUI.Label(new Rect(meioX - 4, meioY - 10, 20, 20), "+");
                if (jogador.textoInteracao != "")
                {
                    GUI.Box(new Rect(meioX - 190, meioY + 40, 380, 45), jogador.textoInteracao);
                }
            }
        }

        if (painelAberto == true)
        {
            DesenharPainel(meioX);
        }

        if (venceu == true)
        {
            GUI.Box(new Rect(meioX - 260, meioY - 130, 520, 70), "FÁBRICA REATIVADA!");
            GUI.Box(new Rect(meioX - 260, meioY - 55, 520, 50), "Fase 1 concluída - Pontuação: " + pontos);
            if (GUI.Button(new Rect(meioX - 110, meioY + 20, 220, 60), "Jogar de novo"))
            {
                Reiniciar();
            }
        }

        if (perdeu == true)
        {
            GUI.Box(new Rect(meioX - 260, meioY - 130, 520, 70), "TEMPO ESGOTADO");
            GUI.Box(new Rect(meioX - 260, meioY - 55, 520, 50), "Aperte R ou clique no botão");
            if (GUI.Button(new Rect(meioX - 110, meioY + 20, 220, 60), "Tentar de novo"))
            {
                Reiniciar();
            }
        }

        // aviso rápido embaixo da tela
        if (Time.time < tempoAviso)
        {
            GUI.Box(new Rect(meioX - 300, Screen.height - 90, 600, 50), aviso);
        }
    }

    void DesenharPainel(float meioX)
    {
        GUI.Box(new Rect(meioX - 200, 70, 400, 590), "PAINEL DE CONTROLE");

        // mostra o que já foi digitado
        string visor = "";
        for (int i = 0; i < painel.codigoCorreto.Length; i++)
        {
            if (i < digitado.Length)
            {
                visor = visor + digitado[i] + " ";
            }
            else
            {
                visor = visor + "_ ";
            }
        }
        GUI.Box(new Rect(meioX - 150, 125, 300, 70), visor);

        // teclado numérico
        string[] teclas = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "C", "0", "OK" };
        for (int i = 0; i < teclas.Length; i++)
        {
            int linha = i / 3;
            int coluna = i % 3;
            Rect posicao = new Rect(meioX - 155 + coluna * 105, 215 + linha * 80, 95, 70);

            if (GUI.Button(posicao, teclas[i]))
            {
                if (teclas[i] == "C")
                {
                    digitado = "";
                }
                else if (teclas[i] == "OK")
                {
                    Confirmar();
                }
                else
                {
                    AdicionarDigito(teclas[i]);
                }
            }
        }

        if (Time.time < tempoMsgPainel)
        {
            GUI.Box(new Rect(meioX - 150, 540, 300, 45), msgPainel);
        }

        if (GUI.Button(new Rect(meioX - 90, 595, 180, 50), "Fechar"))
        {
            FecharPainel();
        }
    }
}
