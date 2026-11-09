using UnityEngine;

// Válvula colorida da Fase 2. O jogador precisa girar na ordem certa.
public class Valvula : MonoBehaviour
{
    public string cor;          // "Azul", "Verde", "Vermelha" ou "Amarela"
    public bool ligada = false;

    Renderer rend;
    Color corPlena;

    void Start()
    {
        rend = GetComponent<Renderer>();
        corPlena = rend.material.color;
        Apagar();
    }

    public void Ligar()
    {
        ligada = true;
        rend.material.color = corPlena;
    }

    public void Apagar()
    {
        ligada = false;
        rend.material.color = new Color(corPlena.r * 0.35f, corPlena.g * 0.35f, corPlena.b * 0.35f, 1f);
    }
}
