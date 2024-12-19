using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class Round_Manager : MonoBehaviour
{
    [SerializeField] private int currentRound = 1;

    [SerializeField] private int playerScore = 0;
    [SerializeField] private int aiScore = 0;

    [SerializeField] private Text roundText;
    [SerializeField] private Text whoWonText;
    [SerializeField] private Text roundCountDownText;
    [SerializeField] private Text nextRoundText;
    [SerializeField] private string whoWon;
    [SerializeField] public static bool isRoundActive = false;
    [SerializeField] private GameObject nextRoundUI;
    [SerializeField] public bool inNextRoundUI = false;
    

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform aiTransform;
    public static Round_Manager roundManagerScript;

    private Vector3 playerStartPos;
    private Vector3 aiStartPos;

    private void Awake()
    {
        if (roundManagerScript == null)
        {
            roundManagerScript = this;
        }
    }
    void Start()
    {
        playerStartPos = playerTransform.position;
        aiStartPos = aiTransform.position;
        StartRound();
    }

    public void StartRound()
    {
        if(playerScore < 15 && aiScore < 15)
        {
            
            isRoundActive = true;
            roundText.text = "Round: " + currentRound;

            ResetPlayerStatus();

            if (currentRound == 1)
            {
                whoWonText.text = "Game is starting";
                StartCoroutine(NextRoundScreen());
            }
            else
            {
                whoWonText.text = whoWon+" Won";
                StartCoroutine(NextRoundScreen());
            }
        }
        else
        {
            //Game Finish
        }
    }

    public void ResetPlayerStatus()
    {
        playerTransform.position = playerStartPos;
        aiTransform.position = aiStartPos;

        AiController.aiScript.ResetStamina();
        Player_Movement_Combat.playerScript.ResetStamina();
    }
    public void EndRound(string whoHit)
    {
        isRoundActive = false;

        if (whoHit == "player")
        {
            playerScore++;
            whoWon = "Player 1";
        }
        else if (whoHit == "ai")
        {
            aiScore++;
            whoWon = "Player 2";
        }


        currentRound++;
        StartRound();
    }

    private void EndGame()
    {

    }

    private IEnumerator NextRoundScreen()
    {
        nextRoundUI.SetActive(true);
        inNextRoundUI = true;
        nextRoundText.text = "Round: " + currentRound;
        Time.timeScale = 0f;

        int countDown = 3;

        while (countDown > 0)
        {
            roundCountDownText.text = countDown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countDown--;
        }
        nextRoundUI.SetActive(false);
        inNextRoundUI = false;
        Time.timeScale = 1f;
    }
}
