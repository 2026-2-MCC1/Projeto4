using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    public enum EstadoFase { Jogando, Vitoria, VitoriaBonus, Derrota }

    public const string NomeCena = "Fase2";

    public static GerenciadorFase Instancia { get; private set; }

    public static EstadoFase Estado { get; private set; } = EstadoFase.Jogando;

    public static float TempoFinal { get; private set; }

    public static event Action OnVitoria;
    public static event Action OnVitoriaBonus;
    public static event Action OnDerrota;

    [Header("Tempo (segundos)")]
    public float tempoLimite = 180f;
    [Tooltip("Se terminar até este tempo, vai para a fase bônus")]
    public float tempoBonus = 90f;

    [Header("Cenas")]
    public string cenaVitoria = "Fase3";
    public string cenaBonus = "FaseBonus";
    public string cenaDerrota = "GameOver";

    [Header("Tela")]
    public bool mostrarTimer = true;
    [Tooltip("Marcado: mostra Game Over aqui mesmo, com botão Reiniciar. Desmarcado: vai pra cenaDerrota.")]
    public bool gameOverNaPropriaCena = true;

    public static string CenaParaReiniciar { get; private set; } = NomeCena;

    float tempoDecorrido;
    bool mostrandoGameOver;

    public float TempoRestante => Mathf.Max(0f, tempoLimite - tempoDecorrido);
    public bool DentroDoBonus => tempoDecorrido <= tempoBonus;

    void Awake()
    {
        Instancia = this;
        Estado = EstadoFase.Jogando;
        tempoDecorrido = 0f;
        Time.timeScale = 1f;
        CenaParaReiniciar = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (mostrandoGameOver && ApertouR()) Reiniciar();
        if (Estado != EstadoFase.Jogando) return;

        tempoDecorrido += Time.deltaTime;
        if (tempoDecorrido >= tempoLimite) Derrota();
    }

    public void Vitoria()
    {
        if (Estado != EstadoFase.Jogando) return;
        TempoFinal = tempoDecorrido;

        if (DentroDoBonus)
        {
            Estado = EstadoFase.VitoriaBonus;
            OnVitoriaBonus?.Invoke();
            Carregar(cenaBonus);
        }
        else
        {
            Estado = EstadoFase.Vitoria;
            OnVitoria?.Invoke();
            Carregar(cenaVitoria);
        }
    }

    public void Derrota()
    {
        if (Estado != EstadoFase.Jogando) return;
        TempoFinal = tempoDecorrido;
        Estado = EstadoFase.Derrota;
        OnDerrota?.Invoke();

        if (gameOverNaPropriaCena)
        {
            mostrandoGameOver = true;
            Time.timeScale = 0f;                     
            Cursor.lockState = CursorLockMode.None;  
            Cursor.visible = true;
        }
        else
        {
            Carregar(cenaDerrota);
        }
    }

    public static void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(CenaParaReiniciar);
    }

    bool ApertouR()
    {
#if ENABLE_INPUT_SYSTEM
        return UnityEngine.InputSystem.Keyboard.current != null &&
               UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.R);
#endif
    }

    void Carregar(string cena)
    {
        if (string.IsNullOrWhiteSpace(cena))
        {
            Debug.LogWarning("GerenciadorFase: nome de cena vazio.");
            return;
        }
        SceneManager.LoadScene(cena);
    }

    void OnGUI()
    {
        if (mostrandoGameOver) { DesenharGameOver(); return; }
        if (!mostrarTimer || Estado != EstadoFase.Jogando) return;

        int t = Mathf.CeilToInt(TempoRestante);
        string texto = $"{t / 60:00}:{t % 60:00}";

        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 32,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter
        };
        estilo.normal.textColor = DentroDoBonus ? Color.yellow : (TempoRestante <= 30f ? Color.red : Color.white);

        GUI.Label(new Rect(0, 15, Screen.width, 50), texto, estilo);

        if (DentroDoBonus)
        {
            var dica = new GUIStyle(estilo) { fontSize = 18, fontStyle = FontStyle.Normal };
            GUI.Label(new Rect(0, 55, Screen.width, 30), "Bônus disponível!", dica);
        }
    }

    void DesenharGameOver()
    {
        var corAntiga = GUI.color;
        GUI.color = new Color(0, 0, 0, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = corAntiga;

        var titulo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 56,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        titulo.normal.textColor = Color.red;
        GUI.Label(new Rect(0, Screen.height * 0.3f, Screen.width, 80), "GAME OVER", titulo);

        var sub = new GUIStyle(titulo) { fontSize = 22, fontStyle = FontStyle.Normal };
        sub.normal.textColor = Color.white;
        GUI.Label(new Rect(0, Screen.height * 0.3f + 80, Screen.width, 40), "O tempo acabou!", sub);

        var botao = new GUIStyle(GUI.skin.button) { fontSize = 24 };
        float w = 240, h = 55;
        if (GUI.Button(new Rect((Screen.width - w) / 2f, Screen.height * 0.55f, w, h), "Reiniciar (R)", botao))
            Reiniciar();
    }
}
