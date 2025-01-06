using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AiScript : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int currentWaypointIndex = 0;

    [Header("Speed")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float currentSpeed;
    private float savedSpeed;

    [Header("Laps")]
    [SerializeField] private int totalLaps = 4;
    [SerializeField] private int currentLap = 1;

    [Header("Animation")]
    Animator animator;
    private const string normalRunningAI = "Normal_Ai_Runner";
    private const string topRunningAI = "Top_Ai_Runner";
    private const string fliippedNormalRunningAI = "Flipped_Normal_Ai_Runner";

    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = minSpeed;
    }

    void Update()
    {
        ChangingSpeed();
        MoveToNextWaypoint();
        ChangeAnimationDueToWayPoint();

        animator.speed = currentSpeed / 2;
    }

    private void ChangingSpeed()
    {
        int currentStation = Random.Range(1, 3);

        switch (currentStation)
        {
            case 1://acceleration
                if (currentSpeed<6)
                {
                    currentSpeed += acceleration;
                }
                break;
            case 2://decceleration
                if (currentSpeed > 1)
                {
                    currentSpeed -= deceleration;
                }
                break;
        }
    }
    private void MoveToNextWaypoint()
    {
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
                StartCoroutine(PassingStafet());
            }
        }
    }

    private void ChangeAnimationDueToWayPoint()
    {

    }

    private void FinishRace()
    {

    }

    private IEnumerator PassingStafet()
    {
        yield return new WaitForSeconds(1f);
    }
}
