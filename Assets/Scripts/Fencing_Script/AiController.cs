using System.Collections;
using System.Collections.Generic;
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

    public Transform player;
    public LayerMask oppositePlayerAI;
    public Transform attackPointTopAI, attackPointBottomAI;
    public float attackRangeAI = 0.5f;
    public float aiCloseCombatRange = 2f;

    private float speedAI = 3f;
    private float thinkTime = 1.5f;
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
    }

    void Update()
    {
        //float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        //Debug.Log(distanceToPlayer);
        AiUpdatingUI();
        RegenerateStamina();
        if (canDoSomething)
        {
            StartCoroutine(ThinkAndDoingSomething());
        }
    }

    IEnumerator ThinkAndDoingSomething()
    {
        canDoSomething = false;

        int action = Random.Range(0, 5); // 0: idle, 1: move, 2: topAttack, 3: bottomAttack

        if (staminaAI <= 0)
        {
            aiStun(2f);
        }
        else if (staminaAI <= 20)
        {
            action = 0;
        }

        switch (action)
        {
            case 0:

                break;

            case 1:
                StartCoroutine(AIMovement('r'));
                break;

            case 2:
                StartCoroutine(AIMovement('l'));
                break;

            case 3:
                if (staminaAI >= 20)
                {
                    TopAttack();
                }
                break;

            case 4:
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
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            if (whichDirection == 'l'&& distanceToPlayer>1.5f)
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
    bool IsWallBehind()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;
        RaycastHit2D isWall = Physics2D.Raycast(origin, direction, 1f, LayerMask.GetMask("wall"));
        
        return isWall;
    }

    void TopAttack()
    {
        aiStatusCombat = "top attack wait";
        takingStance();
        if (staminaAI >= 20)
        {
            staminaAI -= 20f;
            aiStatusCombat = "top attack";
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
        aiStatusCombat = "bot attack wait";
        takingStance();
        if (staminaAI >= 20)
        {

            staminaAI -= 20f;
            aiStatusCombat = "bot attack";
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointBottomAI.position, attackRangeAI, oppositePlayerAI);
            foreach (Collider2D Player_1 in hitPlayer)
            {
                
                StartCoroutine(HittingWaitForASeconds());
                
                Debug.Log("Enemy bot hit to player!");
            }
        }
    }

    void takingStance()
    {

        StartCoroutine(waitingTakingStance());
    }
    IEnumerator waitingTakingStance()
    {

        float time = Random.Range(0.5f, 3f);
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



    
}
