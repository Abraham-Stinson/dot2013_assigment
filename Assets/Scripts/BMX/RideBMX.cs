using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RideBMX : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] private Rigidbody2D bmxRB;


    [SerializeField] private float bmxSpeed = 150f;
    [SerializeField] private float rotationSpeed = 300f;
    private float moveInput;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        frontTireRB.AddTorque(-moveInput * bmxSpeed * Time.fixedDeltaTime);
        backTireRB.AddTorque(-moveInput * bmxSpeed * Time.fixedDeltaTime);
        backTireRB.AddTorque(moveInput * bmxSpeed * Time.fixedDeltaTime);
    }
}
