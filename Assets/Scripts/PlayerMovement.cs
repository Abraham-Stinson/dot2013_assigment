using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    string playerStatus;
    float speed = 5f;
    float dashSpeed = 20f;
    float dashTime = 0.2f;
    float doubleTabTime = 0.2f;


    private float lastTapTimeA, lastTapTimeD;
    private bool isDash;
    private Vector2 dashDirection;
    private float dashTimer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        else  {
            walking();
            dashInput();
        }
        Debug.Log(playerStatus);
    }
    void walking()
    {
        
        float xMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(speed * xMovement * Time.deltaTime, 0, 0);
        if (Input.GetAxis("Horizontal") != 0)
        {
            playerStatus = "walking";
        }
        else {
            playerStatus = "idle";
        }

    }

    void dashInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            if (Time.time - lastTapTimeA < doubleTabTime)
            {
                StartDash(Vector2.left);
                playerStatus = "dashingLeft";
            }
            lastTapTimeA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            
            if (Time.time - lastTapTimeD < doubleTabTime)
            {
                StartDash(Vector2.right);
                playerStatus = "dashingRight";
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
