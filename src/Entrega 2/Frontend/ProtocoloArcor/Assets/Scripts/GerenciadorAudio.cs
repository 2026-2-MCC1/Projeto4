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

    public MusicaDaCena[] musicas;

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

    //  troca a música conforme a cena (se for a mesma música, não reinicia)
    void TocarMusicaDaCena(string nomeCena)
    {
        int i = 0;
        while (i < musicas.Length)
        {
            if (musicas[i].nomeCena == nomeCena)
            {
                if (fonteMusica.clip != musicas[i].musica)
                {
                    fonteMusica.clip = musicas[i].musica;
                    fonteMusica.Play();
                }
                return;
            }
            i = i + 1;
        }
        // cena sem música configurada
        fonteMusica.Stop();
        fonteMusica.clip = null;
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