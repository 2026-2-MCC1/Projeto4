
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CollectItem : MonoBehaviour
{
    private bool mayCollectItem = false;
    private Collider itemAtual;
    [SerializeField] private ObjetivoFase objetivoFase;
    private InputAction teclaE;

    // Tags dos objetos que podem ser coletados
    [SerializeField]
    private List<string> tagsColetaveis = new List<string>()
    {
        "Casco",
        "Cabeca",
        "BracoEsquerdo",
        "BracoDireito",
        "PernaEsquerda",
        "PernaDireita"
    };

    private void Awake()
    {
        teclaE = new InputAction(
            name: "Interagir",
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

    private bool IsColetavel(Collider other)
    {
        return tagsColetaveis.Contains(other.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsColetavel(other))
        {
            Debug.Log("Pode coletar: " + other.name);

            mayCollectItem = true;
            itemAtual = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == itemAtual)
        {
            Debug.Log("Saiu da área: " + other.name);

            mayCollectItem = false;
            itemAtual = null;
        }
    }

    private void OnPressE(InputAction.CallbackContext context)
    {
        if (mayCollectItem && itemAtual != null)
        {
            string tagPeca = itemAtual.tag;

            TortuguitaInventory.RegistrarPeca(
                tagPeca,
                itemAtual.gameObject
            );

            Debug.Log("Você coletou: " + tagPeca);
            objetivoFase.RegistrarPecaEncontrada();

            mayCollectItem = false;
            itemAtual = null;
        }
    }

}
