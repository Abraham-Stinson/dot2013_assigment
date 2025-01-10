using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gloves : MonoBehaviour
{
    public GameObject playerTxt;
    Text playerTextTxt;
    void Start()
    {
        playerTextTxt=playerTxt.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="meteor")
        {
            if (playerTextTxt.text == "Player: 1")
            {
                playerTextTxt.text = "Player: 0";
            }
            if (playerTextTxt.text == "Player: 2")
            {
                playerTextTxt.text = "Player: 1";
            }
            if (playerTextTxt.text == "Player: 3")
            {
                playerTextTxt.text = "Player: 2";
            }
        }
    }
}
    