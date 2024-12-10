using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class RideBMX : MonoBehaviour
{
    public static RideBMX rideBMXScript;
    public float timer = 0f;

    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] public Rigidbody2D bmxRB;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;

    [SerializeField] private float bmxSpeed = 200f;
    [SerializeField, Range(0f, 500)] private float rotationSpeed = 100f;
    [SerializeField] public float moveInputX, moveInputY;

    //stamina
    [SerializeField] private Image staminaMeter;
    [SerializeField] private Gradient gradientForStaminaMeter;
    [SerializeField] private float maxStamina = 100f;

    //distance
    [SerializeField] private TextMeshProUGUI distanceText;
    private float stamina;
    private Vector2 startPosition;

    //is ground
    public bool isGround = false;
    public Vector3 upsidedPositions;
    public bool canInput=true;


    void Start()
    {
        stamina = maxStamina;
        UpdatingUI();


        if (rideBMXScript == null)
        {
            rideBMXScript = this;    
        }

        startPosition = bmxRB.position;
    }

    private void Update()
    {
        if (stamina > 0)
        {
            if (canInput) {
                moveInputX = Input.GetAxisRaw("Horizontal");
                moveInputY = Input.GetAxisRaw("Vertical");
            }
            



            Vector2 distance = (Vector2)bmxRB.position - startPosition;
            distance.y = 0f;

            if (distance.x < 0)
            {
                distance.x = 0;
            }

            distanceText.text = "Score: " + distance.x.ToString("F0");

            if (stamina <= 0)
            {
                Debug.Log("Stamina bitti");
            }

            if (isGround)
            {
                ItsGround();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.position = new Vector3(upsidedPositions.x, upsidedPositions.y + 0.5f, 0);
                    stamina -= 10;
                    isGround = false;
                    bmxSpeed = 200f;
                    bmxRB.constraints = RigidbodyConstraints2D.None;
                    canInput = true;
                    Debug.Log("BMX düzeldi ve 10 stamina azaldı");
                    IsRiderUpsideDown.isRiderUpsideDownScript.isWorkTimer = false;
                    IsRiderUpsideDown.isRiderUpsideDownScript.timer = 0f;
                    ResumeGame();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //tire is on ground 
        bool isFrontTireOnGround = Physics2D.Raycast(frontTireRB.position, Vector2.down, rayDistance, groundLayer);
        bool isBackTireOnGround = Physics2D.Raycast(backTireRB.position, Vector2.down, rayDistance, groundLayer);

        //movement
        //frontTireRB.AddTorque(-moveInput * bmxSpeed * Time.fixedDeltaTime);
        backTireRB.AddTorque(-moveInputX * bmxSpeed * Time.fixedDeltaTime);
        bmxRB.AddTorque(moveInputX * bmxSpeed * Time.fixedDeltaTime);


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

        stamina -= Time.deltaTime * Mathf.Abs(moveInputX)*5;
        UpdatingUI();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(frontTireRB.position, frontTireRB.position + Vector2.down * rayDistance);


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(backTireRB.position, backTireRB.position + Vector2.down * rayDistance);
    }
    private void UpdatingUI()
    {
        staminaMeter.fillAmount = (stamina / maxStamina);
        staminaMeter.color = gradientForStaminaMeter.Evaluate(staminaMeter.fillAmount);
    }

    public void RefullStamina()
    {
        
        stamina = maxStamina;
        UpdatingUI();
    }

    public void ItsGround()
    {
        bmxSpeed = 0;
        isGround = true;
        //timer += Time.deltaTime;

        //if (timer >= 0.5f)
        //{
            PauseGame();
            //timer = 0f;
        //}
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
    
}
