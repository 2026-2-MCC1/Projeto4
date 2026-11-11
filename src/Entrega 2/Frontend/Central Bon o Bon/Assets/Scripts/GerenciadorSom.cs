using UnityEngine;

// Controla os sons do jogo: chuva (começa sozinha), passos, vitória e confirmar.
public class GerenciadorSom : MonoBehaviour
{
    public static GerenciadorSom instancia;

    public AudioClip somChuva;
    public AudioClip somVitoria;
    public AudioClip somConfirmar;
    public AudioClip[] somPassos;

    [Range(0f, 1f)] public float volumeChuva = 0.6f;
    [Range(0f, 1f)] public float volumeChuvaBaixa = 0.15f;
    [Range(0f, 1f)] public float volumePasso = 0.5f;

    AudioSource fonteChuva;
    AudioSource fonteEfeitos;
    bool abaixando = false;

    void Awake()
    {
        instancia = this;

        // garante que o áudio global não está mudo/pausado
        AudioListener.volume = 1f;
        AudioListener.pause = false;

        fonteChuva = gameObject.AddComponent<AudioSource>();
        fonteChuva.clip = somChuva;
        fonteChuva.loop = true;
        fonteChuva.playOnAwake = false;
        fonteChuva.spatialBlend = 0f; // som 2D, ouve de qualquer lugar
        fonteChuva.volume = volumeChuva;

        fonteEfeitos = gameObject.AddComponent<AudioSource>();
        fonteEfeitos.playOnAwake = false;
        fonteEfeitos.spatialBlend = 0f;
    }

    void Start()
    {
        // se não houver nenhum Audio Listener na cena, cria um
        if (FindFirstObjectByType<AudioListener>() == null)
        {
            Debug.LogWarning("GerenciadorSom: NÃO havia Audio Listener na cena. Adicionei um na câmera.");
            if (Camera.main != null)
            {
                Camera.main.gameObject.AddComponent<AudioListener>();
            }
            else
            {
                gameObject.AddComponent<AudioListener>();
            }
        }

        Debug.Log("GerenciadorSom: Chuva=" + (somChuva != null) +
                  " | Vitoria=" + (somVitoria != null) +
                  " | Confirmar=" + (somConfirmar != null) +
                  " | Passos=" + (somPassos != null ? somPassos.Length : 0));

        if (somChuva != null)
        {
            fonteChuva.Play();
            Debug.Log("GerenciadorSom: tocando chuva. isPlaying = " + fonteChuva.isPlaying);
        }
        else
        {
            Debug.LogWarning("GerenciadorSom: o campo Som Chuva está VAZIO no Inspector!");
        }
    }

    void Update()
    {
        if (abaixando == true)
        {
            fonteChuva.volume = Mathf.MoveTowards(fonteChuva.volume, volumeChuvaBaixa, Time.deltaTime * 0.3f);
        }

        // TESTE: aperte T para tocar o som de confirmar
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Teste: tocando Confirmar");
            TocarConfirmar();
        }
    }

    public void TocarVitoria()
    {
        if (somVitoria != null) fonteEfeitos.PlayOneShot(somVitoria);
        else Debug.LogWarning("GerenciadorSom: Som Vitoria vazio!");
    }

    public void TocarConfirmar()
    {
        if (somConfirmar != null) fonteEfeitos.PlayOneShot(somConfirmar);
        else Debug.LogWarning("GerenciadorSom: Som Confirmar vazio!");
    }

    public void TocarPasso()
    {
        if (somPassos != null && somPassos.Length > 0)
        {
            AudioClip clip = somPassos[Random.Range(0, somPassos.Length)];
            if (clip != null) fonteEfeitos.PlayOneShot(clip, volumePasso);
            else Debug.LogWarning("GerenciadorSom: elemento de Som Passos está vazio!");
        }
        else
        {
            Debug.LogWarning("GerenciadorSom: lista Som Passos está vazia!");
        }
    }

    public void TocarPasso(bool correndo)
    {
        TocarPasso();
    }

    public void AbaixarChuva()
    {
        abaixando = true;
    }
}