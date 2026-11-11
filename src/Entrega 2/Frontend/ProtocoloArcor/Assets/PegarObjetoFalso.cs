using UnityEngine;

public class PegarObjetoFalso : MonoBehaviour
{
    public KeyCode tecla = KeyCode.E;
    public int totalNecessario = 4;
    public string textoPerto = "Aperte E para pegar";
    public float tempoMensagem = 2.5f;

    // Contador compartilhado entre todos os objetos
    static int coletados = 0;

    bool jogadorPerto = false;
    bool jaPegou = false;
    string mensagem = "";
    float mensagemAte = 0f;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void Awake()
    {
        // Ao carregar a fase, zera a contagem
        coletados = 0;
        Debug.Log("PegarObjetoFalso (versão com vitória) carregado em: " + gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ControleLentidao>() != null)
            jogadorPerto = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ControleLentidao>() != null)
            jogadorPerto = false;
    }

    void Update()
    {
        if (jogadorPerto && !jaPegou && Input.GetKeyDown(tecla))
        {
            jaPegou = true;
            coletados++;
            Debug.Log("Bala pega: " + coletados + "/" + totalNecessario +
                      " | GerenciadorFasePlutonita na cena: " + (GerenciadorFasePlutonita.Instancia != null));

            if (coletados >= totalNecessario)
            {
                // Última bala: mostra a tela de vitória
                if (GerenciadorFasePlutonita.Instancia != null)
                    GerenciadorFasePlutonita.Instancia.Vitoria();
                else
                    ControleLentidao.Curar();
                return;
            }

            mensagem = "Bala " + coletados + "/" + totalNecessario;
            mensagemAte = Time.time + tempoMensagem;
        }
    }

    void OnGUI()
    {
        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        estilo.normal.textColor = Color.white;

        var area = new Rect(0, Screen.height * 0.7f, Screen.width, 40);

        if (Time.time < mensagemAte)
            GUI.Label(area, mensagem, estilo);
        else if (jogadorPerto && !jaPegou)
            GUI.Label(area, textoPerto, estilo);
    }
}