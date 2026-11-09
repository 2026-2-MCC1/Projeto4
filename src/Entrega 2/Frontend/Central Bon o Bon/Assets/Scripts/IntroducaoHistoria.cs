using UnityEngine;
using UnityEngine.SceneManagement;

// Introdução da história, contada pelo Block Man, em uma cena separada.
// Espaço avança a fala. Esc pula tudo. No fim carrega a Fase 1.
public class IntroducaoHistoria : MonoBehaviour
{
    public string proximaCena = "Fase1_Estacao7Belo";

    public Camera cameraIntro;
    public Transform[] pontos;      // um ponto de câmera para cada fala
    public Light[] luzes;           // luzes da sala
    public Renderer telaPainel;
    public Light luzFaisca;         // luz amarela que pisca no painel

    // partes do Block Man
    public Transform cabeca;
    public Transform boca;
    public Transform bracoDir;

    public string[] falas = {
        "Olá! Eu sou o Block Man, funcionário da fábrica da Arcor. Deixa eu te contar o que aconteceu hoje à noite.",
        "Uma tempestade atípica atingiu a região e sobrecarregou os nossos geradores...",
        "Ouviu isso? Faíscas no painel principal! Tudo apagou e o Protocolo Arcor travou todas as estações da fábrica.",
        "Alguém precisa entrar às escuras e reativar cada setor: a Estação 7 Belo, a Zona Plutônica e a Câmara da Tartaruga.",
        "O técnico Fúlvio Marques se voluntariou. Você é o Fúlvio, e tem só 2 minutos em cada fase!",
        "Eu vou te guiar pelo rádio. Boa sorte, Fúlvio!"
    };

    public float velocidadeTexto = 35f;   // letras por segundo

    int indice = 0;
    float tempoLinha = 0f;
    float tempoPausa = 0f;
    float tempoGeral = 0f;

    float[] luzOriginal;
    bool apagou = false;
    float tempoPisca = 0f;
    float relampago = 0f;
    float proximoRelampago = 4f;

    void Start()
    {
        luzOriginal = new float[luzes.Length];
        for (int i = 0; i < luzes.Length; i++)
        {
            luzOriginal[i] = luzes[i].intensity;
        }

        telaPainel.material.color = new Color(0.2f, 1f, 0.5f);
        luzFaisca.enabled = false;

        cameraIntro.transform.position = pontos[0].position;
        cameraIntro.transform.rotation = pontos[0].rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Terminar();
            return;
        }

        tempoGeral = tempoGeral + Time.deltaTime;
        tempoLinha = tempoLinha + Time.deltaTime;

        string fala = falas[indice];
        int letras = (int)(tempoLinha * velocidadeTexto);
        bool digitando = letras < fala.Length;

        if (digitando == false)
        {
            tempoPausa = tempoPausa + Time.deltaTime;
        }

        // Espaço: se ainda está digitando mostra tudo, senão vai para a próxima fala
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (digitando == true)
            {
                tempoLinha = fala.Length / velocidadeTexto + 0.01f;
            }
            else
            {
                ProximaFala();
                return;
            }
        }

        // passa sozinho depois de uma pausa
        if (digitando == false && tempoPausa > 2.5f)
        {
            ProximaFala();
            return;
        }

        MoverCamera();
        AnimarBlockMan(digitando);
        Efeitos();
    }

    void ProximaFala()
    {
        indice = indice + 1;
        if (indice >= falas.Length)
        {
            Terminar();
            return;
        }
        tempoLinha = 0f;
        tempoPausa = 0f;
    }

    void MoverCamera()
    {
        // a câmera vai chegando devagar no ponto da fala atual
        Transform alvo = pontos[indice];
        cameraIntro.transform.position = Vector3.Lerp(cameraIntro.transform.position, alvo.position, Time.deltaTime * 0.8f);
        cameraIntro.transform.rotation = Quaternion.Slerp(cameraIntro.transform.rotation, alvo.rotation, Time.deltaTime * 1.2f);
    }

    void AnimarBlockMan(bool falando)
    {
        // a cabeça balança de leve
        cabeca.localEulerAngles = new Vector3(Mathf.Sin(Time.time * 2f) * 3f, Mathf.Sin(Time.time * 1.3f) * 8f, 0);

        if (falando == true)
        {
            // a boca abre e fecha e o braço faz gestos
            float abertura = 0.04f + Mathf.Abs(Mathf.Sin(Time.time * 18f)) * 0.08f;
            boca.localScale = new Vector3(0.16f, abertura, 0.02f);
            bracoDir.localEulerAngles = new Vector3(0, 0, -20f + Mathf.Sin(Time.time * 4f) * 20f);
        }
        else
        {
            boca.localScale = new Vector3(0.16f, 0.04f, 0.02f);
            bracoDir.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Efeitos()
    {
        // relâmpagos de vez em quando
        relampago = relampago - Time.deltaTime * 2f;
        if (relampago < 0)
        {
            relampago = 0;
        }
        proximoRelampago = proximoRelampago - Time.deltaTime;
        if (proximoRelampago <= 0)
        {
            relampago = 0.5f;
            proximoRelampago = Random.Range(3f, 6f);
        }

        // fala 2: as luzes ficam piscando
        if (indice == 1)
        {
            tempoPisca = tempoPisca - Time.deltaTime;
            if (tempoPisca <= 0)
            {
                tempoPisca = 0.12f;
                for (int i = 0; i < luzes.Length; i++)
                {
                    if (Random.value > 0.5f)
                    {
                        luzes[i].intensity = luzOriginal[i];
                    }
                    else
                    {
                        luzes[i].intensity = 0;
                    }
                }
            }
        }

        // fala 3: faíscas no painel e apagão
        if (indice == 2 && tempoLinha < 3f)
        {
            luzFaisca.enabled = Random.value > 0.6f;
        }
        else
        {
            luzFaisca.enabled = false;
        }

        if (indice >= 2 && apagou == false)
        {
            apagou = true;
            telaPainel.material.color = new Color(0.05f, 0.05f, 0.05f);
            for (int i = 0; i < luzes.Length; i++)
            {
                luzes[i].intensity = luzOriginal[i] * 0.3f;
            }
        }
    }

    void Terminar()
    {
        SceneManager.LoadScene(proximaCena);
    }

    // ---------- tela ----------

    void DesenharRetangulo(Rect area, Color cor)
    {
        GUI.color = cor;
        GUI.DrawTexture(area, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void OnGUI()
    {
        float largura = Screen.width;
        float altura = Screen.height;

        // clarão do relâmpago
        if (relampago > 0)
        {
            DesenharRetangulo(new Rect(0, 0, largura, altura), new Color(1, 1, 1, relampago * 0.6f));
        }

        // barras pretas de cinema
        DesenharRetangulo(new Rect(0, 0, largura, altura * 0.1f), Color.black);
        DesenharRetangulo(new Rect(0, altura * 0.9f, largura, altura * 0.1f), Color.black);

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.alignment = TextAnchor.MiddleCenter;
        estilo.wordWrap = true;
        estilo.normal.textColor = Color.white;

        // título nos primeiros 4 segundos
        if (tempoGeral < 4f)
        {
            estilo.fontSize = 64;
            estilo.normal.textColor = new Color(1, 1, 1, 1f - tempoGeral / 4f);
            GUI.Label(new Rect(0, altura * 0.12f, largura, 100), "PROTOCOLO ARCOR", estilo);
            estilo.normal.textColor = Color.white;
        }

        // balão de fala do Block Man
        Rect balao = new Rect(largura * 0.15f, altura * 0.68f, largura * 0.7f, altura * 0.2f);
        DesenharRetangulo(new Rect(balao.x - 4, balao.y - 4, balao.width + 8, balao.height + 8), new Color(1f, 0.55f, 0.1f));
        DesenharRetangulo(balao, new Color(0.05f, 0.15f, 0.35f, 0.95f));

        GUIStyle nome = new GUIStyle(GUI.skin.label);
        nome.fontSize = 20;
        nome.fontStyle = FontStyle.Bold;
        nome.normal.textColor = new Color(1f, 0.7f, 0.25f);
        GUI.Label(new Rect(balao.x + 15, balao.y + 8, 300, 30), "BLOCK MAN", nome);

        // texto aparecendo letra por letra
        string fala = falas[indice];
        int letras = (int)(tempoLinha * velocidadeTexto);
        if (letras > fala.Length)
        {
            letras = fala.Length;
        }
        estilo.fontSize = 24;
        estilo.alignment = TextAnchor.UpperLeft;
        GUI.Label(new Rect(balao.x + 15, balao.y + 42, balao.width - 30, balao.height - 50), fala.Substring(0, letras), estilo);

        estilo.fontSize = 15;
        estilo.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(0, altura * 0.9f, largura - 20, altura * 0.1f), "[Espaço] avançar    [Esc] pular", estilo);
    }
}
