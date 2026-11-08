
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public float sensibilidade = 200f;

    private float rotacaoVertical = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidade * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade * Time.deltaTime;

        rotacaoVertical -= mouseY;
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotacaoVertical, 0f, 0f);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
