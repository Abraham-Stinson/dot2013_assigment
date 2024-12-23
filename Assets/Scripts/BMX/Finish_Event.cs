using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish_Event : MonoBehaviour
{
    public static Finish_Event finishScript;
    public bool hasEnterToFinish = false;

    private void Start()
    {
        if (finishScript == null)
        {
            finishScript = this;
        }   
    }
    public void OnTriggerEnter2D(Collider2D finishBoxCollider)
    {
        if (!hasEnterToFinish) { 
            if (finishBoxCollider.gameObject.CompareTag("Player"))
            {
                hasEnterToFinish = true;
                Debug.Log("Player oyunu bitirdi");
                RideBMX.rideBMXScript.canInput = false;
                RideBMX.rideBMXScript.moveInputX = 0f;
                RideBMX.rideBMXScript.moveInputY = 0f;
                RideBMX.rideBMXScript.StartCoroutine(RideBMX.rideBMXScript.EndGame(true));
            }
        }
    }
}
