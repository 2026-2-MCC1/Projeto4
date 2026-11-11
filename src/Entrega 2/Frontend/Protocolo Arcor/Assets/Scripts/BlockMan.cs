using TMPro;
using UnityEngine;

// NPC Block Man: mostra balões de mensagem no canto da tela.
// Se o balão do Canvas (TextMeshPro) estiver ligado, usa ele.
// Se não estiver (ex.: Fase 2, que ainda usa OnGUI), desenha o balão antigo.
// O balão do Canvas é criado pelo menu Protocolo Arcor > Criar interface da Fase 1 (Canvas).
public class BlockMan : MonoBehaviour
{
    public GameObject balao;      // painel do balão (liga e desliga)
    public TMP_Text textoBalao;   // texto da mensagem

    string mensagem = "";
    float tempoFinal = 0f;

    // Awake roda antes de qualquer Start, então a primeira mensagem
    // (chamada pelo gerenciador da fase) não é escondida por engano
    void Awake()
    {
        // o balão do Canvas começa escondido
        if (balao != null)
        {
            balao.SetActive(false);
        }
    }

    // mostra uma mensagem por alguns segundos
    public void Falar(string texto, float segundos)
    {
        mensagem = texto;
        tempoFinal = Time.time + segundos;

        if (balao != null)
        {
            if (textoBalao != null)
            {
                textoBalao.text = texto;
            }
            balao.SetActive(true);
        }
    }

    void Update()
    {
        // esconde o balão do Canvas quando o tempo da mensagem acaba
        if (balao != null && balao.activeSelf == true && Time.time >= tempoFinal)
        {
            balao.SetActive(false);
        }
    }

    // balão antigo, só é usado quando não há Canvas ligado
    void OnGUI()
    {
        if (balao == null && Time.time < tempoFinal)
        {
            GUI.skin.box.fontSize = 18;
            GUI.skin.box.wordWrap = true;
            GUI.Box(new Rect(Screen.width - 430, 10, 420, 150), "BLOCK MAN\n\n" + mensagem);
        }
    }
}