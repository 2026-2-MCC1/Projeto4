using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Instância única do gerenciador para facilitar seu acesso em outras classes.
    public static GameManager Instance;

    // Referências aos elementos da interface da tela de Game Over.
    public GameObject gameOverScreen; 
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;
    public Button menuButton;

    // Indica se a tela de Game Over já foi ativada.
    private bool gameOverActive = false;

    private void Awake()
    {
        // Cria a instância do GameManager, evitando duplicações.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Mantém a tela de Game Over oculta no início do jogo.
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        // Associa o botão de reiniciar à função que recarrega a fase.
        if (reiniciarButton != null)
            reiniciarButton.onClick.AddListener(ReiniciarScene);

        // Associa o botão de menu à função que abre a cena Menu.
        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica os comandos de teclado somente após o Game Over.
        if (gameOverActive)
        {
            // Pressionar R reinicia a fase atual.
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarScene();
            }
            // Pressionar ESC ou M retorna ao menu principal.
            if (Input.GetKeyDown(KeyCode.Escape))  Input.GetKeyDown(KeyCode.M);
            {
                GoToMenu();
            }

        }
    }
    public void GameOver()
    {
        // Impede que o Game Over seja ativado repetidamente.
        if (gameOverActive) return;

        gameOverActive = true;

        // Exibe a interface de fim de jogo.
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        // Define a mensagem e os comandos exibidos na tela.
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER \n\nR - Reiniciar\n ESC - Main Menu";
        }

    }
    public void ReiniciarScene()
    {
        // Restabelece a velocidade normal do jogo antes de recarregar a fase.
        Time.timeScale = 1f;

        // Recarrega a cena que está sendo executada.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMenu()
    {
        // Restabelece a velocidade normal do jogo antes de trocar de cena.
        Time.timeScale = 1f;

        // Carrega a cena do menu principal, que deve se chamar "Menu".
        SceneManager.LoadScene("Menu");
    }
}



