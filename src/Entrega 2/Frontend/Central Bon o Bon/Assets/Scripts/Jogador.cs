using UnityEngine;

// Jogador em primeira pessoa: anda, olha e interage com o E.
public class Jogador : MonoBehaviour
{
    public Transform cabeca;               // objeto que tem a câmera dentro
    public GerenciadorFase gerenciador;    // arraste o GerenciadorFase aqui
    public float velocidade = 4f;
    public float sensibilidade = 2f;
    public float distanciaInteracao = 3f;
    public float raioMira = 0.15f;         // espessura do raio de interação (metros)
    public LayerMask camadaInteracao;
    public bool podeMover = true;
    public string textoInteracao = "";
    public int pocas = 0;                  // quantas poças de baba estou pisando (Fase 2)
    public float intervaloPasso = 0.5f;
    float tempoProximoPasso = 0f;
    CharacterController controle;

    float rotacaoX = 0f;
    float velocidadeY = 0f;

    void Start()
    {
        controle = GetComponent<CharacterController>();

        if (gerenciador == null)
        {
            gerenciador = FindFirstObjectByType<GerenciadorFase>();
        }

        TravarMouse(true);
    }

    void Update()
    {
        if (podeMover == true)
        {
            Olhar();
            Andar();
            Interagir();
        }
    }

    void Olhar()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidade;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade;

        // o corpo gira para os lados e a cabeça para cima e para baixo
        transform.Rotate(0, mouseX, 0);
        rotacaoX = rotacaoX - mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -80f, 80f);
        cabeca.localEulerAngles = new Vector3(rotacaoX, 0, 0);
    }

    void Andar()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * horizontal + transform.forward * vertical;
        movimento = movimento * velocidade;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movimento = movimento * 1.6f;
        }

        // gravidade
        if (controle.isGrounded && velocidadeY < 0)
        {
            velocidadeY = -2f;
        }
        velocidadeY = velocidadeY - 20f * Time.deltaTime;
        movimento.y = velocidadeY;

        controle.Move(movimento * Time.deltaTime);
        // passos: só toca andando e no chão
        Vector3 velHorizontal = new Vector3(controle.velocity.x, 0, controle.velocity.z);
        if (controle.isGrounded && velHorizontal.magnitude > 0.5f && Time.time >= tempoProximoPasso)
        {
            float intervalo = intervaloPasso;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                intervalo = intervalo / 1.6f; // correndo, passos mais rápidos
            }
            tempoProximoPasso = Time.time + intervalo;

            if (GerenciadorSom.instancia != null)
            {
                GerenciadorSom.instancia.TocarPasso();
            }
        }
    }

    void Interagir()
    {
        textoInteracao = "";

        if (gerenciador == null)
        {
            return;
        }

        // "raio grosso" (SphereCast): perdoa a mira e ignora triggers
        RaycastHit hit;
        if (Physics.SphereCast(cabeca.position, raioMira, cabeca.forward, out hit, distanciaInteracao, camadaInteracao, QueryTriggerInteraction.Ignore))
        {
            // procura o script no objeto do collider ou no pai dele
            Pista pista = hit.collider.GetComponentInParent<Pista>();

            Debug.Log("Acertou: " + hit.collider.name + " | tem Pista: " + (pista != null));

            if (pista != null)
            {
                textoInteracao = "[E] Pegue a bala";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gerenciador.PegarBala();
                }
            }
        }
    }

    // poças de baba (Fase 2): os objetos delas têm "Poca" no nome
    void OnTriggerEnter(Collider outro)
    {
        if (outro.name.Contains("Poca"))
        {
            pocas = pocas + 1;
        }
    }

    void OnTriggerExit(Collider outro)
    {
        if (outro.name.Contains("Poca"))
        {
            pocas = pocas - 1;
            if (pocas < 0)
            {
                pocas = 0;
            }
        }
    }

    // true = mouse preso (jogando), false = mouse livre (menus)
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