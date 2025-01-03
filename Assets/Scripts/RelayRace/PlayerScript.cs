using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript playerScript;

    [Header("About Game")]
    [SerializeField] public bool inGame = false;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int currentWaypointIndex = 0;

    [Header("Speed")]
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float decelerationByTime = 1f;

    [SerializeField] private float currentSpeed;
    private float savedSpeed;

    [Header("Moving")]
    [SerializeField] private bool lastKeyWasA = false;
    [SerializeField] private bool isMoving = false;

    [Header("Laps")]
    [SerializeField] private int totalLaps = 4;
    [SerializeField] private int currentLap = 1;

    [Header("Input")]
    [SerializeField] private bool canInput = true;

    [Header("Animation")]
    Animator animator;
    private const string normalRunningPlayer = "Normal_Player_Runner";
    private const string topRunningPlayer = "Top_Player_Runner";
    private const string fliippedNormalRunningPlayer = "Flipped_Normal_Player_Runner";

    [Header("Quick Time Event")]
    [SerializeField] private string[] possibleKeys = { "Q", "W", "E", "R" };
    [SerializeField] private int comboLength = 5; // Başlangıç uzunluğu
    [SerializeField] private int maxComboLength = 10;
    private List<string> currentCombo = new List<string>();
    private int currentComboIndex = 0;
    [SerializeField] private GameObject quickTimeUIPanel;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI comboTimerText;
    private bool isInQuickTimeEvent = false;
    private float qteTimer;
    [SerializeField] private float qteTimeLimit = 5f;

    [Header("Game Timer")]
    [SerializeField] private float gameTimer = 180f; // 3 dakika
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timer;

    void Start()
    {
        if (playerScript == null)
        {
            playerScript = this;
        }
        quickTimeUIPanel.SetActive(false);
        animator = GetComponent<Animator>();

        currentSpeed = minSpeed;
        InvokeRepeating("DecaySpeed", 1.0f, 1.0f);
    }

    void Update()
    {
        if (!GameManager.gameManagerScript.isGameStarted) return;

        

        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            UpdateGameTimerUI();
        }
        else
        {
            FinishRace();
        }

        if (!isInQuickTimeEvent)
        {
            HandleInput();
            MoveToNextWaypoint();
            ChangeAnimationDueToWayPoint();
        }
        else
        {
            HandleQuickTimeInput();
            HandleQteTimer();
        }

        animator.speed = currentSpeed / 2;
    }

    private void HandleInput()
    {
        if (canInput)
        {
            inGame = true;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!lastKeyWasA)
                {
                    lastKeyWasA = true;
                    IncreaseSpeed();
                }
                else
                {
                    DecreaseSpeed();
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (lastKeyWasA)
                {
                    lastKeyWasA = false;
                    IncreaseSpeed();
                }
                else
                {
                    DecreaseSpeed();
                }
            }
        }
    }

    private void IncreaseSpeed()
    {
        if (isInQuickTimeEvent) return;
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration, minSpeed, maxSpeed);
    }

    private void DecreaseSpeed()
    {
        if (isInQuickTimeEvent) return;
        currentSpeed = Mathf.Clamp(currentSpeed - deceleration, minSpeed, maxSpeed);
    }

    void DecaySpeed()
    {
        if (isInQuickTimeEvent) return;
        currentSpeed = Mathf.Clamp(currentSpeed - decelerationByTime, minSpeed, maxSpeed);
    }

    private void MoveToNextWaypoint()
    {
        if (isInQuickTimeEvent || currentSpeed <= 0) return;

        if (currentWaypointIndex < waypoints.Length && currentSpeed > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;

            transform.position += direction * currentSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
            currentLap++;

            if (currentLap > totalLaps)
            {
                FinishRace();
            }
            else
            {
                AdjustComboLength();
                StartQuickTimeEvent();
            }
        }
    }

    private void FinishRace()
    {
        Debug.Log("Oyun bitti!");
        currentSpeed = 0;
        canInput = false;
    }

    private void ChangeAnimationDueToWayPoint()
    {
        if (currentWaypointIndex <= 3) // top runner
        {
            AnimationManager(normalRunningPlayer);
        }
        else if (currentWaypointIndex > 3 && currentWaypointIndex <= 9) // right runner
        {
            AnimationManager(normalRunningPlayer);
        }
        else if (currentWaypointIndex > 9 && currentWaypointIndex <= 15) // bottom runner
        {
            AnimationManager(fliippedNormalRunningPlayer);
        }
        else if (currentWaypointIndex > 15 && currentWaypointIndex <= 21) // left runner
        {
            AnimationManager(topRunningPlayer);
        }
        else if (currentWaypointIndex > 21) // top runner
        {
            AnimationManager(normalRunningPlayer);
        }
    }

    private void AnimationManager(string animation)
    {
        animator.Play(animation);
    }

    private void AdjustComboLength()
    {
        comboLength = Mathf.Clamp(comboLength + 1, 5, maxComboLength);
    }

    private void StartQuickTimeEvent()
    {
        StartCoroutine(StartQuickTimeEventWithCountdown());
    }

    private IEnumerator StartQuickTimeEventWithCountdown()
    {
        canInput = false;
        inGame = false;
        timer.SetActive(false);
        currentCombo.Clear();
        comboText.text = string.Empty;

        quickTimeUIPanel.SetActive(true);
        savedSpeed = currentSpeed;
        currentSpeed = minSpeed;

        comboTimerText.text = "3";
        yield return new WaitForSeconds(1f);
        comboTimerText.text = "2";
        yield return new WaitForSeconds(1f);
        comboTimerText.text = "1";
        yield return new WaitForSeconds(1f);
        comboTimerText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        GenerateRandomCombo();
        UpdateComboUI();
        isInQuickTimeEvent = true;
        qteTimer = qteTimeLimit;
    }

    private void GenerateRandomCombo()
    {
        for (int i = 0; i < comboLength; i++)
        {
            string randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            currentCombo.Add(randomKey);
        }
        currentComboIndex = 0;
    }

    private void UpdateComboUI()
    {
        string displayedCombo = "";
        for (int i = 0; i < currentCombo.Count; i++)
        {
            if (i < currentComboIndex)
            {
                displayedCombo += $"<color=green>{currentCombo[i]}</color> ";
            }
            else
            {
                displayedCombo += $"{currentCombo[i]} ";
            }
        }
        comboText.text = displayedCombo;
    }

    private void HandleQuickTimeInput()
    {
        if (isInQuickTimeEvent && currentComboIndex < currentCombo.Count)
        {
            if (Input.anyKeyDown)
            {
                string pressedKey = Input.inputString.ToUpper();
                if (pressedKey == currentCombo[currentComboIndex])
                {
                    currentComboIndex++;
                    HighlightComboText(true);
                    if (currentComboIndex >= currentCombo.Count)
                    {
                        CompleteQuickTimeEvent();
                    }
                }
                else
                {
                    currentComboIndex = 0;
                    HighlightComboText(false);
                }
                UpdateComboUI();
            }
        }
    }

    private void HighlightComboText(bool isCorrect)
    {
        string color = isCorrect ? "green" : "red";
        comboText.text = "";

        for (int i = 0; i < currentCombo.Count; i++)
        {
            if (i < currentComboIndex)
            {
                comboText.text += $"<color={color}>{currentCombo[i]}</color> ";
            }
            else
            {
                comboText.text += $"{currentCombo[i]} ";
            }
        }
    }

    private void HandleQteTimer()
    {
        if (isInQuickTimeEvent)
        {
            comboTimerText.text = qteTimer.ToString("F1") + " second(s) left";
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                FailQuickTimeEvent();
            }
        }
    }

    private void CompleteQuickTimeEvent()
    {
        float speedBonus = Mathf.Clamp(qteTimeLimit - qteTimer, 0.1f, 1.5f);
        currentSpeed = Mathf.Clamp(savedSpeed + speedBonus, minSpeed, maxSpeed);

        quickTimeUIPanel.SetActive(false);
        isInQuickTimeEvent = false;
        canInput = true;
        inGame = true;
        timer.SetActive(true);
    }

    private void FailQuickTimeEvent()
    {
        currentComboIndex = 0;
        UpdateComboUI();
        comboText.color = Color.black;
        quickTimeUIPanel.SetActive(false);
        isInQuickTimeEvent = false;
        canInput = true;
        inGame = true;
        currentSpeed = 0;
        timer.SetActive(true);
    }

    private void UpdateGameTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (gameTimer <= 0)
        {

        }
    }
}
