using UnityEngine;

public class BotaoMudo : MonoBehaviour
{
    public void Alternar()
    {
        if (GerenciadorAudio.Instancia != null)
        {
            GerenciadorAudio.Instancia.AlternarMudo();
        }
    }
}