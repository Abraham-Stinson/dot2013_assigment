using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 5f;
    float dashSpeed = 20f;
    float dashTime = 0.2f;
    float doubleTabTime = 0.2f;

    private float lastTa,pTimeA, lastTapTimeD;
    private bool isDash;
    private Vector2 dashDirection;
    private float dashTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(speed * xMovement * Time.deltaTime, 0, 0);

        if (isDash)
        {

        }

    }

    
}
