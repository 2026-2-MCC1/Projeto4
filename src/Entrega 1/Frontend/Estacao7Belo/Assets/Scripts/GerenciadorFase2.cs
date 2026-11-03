using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a Fase 2 (Zona Plutônica): tempo, válvulas, baba de bruxa, bala 7 Belo, vitória e derrota.
public class GerenciadorFase2 : MonoBehaviour
{
    // referências (o gerador da cena já liga)
    public Jogador jogador;
    public InteragirFase2 interagir;
    public BlockMan blockMan;
    public Transform porta;
    public Light[] luzes;
    public Valvula[] valvulas;

    public float tempoTotal = 120f;

    // Bala 7 Belo (na história vem da fase bônus; aqui dá para ligar e desligar para testar)
    public bool temBala = true;
    public float duracaoBala = 45f;

    // quanto da velocidade sobra dentro da baba (0.4 = 40%)
    public float fatorLentidao = 0.4f;

    public bool podeInteragir = true;

    float tempoRestante;
    string[] ordemCerta = { "Azul", "Verde", "Vermelha" };
    int progresso = 0;

    float velocidadeBase;

    bool balaUsada = false;
    float tempoBala = 0f;
    bool avisouBaba = false;

    bool venceu = false;
    bool perdeu = false;
    int pontos = 0;

    string manualTexto = "";
    string aviso = "";
    float tempoAviso = 0f;

    bool dica1 = false;
    bool dica2 = false;

    void Awake()
    {
        tempoRestante = tempoTotal;
    }

    void Start()
    {
        velocidadeBase = jogador.velocidade;
        blockMan.Falar("Zona Plutônica: a pressão do setor caiu! Ache o manual na parede e gire as válvulas na ordem certa para abrir a porta. Cuidado com a Baba de Bruxa, ela deixa você lento!", 10f);
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

            AtualizarVelocidade();
            UsarBala();
            Dicas();
        }

        // depois de vencer a porta sobe até uma altura de 4 metros
        if (venceu == true && porta.position.y < 4f)
        {
            porta.Translate(0, Time.deltaTime * 1.2f, 0);
        }

        if ((venceu == true || perdeu == true) && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    void Dicas()
    {
        if (tempoRestante <= 80 && dica1 == false)
        {
            dica1 = true;
            if (manualTexto == "")
            {
                blockMan.Falar("Dica: o manual de pressão está colado na parede esquerda, perto da entrada.", 7f);
            }
        }
        if (tempoRestante <= 40 && dica2 == false)
        {
            dica2 = true;
            blockMan.Falar("Faltam 40 segundos! Lembre: errou a ordem, a pressão cai e você recomeça.", 7f);
        }
    }

    // ---------- baba de bruxa e bala ----------

    void AtualizarVelocidade()
    {
        if (tempoBala > 0)
        {
            tempoBala = tempoBala - Time.deltaTime;
        }

        bool imune = tempoBala > 0;

        if (jogador.pocas > 0 && imune == false)
        {
            jogador.velocidade = velocidadeBase * fatorLentidao;

            if (avisouBaba == false)
            {
                avisouBaba = true;
                if (temBala && balaUsada == false)
                {
                    blockMan.Falar("Você pisou na Baba de Bruxa e ficou lento! Aperte Q para usar a Bala 7 Belo.", 6f);
                }
                else
                {
                    blockMan.Falar("Você pisou na Baba de Bruxa e ficou lento! Desvie das poças verdes.", 5f);
                }
            }
        }
        else
        {
            jogador.velocidade = velocidadeBase;
        }
    }

    void UsarBala()
    {
        if (Input.GetKeyDown(KeyCode.Q) && temBala == true && balaUsada == false)
        {
            balaUsada = true;
            tempoBala = duracaoBala;
            blockMan.Falar("Bala 7 Belo ativada! A Baba de Bruxa não atrapalha por " + duracaoBala + " segundos.", 5f);
        }
    }

    // ---------- válvulas ----------

    public void LerManual(NotaManual manual)
    {
        manualTexto = manual.texto;
        MostrarAviso("Manual de pressão lido!");
        blockMan.Falar("Boa! Siga a ordem do manual: gire as válvulas pela cor.", 5f);
    }

    public void AcionarValvula(Valvula valvula)
    {
        if (valvula.ligada == true)
        {
            MostrarAviso("Essa válvula já está ligada.");
            return;
        }

        if (valvula.cor == ordemCerta[progresso])
        {
            valvula.Ligar();
            progresso = progresso + 1;

            if (progresso == ordemCerta.Length)
            {
                Vencer();
            }
            else
            {
                MostrarAviso("Pressão subindo! (" + progresso + "/" + ordemCerta.Length + ")");
            }
        }
        else
        {
            progresso = 0;
            for (int i = 0; i < valvulas.Length; i++)
            {
                valvulas[i].Apagar();
            }
            MostrarAviso("Ordem errada! A pressão caiu.");
            blockMan.Falar("Ordem errada! Releia o manual e recomece pela primeira cor.", 5f);
        }
    }

    // ---------- fim de fase ----------

    void Vencer()
    {
        venceu = true;
        podeInteragir = false;
        pontos = Mathf.RoundToInt(tempoRestante * 10);

        jogador.velocidade = velocidadeBase;
        jogador.podeMover = false;
        jogador.TravarMouse(false);

        // as luzes de emergência ficam brancas e fortes
        for (int i = 0; i < luzes.Length; i++)
        {
            luzes[i].color = new Color(1f, 0.95f, 0.8f);
            luzes[i].intensity = 2.5f;
        }

        blockMan.Falar("Pressão máxima atingida! A porta selada abriu. Falta pouco para a Câmara da Tartaruga!", 10f);
    }

    void Perder()
    {
        perdeu = true;
        podeInteragir = false;
        jogador.velocidade = velocidadeBase;
        jogador.podeMover = false;
        jogador.TravarMouse(false);
        blockMan.Falar("O tempo acabou... Tente de novo!", 6f);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MostrarAviso(string texto)
    {
        aviso = texto;
        tempoAviso = Time.time + 3f;
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

            // objetivo e manual
            GUI.Box(new Rect(10, 10, 390, 215), "Zona Plutônica");
            GUI.Label(new Rect(20, 50, 370, 40), "Válvulas: " + progresso + "/" + ordemCerta.Length);
            string textoManual = "Manual: ???  (ache na parede)";
            if (manualTexto != "")
            {
                textoManual = "Manual: " + manualTexto;
            }
            GUI.Label(new Rect(20, 85, 370, 140), textoManual);

            // mira e texto de interação
            GUI.Label(new Rect(meioX - 4, meioY - 10, 20, 20), "+");
            if (interagir.textoInteracao != "")
            {
                GUI.Box(new Rect(meioX - 230, meioY + 40, 460, 45), interagir.textoInteracao);
            }

            // estado da baba e da bala
            if (tempoBala > 0)
            {
                GUI.Box(new Rect(meioX - 230, Screen.height - 150, 460, 45), "Bala 7 Belo ativa: " + Mathf.CeilToInt(tempoBala) + " s");
            }
            else if (jogador.pocas > 0)
            {
                string msg = "Você está lento! (Baba de Bruxa)";
                if (temBala && balaUsada == false)
                {
                    msg = msg + "  [Q] usar Bala 7 Belo";
                }
                GUI.Box(new Rect(meioX - 280, Screen.height - 150, 560, 45), msg);
            }
            else if (temBala && balaUsada == false)
            {
                GUI.Label(new Rect(10, Screen.height - 40, 400, 30), "[Q] Bala 7 Belo disponível");
            }
        }

        if (venceu == true)
        {
            GUI.Box(new Rect(meioX - 260, meioY - 130, 520, 70), "PRESSÃO MÁXIMA!");
            GUI.Box(new Rect(meioX - 260, meioY - 55, 520, 50), "Fase 2 concluída - Pontuação: " + pontos);
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

        if (Time.time < tempoAviso)
        {
            GUI.Box(new Rect(meioX - 300, Screen.height - 90, 600, 50), aviso);
        }
    }
}
