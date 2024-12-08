using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish_Event : MonoBehaviour
{

    public bool hasEnterToFinish = false;

    public void OnTriggerEnter2D(Collider2D finishBoxCollider)
    {
        if (!hasEnterToFinish) { 
            if (finishBoxCollider.gameObject.CompareTag("Player"))
            {
                hasEnterToFinish = true;
                Debug.Log("Player oyunu bitirdi");
            }
        }
    }
}
