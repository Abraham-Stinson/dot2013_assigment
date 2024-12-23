using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Services.CloudSave.Models.Data.Player;
using UnityEngine;
using UnityEngine.UI;

public class AiController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D aiRB;
    public string aiStatusCombat;
    [SerializeField] private string aiStatusMoving;

    //UI
    [SerializeField] private Text aiStunnedUI;
    [SerializeField] private Text aiCombatStatusUI;

    [SerializeField] private Image aiStaminaMeter;
    [SerializeField] private Gradient aiStaminasMeterGradient;

    [SerializeField] private Text aiMovementStatusUI;
    [SerializeField] private Text aiIsHit;
    [SerializeField] private int movemetAction;
    [SerializeField] private Text distancePlayer;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask oppositePlayerAI;
    [SerializeField] private Transform attackPointTopAI, attackPointBottomAI;

    [SerializeField] private Transform rightWall;
    [SerializeField] private float attackRangeTop = 0.5f;
    [SerializeField] private float attackRangeBottom = 0.5f;
    [SerializeField] private float aiCloseCombatRange = 2f;

    [SerializeField] private float speedAI = 3f;
    [SerializeField] private float dashSpeedAI = 15f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float thinkTime = 1f;
    [SerializeField] private bool canDoSomething = true;
    [SerializeField] private bool isStunned = false;

    [SerializeField] private float staminaAI, maxStamina = 100f;
    [SerializeField] private float staminaRegenSpeed = 5f;

    [SerializeField] public bool isAiTakeDamage=false;

    public static AiController aiScript;

    Animator animator;
    string currentState;

    const string ai_idle = "ai_idle";
    const string ai_walk_left = "ai_walk_left";
    const string ai_walk_right = "ai_walk_right";
    const string ai_dash_left = "ai_dash_left";
    const string ai_dash_right = "ai_dash_right";
    const string ai_attack_idle = "ai_attack_idle";
    const string ai_top_attack = "ai_top_attack";
    const string ai_bottom_attack = "ai_bottom_attack";
    const string ai_top_defence = "ai_top_defence";
    const string ai_bottom_defence = "ai_bottom_defence";
    const string ai_stun = "ai_stun";
    const string ai_Passed_Out = "ai_passed_out";

    private void Awake()
    {
        if (aiScript == null)
        {
            aiScript = this;
        }
    }
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        aiCombatStatusUI.text = "";
        aiStunnedUI.text = "start and no stun";
        aiMovementStatusUI.text = " ";
        aiIsHit.text = "is hit: no";
        distancePlayer.text="distance: ";
    }

    void FixedUpdate()
    {

        DifficultOfAI();
        AiUpdatingUI();
        if (!Pause_Menu.isPaused && !Round_Manager.roundManagerScript.inNextRoundUI&&!Round_Manager.roundManagerScript.isGameEnd)
        {
            RegenerateStamina();

            if (staminaAI <= 0)
            {
                if (!isStunned)
                {
                    StartCoroutine(AiStun(2f));
                }
            }
            else if (canDoSomething && !isStunned)
            {
                StartCoroutine(ActionAi());
            }

            
        }
        distancePlayer.text = "distance: " + distancingToPlayer().ToString("F1");
    }

    IEnumerator ActionAi()
    {
        canDoSomething = false;

        
        yield return new WaitForSeconds(thinkTime);

        if (!isStunned)
        {
            
            if (distancingToPlayer() < aiCloseCombatRange)
            {
                movemetAction = Random.Range(1, 9);
            }
            else
            {
                movemetAction = Random.Range(1, 5);
            }


            switch (movemetAction)
            {
                case 0:
                    // Bekleme durumu
                    break;

                case 1:
                    if (distancingToPlayer() <= 6f)
                    {
                        StartCoroutine(AIMovement('r'));
                    }
                    break;

                case 2:
                    StartCoroutine(AIMovement('l'));
                    break;

                case 3:
                    if (staminaAI >= 15f && distancingToPlayer() <= 10f && !IsWallBehind() && DistanceWall() > 5f)
                    {
                        StartCoroutine(AiDash(Vector2.right, 'r'));
                        staminaAI -= 15f;
                    }
                    break;

                case 4:
                    if (staminaAI >= 15f && distancingToPlayer() > 3f)
                    {
                        StartCoroutine(AiDash(Vector2.left, 'l'));
                        staminaAI -= 15f;
                    }
                    else if (distancingToPlayer() <= 2f && DistanceWall() <= 2f && IsWallBehind())
                    {
                        StartCoroutine(AiDash(Vector2.left, 'l'));
                        staminaAI -= 15f;
                    }
                    break;
                case 5:
                    TopAttack();
                    break;
                case 6:
                    BottomAttack();
                    break;
                case 7:
                    StartCoroutine(TakingDefence('t'));
                    break;
                case 8:
                    StartCoroutine(TakingDefence('b'));
                    break;
            }

        }
        canDoSomething = true;
    }

    IEnumerator AIMovement(char whichDirection)
    {
        float moveDuration = Random.Range(0.5f, 3f);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            if (whichDirection == 'l'&& distancingToPlayer() > 1.25f && staminaAI>=30)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "walking_Left";
                AnimationManager("ai_walk_left");
            }
            else if(whichDirection == 'l' && distancingToPlayer() < 1.5f && IsWallBehind()&&DistanceWall()<=3f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "walking_Left";
                AnimationManager("ai_walk_left");
            }

            if (whichDirection == 'r' && !IsWallBehind() && distancingToPlayer()<=12f)
            {

                Vector3 direction = (transform.position - player.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "walking_Right";
                AnimationManager("ai_walk_right");
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(moveDuration);
    }

    IEnumerator AiDash(Vector2 direction,char whichDirection)
    {
        
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            if (whichDirection == 'l')
            {
                transform.Translate(dashSpeedAI * direction * Time.deltaTime);
                aiStatusMoving = "dash_Left";
                AnimationManager("ai_dash_left");
            }
            else if (whichDirection == 'r')
            {

                transform.Translate(dashSpeedAI * direction * Time.deltaTime);
                
                aiStatusMoving = "dash_Right";
                AnimationManager("ai_dash_right");
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(dashDuration);
    }
    bool IsWallBehind()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;
        RaycastHit2D isWall = Physics2D.Raycast(origin, direction, 2f, LayerMask.GetMask("wall"));
        
        return isWall;
    }

    float DistanceWall()
    {
        return Vector3.Distance(transform.position,rightWall.position);
    }

    void TopAttack()
    {
        aiStatusCombat = "top_Attack_Idle";
        StartCoroutine(waitingTakingStance());
        if (staminaAI >= 20&& distancingToPlayer() <= 1.75f)
        {
            staminaAI -= 20f;
            aiStatusCombat = "top attacking";
            AnimationManager("ai_top_attack");
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointTopAI.position, attackRangeTop, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            { 
                if (Player_Movement_Combat.playerScript.playerStatusCombat == "top_Defence")
                {
                    Debug.Log("Player_1 Defended the AI's top attack");
                    staminaAI -= 5;
                }
                else
                {
                    //Animation is here

                    Player_Movement_Combat.playerScript.isPlayerTakeDamage = true;
                    StartCoroutine(HittingEvent());
                    //hit status
                    Debug.Log("Enemy bot hit from top to player!");
                    //StartCoroutine(HittingWaitForASeconds());

                }
            }
        }
    }

    void BottomAttack()
    {
        aiStatusCombat = "bot_Attack_Idle";
        StartCoroutine(waitingTakingStance());
        if (staminaAI >= 20 && distancingToPlayer() <= 1.75f)
        {
            staminaAI -= 20f;
            aiStatusCombat = "bot_attacking";
            AnimationManager("ai_bottom_attack");
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointBottomAI.position, attackRangeBottom, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            {
                if (Player_Movement_Combat.playerScript.playerStatusCombat == "bottom_Defence")
                {
                    Debug.Log("Player_1 Defended the AI's bottom attack");
                    staminaAI -= 5;
                }
                else
                {
                    //Animation is here

                    Player_Movement_Combat.playerScript.isPlayerTakeDamage = true;
                    StartCoroutine(HittingEvent());

                    //hit status
                    Debug.Log("Enemy bot hit from bottom to player!");
                    //StartCoroutine(HittingWaitForASeconds());

                }
            }
        }
    }

    private IEnumerator HittingEvent()
    {
        yield return new WaitForSeconds(1f);
        Round_Manager.roundManagerScript.EndRound("ai");
        
    }

    IEnumerator waitingTakingStance()
    {
        yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
    }

    IEnumerator TakingDefence(char direction)
    {
        if(direction == 't')
        {
            aiStatusCombat = "top_Defence";
            AnimationManager("ai_top_defence");
        }
        else if(direction == 'b')
        {
            aiStatusCombat = "bottom_Defence";
            AnimationManager("ai_bottom_defence");

        }
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
    }
    void RegenerateStamina()
    {
        if (staminaAI < maxStamina)
        {
            staminaAI += Time.deltaTime * staminaRegenSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointTopAI == null || attackPointBottomAI == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPointTopAI.position, attackRangeTop);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(attackPointBottomAI.position, new Vector2(aiRB.position.x, aiRB.position.y + 1.4f) + Vector2.right * attackRangeBottom);
    }

    public IEnumerator AiStun(float stunDuration)
    {
        //stun animation
        isStunned = true;
        canDoSomething = false;

        aiMovementStatusUI.text = "stun";
        aiStunnedUI.text = "stunned";
        AnimationManager("ai_stun");
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        canDoSomething = true;
    }

    public void AiUpdatingUI()
    {
        aiStaminaMeter.fillAmount = (staminaAI / maxStamina);
        aiStaminaMeter.color = aiStaminasMeterGradient.Evaluate(aiStaminaMeter.fillAmount);

        aiCombatStatusUI.text = aiStatusCombat;
        aiMovementStatusUI.text = aiStatusMoving;

        if (isStunned)
        {
            aiMovementStatusUI.text = "stun";
            aiStunnedUI.text = "stunned";
            Debug.Log("Stun");
        }
        else
        {
            aiStunnedUI.text = "no stunned";
            AnimationManager("ai_idle");
            aiStatusCombat = "idle";
            aiStatusMoving = "idle";
        }
    }

    /*IEnumerator HittingWaitForASeconds()
    {

        aiIsHit.text = "hit: yes";
        yield return new WaitForSeconds(1f);
        aiIsHit.text = "hit: no";
    }*/

    float distancingToPlayer()
    {
        return Vector3.Distance(player.position, transform.position);
    }

    public void ResetStamina()
    {
        staminaAI = maxStamina;
    }

    public void DifficultOfAI()
    {
        int scoreDifference = Round_Manager.roundManagerScript.aiScore - Round_Manager.roundManagerScript.playerScore;

        if (scoreDifference > 0) 
        {
            thinkTime = 1.0f + (scoreDifference * 0.1f);
        }
        else if (scoreDifference < 0)
        {
            thinkTime = Mathf.Max(0.1f, 1.0f - Mathf.Abs(scoreDifference) * 0.1f);
        }
        else 
        { 
            thinkTime = 1.0f;
        }
        Debug.Log(thinkTime);
    }

    private void AnimationManager(string newState)
    {
        if (newState == currentState)
        {
            return;
        }

        animator.Play(newState);
        currentState = newState;
    }

}