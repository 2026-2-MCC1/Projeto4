using UnityEngine;

public class Pegar : MonoBehaviour
{
    public Camera cam;
    public Transform mao;

    Rigidbody objeto;

    void Update()
    {

        Debug.DrawRay(cam.transform.position, cam.transform.forward * 3f, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Apertou E");

            if (objeto == null)
                Pegar_();
            else
                Soltar();
        }
    }

    void Pegar_()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f))
        {
            Debug.Log("Raio acertou: " + hit.collider.name);

            if (hit.rigidbody != null)
            {
                Debug.Log("Tem Rigidbody, pegando: " + hit.rigidbody.name);
                objeto = hit.rigidbody;
                objeto.isKinematic = true;
                objeto.GetComponent<Collider>().enabled = false;
                objeto.transform.SetParent(mao);
                objeto.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.Log("Acertou, mas não tem Rigidbody");
            }
        }
        else
        {
            Debug.Log("O raio não acertou nada");
        }
    }

    void Soltar()
    {
        Debug.Log("Soltando");
        objeto.transform.SetParent(null);
        objeto.GetComponent<Collider>().enabled = true;
        objeto.isKinematic = false;
        objeto = null;
    }
}
