using UnityEngine;

// Balança o objeto enquanto o jogador anda. Coloque dentro do Jogador (nas mãos, por exemplo).
public class Balanco : MonoBehaviour
{
    public float velocidadeBalanco = 8f;
    public float altura = 0.03f;
    public float lado = 0.02f;

    CharacterController controle;
    Vector3 posicaoInicial;
    float tempo = 0f;

    void Start()
    {
        controle = GetComponentInParent<CharacterController>();
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        if (controle == null)
        {
            return;
        }

        Vector3 velocidade = controle.velocity;
        velocidade.y = 0;

        if (velocidade.magnitude > 0.1f)
        {
            tempo = tempo + Time.deltaTime * velocidadeBalanco;
            float x = Mathf.Sin(tempo) * lado;
            float y = Mathf.Sin(tempo * 2f) * altura;
            transform.localPosition = posicaoInicial + new Vector3(x, y, 0);
        }
        else
        {
            transform.localPosition = posicaoInicial;
        }
    }
}
