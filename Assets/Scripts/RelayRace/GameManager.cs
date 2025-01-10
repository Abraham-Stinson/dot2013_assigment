using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerScript;

    [Header("UI Elements")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Game Settings")]
    [SerializeField] public bool isGameStarted = false;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    public static bool isPaused = false;
    [SerializeField] private GameObject countDownUI;
    [SerializeField] public TextMeshProUGUI countDownText;

    [Header("Game Finished Screen")]
    [SerializeField] private GameObject gameFinishedScreen;
    [SerializeField] private TextMeshProUGUI gameFinishedHeaderText;
    [SerializeField] private TextMeshProUGUI gameFinishedText;
    public bool isWon;

    [Header("Start Animation")]
    [SerializeField] private GameObject Starter;
    [SerializeField] private Animator StarterAnimator;

    [Header("Starter")]
    [SerializeField] private AudioSource starterAudio;

    public bool isPlayerWon=false;

    void Start()
    {
        if (gameManagerScript == null)
        {
            gameManagerScript = this;
        }

        pauseMenu.SetActive(false);
        countDownUI.SetActive(false);
        gameFinishedScreen.SetActive(false);

        StartGame();
        startPanel.SetActive(true);
        countdownText.text = "3";
        isGameStarted = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else if (PlayerScript.playerScript.inGame) // E?er oyun oynan?yorsa pause menüsüne gir
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            Time.timeScale = 0f;
            StartCoroutine(StartCountdown());
            
        }
    }

    private IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);
        if (!isGameStarted)
        {
            StarterAnimator.Play("relayRaceStartAnim");
            starterAudio.Play();
        }
        isGameStarted = true;
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        isPaused = false;
        PlayerScript.playerScript.inGame = true;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        PlayerScript.playerScript.inGame = false; // Oyuncu durduruldu
        Time.timeScale = 0f; // Zaman durdu
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(ResumeCountdown());
    }

    private IEnumerator ResumeCountdown()
    {
        isPaused = false; // Oyuna dönerken duraklama iptal edildi
        countDownUI.SetActive(true);
        Time.timeScale = 0f;

        int countDown = 3;

        while (countDown > 0)
        {
            countDownText.text = countDown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countDown--;
        }

        countDownUI.SetActive(false);
        PlayerScript.playerScript.inGame = true; // Oyuncu tekrar aktif
        Time.timeScale = 1f; // Zaman tekrar akt?
    }

    public void GoToAtariMenu()
    {
        if (isPlayerWon)
        {

            SceneManager.LoadScene("3D_Gameplay_Scene");
        }
        else
        {

            SceneManager.LoadScene("3D_Gameplay_Scene");
        }

        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        // scene to main menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndGame(bool isWin)
    {
        // End game logic
        AiScript.aiScript.isAvailable = false;
        PlayerScript.playerScript.currentSpeed = 0f;
        AiScript.aiScript.currentSpeed = 0f;
        PlayerScript.playerScript.inGame = false;
        Time.timeScale = 0f;
        if (isWin)
        {
            isWon = true;
            isPlayerWon = true;
            Debug.Log("You win");
            gameFinishedScreen.SetActive(true);
            gameFinishedHeaderText.text = "You Won";
            gameFinishedText.text = "You finished the relay race in 1st place.";
            Time.timeScale = 0f;
        }
        else
        {
            isWon=false;
            isPlayerWon = false;
            Debug.Log("Lose");
            gameFinishedScreen.SetActive(true);
            gameFinishedHeaderText.text = "You Lost";
            gameFinishedText.text = "You couldn't finish the relay race in 1st place, you lost.";
            Time.timeScale = 0f;
        }
    }
}
