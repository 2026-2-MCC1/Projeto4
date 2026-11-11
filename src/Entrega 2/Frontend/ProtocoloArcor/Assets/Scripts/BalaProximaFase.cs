using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Coloque na Bala 7 Belo da fase bônus.
/// Quando o jogador chega perto, aparece "[E] Pegar Bala 7 Belo".
/// Apertando E: guarda que pegou a bala (protege da lentidão na Fase 2)
/// e carrega a próxima fase.
/// Não depende de nenhum outro script da fase.
/// </summary>
public class BalaProximaFase : MonoBehaviour
{
    [Tooltip("Nome exato da próxima cena (igual ao da Build Profiles)")]
    public string proximaCena = "Fase 2 Plutônica";

    [Tooltip("Distância máxima (em metros) entre a câmera do jogador e a bala")]
    public float distancia = 3f;

    [Tooltip("Pega só de chegar perto, sem precisar apertar E")]
    public bool pegarAoEncostar = false;

    bool perto;
    bool pegou;

    void Update()
    {
        if (pegou) return;

        // Câmera do jogador: a "MainCamera" ou, se não tiver essa tag, qualquer câmera ativa
        var cam = Camera.main;
        if (cam == null) cam = FindFirstObjectByType<Camera>();
        if (cam == null) return;

        // Também mede a distância até o corpo do jogador (CharacterController), se existir
        float dist = Vector3.Distance(cam.transform.position, transform.position);
        var corpo = FindFirstObjectByType<CharacterController>();
        if (corpo != null)
            dist = Mathf.Min(dist, Vector3.Distance(corpo.transform.position, transform.position));

        perto = dist <= distancia;

        if (perto && (pegarAoEncostar || ApertouE()))
            Pegar();
    }

    void Pegar()
    {
        pegou = true;
        Bala7Belo.Coletada = true;   // a lentidão da Fase 2 não vai afetar o jogador
        EstadoJogo.GanharBala();     // sistema do grupo (PlayerPrefs)

        // tela de encerramento: fase bônus (Central Bon o Bon) concluída
        Tempos.passouFase3 = true;
        Tempos.tempoFase3 = Time.timeSinceLevelLoad;
        Time.timeScale = 1f;
        SceneManager.LoadScene(proximaCena);
    }

    bool ApertouE()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.E);
#endif
    }

    void OnGUI()
    {
        if (!perto || pegou || pegarAoEncostar) return;

        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 26,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        estilo.normal.textColor = Color.yellow;
        GUI.Label(new Rect(0, Screen.height * 0.65f, Screen.width, 40), "[E] Pegar Bala 7 Belo", estilo);
    }
}