using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaComItem : MonoBehaviour
{
    [Tooltip("Nome exato da cena da próxima fase")]
    public string proximaCena = "Fase2";

    [Tooltip("Nomes dos itens exigidos (iguais ao campo Nome de cada ItemColetavel)")]
    public string[] itensNecessarios =
    {
        "Bala Língua Ácida",
        "Bala Baba de Bruxa",
        "Bala Zumbi",
        "Bala Cabeça de Abóbora"
    };

    [Tooltip("Se marcado, os itens são gastos ao passar pela porta")]
    public bool consumirItens = true;

    string mensagem;
    float mensagemAte;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inventario = other.transform.root.GetComponentInChildren<Inventario>();
        if (inventario == null)
        {
            Debug.LogWarning("PortaComItem: Inventario não encontrado no player.");
            return;
        }

        var exigido = new Dictionary<string, int>();
        foreach (var item in itensNecessarios)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            var nome = item.Trim();
            exigido.TryGetValue(nome, out int q);
            exigido[nome] = q + 1;
        }

        int total = 0, tem = 0;
        var faltando = new List<string>();
        foreach (var kv in exigido)
        {
            total += kv.Value;
            int possui = 0;
            while (possui < kv.Value && inventario.Tem(kv.Key, possui + 1)) possui++;
            tem += possui;
            if (possui < kv.Value)
                faltando.Add(kv.Value - possui > 1 ? $"{kv.Value - possui}x {kv.Key}" : kv.Key);
        }

        if (faltando.Count > 0)
        {
            mensagem = $"Itens: {tem}/{total}\nFaltam: {string.Join(", ", faltando)}";
            mensagemAte = Time.time + 3f;
            return;
        }

        if (consumirItens)
            foreach (var kv in exigido)
                inventario.Remover(kv.Key, kv.Value);

        if (GerenciadorFasePlutonita.Instancia != null)
            GerenciadorFasePlutonita.Instancia.Vitoria();
        else
            SceneManager.LoadScene(proximaCena);
    }

    void OnGUI()
    {
        if (Time.time >= mensagemAte) return;
        var estilo = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        GUI.Label(new Rect(0, Screen.height * 0.25f, Screen.width, 80), mensagem, estilo);
    }
}
