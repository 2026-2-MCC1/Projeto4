using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Configuração")]
    public string cenaDoJogo = "Introducao";
    public string linkRepositorio = "https://github.com/2026-2-MCC1/Projeto4";

    [Header("Painéis")]
    public GameObject MenuPrincipal;
    public GameObject PanelSettings;
    public GameObject PanelCredits;
    public GameObject LogoProtocoloArcor;

    [Header("Volume / Vídeo")]
    public VideoPlayer VideoPlayer;
    public Slider SliderVolume;
    public TMP_Text VolumeValue;

    AudioSource fonteAudioVideo;

    // Ao iniciar, mostra só o menu principal
    void Start()
    {
        MenuPrincipal.SetActive(true);
        PanelSettings.SetActive(false);
        PanelCredits.SetActive(false);
        if (LogoProtocoloArcor != null) LogoProtocoloArcor.SetActive(true);

      
    }
    // ------------------------------------------------------------ Navegação

    public void Jogar()
    {
        SceneManager.LoadScene(cenaDoJogo);
    }

    public void AbrirLinkRepositorio()
    {
        Application.OpenURL(linkRepositorio);
    }

    // Configurações
    public void AbrirOpcoes()
    {
        MenuPrincipal.SetActive(false);
        LogoProtocoloArcor.SetActive(false);
        PanelSettings.SetActive(true);
    }

    public void FecharOpcoes()
    {
        PanelSettings.SetActive(false);
        MenuPrincipal.SetActive(true);
        LogoProtocoloArcor.SetActive(true);
    }

    // Créditos
    public void AbrirCreditos()
    {
        MenuPrincipal.SetActive(false);
        PanelCredits.SetActive(true);
        LogoProtocoloArcor.SetActive(false);
    }

    public void FecharCreditos()
    {
        PanelCredits.SetActive(false);
        MenuPrincipal.SetActive(true);
        LogoProtocoloArcor.SetActive(true);
    }

    // ------------------------------------------------------------ Volume

    public void MudarVolume(float valor)
    {
        AudioListener.volume = valor;

        if (VolumeValue != null)
            VolumeValue.text = Mathf.RoundToInt(valor * 100) + "%";

        if (VideoPlayer != null && VideoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
            VideoPlayer.SetDirectAudioVolume(0, valor);
    }

    public void SairJogo()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}