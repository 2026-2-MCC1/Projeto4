using UnityEngine;

public class CuraLentidao : MonoBehaviour
{
    [Tooltip("Se marcado, o objeto some depois de ser coletado")]
    public bool destruirAoUsar = true;

    [Tooltip("Opcional: som tocado ao curar")]
    public AudioClip somCura;

    void Reset()
    {
        // Garante que o collider do objeto seja Trigger ao adicionar o script
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Só reage se quem encostou for o player (o que tem o ControleLentidao)
        if (other.GetComponentInParent<ControleLentidao>() == null) return;

        // Se o player não está lento, não gasta o objeto
        if (!ControleLentidao.Lento) return;

        ControleLentidao.Curar();

        if (somCura != null)
            AudioSource.PlayClipAtPoint(somCura, transform.position);

        if (destruirAoUsar)
            Destroy(gameObject);
    }
}