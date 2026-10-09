using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverScreen;

    public TextMeshProUGUI gameOverText;

    public Button restartButton;

    public Button menuButton;

    private bool gameOverActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        if (restartButton != null) 
            restartButton.onClick.AddListener(RestartGame);

        if (menuButton != null)
            menuButton.onClick.AddListener(ReturnToMenu);

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                ReturnToMenu();
            }
        }
    }

    public void GameOver()
    {
        if (gameOverActive) return;
        gameOverActive = true;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over \n\n - Press R to Restart or M for Menu";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    

    
}
