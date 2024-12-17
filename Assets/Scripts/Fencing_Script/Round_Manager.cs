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

    public Text roundText;

    [SerializeField] public static bool isRoundActive = false;

    public static Round_Manager roundManagerScript;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform aiTransform;

    private Vector3 playerStartPos;
    private Vector3 aiStartPos;

    void Start()
    {
        if (roundManagerScript == null)
        {
            roundManagerScript = this;
        }

        playerStartPos = playerTransform.position;
        aiStartPos = aiTransform.position;

        StartRound();
    }

    private void StartRound()
    {
        if(playerScore < 15 && aiScore < 15)
        {
            
            isRoundActive = true;
            roundText.text = "Round: " + currentRound;

            ResetPlayerStatus();
            StartCoroutine(Pause_Menu.pauseMenuScript.CountDown());
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
        }
        else if (whoHit == "ai")
        {
             aiScore++;
        }

        currentRound++;
        StartRound();
    }

    private void EndGame()
    {

    }
}
