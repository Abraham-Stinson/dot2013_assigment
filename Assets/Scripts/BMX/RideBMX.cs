using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class RideBMX : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] private Rigidbody2D bmxRB;


    [SerializeField] private float bmxSpeed = 150f;
    [SerializeField, Range(0f, 500)] private float rotationSpeed = 100f;
    private float moveInputX, moveInputY;

    //stamina
    public static RideBMX instance;
    [SerializeField] private Image staminaMeter;
    [SerializeField] private Gradient gradientForStaminaMeter;
    [SerializeField] private float maxStamina = 100f;

    //distance
    [SerializeField] private TextMeshProUGUI distanceText;
    private float stamina;
    private Vector2 startPosition;
    void Start()
    {
        stamina = maxStamina;
        UpdatingUI();

        if (instance == null)
        {
            instance = this;    
        }

        startPosition = bmxRB.position;
    }

    // Update is called once per frame
    private void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");

        Vector2 distance = (Vector2)bmxRB.position - startPosition;
        distance.y = 0f;

        if (distance.x < 0)
        {
            distance.x = 0;
        }

        distanceText.text = "Score: "+distance.x.ToString("F0");
        
    }

    private void FixedUpdate()
    {
        //frontTireRB.AddTorque(-moveInput * bmxSpeed * Time.fixedDeltaTime);
        backTireRB.AddTorque(-moveInputX * bmxSpeed * Time.fixedDeltaTime);
        bmxRB.AddTorque(moveInputX * bmxSpeed * Time.fixedDeltaTime);

        if (moveInputY > 0)
        {
            bmxRB.transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);

        }
        else if (moveInputY < 0)
        {
            bmxRB.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        }

        stamina -= Time.deltaTime * Mathf.Abs(moveInputX);
        UpdatingUI();
    }

    private void UpdatingUI()
    {
        staminaMeter.fillAmount = (stamina / maxStamina);
        staminaMeter.color = gradientForStaminaMeter.Evaluate(staminaMeter.fillAmount);
    }
    public void RefullStamina()
    {
        stamina = maxStamina;
        UpdatingUI();
    }
}
