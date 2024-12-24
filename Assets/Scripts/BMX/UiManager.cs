using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager uiScript;

    [Header("Navigator")]
    [SerializeField] private Transform startPointPosition;
    [SerializeField] private Transform finishPointPosition;
    [SerializeField] private RectTransform navigatorBack;
    [SerializeField] private RectTransform navigatorFront;

    [Header("StaminaMeter")]
    [SerializeField] private Image staminaMeter;
    [SerializeField] private Gradient gradientForStaminaMeter;

    [SerializeField] private float distanceStartToFinish;

    [Header("UpsideDown")]
    [SerializeField] public GameObject upsideDownUI;
    [SerializeField] public GameObject upSideDownAndStaminaOffUI;

    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public static bool isPaused = false;

    [SerializeField] public GameObject countDownUI;
    [SerializeField] public TextMeshProUGUI countDownText;

    [Header("Stamina Off Text")]
    [SerializeField] public GameObject staminaOffGO;
    [SerializeField] public TextMeshProUGUI staminaOffText;

    [Header("Win or Lose UI")]
    [SerializeField] public GameObject winOrLoseUI;
    [SerializeField] public TextMeshProUGUI statusWinLoseUI;
    [SerializeField] public TextMeshProUGUI congratsOrBlameUI;
    // Start is called before the first frame update
    void Start()
    {
        if (uiScript == null)
        {
            uiScript = this;
        }
        upsideDownUI.SetActive(false);

        pauseMenu.SetActive(false);
        countDownUI.SetActive(false);
        staminaOffGO.SetActive(false);
        winOrLoseUI.SetActive(false);
        upSideDownAndStaminaOffUI.SetActive(false);

        UpdatingStaminaMeterUI();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateUINavigation();
        UpdatingStaminaMeterUI();
        PauseMenuInput();
    }
    private void FixedUpdate()
    {

    }
    private void UpdateUINavigation()
    {
        float mapLength = Vector3.Distance(startPointPosition.position, finishPointPosition.position);
        float playerDistance = Vector3.Distance(startPointPosition.position, RideBMX.rideBMXScript.bmxTF.position);

        float progress = Mathf.Clamp01(playerDistance / mapLength);

        float navBarWidth = navigatorBack.rect.width;

        float frontPositionX = progress * navBarWidth;

        navigatorFront.anchoredPosition = new Vector2(frontPositionX, 0);

        //Debug.Log($"Progress: {progress}, Front Position X: {frontPositionX}, NavigatorBack Width: {navBarWidth}");
    }
    public void UpdatingStaminaMeterUI()
    {
        staminaMeter.fillAmount = (RideBMX.rideBMXScript.stamina / RideBMX.rideBMXScript.maxStamina);
        staminaMeter.color = gradientForStaminaMeter.Evaluate(staminaMeter.fillAmount);
    }

    public void UpsideDownUI(bool isActive)
    {
        upsideDownUI.SetActive(isActive);
    }

    public void UpSideDownAndStaminaOffUI(bool isActive)
    {
        upSideDownAndStaminaOffUI.SetActive(isActive);
    }
    
    private void PauseMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&RideBMX.rideBMXScript.inGame)
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
        Timer.timerScript.isBMXTimerWorking = false;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(CountDown());
        Timer.timerScript.isBMXTimerWorking = true;
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
