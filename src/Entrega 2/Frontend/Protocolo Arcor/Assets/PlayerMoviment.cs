using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    public float speed = 5;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * x + transform.forward * z;
        controller.Move(movimento * speed * Time.deltaTime);
    }
}