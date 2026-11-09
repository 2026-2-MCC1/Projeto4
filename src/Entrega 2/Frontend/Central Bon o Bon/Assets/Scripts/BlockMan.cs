using UnityEngine;

// NPC Block Man: mostra balões de mensagem no canto da tela.
public class BlockMan : MonoBehaviour
{
    string mensagem = "";
    float tempoFinal = 0f;

    public void Falar(string texto, float segundos)
    {
        mensagem = texto;
        tempoFinal = Time.time + segundos;
    }

    void OnGUI()
    {
        if (Time.time < tempoFinal)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.box);
            estilo.fontSize = 18;
            estilo.wordWrap = true;
            estilo.alignment = TextAnchor.UpperLeft;
            estilo.padding = new RectOffset(12, 12, 10, 10);

            float largura = 480f;
            float altura = 220f;

            GUI.Box(new Rect(Screen.width - largura - 10, 10, largura, altura), "BLOCK MAN\n\n" + mensagem, estilo);
        }
    }
}