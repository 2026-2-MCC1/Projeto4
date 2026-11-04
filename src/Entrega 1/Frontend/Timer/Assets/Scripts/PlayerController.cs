using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cam; // arraste a câmera aqui no Inspector
    float speed = 10f;
    public float mouseSensibility = 150f;
    float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // trava o mouse no centro
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        // Gira o CORPO para esquerda/direita
        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        // Gira a CÂMERA para cima/baixo
        cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.Translate(0, 0, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.Translate(0, 0, -speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}