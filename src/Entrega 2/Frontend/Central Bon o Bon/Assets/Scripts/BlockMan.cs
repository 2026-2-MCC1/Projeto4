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
            GUI.skin.box.fontSize = 18;
            GUI.skin.box.wordWrap = true;
            GUI.Box(new Rect(Screen.width - 430, 10, 420, 150), "BLOCK MAN\n\n" + mensagem);
        }
    }
}
