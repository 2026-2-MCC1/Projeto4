using UnityEngine;

public class PickupItems : MonoBehaviour
{
    bool _canPickup;
    [SerializeField] GameObject _object;

    void Update()
    {
        if (_canPickup && Input.GetKeyDown(KeyCode.E))
        {
            this.transform.parent = _object.transform;
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _canPickup = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _canPickup = false;
    }

}
