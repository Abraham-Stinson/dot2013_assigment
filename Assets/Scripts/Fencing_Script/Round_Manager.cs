using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class Round_Manager : MonoBehaviour
{
    [SerializeField] private bool isGoldenTouch= false;

    [SerializeField] public int playerScore = 0;
    [SerializeField] public int aiScore = 0;
    [SerializeField] private float timer;
    [SerializeField] private float setTime = 180f;
    [SerializeField] private int setCounter = 0;
    [SerializeField] private bool isSetTimerWorking = true;
    
    //[SerializeField] private int 
    [SerializeField] private Text setText;
    [SerializeField] private Text whoWonText;
    [SerializeField] private Text afterHitCountDownText;
    [SerializeField] private Text betweenSetMessageText;
    [SerializeField] private Text betweenSetCountDownText;
    [SerializeField] private Text player1ScoreOnBtwSetText;
    [SerializeField] private Text aiScoreOnBtwSetText;
    [SerializeField] private Text timerText;

    [SerializeField] private string whoWon;
    [SerializeField] public static bool isSetActive = false;
    [SerializeField] private GameObject afterHitUI;
    [SerializeField] private GameObject betweenSet;
    [SerializeField] public bool inNextRoundUI = false;

    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text aiScoreText;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform aiTransform;

    [SerializeField] private GameObject finishUI;
    [SerializeField] private Text winningText;

    [SerializeField] public bool isGameEnd = false;

    public static Round_Manager roundManagerScript;

    private Vector3 playerStartPos;
    private Vector3 aiStartPos;


    private AudioSource audioSource;


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
        setCounter = 1;
        StartRound();
        //afterHitUI.SetActive(false);
        betweenSet.SetActive(false);
        finishUI.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        audioSource.Play();
    }

    private void Update()
    {
        UpdatingUI();

        if (isSetTimerWorking)
        {
            UpdateTimerUI();
            setTime -= Time.deltaTime;
            if (setTime <= 0)
            {
                StartCoroutine(TimerFinished());
            }

        }
    }
    public void StartRound()
    {
        if (playerScore < 15 && aiScore < 15)
        {
            AiController.aiScript.isAiTakeDamage = false;
            Player_Movement_Combat.playerScript.isPlayerTakeDamage = false;

            isSetActive = true;
            ResetPlayerStatus();

            if (playerScore == 0 && aiScore == 0)
            {
                whoWonText.text = "Game is starting";
                StartCoroutine(NextRoundScreen());
            }
            else
            {
                
                if (isGoldenTouch)
                {
                    whoWonText.text = whoWon + " Hit";
                    EndGame(whoWon);
                }
                else
                {
                    whoWonText.text = whoWon + " Hit";
                    StartCoroutine(NextRoundScreen());
                }
            }
        }
        else
        {
            if (playerScore == 15)
            {
                EndGame("Player 1");
            }
            else
            {
                EndGame("Ai");
            }
        }
    }

    public void ResetPlayerStatus()
    {
        playerTransform.position = playerStartPos;
        aiTransform.position = aiStartPos;

        AiController.aiScript.aiSpriteRenderer.color = Color.white;
        Player_Movement_Combat.playerScript.playerSprite.color = Color.white;

        AiController.aiScript.canDoSomething = true;
        Player_Movement_Combat.playerScript.canAttackOrDefend = true;

        AiController.aiScript.isAiTakeDamage = false;
        Player_Movement_Combat.playerScript.isPlayerTakeDamage = false;

        AiController.aiScript.ResetStamina();
        Player_Movement_Combat.playerScript.ResetStamina();

        AiController.aiScript.movemetAction = 0;
    }
    public void EndRound(string whoHit)
    {
        Time.timeScale = 0;
        isSetActive = false;

        AiController.aiScript.AnimationManager("ai_idle");
        Player_Movement_Combat.playerScript.AnimationManager("player_idle");

        if (whoHit == "player")
        {
            if (Player_Movement_Combat.playerScript.isPlayerTakeDamage&&!AiController.aiScript.isAiTakeDamage)
            {
                return;
            }

            playerScore++;
            whoWon = "Player 1";
        }
        else if (whoHit == "ai")
        {
            if (AiController.aiScript.isAiTakeDamage&&!Player_Movement_Combat.playerScript.isPlayerTakeDamage)
            {
                return;
            }
            aiScore++;
            whoWon = "Ai";
        }

        StartRound();
    }

    private void EndGame(string win)
    {
        Time.timeScale = 0f;
        isGameEnd = true;
        Debug.LogError("Bitti oglumuz");
        finishUI.SetActive(true);
        if(win=="Player 1")
        {
            winningText.text = "You won, fighter.";
        }
        else
        {
            winningText.text = "Loooser";
        }

        //There will and return to atari menu for win or lose condition
    }

    private IEnumerator NextRoundScreen()
    {
        afterHitUI.SetActive(true);
        inNextRoundUI = true;
        isSetTimerWorking = false;

        int countDown = 3;

        while (countDown > 0)
        {
            afterHitCountDownText.text = countDown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countDown--;
        }
        afterHitUI.SetActive(false);
        inNextRoundUI = false;
        isSetTimerWorking = true;
        Time.timeScale = 1f;
    }

    private IEnumerator TimerFinished()
    {
        isSetTimerWorking = false;
        if (setCounter <= 3)
        {
            switch (setCounter)
            {
                case 1:
                    betweenSetMessageText.text = "First set finished.\nSecond Set Starting";
                    break;

                case 2:
                    betweenSetMessageText.text = "Second set finished.\nThird Set Starting";
                    break;
                case 3:
                    if (playerScore==aiScore)
                    {
                        betweenSetMessageText.text = "Third set finished.\nNow Golden Touch";
                        isGoldenTouch=true;
                    }
                    else if (playerScore>aiScore)
                    {
                        EndGame("Player 1");
                    }
                    else if (playerScore<aiScore)
                    {
                        EndGame("Ai");
                    }
                    break;
            }
            setCounter++;
            setTime = 180f;
            betweenSet.SetActive(true);

            player1ScoreOnBtwSetText.text = "Player Score: " + playerScore;
            aiScoreOnBtwSetText.text = "Ai Score: " + aiScore;
            inNextRoundUI = true;
            Time.timeScale = 0f;

            int countDown = 3;
            while (countDown > 0)
            {
                betweenSetCountDownText.text = countDown.ToString();
                yield return new WaitForSecondsRealtime(1f);
                countDown--;
            }
            betweenSet.SetActive(false);
            inNextRoundUI = false;
            Time.timeScale = 1f;

            isSetTimerWorking = true;
            ResetPlayerStatus();
        }
        else
        {
            isGoldenTouch = true;
        }
    }

    private void UpdateTimerUI(){
        int minutes = Mathf.FloorToInt(setTime / 60);
        int seconds = Mathf.FloorToInt(setTime % 60);

        timerText.text=$"{minutes:00}:{seconds:00}";
    } 
    private void UpdatingUI()
    {
        if (isGoldenTouch)
        {
            setText.text="Golden Touch";
        }
        else
        {
            setText.text = "Set: " + setCounter;
        }
        playerScoreText.text = "Score: " + playerScore;
        aiScoreText.text = "Score: " + aiScore;
        
    }

    

}
