using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public static bool isPaused = false;
    
    [SerializeField] private GameObject countDownUI;
    [SerializeField] public TextMeshProUGUI countDownText;
    

    public static Pause_Menu pauseMenuScript;
    private void Awake()
    {
        if (pauseMenuScript == null)
        {
            pauseMenuScript = this;
        }

    }
    void Start()
    {
        pauseMenu.SetActive(false);
        countDownUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Round_Manager.roundManagerScript.inNextRoundUI)
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

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(CountDown());
        isPaused = false;
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
