using UnityEngine;

public class MensagemInicial : MonoBehaviour
{
    [TextArea] public string mensagem = "Encontre as 4 balas e passe pelo labirinto";
    [Tooltip("Quanto tempo a mensagem fica na tela (segundos)")]
    public float duracao = 4f;
    [Tooltip("Tempo do efeito de sumir (segundos)")]
    public float fade = 1f;
    public int tamanhoFonte = 34;
    public Color cor = Color.white;

    float inicio;

    void Start()
    {
        inicio = Time.time;
    }

    void OnGUI()
    {
        float passou = Time.time - inicio;
        if (passou > duracao) return;

        float alpha = Mathf.Clamp01((duracao - passou) / Mathf.Max(0.01f, fade));

        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = tamanhoFonte,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };

        var area = new Rect(Screen.width * 0.1f, Screen.height * 0.35f, Screen.width * 0.8f, 100);

        estilo.normal.textColor = new Color(0, 0, 0, alpha * 0.8f);
        GUI.Label(new Rect(area.x + 2, area.y + 2, area.width, area.height), mensagem, estilo);

        estilo.normal.textColor = new Color(cor.r, cor.g, cor.b, alpha);
        GUI.Label(area, mensagem, estilo);
    }
}
