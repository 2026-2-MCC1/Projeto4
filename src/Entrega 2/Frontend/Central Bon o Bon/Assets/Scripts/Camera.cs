using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Camera : MonoBehaviour
{
    public Transform characterBody;
    public Transform characterHead;

    float sensivilityY = 0.5f;
    float sensivilityX = 0.5f;
    float rotationX = 0f;
    float rotationY = 0f;

    float angleYmin = -90f;
    float angleYmax = 90f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void LateUpdate()
    {
        transform.position = characterHead.position;
    }
    void Update()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * sensivilityY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * sensivilityX;

        rotationX += horizontalDelta;
        rotationY += verticalDelta;

        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYmax);

        characterBody.localEulerAngles = new Vector3(0f, rotationX, 0f);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }

}




