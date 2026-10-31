using UnityEngine;

// Controla o Fúlvio em primeira pessoa: andar, olhar e interagir com o E.
public class Jogador : MonoBehaviour
{
    public Transform cabeca;          // objeto que tem a câmera dentro
    public float velocidade = 4f;
    public float sensibilidade = 2f;
    public float distanciaInteracao = 3f;

    public bool podeMover = true;
    public string textoInteracao = "";

    CharacterController controle;
    float rotacaoX = 0f;
    float velocidadeY = 0f;

    void Start()
    {
        controle = GetComponent<CharacterController>();
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

        // gira o corpo para os lados e a cabeça para cima/baixo
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
    }

    void Interagir()
    {
        textoInteracao = "";

        RaycastHit hit;
        if (Physics.Raycast(cabeca.position, cabeca.forward, out hit, distanciaInteracao))
        {
            Pista pista = hit.collider.GetComponent<Pista>();
            Painel painel = hit.collider.GetComponent<Painel>();

            if (pista != null)
            {
                textoInteracao = "[E] Ler bilhete";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GerenciadorFase.instancia.LerPista(pista);
                }
            }

            if (painel != null && painel.resolvido == false)
            {
                textoInteracao = "[E] Usar painel de controle";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GerenciadorFase.instancia.AbrirPainel();
                }
            }
        }
    }

    // true = mouse preso no centro (jogando), false = mouse livre (menus)
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
