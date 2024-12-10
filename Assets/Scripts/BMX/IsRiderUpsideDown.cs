using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRiderUpsideDown : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !RideBMX.instance.isGround)
        {
            Debug.Log("Kafayı vurdu");
            RideBMX.instance.isGround = true;
            RideBMX.instance.upsidedPositions = RideBMX.instance.transform.position;
        }
    }
}
