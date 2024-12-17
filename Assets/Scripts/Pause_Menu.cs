using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public static bool isPaused = false;
    
    [SerializeField] private GameObject countDownUI;
    [SerializeField] public TextMeshProUGUI countDownText;
    void Start()
    {
        pauseMenu.SetActive(false);
        countDownUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                StartCoroutine(ResumeGame());
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

    private IEnumerator ResumeGame()
    {
        pauseMenu.SetActive(false);

        countDownUI.SetActive(true);

        int countDown = 3;

        while (countDown > 0)
        {
            countDownText.text=countDown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countDown--;
        }
        countDownUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void GoToAtariMenu()
    {

        //scene to atari menu
        Time.timeScale = 1f;
    }
    private void GoToMainMenu()
    {

        //scene to main menu
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
