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
    bool isDodge = false;
    public GameObject cin;
    public string cinTransform, playerTransform;
    int cinRandomTransform = 0,cinRandomInvoke=0,cinRandomPunchInvoke=0;
    public int cinHealth = 10000,playerHealth=100;
    SpriteRenderer cinSpriteRenderer;

    void Start()
    {
        cinRandomPunchInvoke = Random.Range(1, 5);
        cinRandomTransform = Random.Range(0, 3);
        cinRandomInvoke= Random.Range(1, 3);
        Invoke("CinRandomPunch1", cinRandomPunchInvoke);
        Invoke("RandomCinTransform1", cinRandomInvoke);
        cinSpriteRenderer= cin.GetComponent<SpriteRenderer>();
    }
    void CinRandomPunch1()
    {
        //cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        cinRandomPunchInvoke = Random.Range(1, 5);
        if (cinTransform == playerTransform&&(isDodge==false))
        {
            playerHealth -= 5;
        }
        cin.transform.localScale = new Vector3(5, 5, 5);
        Invoke("CinRandomPunch2", cinRandomPunchInvoke);
    }
    void CinRandomPunch2()
    {
        cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        cinRandomPunchInvoke = Random.Range(1, 2);
        Invoke("CinRandomPunch1", cinRandomPunchInvoke);
    }
    void RandomCinTransform1()
    {
        cinRandomTransform = Random.Range(0, 3);
        cinRandomInvoke = Random.Range(1, 3);
        Invoke("RandomCinTransform2", cinRandomInvoke);
    }
    void RandomCinTransform2()
    {
        cinRandomTransform = Random.Range(0, 3);
        cinRandomInvoke = Random.Range(1, 3);
        Invoke("RandomCinTransform1", cinRandomInvoke);
    }

    void FixedUpdate()
    {
        
        if (cinRandomTransform==0)
        {
            cin.transform.position = new Vector3(-6f, -1.3815f, -0.33f);
        }
        if (cinRandomTransform == 1)
        {
            cin.transform.position = new Vector3(0f, -1.3815f, -0.33f);
        }
        if (cinRandomTransform == 2)
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
            cinHit();
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
    public void cinHit()
    {
        if (isPunchedLeft == true && cinTransform == playerTransform && isPunchedRight == true && cinTransform == playerTransform)
        {
            //Dodge Area
            cinSpriteRenderer.color = Color.white;
            isDodge = true;
        }
        else
        {
            //Punch Area
            isDodge=false;
            cinSpriteRenderer.color = Color.red;
            cinHealth -= 1;
            //isPunchedLeft= false;
           // isPunchedRight= false;
        }
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
