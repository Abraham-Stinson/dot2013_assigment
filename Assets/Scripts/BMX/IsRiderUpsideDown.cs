﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class IsRiderUpsideDown : MonoBehaviour
{
    public static IsRiderUpsideDown isRiderUpsideDownScript;
    public float timer = 0;
    public bool isWorkTimer = false;
    [SerializeField] public bool firstTouch;
    void Start()
    {
        firstTouch = true;
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

            if (timer >= 0.5f)
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
            if (firstTouch)
            {
                RideBMX.rideBMXScript.upsidedPositions = RideBMX.rideBMXScript.transform.position;
                firstTouch = false;
            }
            RideBMX.rideBMXScript.moveInputX = 0;
            RideBMX.rideBMXScript.moveInputY = 0;
            RideBMX.rideBMXScript.bmxRB.mass = 200f;
            RideBMX.rideBMXScript.canInput = false;
            RideBMX.rideBMXScript.inGame = false;
            Timer.timerScript.isBMXTimerWorking = false;
            isWorkTimer = true;
        }
    } 
}
