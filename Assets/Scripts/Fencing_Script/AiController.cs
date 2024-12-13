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
    [SerializeField] private Text aiStaminaUI;
    [SerializeField] private Text aiMovementStatusUI;
    [SerializeField] private Text aiIsHit;
    [SerializeField] private Text distancePlayer;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask oppositePlayerAI;
    [SerializeField] private Transform attackPointTopAI, attackPointBottomAI;

    [SerializeField] private Transform rightWall;
    [SerializeField] private float attackRangeAI = 0.5f;
    [SerializeField] private float aiCloseCombatRange = 2f;

    [SerializeField] private float speedAI = 3f;
    [SerializeField] private float dashSpeedAI = 15f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float thinkTime = 1f;
    [SerializeField] private bool canDoSomething = true;
    [SerializeField] private bool isStunned = false;

    [SerializeField] private float staminaAI, maxStamina = 100f;
    [SerializeField] private float staminaRegenSpeed = 5f;

    public static AiController aiScript;

    void Start()
    {
        if (aiScript == null)
        {
            aiScript = this;
        }

        staminaAI = maxStamina;

        aiCombatStatusUI.text = "";
        aiStunnedUI.text = "start and no stun";
        aiStaminaUI.text = "";
        aiMovementStatusUI.text = " ";
        aiIsHit.text = "is hit: no";
        distancePlayer.text="distance: ";
    }

    void Update()
    {
        AiUpdatingUI();
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

        distancePlayer.text = "distance: " + distancingToPlayer().ToString("F1");
    }

    IEnumerator ActionAi()
    {
        canDoSomething = false;

        
        yield return new WaitForSeconds(thinkTime);

        if (!isStunned)
        {
            Debug.Log("Movement action");
            int movemetAction = Random.Range(1, 5);

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
            }

            if (distancingToPlayer() < aiCloseCombatRange)
            {
                Debug.Log("Combat action");
                int combatAction = Random.Range(0, 4);

                switch (combatAction)
                {
                    case 0:
                        TopAttack();
                        break;
                    case 1:
                        BottomAttack();
                        break;
                    case 2:
                        StartCoroutine(TakingDefence('t'));
                        break;
                    case 3:
                        StartCoroutine(TakingDefence('b'));
                        break;
                }
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

                aiStatusMoving = "moving_left";
            }
            else if(whichDirection == 'l' && distancingToPlayer() < 1.5f && IsWallBehind()&&DistanceWall()<=3f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "moving_left";
            }

            if (whichDirection == 'r' && !IsWallBehind() && distancingToPlayer()<=12f)
            {

                Vector3 direction = (transform.position - player.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "moving_right";
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
                aiStatusMoving = "dashing_Left";
            }
            else if (whichDirection == 'r')
            {

                transform.Translate(dashSpeedAI * direction * Time.deltaTime);
                
                aiStatusMoving = "dashing_Right";
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
        aiStatusCombat = "top attack wait";
        StartCoroutine(waitingTakingStance());
        if (staminaAI >= 20&& distancingToPlayer() <= 1.75f)
        {
            staminaAI -= 20f;
            aiStatusCombat = "top attacking";
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointTopAI.position, attackRangeAI, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            { 
                if (Player_Movement_Combat.playerScript.playerStatusCombat == "top_defence")
                {
                    Debug.Log("Player_1 Defended the AI's top attack");
                    staminaAI -= 5;
                }
                else
                {
                    //hit status
                    Debug.Log("Enemy bot hit from top to player!");
                    StartCoroutine(HittingWaitForASeconds());

                }
            }
        }
    }

    void BottomAttack()
    {
        aiStatusCombat = "bot_attack_wait";
        StartCoroutine(waitingTakingStance());
        if (staminaAI >= 20 && distancingToPlayer() <= 1.75f)
        {
            staminaAI -= 20f;
            aiStatusCombat = "bot_attacking";
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointBottomAI.position, attackRangeAI, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            {
                StartCoroutine(HittingWaitForASeconds());
                if (Player_Movement_Combat.playerScript.playerStatusCombat == "bottom_defence")
                {
                    Debug.Log("Player_1 Defended the AI's bottom attack");
                    staminaAI -= 5;
                }
                else
                {
                    //hit status
                    Debug.Log("Enemy bot hit from bottom to player!");
                    StartCoroutine(HittingWaitForASeconds());

                }
            }
        }
    }

    IEnumerator waitingTakingStance()
    {
        yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
    }

    IEnumerator TakingDefence(char direction)
    {
        if(direction == 't')
        {
            aiStatusCombat = "top_defence";
        }
        else if(direction == 'b')
        {
            aiStatusCombat = "bottom_defence";
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
        Gizmos.DrawWireSphere(attackPointTopAI.position, attackRangeAI);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(attackPointBottomAI.position, aiRB.position + Vector2.left * attackRangeAI);
    }

    public IEnumerator AiStun(float stunDuration)
    {
        //stun animation
        isStunned = true;
        canDoSomething = false;

        aiCombatStatusUI.text = "stunned";
        aiMovementStatusUI.text = "stunned";
        aiStunnedUI.text = "stunned";
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        canDoSomething = true;
    }

    public void AiUpdatingUI()
    {

        aiStaminaUI.text = "Stamina: " + staminaAI.ToString("F0");
        aiCombatStatusUI.text = aiStatusCombat;
        aiMovementStatusUI.text = aiStatusMoving;

        if (isStunned)
        {
            aiCombatStatusUI.text = "stunned";
            aiMovementStatusUI.text = "stunned";
            aiStunnedUI.text = "stunned";
            Debug.Log("Stun");
        }
        else
        {
            aiStunnedUI.text = "no stunned";
            aiStatusCombat = "thinking";
            aiStatusMoving = "thinking";
        }
    }

    IEnumerator HittingWaitForASeconds()
    {

        aiIsHit.text = "hit: yes";
        yield return new WaitForSeconds(1f);
        aiIsHit.text = "hit: no";
    }

    float distancingToPlayer()
    {
        return Vector3.Distance(player.position, transform.position);
    }
}