using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave.Models.Data.Player;
using UnityEngine;
using UnityEngine.UI;

public class AiController : MonoBehaviour
{
    public string aiStatusCombat;
    public string aiStatusMoving;

    //UI
    public Text aiStunnedUI;
    public Text aiCombatStatusUI;
    public Text aiStaminaUI;
    public Text aiMovementStatusUI;
    public Text aiIsHit;
    public Text distancePlayer;

    public Transform player;
    public LayerMask oppositePlayerAI;
    public Transform attackPointTopAI, attackPointBottomAI;

    public Transform rightWall;
    public float attackRangeAI = 0.5f;
    private float aiCloseCombatRange = 2f;

    private float speedAI = 3f;
    private float dashSpeedAI = 15f;
    private float dashDuration = 0.3f;
    private float thinkTime = 1.25f;
    private bool canDoSomething = true;

    private float staminaAI, maxStamina = 100f;
    private float staminaRegenSpeed = 5f;


    void Start()
    {
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
            canDoSomething = false;
            aiStun(2f);
        }

        else if (canDoSomething)
        {
            StartCoroutine(ThinkAndDoingSomething());
        }
        
        //Debug.Log(distancingToPlayer());
        distancePlayer.text = "distance: " + distancingToPlayer().ToString("F1");
    }

    IEnumerator ThinkAndDoingSomething()
    {
        int action;
        canDoSomething = false;

        if (distancingToPlayer() <= 2.5f)
        {
            action = Random.Range(0, 7);
        }
        else
        {
            action = Random.Range(0, 5);
        }
        
        switch (action)
        {
            case 0:
                //waiting
                break;

            case 1:
                StartCoroutine(AIMovement('r'));
                break;

            case 2:
                StartCoroutine(AIMovement('l'));
                break;

            case 3:
                if (staminaAI>=15f)
                {
                    staminaAI -= 15f;
                    StartCoroutine(AiDash(Vector2.right, 'r'));
                }
                break;

            case 4:
                if (staminaAI >= 15f)
                {
                    staminaAI -= 15f;
                    StartCoroutine(AiDash(Vector2.left, 'l'));
                }
                break;

            case 5:
                if (staminaAI >= 20)
                {
                    TopAttack();
                }
                break;

            case 6:
                if (staminaAI >= 20)
                {
                    BottomAttack();
                }
                break;
        }

        yield return new WaitForSeconds(thinkTime);
        aiStatusCombat = "thinking";
        aiStatusMoving = "thinking";

        canDoSomething = true;
    }

    IEnumerator AIMovement(char whichDirection)
    {
        float moveDuration = Random.Range(0f, 3f);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            if (whichDirection == 'l'&& distancingToPlayer() > 1.25f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speedAI * Time.deltaTime;

                aiStatusMoving = "moving_left";
            }
            else if (whichDirection == 'r' && !IsWallBehind())
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
            if (whichDirection == 'l' && distancingToPlayer() > 2f)
            {
                transform.Translate(dashSpeedAI * direction * Time.deltaTime);
                aiStatusMoving = "dashing_Left";
            }
            else if (whichDirection == 'r' && !IsWallBehind() && DistanceWall() < 3f)
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
        RaycastHit2D isWall = Physics2D.Raycast(origin, direction, 1f, LayerMask.GetMask("wall"));
        
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
        if (staminaAI >= 20&& distancingToPlayer() <= aiCloseCombatRange)
        {
            staminaAI -= 20f;
            aiStatusCombat = "top attacking";
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointTopAI.position, attackRangeAI, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            {                
                StartCoroutine(HittingWaitForASeconds());

                Debug.Log("Enemy bot hit to player!");
            }
        }
    }

    void BottomAttack()
    {
        float distanceToPlayer=distancingToPlayer();
        aiStatusCombat = "bot attack wait";
        StartCoroutine(waitingTakingStance());
        if (staminaAI >= 20&&distanceToPlayer<= aiCloseCombatRange)
        {
            staminaAI -= 20f;
            aiStatusCombat = "bot attacking";
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointBottomAI.position, attackRangeAI, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            {
                
                StartCoroutine(HittingWaitForASeconds());
                
                Debug.Log("Enemy bot hit to player!");
            }
        }
    }
    IEnumerator waitingTakingStance()
    {

        float time = Random.Range(0.25f, 1f);
        yield return new WaitForSeconds(time);
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

        Gizmos.DrawWireSphere(attackPointTopAI.position, attackRangeAI);
        Gizmos.DrawWireSphere(attackPointBottomAI.position, attackRangeAI);
    }

    public void aiStun(float duration)
    {
        StartCoroutine(AiStunCoroutine(duration));
    }
    private IEnumerator AiStunCoroutine(float stunDuration)
    {
        canDoSomething = false;
        //stun animation

        aiStaminaUI.text = "stunned";
        aiCombatStatusUI.text = "stunned";
        aiMovementStatusUI.text = "stunned";
        yield return new WaitForSeconds(stunDuration);
        aiStunnedUI.text = "no stunned";
        canDoSomething = true;
    }

    public void AiUpdatingUI()
    {
        aiStaminaUI.text = "Stamina: " + staminaAI.ToString("F0");
        aiCombatStatusUI.text = aiStatusCombat;
        aiMovementStatusUI.text = aiStatusMoving;
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
