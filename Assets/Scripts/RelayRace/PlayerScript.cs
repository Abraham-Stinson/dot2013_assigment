using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int currentWaypointIndex = 0;


    [Header("Speed")]
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float decelerationByTime = 1f;

    [SerializeField] private float currentSpeed;

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

    void Start()
    {
        animator = GetComponent<Animator>();

        currentSpeed = minSpeed;
        InvokeRepeating("DecaySpeed", 1.0f, 1.0f);
    }

    void Update()
    {
        HandleInput();
        MoveToNextWaypoint();

        ChangeAnimationDueToWayPoint();
        //Debugs
        animator.speed = currentSpeed/2;
        Debug.Log(currentSpeed);

    }

    private void HandleInput()
    {
        if (canInput)
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
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration, minSpeed, maxSpeed);
    }

    private void DecreaseSpeed()
    {
        currentSpeed = Mathf.Clamp(currentSpeed - deceleration, minSpeed, maxSpeed);
    }

    void DecaySpeed()
    {
        currentSpeed = Mathf.Clamp(currentSpeed - decelerationByTime, minSpeed, maxSpeed);
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
        }
    }

    private void FinishRace()
    {
        Debug.Log("Bitti oğlumuz yaaav");
        currentSpeed = 0;
        canInput = false;
    }
    private void ChangeAnimationDueToWayPoint()
    {
        if (currentWaypointIndex <= 3)//top runner
        {
            AnimationManager(normalRunningPlayer);
        }
        else if(currentWaypointIndex >3  && currentWaypointIndex <= 9)//right runner
        {
            AnimationManager(normalRunningPlayer);
            
        }
        else if(currentWaypointIndex >9  && currentWaypointIndex <= 15)//bottom runner
        {
            AnimationManager(fliippedNormalRunningPlayer);
        }
        else if(currentWaypointIndex >15  && currentWaypointIndex <= 21) //left runner
        {
            AnimationManager(topRunningPlayer);
        }
        else if(currentWaypointIndex > 21)//top runner
        {
            AnimationManager(normalRunningPlayer);
        }
    }

    private void AnimationManager(string animation)
    {
        animator.Play(animation);
    }
}
