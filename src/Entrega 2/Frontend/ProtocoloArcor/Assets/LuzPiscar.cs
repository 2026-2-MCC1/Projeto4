using UnityEngine;

public class LuzPiscar : MonoBehaviour
{
    private Light luzVermelha;
    public float minTempo = 0.1f;
    public float maxTempo = 0.5f;
    private float proximoPiscar;

    void Start()
    {
        luzVermelha = GetComponent<Light>();
    }

    void Update()
    {
        if (Time.time > proximoPiscar)
        {
            luzVermelha.enabled = !luzVermelha.enabled;
            proximoPiscar = Time.time + Random.Range(minTempo, maxTempo);
        }
    }
}