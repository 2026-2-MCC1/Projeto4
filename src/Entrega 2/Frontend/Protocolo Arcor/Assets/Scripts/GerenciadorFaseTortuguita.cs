using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a Fase 3: tempo, vitória e derrota.
public class GerenciadorFaseTortuguita : MonoBehaviour
{
    public static GerenciadorFaseTortuguita Instancia;

    public float tempoTotal = 180f;
    public string cenaSeguinte = "Encerramento";

    float tempoRestante;
    bool venceu = false;
    bool perdeu = false;
    float tempoGasto = 0f;

    void Awake()
    {
        Instancia = this;
        tempoRestante = tempoTotal;
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
        }

        if (perdeu == true && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    // chamado pelo ObjetivoFase quando a última peça é encaixada
    public void Vencer()
    {
        if (venceu == true || perdeu == true)
        {
            return;
        }

        venceu = true;
        tempoGasto = tempoTotal - tempoRestante;

    
        Tempos.tempoFase4 = tempoGasto;
        Tempos.passouFase4 = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GerenciadorAudio.Instancia != null)
        {
            GerenciadorAudio.Instancia.TocarVitoria();
        }
    }

    void Perder()
    {
        perdeu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnGUI()
    {
        float meioX = Screen.width / 2f;
        float meioY = Screen.height / 2f;

        if (venceu == false && perdeu == false)
        {
            int segundos = Mathf.CeilToInt(tempoRestante);
            string relogio = (segundos / 60).ToString("00") + ":" + (segundos % 60).ToString("00");
            GUI.Box(new Rect(meioX - 50, 16, 100, 30), relogio);
        }

        if (venceu == true)
        {
            GUI.Box(new Rect(meioX - 150, meioY - 100, 300, 100),
                "PARABÉNS, A FÁBRICA FOI REATIVADA!!\n\nSeu tempo: " + tempoGasto.ToString("0.0") + " s");

            if (GUI.Button(new Rect(meioX - 75, meioY + 20, 150, 35), "Continuar"))
            {
                SceneManager.LoadScene(cenaSeguinte);
            }
        }

        if (perdeu == true)
        {
            GUI.Box(new Rect(meioX - 150, meioY - 60, 300, 70),
                "TEMPO ESGOTADO\n\nAperte R ou clique no botão");

            if (GUI.Button(new Rect(meioX - 75, meioY + 20, 150, 35), "Tentar de novo"))
            {
                Reiniciar();
            }
        }
    }
}