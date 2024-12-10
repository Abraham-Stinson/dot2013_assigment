using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRiderUpsideDown : MonoBehaviour
{
    public static IsRiderUpsideDown isRiderUpsideDownScript;
    public float timer = 0;
    public bool isWorkTimer = false;

    void Start()
    {
        if (isRiderUpsideDownScript == null)
        {
            isRiderUpsideDownScript = this;
        }
    }

    private void Update()
    {
        if (isWorkTimer)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                
                RideBMX.rideBMXScript.isGround = true;
            }
        }
        else
        {
            timer = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            RideBMX.rideBMXScript.upsidedPositions = RideBMX.rideBMXScript.transform.position;
            RideBMX.rideBMXScript.canInput = false;
            RideBMX.rideBMXScript.moveInputX = 0;
            RideBMX.rideBMXScript.moveInputY = 0;
            isWorkTimer = true;
        }
    } 
}
