using UnityEngine;

public class GosmaLenta : MonoBehaviour
{
    [Tooltip("0.4 = player anda com 40% da velocidade normal")]
    [Range(0.1f, 1f)] public float fatorLentidao = 0.4f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var raiz = other.transform.root;
        if (raiz.GetComponentInChildren<ControleLentidao>() == null)
            raiz.gameObject.AddComponent<ControleLentidao>();

        ControleLentidao.Aplicar(fatorLentidao);
    }

    void OnDrawGizmos()
    {
        var col = GetComponent<BoxCollider>();
        if (col == null) return;
        Gizmos.color = new Color(0.3f, 1f, 0.2f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(col.center, col.size);
    }
}
