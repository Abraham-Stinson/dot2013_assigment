using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer timerScript;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float timer = 180f;
    public bool isBMXTimerWorking = false;
    void Start()
    {
        if (timerScript == null)
        {
            timerScript = this;
        }
        StartCoroutine(UiManager.uiScript.CountDown());

    }

    void Update()
    {
        if (isBMXTimerWorking)
        {
            UpdateTimerUI();
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isBMXTimerWorking = false;
                StartCoroutine(RideBMX.rideBMXScript.EndGame(false));
            }

        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
