using UnityEngine;

public class MovimentoSimples : MonoBehaviour
{
    public float velocidade = 5f;
    public float sensibilidadeMouse = 2f;
    private CharacterController controller;
    private Transform camTransform;
    private float rotacaoX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimento WASD
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        Vector3 movimento = (transform.right * moveH + transform.forward * moveV) * velocidade;
        controller.Move(movimento * Time.deltaTime);

        // Movimento do Mouse (Câmera e Corpo)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        transform.Rotate(Vector3.up * mouseX);

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -80f, 80f);
        camTransform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }
}