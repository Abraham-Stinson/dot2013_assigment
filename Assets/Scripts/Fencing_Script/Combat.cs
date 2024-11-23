using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public BoxCollider2D foe;
    public float swordSpeed = 5f, swingSpeed = 100f;
    public float targetAngelY, targetAngelZ;
    private BoxCollider2D playerSabre;
    public string playerStatusCombat;
    void Start()
    {
        playerSabre = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.W)&& !Input.GetMouseButtonUp(0))
            {
                topAttackWait();
            }

            else if (Input.GetKey(KeyCode.S) && !Input.GetMouseButtonUp(0))
            {
                bottomAttackWait();
            }

            else if (Input.GetKeyUp(KeyCode.W) && Input.GetMouseButtonUp(0))
            {
                topAttacking();
            }

            else if (Input.GetKeyUp(KeyCode.S) && Input.GetMouseButtonUp(0))
            {
                bottomAttacking();
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
    void test() {
        Debug.Log("szxdxzasdad");
    }
    void topAttackWait()
    {
        Debug.Log("Top Attack");
        changeRotate(45f, 45f);
    }

    void bottomAttackWait()
    {
        Debug.Log("Bot Attack");
        changeRotate(45f, -30f);
    }

    void topAttacking()
    {
        changeRotate(-30f, -30f);
    }
    void bottomAttacking()
    {
        changeRotate(30f, -30f);
    }


    void topDefence()
    {
        Debug.Log("Top Defence");
    }

    void bottomDefence()
    {
        Debug.Log("Bot Defence");
    }

    void attackIdlePosition()
    {
        changeRotate(45f, 0f);
    }
    void defendIdlePosition()
    {

    }

    
    void idlePosition()
    {
        changeRotate(0f, 0f);
    }

    void changeRotate(float targetAngelY, float targetAngelZ)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngelY, targetAngelZ);

        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, swordSpeed * Time.deltaTime);
    }
}
