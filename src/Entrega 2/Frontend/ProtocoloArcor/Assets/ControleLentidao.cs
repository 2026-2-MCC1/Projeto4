using System.Reflection;
using UnityEngine;

/// <summary>
/// Guarda se o player está lento e aplica isso na velocidade dele.
/// O estado CONTINUA entre as fases: se pisou na gosma na Fase 2,
/// o player começa a próxima fase ainda lento, até comer a bala de cura.
///
/// Coloque no player de cada fase onde a lentidão deve valer.
/// (Na fase da gosma, a própria gosma adiciona este script se ele não existir.)
/// </summary>
public class ControleLentidao : MonoBehaviour
{
    // ---- Estado global (sobrevive à troca de cena) ----
    public static bool Lento { get; private set; }
    public static float Fator { get; private set; } = 1f;
    static float mensagemCuradoAte;
    static float mensagemProtegidoAte;

    [Tooltip("Marque na fase da gosma: ao reiniciar a fase, o player começa com velocidade normal")]
    public bool curarAoIniciarCena = false;

    [Tooltip("Nome da variável de velocidade no script de movimento. Vazio = detectar automaticamente")]
    public string nomeVariavel = "";

    public bool mostrarAviso = true;
    public string textoAviso = "Lento! Encontre a bala de cura";

    static readonly string[] nomesComuns =
    {
        "velocidade", "velocidadeMovimento", "velocidadeAndar", "velocidadeNormal",
        "speed", "moveSpeed", "walkSpeed", "movementSpeed", "playerSpeed"
    };

    Component comp;
    FieldInfo campo;
    PropertyInfo prop;
    float original;
    bool achou;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetarAoIniciarJogo()
    {
        Lento = false;
        Fator = 1f;
        mensagemCuradoAte = 0f;
        mensagemProtegidoAte = 0f;
    }

    void Awake()
    {
        if (curarAoIniciarCena)
        {
            Lento = false;
            Fator = 1f;
        }
    }

    void Start()
    {
        achou = EncontrarVelocidade();
        if (!achou)
        {
            Debug.LogWarning("ControleLentidao: não achei a variável de velocidade do player. " +
                             "Escreva o nome dela no campo 'Nome Variavel'.");
            return;
        }
        original = Valor;
        Atualizar();
    }

    /// <summary>Deixa o player lento (chamado pela gosma).</summary>
    public static void Aplicar(float fator)
    {
        // Pegou a Bala 7 Belo na fase bônus? Então a baba de bruxa não faz efeito
        if (Bala7Belo.Coletada || EstadoJogo.TemBala())
        {
            mensagemProtegidoAte = Time.time + 2.5f;
            return;
        }

        Lento = true;
        Fator = Mathf.Min(Fator, fator);
        AtualizarTodos();
    }

    /// <summary>Volta à velocidade normal (chamado pela bala de cura).</summary>
    public static void Curar()
    {
        if (!Lento) return;
        Lento = false;
        Fator = 1f;
        mensagemCuradoAte = Time.time + 2.5f;
        AtualizarTodos();
    }

    static void AtualizarTodos()
    {
        foreach (var c in FindObjectsByType<ControleLentidao>(FindObjectsSortMode.None))
            c.Atualizar();
    }

    void Atualizar()
    {
        if (!achou) return;
        Valor = original * (Lento ? Fator : 1f);
    }

    float Valor
    {
        get => campo != null ? (float)campo.GetValue(comp) : (float)prop.GetValue(comp);
        set { if (campo != null) campo.SetValue(comp, value); else prop.SetValue(comp, value); }
    }

    bool EncontrarVelocidade()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public |
                                   BindingFlags.NonPublic | BindingFlags.IgnoreCase;
        var nomes = string.IsNullOrWhiteSpace(nomeVariavel) ? nomesComuns : new[] { nomeVariavel.Trim() };

        foreach (var c in transform.root.GetComponentsInChildren<MonoBehaviour>())
        {
            if (c == null || c is ControleLentidao) continue;
            var tipo = c.GetType();
            foreach (var nome in nomes)
            {
                var f = tipo.GetField(nome, flags);
                if (f != null && f.FieldType == typeof(float)) { comp = c; campo = f; return true; }

                var p = tipo.GetProperty(nome, flags);
                if (p != null && p.PropertyType == typeof(float) && p.CanRead && p.CanWrite)
                { comp = c; prop = p; return true; }
            }
        }
        return false;
    }

    void OnGUI()
    {
        if (!mostrarAviso) return;

        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        estilo.normal.textColor = new Color(0.4f, 1f, 0.3f);

        var area = new Rect(0, Screen.height * 0.8f, Screen.width, 40);
        if (Lento)
            GUI.Label(area, textoAviso, estilo);
        else if (Time.time < mensagemCuradoAte)
            GUI.Label(area, "Velocidade normal!", estilo);
        else if (Time.time < mensagemProtegidoAte)
            GUI.Label(area, "A Bala 7 Belo te protegeu da baba de bruxa!", estilo);
    }
}