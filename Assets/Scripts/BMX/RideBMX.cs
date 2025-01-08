using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class RideBMX : MonoBehaviour
{
    public static RideBMX rideBMXScript;
    
    
    [Header ("Bike")]
    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] public Rigidbody2D bmxRB;
    [SerializeField] public Transform bmxTF;
    [SerializeField] private float bmxSpeed = 200f;
    [SerializeField, Range(0f, 500)] private float rotationSpeed = 100f;

    [Header ("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;

    [Header ("Input")]
    public bool canInput = true;
    [SerializeField] public float moveInputX;
    [SerializeField] public float moveInputY;

    [Header("Stamina")]
    [SerializeField] public float maxStamina = 100f;
    [SerializeField] public float stamina;
    [SerializeField] private float staminaCostMultiple = 5f;

    [Header("Start Position")]
    private Vector2 startPosition;

    [Header ("Is UpsideDown")]
    public bool isGround = false;
    public Vector3 upsidedPositions;

    [Header ("Others")]
    [SerializeField] public bool inGame;

    [SerializeField] private float timer = 5f;
    [SerializeField] private float staminaOffTimer = 5f;

    [SerializeField] private Animator animator;


    void Start()
    {
        stamina = maxStamina;

        if (rideBMXScript == null)
        {
            rideBMXScript = this;    
        }

        startPosition = bmxRB.position;
    }

    private void Update()
    {
        Debug.Log(inGame);

            HandleInput();
            MeasuringDistance();
            NormalizeTheBike();
            UiManager.uiScript.UpdatingStaminaMeterUI();
            
    }

    private void FixedUpdate()
    {
            MovingTires();
            StaminaOff();
            DecreseStamina();
            AnimatonPlayer();
    }
    private void HandleInput()
    {
        if (!canInput) return;

        inGame = true;
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxis("Vertical");
        
    }
    private void MeasuringDistance()
    {
        Vector2 distance = bmxRB.position - startPosition;
        distance.y = 0f;

        if (distance.x < 0)
        {
            distance.x = 0;
        }

        //distanceText.text = "Score: " + distance.x.ToString("F0");
    }

    private void StaminaOff()
    {
        if (stamina > 0) return;

        inGame = false;
        Debug.LogError("Stamina bitti");
        canInput = false;
        moveInputX = 0;
        moveInputY = 0;
        
        
        UiManager.uiScript.staminaOffGO.SetActive(true);
        if (staminaOffTimer > 0f)
        {
            UiManager.uiScript.staminaOffText.text = "Your stamina runs out<br>Game finish in: "+staminaOffTimer.ToString("F0");
            staminaOffTimer -= Time.deltaTime;
        }
        else if (staminaOffTimer <= 0f)
        {
            Timer.timerScript.isBMXTimerWorking = false;

            bmxSpeed = 0f;
            bmxRB.angularVelocity = 0f;
            backTireRB.angularVelocity = 0f;
            frontTireRB.angularVelocity = 0f;
            bmxRB.velocity = Vector2.zero;
            backTireRB.velocity = Vector2.zero;
            frontTireRB.velocity = Vector2.zero;
            Time.timeScale = 0f;

            UiManager.uiScript.staminaOffGO.SetActive(false);
            StartCoroutine(EndGame(false));
        }
    }

    private void NormalizeTheBike()
    {
        if (!isGround) return;
        ItsGround();
        if (stamina >= 15)
        {
            UiManager.uiScript.UpsideDownUI(true);
            if (Input.GetKeyDown(KeyCode.Space) && stamina >= 15)
            {
                FixBike();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(EndGame(false));
            }
        }
        else
        {
            UiManager.uiScript.UpSideDownAndStaminaOffUI(true);
            
            StartCoroutine(EndGame(false));
        }
    }

    private void FixBike()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(upsidedPositions.x, upsidedPositions.y + 0.5f, 0);
        stamina -= 10;
        isGround = false;
        bmxSpeed = 200f;
        //bmxRB.constraints = RigidbodyConstraints2D.None;
        bmxRB.angularVelocity = 0f;
        backTireRB.angularVelocity = 0f;
        frontTireRB.angularVelocity = 0f;
        bmxRB.velocity = Vector2.zero;
        backTireRB.velocity = Vector2.zero;
        frontTireRB.velocity = Vector2.zero;
        inGame = true;
        bmxRB.mass = 2f;
        canInput = true;
        Timer.timerScript.isBMXTimerWorking = true;
        Timer.timerScript.timer -= 5;
        Debug.Log("BMX düzeldi ve 10 stamina azaldı");
        IsRiderUpsideDown.isRiderUpsideDownScript.isWorkTimer = false;
        IsRiderUpsideDown.isRiderUpsideDownScript.firstTouch = true;
        IsRiderUpsideDown.isRiderUpsideDownScript.timer = 0f;
        UiManager.uiScript.UpsideDownUI(false);

        ResumeGame();
    }
    private void TireOnGround()
    {
        bool isFrontTireOnGround = Physics2D.Raycast(frontTireRB.position, Vector2.down, rayDistance, groundLayer);
        bool isBackTireOnGround = Physics2D.Raycast(backTireRB.position, Vector2.down, rayDistance, groundLayer);
        Debug.Log(isFrontTireOnGround+" "+ isBackTireOnGround);

        if (!isFrontTireOnGround && !isBackTireOnGround)
        {
            if (moveInputY > 0)
            {
                bmxRB.transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);

            }
            else if (moveInputY < 0)
            {
                bmxRB.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            }
        }
    }

    private void DecreseStamina()
    {
        stamina -= Time.deltaTime * Mathf.Abs(moveInputX) * staminaCostMultiple;
    }
    private void MovingTires()
    {
        //frontTireRB.AddTorque(-moveInput * bmxSpeed * Time.fixedDeltaTime);
        backTireRB.AddTorque(-moveInputX * bmxSpeed * Time.fixedDeltaTime);
        bmxRB.AddTorque(moveInputX * bmxSpeed * Time.fixedDeltaTime);
        TireOnGround();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(frontTireRB.position, frontTireRB.position + Vector2.down * rayDistance);


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(backTireRB.position, backTireRB.position + Vector2.down * rayDistance);
    }
    public IEnumerator EndGame(bool isWin)
    {
        ClearUI();
        inGame = false;
        Debug.Log("Girdi uğlumuz");
        yield return new WaitForSecondsRealtime(0.5f);
        Timer.timerScript.isBMXTimerWorking = false;
        canInput = false;

        if (isWin)
        {
            Debug.Log("NO WAAAAY");//win
            UiManager.uiScript.winOrLoseUI.SetActive(true);
            UiManager.uiScript.statusWinLoseUI.text="Congrats you win";
            UiManager.uiScript.congratsOrBlameUI.text= "You are a great cyclist on moon";
        }
        else
        {
            Debug.Log("Loser");
            UiManager.uiScript.winOrLoseUI.SetActive(true);
            UiManager.uiScript.statusWinLoseUI.text = "Shame on you";
            UiManager.uiScript.congratsOrBlameUI.text = "Boooo";//Niggas in Paris
            
        }
    }

    void ClearUI()
    {
        UiManager.uiScript.staminaOffGO.SetActive(false);
        UiManager.uiScript.upsideDownUI.SetActive(false);

        //UiManager.uiScript.upSideDownAndStaminaOffUI.SetActive(false);

    }
    public void RefullStamina(float staminaAdd)
    {
        staminaOffTimer = 5f;
        UiManager.uiScript.countDownUI.SetActive(false);
        UiManager.uiScript.staminaOffGO.SetActive(false);
        stamina += staminaAdd;
        if (stamina > 100)
        {
            stamina = maxStamina;
        }
        canInput = true;
        UiManager.uiScript.UpdatingStaminaMeterUI();
    }

    public void ItsGround()
    {
        bmxSpeed = 0;
        isGround = true;
        
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void AnimatonPlayer()
    {
        if (moveInputX > 0)
        {
            animator.speed = 1;
            animator.Play("ridingBmxAnimForward");
        }
        else if (moveInputX < 0)
        {
            animator.speed = 1;
            animator.Play("ridingBmxAnimBackward");
        }
        else
        {
            animator.speed = 0;
        }

    }

}