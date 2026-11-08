using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       FindAnyObjectByType<ScoreManager>().AddPart();
    }
}
