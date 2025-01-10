using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public InputField nameField;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatGround;
    bool grounded;

    public GameObject panel;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

  
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatGround);
        MyInput();
        SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
        
        //Running();
    }
    public void InputField()
    {
        if (nameField.text == "furkan")
        {
            Debug.Log("sa");
        }
    }
    private void Running()
    {
       /* if (sifte basili tutarsa)
        {
       hýzý yukselt
        }*/
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); 
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude>moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag=="atari")
        {
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "atari")
        {
            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void Fencing()
    {
        SceneManager.LoadScene(1);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
    public void Bmx()
    {
        SceneManager.LoadScene(2);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
    public void Boxing()
    {
        SceneManager.LoadScene(3);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
    public void Shooting()
    {
        SceneManager.LoadScene(5);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
    public void Runer()
    {
        SceneManager.LoadScene(4);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
}
