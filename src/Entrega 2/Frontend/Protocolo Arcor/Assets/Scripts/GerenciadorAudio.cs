using UnityEngine;
using UnityEngine.SceneManagement;


public class GerenciadorAudio : MonoBehaviour
{
    [System.Serializable]
    public class MusicaDaCena
    {
        public string nomeCena;      // nome exato da cena
        public AudioClip musica;
       
    }

    public static GerenciadorAudio Instancia;

    // Exceções: cenas com música própria (ex.: introdução com chuva, encerramento)
    public MusicaDaCena[] musicas;

    // Toca em toda cena que NÃO estiver na lista acima
    public AudioClip musicaPadrao;
    public AudioClip somVitoria;
    public AudioClip somDerrota;

    [Range(0f, 1f)] public float volumeMusica = 0.5f;
    [Range(0f, 1f)] public float volumeEfeitos = 0.8f;

    AudioSource fonteMusica;
    AudioSource fonteEfeitos;
    bool mudo = false;

    void Awake()
    {
        // só pode existir um gerenciador no jogo
        if (Instancia != null)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        fonteMusica = gameObject.AddComponent<AudioSource>();
        fonteMusica.loop = true;
        fonteMusica.playOnAwake = false;

        fonteEfeitos = gameObject.AddComponent<AudioSource>();
        fonteEfeitos.playOnAwake = false;

        // lembra se o jogador deixou o som desligado
        if (PlayerPrefs.GetInt("Mudo", 0) == 1)
        {
            mudo = true;
        }
        AplicarVolumes();

        SceneManager.sceneLoaded += AoCarregarCena;
    }

    void Start()
    {
        TocarMusicaDaCena(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        if (Instancia == this)
        {
            SceneManager.sceneLoaded -= AoCarregarCena;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AlternarMudo();
        }
    }

    void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        TocarMusicaDaCena(cena.name);
    }

    // Troca a música conforme a cena.
    // Se a cena não estiver na lista, toca a música padrão.
    // Se já estiver tocando a mesma música, não reinicia.
    void TocarMusicaDaCena(string nomeCena)
    {
        AudioClip escolhida = musicaPadrao;

        int i = 0;
        while (i < musicas.Length)
        {
            if (musicas[i].nomeCena == nomeCena)
            {
                escolhida = musicas[i].musica;
                break;
            }
            i = i + 1;
        }

        if (fonteMusica.clip == escolhida && fonteMusica.isPlaying)
        {
            return;
        }

        if (escolhida == null)
        {
            fonteMusica.Stop();
            fonteMusica.clip = null;
            return;
        }

        fonteMusica.clip = escolhida;
        fonteMusica.Play();
    }
    public void TocarVitoria()
    {
        if (somVitoria == null)
        {
            return;
        }

        // para a música de fundo para o som de vitória aparecer
        fonteMusica.Stop();
        fonteMusica.clip = null;

        fonteEfeitos.PlayOneShot(somVitoria);
    }

    public void TocarDerrota()
    {
        if (somDerrota == null)
        {
            return;
        }

        // para a música de fundo para o som de derrota aparecer
        fonteMusica.Stop();
        fonteMusica.clip = null;

        fonteEfeitos.PlayOneShot(somDerrota);
    }
    // PROCEDIMENTO: liga e desliga o som
    public void AlternarMudo()
    {
        mudo = !mudo;
        if (mudo == true)
        {
            PlayerPrefs.SetInt("Mudo", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Mudo", 0);
        }
        PlayerPrefs.Save();
        AplicarVolumes();
    }

    // diz se o som está desligado
    public bool EstaMudo()
    {
        return mudo;
    }

    void AplicarVolumes()
    {
        if (mudo == true)
        {
            fonteMusica.volume = 0f;
            fonteEfeitos.volume = 0f;
        }
        else
        {
            fonteMusica.volume = volumeMusica;
            fonteEfeitos.volume = volumeEfeitos;
        }
    }

    // Ao reiniciar a cena, a música da cena volta a tocar sozinha.
    public static void ParaMusica()
    {
        if (Instancia == null)
        {
            return;
        }
        Instancia.fonteMusica.Stop();
        Instancia.fonteMusica.clip = null;
    }

    // Se não existir gerenciador ou o clip for vazio, não faz nada.
    public static void Efeito(AudioClip clip)
    {
        if (Instancia == null || clip == null)
        {
            return;
        }
        Instancia.fonteEfeitos.PlayOneShot(clip);
    }
}