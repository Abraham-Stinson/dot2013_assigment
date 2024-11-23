using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject StaminaController;
    public Stamina staminaScript;

    public GameObject Player_1;
    public Combat combatScript;

    
    public Text movement_status_text_ui;

    public string playerStatusMoving;
    float speed = 5f;
    float dashSpeed = 20f;
    float dashTime = 0.2f;
    float doubleTabTime = 0.2f;


    private float lastTapTimeA, lastTapTimeD;
    private bool isDash;
    private Vector2 dashDirection;
    private float dashTimer;

    protected internal bool canMove = true;
    void Start()
    {
        staminaScript = StaminaController.GetComponent<Stamina>();
        combatScript = Player_1.GetComponent<Combat>();
        movement_status_text_ui.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        float PlayerStamina = staminaScript.stamina;
        movement_status_text_ui.text = playerStatusMoving;
        if (canMove)
        {
            if (isDash)
            {

                transform.Translate(dashSpeed * dashDirection * Time.deltaTime);

                dashTimer -= Time.deltaTime;
                if (dashTimer < 0)
                {
                    isDash = false;
                }
            }
            else
            {
                walking();
                dashInput(PlayerStamina);
            }
            //Debug.Log(playerStatus);
        }
    }
    void walking()
    {
        
        float xMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(speed * xMovement * Time.deltaTime, 0, 0);
        if (Input.GetAxis("Horizontal") >= 0.10)
        {
            playerStatusMoving = "walkingRight";
        }
        else if (Input.GetAxis("Horizontal") <= -0.10)
        {
            playerStatusMoving = "walkingLeft";
        }
        else
        {
            playerStatusMoving = "idle";
        }

    }

    void dashInput(float stamina)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            if (Time.time - lastTapTimeA < doubleTabTime)
            {
                if(stamina >= 15)
                {
                    StartDash(Vector2.left);
                    playerStatusMoving = "dashingLeft";
                    staminaScript.staminaCost(15f);
                }
                else if (stamina < 15)
                {
                    combatScript.stunned(2f);
                }

            }
            lastTapTimeA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            
            if (Time.time - lastTapTimeD < doubleTabTime)
            {
                if(stamina >= 15)
                {
                    StartDash(Vector2.right);
                    playerStatusMoving = "dashingRight";
                    staminaScript.staminaCost(15f);
                }
                else if (stamina < 15)
                {
                    combatScript.stunned(2f);
                }
            }
            lastTapTimeD = Time.time;
        }
    }

    private void StartDash(Vector2 direction)
    {
        isDash = true;
        dashDirection = direction;
        dashTimer = dashTime;
    }
}
