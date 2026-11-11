
using UnityEngine;
using UnityEngine.InputSystem;

public class Bancada : MonoBehaviour
{
    [Header("Partes da Tortuguita")]
    [SerializeField] private GameObject casco;
    [SerializeField] private GameObject cabeca;
    [SerializeField] private GameObject bracoEsquerdo;
    [SerializeField] private GameObject bracoDireito;
    [SerializeField] private GameObject pernaEsquerda;
    [SerializeField] private GameObject pernaDireita;

    [SerializeField] private ObjetivoFase objetivoFase;

    private int pecasMontadas = 0;

    private bool jogadorPerto = false;

    private InputAction teclaE;

    private void Awake()
    {
        teclaE = new InputAction(
            name: "Montar",
            type: InputActionType.Button,
            binding: "<Keyboard>/e"
        );

        teclaE.performed += OnPressE;
    }

    private void OnEnable()
    {
        teclaE.Enable();
    }

    private void OnDisable()
    {
        teclaE.Disable();
    }

    private void OnDestroy()
    {
        teclaE.performed -= OnPressE;
        teclaE.Dispose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;

            Debug.Log("Você está na bancada!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }
    private void RegistrarMontagem()
    {
        pecasMontadas++;

        objetivoFase.RegistrarPecaMontada();

        if (pecasMontadas >= 6)
        {
            objetivoFase.ConcluirFase();
            Debug.Log("Tortuguita montada! Fase concluída!");
        }
    }

    private void OnPressE(InputAction.CallbackContext context)
    {
        if (!jogadorPerto)
            return;

        // Monta o casco
        if (TortuguitaInventory.PossuiPeca("Casco") && !casco.activeSelf)
        {
            casco.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Casco montado!");
            return;
        }

        // Monta a perna esquerda
        if (TortuguitaInventory.PossuiPeca("PernaEsquerda") && !pernaEsquerda.activeSelf)
        {
            pernaEsquerda.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Perna esquerda montada!");
            return;
        }

        // Monta a perna direita
        if (TortuguitaInventory.PossuiPeca("PernaDireita") && !pernaDireita.activeSelf)
        {
            pernaDireita.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Perna direita montada!");
            return;
        }

        // Monta o braço esquerdo
        if (TortuguitaInventory.PossuiPeca("BracoEsquerdo") && !bracoEsquerdo.activeSelf)
        {
            bracoEsquerdo.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Braço esquerdo montado!");
            return;
        }

        // Monta o braço direito
        if (TortuguitaInventory.PossuiPeca("BracoDireito") && !bracoDireito.activeSelf)
        {
            bracoDireito.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Braço direito montado!");
            return;
        }

        // Monta a cabeça
        if (TortuguitaInventory.PossuiPeca("Cabeca") && !cabeca.activeSelf)
        {
            cabeca.SetActive(true);
            RegistrarMontagem();
            Debug.Log("Cabeça montada!");
            return;
        }

        Debug.Log("Não há mais peças disponíveis para montagem.");
    }
}
