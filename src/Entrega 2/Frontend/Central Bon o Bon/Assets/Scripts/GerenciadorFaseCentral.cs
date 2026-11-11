using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a Fase Bônus: tempo, vitória e derrota.
public class GerenciadorFase : MonoBehaviour
{
    // referências (arrastadas na cena)
    public Jogador jogador;
    public BlockMan blockMan;
    public Light[] luzes;

    public float tempoTotal = 120f;

    // usado pela introdução da história: enquanto for true, o tempo não corre
    public bool emIntroducao = false;

    float tempoRestante;

    bool venceu = false;
    bool perdeu = false;
    int pontos = 0;
    float tempoGasto = 0f;
    float melhorTempo = 0f;

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

    // chamado no começo (ou quando a introdução termina)
    public void ComecarJogo()
    {
        emIntroducao = false;
        blockMan.Falar("Procure a única bala 7 Belo perdida, no meio desses deliciosos Bon o Bon, antes que o tempo acabe!", 10f);
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

    // ---------- bala ----------

    public void PegarBala()
    {
        if (venceu == true || perdeu == true)
        {
            return;
        }

        Vencer();
    }

    // ---------- fim de fase ----------

    void Vencer()
    {
        venceu = true;

        if (GerenciadorSom.instancia != null)
        {
            GerenciadorSom.instancia.TocarVitoria();
            GerenciadorSom.instancia.AbaixarChuva();
        }

        pontos = Mathf.RoundToInt(tempoRestante * 10);

        // guarda o melhor tempo
        tempoGasto = tempoTotal - tempoRestante;
        melhorTempo = PlayerPrefs.GetFloat("MelhorTempoFaseBonus", 9999f);
        if (tempoGasto < melhorTempo)
        {
            melhorTempo = tempoGasto;
            PlayerPrefs.SetFloat("MelhorTempoFaseBonus", melhorTempo);
            PlayerPrefs.Save();
        }

        jogador.podeMover = false;
        jogador.TravarMouse(false);

        blockMan.Falar("Você conseguiu, mas ainda falta um pouco para concluirmos, vamos para a próxima fase!", 8f);
    }

    void Perder()
    {
        perdeu = true;
        jogador.podeMover = false;
        jogador.TravarMouse(false);
        blockMan.Falar("O tempo acabou... Tente de novo!", 6f);
    }

    void AcenderLuzes()
    {
        // o brilho sobe até 1 em uns 3 segundos
        brilho = brilho + Time.deltaTime / 3f;
        if (brilho > 1f)
        {
            brilho = 1f;
        }

        for (int i = 0; i < luzes.Length; i++)
        {
            luzes[i].intensity = intensidadeOriginal[i] * brilho;
        }
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ---------- tela (HUD) ----------

    void OnGUI()
    {
        if (emIntroducao == true)
        {
            return;
        }

        float meioX = Screen.width / 2f;
        float meioY = Screen.height / 2f;

        if (venceu == false && perdeu == false)
        {
            // cronômetro
            int segundos = Mathf.CeilToInt(tempoRestante);
            string relogio = (segundos / 60).ToString("00") + ":" + (segundos % 60).ToString("00");
            GUI.Box(new Rect(meioX - 50, 16, 100, 30), relogio);

            // mira
            GUI.Label(new Rect(meioX - 10, meioY - 10, 20, 20), "+");

            // texto de interação, mais embaixo na tela
            if (jogador != null && jogador.textoInteracao != "")
            {
                GUI.Box(new Rect(meioX - 150, Screen.height * 0.75f, 300, 30), jogador.textoInteracao);
            }
        }

        if (venceu == true)
        {
            GUI.Box(new Rect(meioX - 150, meioY - 100, 300, 120),
                "FASE CONCLUÍDA!\n\n" +
                "Seu tempo: " + tempoGasto.ToString("0.0") + " s\n" +
                "Melhor tempo: " + melhorTempo.ToString("0.0") + " s\n" +
                "Pontuação: " + pontos);

            if (GUI.Button(new Rect(meioX - 75, meioY + 30, 150, 35), "Jogar de novo"))
            {
                if (GerenciadorSom.instancia != null) GerenciadorSom.instancia.TocarConfirmar();
                Reiniciar();
            }
        }

        if (perdeu == true)
        {
            GUI.Box(new Rect(meioX - 150, meioY - 60, 300, 70),
                "TEMPO ESGOTADO\n\nAperte R ou clique no botão");

            if (GUI.Button(new Rect(meioX - 75, meioY + 20, 150, 35), "Tentar de novo"))
            {
                if (GerenciadorSom.instancia != null) GerenciadorSom.instancia.TocarConfirmar();
                Reiniciar();
            }
        }
    }
}