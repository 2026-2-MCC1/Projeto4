using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class Inventario : MonoBehaviour
{
    public float alcance = 3f;

    readonly Dictionary<string, int> itens = new Dictionary<string, int>();
    ItemColetavel mirando;
    bool aberto;
    string mensagem;
    float mensagemAte;

    [Tooltip("Mostra na tela em qual objeto o raio está batendo")]
    public bool debug = true;
    string acertouDebug = "-";

    void Update()
    {
        mirando = null;
        acertouDebug = "nada";

        var hits = Physics.RaycastAll(transform.position, transform.forward, alcance, ~0, QueryTriggerInteraction.Collide);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var h in hits)
        {
            if (h.collider.transform.root == transform.root) continue;

            var item = h.collider.GetComponentInParent<ItemColetavel>();
            if (item != null) { mirando = item; acertouDebug = item.name; break; }

            if (h.collider.isTrigger) continue;

            acertouDebug = h.collider.name;
            break;
        }

        if (debug) Debug.DrawRay(transform.position, transform.forward * alcance, Color.green);

        if (mirando != null && Apertou(Tecla.E)) Pegar(mirando);
        if (Apertou(Tecla.I)) aberto = !aberto;
    }

    void Pegar(ItemColetavel item)
    {
        itens.TryGetValue(item.nome, out int atual);
        itens[item.nome] = atual + item.quantidade;

        mensagem = $"+{item.quantidade} {item.nome}";
        mensagemAte = Time.time + 2f;

        Destroy(item.gameObject);
    }

    public bool Tem(string nome, int qtd = 1) =>
        itens.TryGetValue(nome, out int q) && q >= qtd;

    public bool Remover(string nome, int qtd = 1)
    {
        if (!Tem(nome, qtd)) return false;
        itens[nome] -= qtd;
        if (itens[nome] <= 0) itens.Remove(nome);
        return true;
    }

    void OnGUI()
    {
        var estilo = new GUIStyle(GUI.skin.label) { fontSize = 22, alignment = TextAnchor.MiddleCenter };

        GUI.Label(new Rect(0, Screen.height / 2f - 20, Screen.width, 40), "+", estilo);
        if (debug)
            GUI.Label(new Rect(0, Screen.height - 50, Screen.width, 40), $"Mirando: {acertouDebug}", estilo);

        if (mirando != null)
            GUI.Label(new Rect(0, Screen.height * 0.6f, Screen.width, 40), $"[E] Pegar {mirando.nome}", estilo);

        if (Time.time < mensagemAte)
            GUI.Label(new Rect(0, Screen.height * 0.65f, Screen.width, 40), mensagem, estilo);

        if (!aberto) return;
        float w = 300, h = 60 + itens.Count * 32;
        var area = new Rect(20, 20, w, Mathf.Max(h, 100));
        GUI.Box(area, "");

        var titulo = new GUIStyle(GUI.skin.label) { fontSize = 24, fontStyle = FontStyle.Bold };
        var linha = new GUIStyle(GUI.skin.label) { fontSize = 20 };
        GUI.Label(new Rect(35, 25, w, 32), "Inventário", titulo);

        if (itens.Count == 0)
        {
            GUI.Label(new Rect(35, 60, w, 30), "(vazio)", linha);
            return;
        }

        float y = 60;
        foreach (var kv in itens)
        {
            GUI.Label(new Rect(35, y, w, 30), $"{kv.Key}  x{kv.Value}", linha);
            y += 32;
        }
    }

    enum Tecla { E, I }

    bool Apertou(Tecla t)
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return false;
        return t == Tecla.E ? Keyboard.current.eKey.wasPressedThisFrame
                            : Keyboard.current.iKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(t == Tecla.E ? KeyCode.E : KeyCode.I);
#endif
    }
}
