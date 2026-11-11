using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Peças colocadas")]
    public bool corpoColocado;
    public bool cabecaColocada;
    public bool pataColocada;
    public bool pata1Colocada;
    public bool pernaColocada;
    public bool perna1Colocada;
    public CharacterController characterController;
    [SerializeField] float speed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter: " + other.gameObject.name);
        switch (other.tag)
        {
            case "Corpo": corpoColocado = true; break;
            case "Cabeca": cabecaColocada = true; break;
            case "Pata": pataColocada = true; break;
            case "Pata1": pata1Colocada = true; break;
            case "Perna": pernaColocada = true; break;
            case "Perna1": perna1Colocada = true; break;
            default: return; // não é uma peça
        }
        
        Debug.Log("Peça " + other.tag + " coletada!");
        //Destroy(other.gameObject); // remove a peça solta

    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0f, z);
        movement = transform.TransformDirection(movement);
        characterController.Move(movement * speed * Time.deltaTime);
    }
    
}