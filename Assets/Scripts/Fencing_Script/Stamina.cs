using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{   
    
    public GameObject Player_1;


    protected internal float stamina = 100f, maxStamina = 100f, minStamina = 0f;
    private float staminaRegenSpeed = 10f;
    public Combat combatScript;
    public PlayerMovement playerMovementScript;

    public Text stamineUIText;
    void Start()
    {
         combatScript = Player_1.GetComponent<Combat>();
         playerMovementScript =  Player_1.GetComponent<PlayerMovement>();
         stamineUIText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        updatingStaminaUI();
        staminaRegeneration(staminaRegenSpeed);
        if (stamina == 0f)
        {
            combatScript.stunned(1f);
        }
    }

    void updatingStaminaUI()
    {
        stamineUIText.text = "Stamina: " + stamina.ToString("F0");
        Debug.Log(stamina);

    }
    void staminaRegeneration(float regenSpeed)
    {

        if (stamina >= 0 && stamina < 100)
        {
            stamina += Time.deltaTime * regenSpeed;
        }
    }
    public void staminaCost(float cost)
    {
        stamina -= cost;
    }
}
