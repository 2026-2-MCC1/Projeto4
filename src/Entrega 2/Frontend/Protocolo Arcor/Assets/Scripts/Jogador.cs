
using UnityEngine;

// Jogador em primeira pessoa: anda, olha e interage com E.
public class Jogador : MonoBehaviour
{
    public Transform cabeca;

    // Gerenciadores das duas fases
    public GerenciadorFase gerenciador;
    public GerenciadorFaseCentral gerenciadorCentral;

    public float velocidade = 4f;
    public float sensibilidade = 2f;
    public float distanciaInteracao = 3f;
    public float raioMira = 0.15f;
    public LayerMask camadaInteracao;

    public bool podeMover = true;
    public string textoInteracao = "";

    public int pocas = 0;
    public float intervaloPasso = 0.5f;

    float tempoProximoPasso = 0f;
    CharacterController controle;

    float rotacaoX = 0f;
    float velocidadeY = 0f;

    void Start()
    {
        controle = GetComponent<CharacterController>();

        if (gerenciador == null)
            gerenciador = FindFirstObjectByType<GerenciadorFase>();

        if (gerenciadorCentral == null)
            gerenciadorCentral = FindFirstObjectByType<GerenciadorFaseCentral>();

        if (cabeca == null)
            Debug.LogError("Cabeca não foi atribuída no Jogador!");

        if (controle == null)
            Debug.LogError("CharacterController não encontrado!");

        TravarMouse(true);
    }

    void Update()
    {
        if (podeMover)
        {
            Olhar();
            Andar();
            Interagir();
        }
        else
        {
            textoInteracao = "";
        }
    }

    void Olhar()
    {
        if (cabeca == null) return;

        float mouseX = Input.GetAxis("Mouse X") * sensibilidade;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade;

        transform.Rotate(0, mouseX, 0);

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -80f, 80f);

        cabeca.localEulerAngles = new Vector3(rotacaoX, 0, 0);
    }

    void Andar()
    {
        if (controle == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento =
            transform.right * horizontal +
            transform.forward * vertical;

        movimento *= velocidade;

        if (Input.GetKey(KeyCode.LeftShift))
            movimento *= 1.6f;

        // Gravidade
        if (controle.isGrounded && velocidadeY < 0)
            velocidadeY = -2f;

        velocidadeY -= 20f * Time.deltaTime;
        movimento.y = velocidadeY;

        controle.Move(movimento * Time.deltaTime);

        // Sons de passos
        Vector3 velHorizontal = new Vector3(
            controle.velocity.x, 0, controle.velocity.z
        );

        if (controle.isGrounded &&
            velHorizontal.magnitude > 0.5f &&
            Time.time >= tempoProximoPasso)
        {
            float intervalo = intervaloPasso;

            if (Input.GetKey(KeyCode.LeftShift))
                intervalo /= 1.6f;

            tempoProximoPasso = Time.time + intervalo;
        }
    }

    void Interagir()
    {
        textoInteracao = "";

        if (cabeca == null) return;

        int mascara = camadaInteracao.value == 0
            ? Physics.DefaultRaycastLayers
            : camadaInteracao.value;

        RaycastHit[] acertos = Physics.SphereCastAll(
            cabeca.position,
            raioMira,
            cabeca.forward,
            distanciaInteracao,
            mascara,
            QueryTriggerInteraction.Collide
        );

        // Procura primeiro o painel
        foreach (RaycastHit hit in acertos)
        {
            Painel painel = hit.collider.GetComponentInParent<Painel>();

            if (painel != null)
            {
                textoInteracao = "[E] Abrir painel";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gerenciador != null)
                    {
                        gerenciador.AbrirPainel();
                    }
                    else
                    {
                        Debug.LogError("GerenciadorFase não encontrado!");
                    }
                }

                return;
            }
        }

        // Procura as pistas e balas
        foreach (RaycastHit hit in acertos)
        {
            Pista pista = hit.collider.GetComponentInParent<Pista>();

            if (pista != null)
            {
                textoInteracao = "[E] Ler pista";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gerenciador != null)
                    {
                        gerenciador.LerPista(pista);
                    }
                    else if (gerenciadorCentral != null)
                    {
                        gerenciadorCentral.PegarBala();
                    }
                    else
                    {
                        Debug.LogError("Nenhum gerenciador encontrado!");
                    }
                }

                return;
            }
        }
    }

    // Poças de baba
    void OnTriggerEnter(Collider outro)
    {
        if (outro.name.Contains("Poca"))
            pocas++;
    }

    void OnTriggerExit(Collider outro)
    {
        if (outro.name.Contains("Poca"))
        {
            pocas--;

            if (pocas < 0)
                pocas = 0;
        }
    }

    // Controle do mouse
    public void TravarMouse(bool travar)
    {
        if (travar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
