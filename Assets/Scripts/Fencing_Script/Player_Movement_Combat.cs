using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement_Combat : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    // Player status
    public string playerStatusCombat;
    [SerializeField] private string playerStatusMoving;
    
        // UI

    [SerializeField] private Text stunnedUI;
    [SerializeField] private Text combatStatusUI;
    [SerializeField] private Image staminaMeter;
    [SerializeField] private Gradient staminaGradient;
    [SerializeField] private Text movementStatusUI;
    [SerializeField] private Text isHit;

    // Opponent layer
    [SerializeField] private LayerMask oppositePlayer;

    // Combat
    [SerializeField] private float attackRangeTop = 0.5f;
    [SerializeField] private float attackRangeBottom = 0.5f;
    [SerializeField] private Transform attackPointTop, attackPointBottom;
    [SerializeField] private bool isAttackingOrDefending = false;

    // Stamina
    [SerializeField] private float stamina, maxStamina = 100f;
    [SerializeField] private float staminaRegenSpeed = 10f;

    // Movement
    [SerializeField] public bool canAttackOrDefend = true;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float doubleTapTime = 0.2f;
    private float lastTapTimeA, lastTapTimeD;
    private bool isDash;
    private Vector2 dashDirection;
    private float dashTimer;
    [SerializeField] private bool canMove = true;

    public static Player_Movement_Combat playerScript;

    // Animation states
    Animator animator;
    string currentState;

    const string player_idle = "player_idle";
    const string player_walk_left = "player_walk_left";
    const string player_walk_right = "player_walk_right";
    const string player_dash_left = "player_dash_left";
    const string player_dash_right = "player_dash_right";
    const string player_attack_idle = "player_attack_idle";
    const string player_top_attack = "player_top_attack";
    const string player_bottom_attack = "player_bottom_attack";
    const string player_top_defence = "player_top_defence";
    const string player_bottom_defence = "player_bottom_defence";
    const string player_stun = "player_stun";
    const string player_Passed_Out = "player_passed_out";

    float xMovement;

    bool leftMouseButtonDown = false;
    bool rightMouseButtonDown = false;
    bool wKeyDown = false;
    bool sKeyDown = false;

    bool isTopAttacking = false;
    bool isBottomAttacking = false;
    bool isTakingGuardOnAttack = false;
    bool isTopDefending = false;
    bool isBottomDefending = false;

    [SerializeField] public bool isPlayerTakeDamage = false;//çakışma olmasın

    [SerializeField] private float attackCoolDown = 0.5f;
    [SerializeField] float nextAttackTime = 0f;
    private void Awake()
    {
        if (playerScript == null)
        {
            playerScript = this;
        }
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        stunnedUI.text = "start and no stun";
        combatStatusUI.text = "";
        movementStatusUI.text = " ";
        isHit.text = "hit: no";
    }

    void Update()
    {
        //Debug.Log(currentState);
        UpdatingUI();
        Controller();
        isAliveControl();
    }
   
    private void FixedUpdate()
    {
        if (!Pause_Menu.isPaused && !Round_Manager.roundManagerScript.inNextRoundUI && !Round_Manager.roundManagerScript.isGameEnd)
        {
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
                }
            }

            if (stamina == 0f)
            {
                Stunned(1f);
            }

            if (isTopAttacking)
            {
                AnimationManager("player_top_attack");
                TopAttack();
                //Debug.Log("Yukardan Vuruyoz ya olumuz");
                isTopAttacking = false;
                //AttackCoolDown();
            }
            else if (isBottomAttacking)
            {
                AnimationManager(player_bottom_attack);
                //StartCoroutine(WaitForAnimation("player_bottom_attack"));
                BottomAttack();
                //Debug.Log("Aşşadan Vuruyoz ya olumuz");
                isBottomAttacking = false;
                //AttackCoolDown();
            }
            else if (isTakingGuardOnAttack)
            {
                AttackIdle();
                //Debug.Log("Saldırmayı bekliyoz ya olumuz");
            }
            else if (isTopDefending)
            {
                TopDefence();
                isTopDefending = false;

            }
            else if (isBottomDefending)
            {
                BottomDefence();
                isBottomDefending = false;
            }
        }
    }
    private void isAliveControl()
    {
        if (isPlayerTakeDamage)
        {
            canAttackOrDefend = false;
        }
    }

    #region CONTROLLER
    private void Controller()
    {
        //Debug.Log("Gard alıyon muu " + isTakingGuardOnAttack);
        if (!Pause_Menu.isPaused && !Round_Manager.roundManagerScript.inNextRoundUI && !Round_Manager.roundManagerScript.isGameEnd && canAttackOrDefend)
        {
            xMovement = Input.GetAxis("Horizontal");
            StaminaRegen(staminaRegenSpeed);

            leftMouseButtonDown = Input.GetMouseButton(0);
            rightMouseButtonDown = Input.GetMouseButton(1);
            wKeyDown = Input.GetKeyDown(KeyCode.W);
            sKeyDown = Input.GetKeyDown(KeyCode.S);
            //Debug.Log($"mb0{leftMouseButtonDown}, mb1{rightMouseButtonDown}, w{wKeyDown}, s{sKeyDown}");
            if (!isDash)
            {
                dashInput(stamina);
            }

            if (leftMouseButtonDown)
            {
                isTakingGuardOnAttack = true;

            }
            else
            {
                isTakingGuardOnAttack = false;
            }

            if (leftMouseButtonDown || rightMouseButtonDown)
            {
                canMove = false;
                if (wKeyDown || sKeyDown)
                {
                    if (leftMouseButtonDown && stamina >= 20)
                    {

                        isAttackingOrDefending = true;

                        if (Time.time >= nextAttackTime)
                        {
                            if (wKeyDown)
                            {
                                isTopAttacking = true;
                                isBottomAttacking = false;
                                isTakingGuardOnAttack = false;
                            }
                            else if (sKeyDown)
                            {
                                isTopAttacking = false;
                                isBottomAttacking = true;
                                isTakingGuardOnAttack = false;
                            }
                        }
                        else
                        {
                            isTopAttacking = false;
                            isBottomAttacking = false;
                            isAttackingOrDefending = false;
                        }
                    }
                    else if (leftMouseButtonDown)
                    {
                        Stunned(3f);
                    }
                    if (rightMouseButtonDown)
                    {
                        isAttackingOrDefending = true;

                        if (wKeyDown)
                        {
                            isBottomDefending = false;
                            isTopDefending = true;//

                        }

                        else if (sKeyDown)
                        {
                            isTopDefending = false;
                            isBottomDefending = true;//

                        }
                        else
                        {
                            isTopDefending = false;
                            isBottomDefending = false;
                            isAttackingOrDefending = false;
                        }

                    }
                }
            }
            else
            {
                playerStatusCombat = "idle";
                canMove = true;
                isAttackingOrDefending = false;
            }
        }
    }
    #endregion
    // Movement
    void walking()
    {
        //Debug.Log(isAttackingOrDefending);
        transform.position += new Vector3(speed * xMovement * Time.deltaTime, 0, 0);

        if (xMovement >= 0.10)
        {
            AnimationManager("player_walk_right");
            playerStatusMoving = "walking_Right";
        }
        else if (xMovement <= -0.10)
        {
            AnimationManager("player_walk_left");
            playerStatusMoving = "walking_Left";
        }
        else
        {
            if (!isAttackingOrDefending)
            {
                AnimationManager("player_idle");
                playerStatusMoving = "idle";
            }
        }
    }

    // Dash input
    void dashInput(float stamina)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastTapTimeA < doubleTapTime)
            {
                if (stamina >= 15)
                {
                    AnimationManager("player_dash_left");
                    StartDash(Vector2.left);
                    playerStatusMoving = "dash_Left";
                    StaminaCost(15f);
                }
                else
                {
                    Stunned(2f);
                }
            }
            lastTapTimeA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapTimeD < doubleTapTime)
            {
                if (stamina >= 15)
                {
                    AnimationManager("player_dash_right");
                    StartDash(Vector2.right);
                    playerStatusMoving = "dash_Right";
                    StaminaCost(15f);
                }
                else
                {
                    Stunned(2f);
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

    // Attack
    public void TopAttack()
    {
        StaminaCost(20f);
        playerStatusCombat = "top_Attack";
        

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointTop.position, attackRangeTop, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {
            if (AiController.aiScript.aiStatusCombat == "top_Defence")
            {
                stamina -= 5;
                Debug.Log("AI Defended the Player_1's top attack");
            }
            else if (AiController.aiScript.aiStatusCombat == "top_Attack")
            {
                Debug.Log("Kılıçlar çarpışır topraaam");
            }
            else if(!AiController.aiScript.isAiTakeDamage)
            {
                //Animation is here
                AiController.aiScript.isAiTakeDamage = true;
                StartCoroutine(HittingEvent());
                Debug.Log("Hit top attack to player 2");
                StartCoroutine(HittingWaitForASeconds());
            }
        }
    }

    public void BottomAttack()
    {
        StaminaCost(20f);
        playerStatusCombat = "bottom_Attack";

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointBottom.position, attackRangeBottom, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {
            if (AiController.aiScript.aiStatusCombat == "bottom_Defence")
            {
                stamina -= 5;
                Debug.Log("AI Defended the Player_1's bottom attack");
            }
            else if (AiController.aiScript.aiStatusCombat == "bottom_Attack")
            {
                Debug.Log("Kılıçlar çarpışır topraaam");
            }
            else if (!AiController.aiScript.isAiTakeDamage)
            {
                //Animation is here
                AiController.aiScript.isAiTakeDamage = true;
                StartCoroutine(HittingEvent());
                Debug.Log("Hit bot attack to player 2");
                StartCoroutine(HittingWaitForASeconds());
            }
        }
    }
    private IEnumerator HittingEvent()
    {
        
        //Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        Round_Manager.roundManagerScript.EndRound("player");

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
        Gizmos.DrawLine(attackPointBottom.position, new Vector2(playerRB.position.x, playerRB.position.y + 1.4f) + Vector2.right * attackRangeBottom);
    }

    // Defence
    void TopDefence()
    {
        AnimationManager("player_top_defence");
        playerStatusCombat = "top_Defence";
    }

    void BottomDefence()
    {
        AnimationManager("player_bottom_defence");
        playerStatusCombat = "bottom_Defence";
    }

    // Idle
    void AttackIdle()
    {
        AnimationManager("attack_idle");
        playerStatusCombat = "attack_Idle";
    }

    // Stun
    public void Stunned(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float stunDuration)
    {
        canMove = false;
        canAttackOrDefend = false;
        stunnedUI.text = "stunned";
        playerStatusMoving = "stun";
        AnimationManager("player_stun");
        yield return new WaitForSeconds(stunDuration);
        stunnedUI.text = "no stunned";
        canMove = true;
        canAttackOrDefend = true;
    }

    // Refresh UI
    public void UpdatingUI()
    {
        staminaMeter.fillAmount = (stamina / maxStamina);
        staminaMeter.color = staminaGradient.Evaluate(staminaMeter.fillAmount);
        combatStatusUI.text = playerStatusCombat;
        movementStatusUI.text = playerStatusMoving;
    }

    // Stamina
    void StaminaRegen(float regenSpeed)
    {
        if (stamina >= 0 && stamina < 100)
        {
            stamina += Time.deltaTime * regenSpeed;
        }
    }

    public void StaminaCost(float cost)
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
        isHit.text = "hit: no";
    }

    public void ResetStamina()
    {
        stamina = maxStamina;
    }

    public void AnimationManager(string newState)
    {
        if (newState == currentState)
        {
            return;
        }

        animator.Play(newState);
        currentState = newState;
    }

    private void AttackCoolDown()
    {
        nextAttackTime = Time.time + attackCoolDown;
    }
}

    
