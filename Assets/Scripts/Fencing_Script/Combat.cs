using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    bool canAttackorDefence = true;
    public GameObject StaminaController;
    public Stamina staminaScript;

    public GameObject Player_1;
    public PlayerMovement playerMovementScript;

    public GameObject Stunned_Player_1;
    public Text stunnedUIText;


    public Text combat_status_text_ui;

    public Transform attackPointTop, attackPointBottom;
    public float attackRange = 0.5f;

    public LayerMask oppositePlayer;



    public float swordSpeed = 5f, swingSpeed = 100f;
    public float targetAngelY, targetAngelZ;
    public string playerStatusCombat;



    void Start()
    {
        staminaScript = StaminaController.GetComponent<Stamina>();
        playerMovementScript = Player_1.GetComponent<PlayerMovement>();
        stunnedUIText.text = "start and no stun";
        combat_status_text_ui.text = "";
    }


    void Update()
    {
        combat_status_text_ui.text = playerStatusCombat;
        float PlayerStamina = staminaScript.stamina;
        if (canAttackorDefence)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (PlayerStamina >= 20)
                    {
                        topAttacking();
                        StartCoroutine(WaitForSeconds());
                    }
                    else
                    {
                        stunned(3f);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (PlayerStamina >= 20)
                    {
                        bottomAttacking();
                        StartCoroutine(WaitForSeconds());
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

    void topAttacking()
    {
        //top attack animation
        staminaScript.staminaCost(20f);
        playerStatusCombat = "top_attack";
        //StartCoroutine(WaitForSeconds());
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointTop.position, attackRange, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {
            Debug.Log("Hit to player 2");
        }
    }
    private void OnDrawGizmosSelected()
    {

    }
    void bottomAttacking()
    {
        //top attack animation
        staminaScript.staminaCost(20f);
        playerStatusCombat = "bot_attack";

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPointBottom.position, attackRange, oppositePlayer);
        foreach (Collider2D Player_2 in hitEnemy)
        {
            Debug.Log("Hit to player 2");
        }
    }

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

    void attackIdlePosition()
    {
        //attack idle animation
        playerStatusCombat = "attack idle";
    }
    void defendIdlePosition()
    {
        //defend idle animation
        playerStatusCombat = "defend idle";
    }
    void idlePosition()
    {
        //idle position animation
        playerStatusCombat = "idle";
    }

    public void stunned(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float stunDuration)
    {
        playerMovementScript.canMove = false;
        canAttackorDefence = false;

        //stun animation
        stunnedUIText.text = "Stunned";
        playerStatusCombat = "stunned";
        yield return new WaitForSeconds(stunDuration);
        stunnedUIText.text = "no stunned";
        playerMovementScript.canMove = true;
        canAttackorDefence = true;

        playerStatusCombat = "idle";
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(10f);
    }


}

