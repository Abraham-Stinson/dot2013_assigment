using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxing : MonoBehaviour
{
    public GameObject leftArea;
    public GameObject MiddleArea;
    public GameObject RightArea;
    public GameObject Gloves;
    public GameObject LeftGlove;
    public GameObject RightGlove;
    public bool isPunchedRight=false;
    public bool isPunchedLeft=false;
    public GameObject cin;
    public string cinTransform, playerTransform;
    int cinRandom = 0,randomInvoke=0;
    SpriteRenderer cinSpriteRenderer;

    void Start()
    {
        cinRandom = Random.Range(0, 3);
        randomInvoke= Random.Range(1, 3);
        Invoke("RandomCinTransform1", randomInvoke);
        cinSpriteRenderer= cin.GetComponent<SpriteRenderer>();
    }
    void RandomCinTransform1()
    {
        cinRandom = Random.Range(0, 3);
        randomInvoke = Random.Range(1, 3);
        Invoke("RandomCinTransform2", randomInvoke);
    }
    void RandomCinTransform2()
    {
        cinRandom = Random.Range(0, 3);
        randomInvoke = Random.Range(1, 3);
        Invoke("RandomCinTransform1", randomInvoke);
    }

    void FixedUpdate()
    {
        
        if (cinRandom==0)
        {
            cin.transform.position = new Vector3(-6f, -1.3815f, -0.33f);
        }
        if (cinRandom == 1)
        {
            cin.transform.position = new Vector3(0f, -1.3815f, -0.33f);
        }
        if (cinRandom == 2)
        {
            cin.transform.position = new Vector3(6f, -1.3815f, -0.33f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Gloves.transform.position = new Vector3(leftArea.transform.position.x,leftArea.transform.position.y,leftArea.transform.position.z);
            while (Gloves.transform.position.x>=-6)
            {
                Gloves.transform.position -= new Vector3(0.1f * Time.deltaTime, 0, 0);
                playerTransform = "Left";
            }
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            Gloves.transform.position = new Vector3(MiddleArea.transform.position.x, MiddleArea.transform.position.y, MiddleArea.transform.position.z);
            playerTransform = "Middle";
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Gloves.transform.position = new Vector3(RightArea.transform.position.x, RightArea.transform.position.y, RightArea.transform.position.z);
            while (Gloves.transform.position.x <= 6)
            {
                Gloves.transform.position += new Vector3(0.1f * Time.deltaTime, 0, 0);
                playerTransform = "Right";
            }
        }
        if (Input.GetKey(KeyCode.Q)&&isPunchedLeft==false)
        {
            LeftPunch();
        }
        if (Input.GetKey(KeyCode.E)&&isPunchedRight==false)
        {
            RightPunch();
        }
        if (cin.transform.position.x==6)
        {
            cinTransform = "Right";
        }
        if (cin.transform.position.x == 0)
        {
            cinTransform = "Middle";
        }
        if (cin.transform.position.x == -6)
        {
            cinTransform = "Left";
        }
        if (isPunchedLeft==true && cinTransform==playerTransform|| isPunchedRight == true && cinTransform == playerTransform)
        {
            cinSpriteRenderer.color= Color.red;
        }
        else
        {
            cinSpriteRenderer.color = Color.white;
        }
       /*if (isPunchedRight == true && cinTransform == playerTransform)
        {
            Debug.Log("Hit");
            cinSpriteRenderer.color = Color.red;
        }*/
    }
    public void LeftPunch()
    {
        isPunchedLeft= true;
        LeftGlove.transform.localScale += new Vector3(250f*Time.deltaTime, 250f*Time.deltaTime, 1);
        LeftGlove.transform.position += new Vector3(3f, Time.deltaTime*5f, 0);
        Invoke("CoolDownLeft", 0.5f);
    }
    public void CoolDownLeft()
    {
        LeftGlove.transform.localScale = new Vector3(5,5, 1);
        LeftGlove.transform.localPosition = new Vector3(1.25f, -1.507821f, -8f);
        isPunchedLeft = false;
    }
    public void CoolDownRight()
    {
        RightGlove.transform.localScale = new Vector3(5, 5, 1);
        RightGlove.transform.localPosition = new Vector3(-1.251768f, -1.507821f, -8f);
        isPunchedRight = false;
    }
    public void RightPunch()
    {
        isPunchedRight = true;
        RightGlove.transform.localScale += new Vector3(250f * Time.deltaTime, 250f * Time.deltaTime, 1);
        RightGlove.transform.position += new Vector3(-3f, Time.deltaTime * 5f, 0);
        Invoke("CoolDownRight", 0.5f);
    }
}
