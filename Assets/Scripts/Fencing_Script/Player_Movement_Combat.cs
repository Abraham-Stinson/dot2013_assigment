using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement_Combat : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    //playerstatus
    public string playerStatusCombat;
    [SerializeField] private string playerStatusMoving;

    //UI
    [SerializeField] private Text stunnedUI;
    [SerializeField] private Text combatStatusUI;

    [SerializeField] private Image staminaMeter;
    [SerializeField] private Gradient staminaGradient;


    [SerializeField] private Text movementStatusUI;
    [SerializeField] private Text isHit;


    //opposite player layer
    [SerializeField] private LayerMask oppositePlayer;

    //combat
    [SerializeField] private float attackRangeTop = 0.5f;
    [SerializeField] private float attackRangeBottom = 0.5f;
    [SerializeField] private Transform attackPointTop, attackPointBottom;

    //stamina
    [SerializeField] private float stamina, maxStamina = 100f;
    [SerializeField] private float staminaRegenSpeed = 10f;

    //movement 
    private bool canAttackorDefence = true;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float doubleTabTime = 0.2f;
    private float lastTapTimeA, lastTapTimeD;
    private bool isDash;
    private Vector2 dashDirection;
    private float dashTimer;
    [SerializeField] private bool canMove = true;

    public static Player_Movement_Combat playerScript;

    //Animation 
    private Animator animator;
    private Animation animation;
        //dash
    

    private void Awake()
    {
        if (playerScript == null)
        {
            playerScript = this;
        }
    }
    void Start()
    {
        animator=GetComponent<Animator>();   
        stunnedUI.text = "start and no stun";
        combatStatusUI.text = "";
        movementStatusUI.text = " ";
        isHit.text = "hit: no";
    }


    void Update()
    {
        updatingUI();
        if (!Pause_Menu.isPaused&& !Round_Manager.roundManagerScript.inNextRoundUI && !Round_Manager.roundManagerScript.isGameEnd)
        {
            
            staminaRegeneration(staminaRegenSpeed);//stamina regen

            //movement

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
                    dashInput(stamina);
                }
                //Debug.Log(playerStatus);
            }

            if (stamina == 0f)
            {
                stunned(1f);
            }

            if (canAttackorDefence)
            {
                if (Input.GetMouseButton(0))
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (stamina >= 20)
                        {
                            topAttacking();

                        }
                        else
                        {
                            stunned(3f);
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (stamina >= 20)
                        {
                            bottomAttacking();

                        }
                        else
                        {
                            stunned(3f);
                        }
                    }
                    else
                    {
                        attackIdlePosition();
                    }
                }

                else if (Input.GetMouseButton(1))
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        topDefence();
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        bottomDefence();
                    }
                    else
                    {
                        defendIdlePosition();
                    }
                }

                else
                {
                    idlePosition();
                }
            }

        }
    }
    //movement
    void walking()
    {

        float xMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(speed * xMovement * Time.deltaTime, 0, 0);
        if (Input.GetAxis("Horizontal") >= 0.10)
        {
            playerStatusMoving = "walking_right";
            animator.SetInteger("walkingCondition", 1);
        }
        else if (Input.GetAxis("Horizontal") <= -0.10)
        {
            playerStatusMoving = "walking_left";
            animator.SetInteger("walkingCondition", -1);
        }
        else
        {
            playerStatusMoving = "idle";
            animator.SetInteger("walkingCondition", 0);
        }

    }
    //dash
    void dashInput(float stamina)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            if (Time.time - lastTapTimeA < doubleTabTime)
            {
                if (stamina >= 15)
                {
                    StartDash(Vector2.left);
                    playerStatusMoving = "dashing_left";
                    staminaCost(15f);
                }
                else if (stamina < 15)
                {
                    stunned(2f);
                }

            }
            lastTapTimeA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            if (Time.time - lastTapTimeD < doubleTabTime)
            {
                if (stamina >= 15)
                {
                    StartDash(Vector2.right);
                    playerStatusMoving = "dashing_right";
                    staminaCost(15f);
                }
                else if (stamina < 15)
                {
                    stunned(2f);
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

    //attack
    public void topAttacking()
    {
        //top attack animation
        staminaCost(20f);
        playerStatusCombat = "top_attack";

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointTop.position, attackRangeTop, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {

            if (AiController.aiScript.aiStatusCombat=="top_defence")
            {
                //defence status
                stamina -= 5;
                Debug.Log("AI Defended the Player_1's top attack");
                
            }
            else
            {
                Round_Manager.roundManagerScript.EndRound("player");
                Debug.Log("Hit top attack to player 2");
                StartCoroutine(HittingWaitForASeconds());
            }
            
        }
    }

    public void bottomAttacking()
    {
        //top attack animation
        staminaCost(20f);
        playerStatusCombat = "botom_attack";

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointBottom.position, attackRangeBottom, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {
            if (AiController.aiScript.aiStatusCombat == "bottom_defence")
            {
                //defence status
                stamina -= 5;
                Debug.Log("AI Defended the Player_1's bottom attack");

            }
            else
            {
                Debug.Log("Hit bottom attack to player 2");
                Round_Manager.roundManagerScript.EndRound("player");
                StartCoroutine(HittingWaitForASeconds());
            }


        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointTop == null || attackPointBottom == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPointTop.position, attackRangeTop);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(attackPointBottom.position,new Vector2(playerRB.position.x, playerRB.position.y+1.4f) + Vector2.right *  attackRangeBottom );
        
    }
    //defence
    void topDefence()
    {
        //top defence  animation
        playerStatusCombat = "top_defence";
    }

    void bottomDefence()
    {
        //bot defence  animation
        playerStatusCombat = "bottom_defence";
    }
    //idle 
    void attackIdlePosition()
    {
        //attack idle animation
        playerStatusCombat = "attack_idle";
    }
    void defendIdlePosition()
    {
        //defend idle animation
        playerStatusCombat = "defend_idle";
    }
    void idlePosition()
    {
        //idle position animation
        playerStatusCombat = "idle";
    }
    //stun
    public void stunned(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float stunDuration)
    {
        canMove = false;
        canAttackorDefence = false;

        //stun animation
        stunnedUI.text = "stunned";
        playerStatusCombat = "stunned";
        playerStatusMoving = "stunned";
        yield return new WaitForSeconds(stunDuration);
        stunnedUI.text = "no stunned";
        canMove = true;
        canAttackorDefence = true;
    }
    //refreshing UI
    public void updatingUI()
    {
        staminaMeter.fillAmount = (stamina / maxStamina);
        staminaMeter.color = staminaGradient.Evaluate(staminaMeter.fillAmount);

        combatStatusUI.text = playerStatusCombat;
        movementStatusUI.text = playerStatusMoving;
    }
    //stamina
    void staminaRegeneration(float regenSpeed)
    {

        if (stamina >= 0 && stamina < 100)
        {
            stamina += Time.deltaTime * regenSpeed;
        }
    }
    public void staminaCost(float cost)
    {
        stamina -= cost;
    }

    IEnumerator WaitForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    
    IEnumerator HittingWaitForASeconds()
    {
        isHit.text = "hit: yes";
        yield return new WaitForSeconds(1f);
        isHit .text = "hit: no";
    }

    public void ResetStamina()
    {
        stamina = maxStamina;
    }
}

