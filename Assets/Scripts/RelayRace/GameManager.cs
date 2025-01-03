using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] public static bool isPaused = false;

    [SerializeField] private GameObject countDownUI;
    [SerializeField] public TextMeshProUGUI countDownText;

    void Start()
    {
        if (gameManagerScript == null)
        {
            gameManagerScript = this;
        }

        pauseMenu.SetActive(false);
        countDownUI.SetActive(false);

        StartGame();
        startPanel.SetActive(true);
        countdownText.text = "3";
        isGameStarted = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&PlayerScript.playerScript.inGame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
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

            isGameStarted = true;
            Time.timeScale = 1f;
            startPanel.SetActive(false);
            isPaused = false;

    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        PlayerScript.playerScript.inGame = false;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        PlayerScript.playerScript.inGame = true;
        StartCoroutine(CountDown());
        
    }

    public IEnumerator CountDown()
    {
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
        Time.timeScale = 1f;
    }
    public void GoToSettings()
    {

        //scene to settings
    }
    public void GoToAtariMenu()
    {

        //scene to atari menu
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {

        //scene to main menu
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
