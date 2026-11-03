using UnityEngine;

// Fica no Jogador na Fase 2: o raio que enxerga as válvulas e o manual.
public class InteragirFase2 : MonoBehaviour
{
    public GerenciadorFase2 gerenciador;   // o gerador da cena já liga
    public float distancia = 3f;
    public string textoInteracao = "";

    Transform cabeca;

    void Start()
    {
        cabeca = GetComponent<Jogador>().cabeca;
    }

    void Update()
    {
        textoInteracao = "";

        if (gerenciador == null || gerenciador.podeInteragir == false)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(cabeca.position, cabeca.forward, out hit, distancia, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Valvula valvula = hit.collider.GetComponent<Valvula>();
            NotaManual manual = hit.collider.GetComponent<NotaManual>();

            if (valvula != null)
            {
                textoInteracao = "[E] Girar válvula " + valvula.cor;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gerenciador.AcionarValvula(valvula);
                }
            }

            if (manual != null)
            {
                textoInteracao = "[E] Ler manual de pressão";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gerenciador.LerManual(manual);
                }
            }
        }
    }
}
