using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefullStamina : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RideBMX.rideBMXScript.RefullStamina();
            Destroy(gameObject);
        }
    }
    
}
