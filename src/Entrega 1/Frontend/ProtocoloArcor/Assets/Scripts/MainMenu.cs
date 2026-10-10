using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class MainMenu : MonoBehaviour
{
    string linkRepositorio = "https://github.com/2026-2-MCC1/Projeto4";

    public GameObject MenuPrincipal, PanelSettings, PanelCredits, LogoProtocoloArcor;
    public VideoPlayer VideoPlayer;
    public TMP_Text VolumeValue;

    // Ao iniciar, mostra só o menu principal
    void Start()
    {
        MenuPrincipal.SetActive(true);
        PanelSettings.SetActive(false);
        PanelCredits.SetActive(false);
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Estacao7Belo");
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
    }

    // Volume
    public void MudarVolume(float valor)
    {
        AudioListener.volume = valor;
        VolumeValue.text = Mathf.RoundToInt(valor * 100) + "%";
        if (VideoPlayer != null)
        {
            if (VideoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
                VideoPlayer.SetDirectAudioVolume(0, valor);
            else if (VideoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
                VideoPlayer.GetTargetAudioSource(0).volume = valor;
        }
    }
    public void SairJogo()
    {
        Application.Quit();
    }
}