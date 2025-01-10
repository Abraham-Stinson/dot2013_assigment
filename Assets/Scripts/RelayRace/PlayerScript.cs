using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    [SerializeField] public float currentSpeed;
    private float savedSpeed;
    public float speedBonus;
    [SerializeField] private TextMeshProUGUI SpeedUIText;

    [Header("Moving")]
    [SerializeField] private bool lastKeyWasA = false;
    //[SerializeField] private bool isMoving = false;

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
    private const string matryoshkaPlayer = "matryoshka_Player";

    public bool isInMatryoshkaAnim = false;

    [Header("Quick Time Event")]
    [SerializeField] private string[] possibleKeys = { "Q", "W", "E", "R" };
    [SerializeField] private int comboLength = 5;
    [SerializeField] private int maxComboLength = 10;
    private List<string> currentCombo = new List<string>();
    private int currentComboIndex = 0;
    [SerializeField] private GameObject quickTimeUIPanel;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI comboTimerText;
    private bool isInQuickTimeEvent = false;
    private float qteTimer;
    [SerializeField] private float qteTimeLimit = 5f;
    [SerializeField] private GameObject bonusSpeedUI;
    [SerializeField] private TextMeshProUGUI bonusSpeedText;

    [SerializeField] private GameObject resutltOfQTE;
    [SerializeField] private TextMeshProUGUI resutltOfQTEText;


    /*[Header("Game Timer")]
    [SerializeField] private float gameTimer = 180f; // 3 dakika
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timer;*/

    [Header("SoundEffect")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip runingSFX;
    [SerializeField] private AudioClip passingStafetCloseSFX;
    public bool isMatruskaClose = false;

    void Start()
    {
        
        if (playerScript == null)
        {
            playerScript = this;
        }
        quickTimeUIPanel.SetActive(false);
        bonusSpeedUI.SetActive(false);
        resutltOfQTE.SetActive(false);
        animator = GetComponent<Animator>();
        

        currentSpeed = minSpeed;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("DecaySpeed", 1.0f, 1.0f);
    }

    void Update()
    {
        if (!GameManager.gameManagerScript.isGameStarted) return;

        

        /*if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            UpdateGameTimerUI();
        }
        else
        {
            FinishRace();
        }*/

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
        UpdatingUI();
        animator.speed = currentSpeed / 2;
    }

    private void HandleInput()
    {
        if (canInput&&inGame)
        {
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

        PlaySound(runingSFX);
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
        if (isInQuickTimeEvent) return;

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
        GameManager.gameManagerScript.EndGame(true);
    }

    private void ChangeAnimationDueToWayPoint()
    {
        if (isInMatryoshkaAnim)
        {

        }
        else if (currentWaypointIndex <= 3) // top runner
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
        //timer.SetActive(false);
        currentCombo.Clear();
        comboText.text = string.Empty;

        SpeedUIText.text = "";

        quickTimeUIPanel.SetActive(true);
        savedSpeed = currentSpeed;
        currentSpeed = minSpeed;

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
        WritingResultUI('w');
        quickTimeUIPanel.SetActive(false);
        isInMatryoshkaAnim = true;
        StartCoroutine(MatryoshkaAnim());

        speedBonus = Mathf.Clamp((qteTimer / qteTimeLimit) * 1.5f, 0.1f, 1.5f);
        currentSpeed = savedSpeed * speedBonus;
        isInQuickTimeEvent = false;
        canInput = true;
        inGame = true;
        //timer.SetActive(true);
    }

    

    private void FailQuickTimeEvent()
    {
        currentComboIndex = 0;
        UpdateComboUI();
        isInMatryoshkaAnim = true;
        comboText.color = Color.black;
        WritingResultUI('l');
        quickTimeUIPanel.SetActive(false);
        StartCoroutine(MatryoshkaAnim());
        isInQuickTimeEvent = false;
        canInput = true;
        inGame = true;
        currentSpeed = 0;

        //timer.SetActive(true);
    }

    private void WritingResultUI(char isCook)
    {
        if (isCook == 'w')
        {
            resutltOfQTE.SetActive(true);
            resutltOfQTEText.text= "You delivered your flag and your speed was multiplied by this number of times based on your delivery time: " + speedBonus.ToString("F1");
        }

        else if (isCook == 'l')
        {
            resutltOfQTE.SetActive(true);
            resutltOfQTEText.text = "Since you couldn't do the combo correctly, you had a coordination problem while delivering the flag and your speed was reset.";
        }
    }

    private IEnumerator MatryoshkaAnim()
    {
        AnimationManager(matryoshkaPlayer);
        PlaySound(passingStafetCloseSFX);
        yield return new WaitForSeconds(2f);
       
        resutltOfQTE.SetActive(false);
    }

    /*private void UpdateGameTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (gameTimer <= 0)
        {

        }
    }*/

    private void UpdatingUI()
    {
        SpeedUIText.text = "Your speed: "+currentSpeed.ToString("F1");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
