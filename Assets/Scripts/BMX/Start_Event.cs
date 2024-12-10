using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Event : MonoBehaviour
{
    public bool hasExitFromStart = false;
    public void OnTriggerExit2D(Collider2D startBoxCollider)
    {
        if (!hasExitFromStart)
        {
            if (startBoxCollider.gameObject.CompareTag("Player"))
            {
                hasExitFromStart = true;
                Debug.Log("Player başlangıcı yaptı");
            }
        }
    }
}
