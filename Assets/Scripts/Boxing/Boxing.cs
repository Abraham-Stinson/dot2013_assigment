using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boxing : MonoBehaviour
{
    public GameObject leftArea,meteors;
    public GameObject MiddleArea;
    public GameObject RightArea;
    public GameObject Gloves;
    public GameObject LeftGlove;
    public GameObject RightGlove;
    public bool isPunchedRight=false;
    public bool isPunchedLeft=false;
    bool isDodge = false, canMove=true; 
    public GameObject cin,playerText,enemyText,cinRed,cinYellow,cinGreen,cinHits,cinSpeak;
    string cinTransform, playerTransform,cinToDo;
    int cinRandomTransform = 0,cinRandomInvoke=0,cinRandomPunchInvoke=0,cinToDoRandom=0;
    int cinHealth = 1000,playerHealth=100;
    Text playerTextTxt,enemyTextTxt;
    SpriteRenderer cinSpriteRendererRed, cinSpriteRendererYellow, cinSpriteRendererGreen, cinSpriteRendererHit, cinSpriteRendererSpeak;

    void Start()
    {
        playerTextTxt=playerText.GetComponent<Text>();
        enemyTextTxt=enemyText.GetComponent<Text>();
        cinRandomPunchInvoke = Random.Range(1, 5);
        cinRandomTransform = Random.Range(0, 3);
        cinRandomInvoke= Random.Range(1, 3);
        Invoke("CinRandomPunch1", cinRandomPunchInvoke);
        Invoke("RandomCinTransform1", cinRandomInvoke);
        RandomCinTransform3();
        cinSpriteRendererRed = cinRed.GetComponent<SpriteRenderer>();
        cinSpriteRendererYellow = cinYellow.GetComponent<SpriteRenderer>();
        cinSpriteRendererGreen = cinGreen.GetComponent<SpriteRenderer>();
        cinSpriteRendererHit = cinHits.GetComponent<SpriteRenderer>();
        cinSpriteRendererSpeak = cinSpeak.GetComponent<SpriteRenderer>();
        cinHits.SetActive(false); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(true);
    }
    void CinRandomPunch1()
    {
        cin.SetActive(true);
        meteors.SetActive(false);
        meteors.transform.position = new Vector3(0, 0, 0);
        //cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        if (cinToDo=="Hit")
        {
            cinRandomPunchInvoke = Random.Range(1, 3);
            if (cinTransform == playerTransform && (isDodge == false))
            {
                cinHits.SetActive(true); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(false);
                playerHealth -= 5;
            }
            cin.transform.localScale = new Vector3(5, 5, 5);
            canMove = false;
            Invoke("CinRandomPunch2", cinRandomPunchInvoke);
        }
        if (cinToDo=="Red")
        {
            canMove = true;
            cin.SetActive(false);
            meteors.SetActive(true);
            Invoke("CinRandomPunch2", 6f);
        }
        if (cinToDo == "Green")
        {
            canMove = true;
            cinHealth += 200;
            Invoke("CinRandomPunch2", 9f);
        }
        if (cinToDo == "Yellow")
        {
            Invoke("CinRandomPunch2", 2f);
        }
       
    }
    void CinRandomPunch2()
    {
        cin.SetActive(true);
        meteors.SetActive(false);
        meteors.transform.position = new Vector3(0, 0, 0);
        cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        if (cinToDo=="Hit")
        {
            cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
            canMove = true;
            cinRandomPunchInvoke = Random.Range(1, 2);
            Invoke("CinRandomPunch1", cinRandomPunchInvoke);
        }
        if (cinToDo == "Red")
        {
            canMove = true;
            cin.SetActive(false);
            meteors.SetActive(true);
            Invoke("CinRandomPunch1", 6f);
        }
        if (cinToDo == "Green")
        {
            canMove = true;
            cinHealth += 200;
            Invoke("CinRandomPunch1", 9f);
        }
        if (cinToDo == "Yellow")
        {

            Invoke("CinRandomPunch1", 2f);
        }
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
    void RandomCinTransform3()
    {
        cinToDoRandom = Random.Range(0, 4);

        Invoke("RandomCinTransform4", 5f);
    }
    void RandomCinTransform4()
    {
        cinToDoRandom = Random.Range(0, 4);
        Invoke("RandomCinTransform3", 5f);
    }

    void FixedUpdate()
    {

        playerTextTxt.text = "Player: " + playerHealth.ToString();
        enemyTextTxt.text= "Enemy: " + cinHealth.ToString();
        if (cinToDoRandom==0)
        {
            cinToDo = "Red";
            cinHits.SetActive(false); cinRed.SetActive(true); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(false);
        }
        if (cinToDoRandom == 1)
        {
            cinToDo = "Green";
            cinHits.SetActive(false); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(true); cinSpeak.SetActive(false);
        }
        if (cinToDoRandom == 2)
        {
            cinToDo = "Yellow";
            cinHits.SetActive(false); cinRed.SetActive(false); cinYellow.SetActive(true); cinGreen.SetActive(false); cinSpeak.SetActive(false);
        }
        if (cinToDoRandom == 3)
        {
            cinToDo = "Hit";
            cinHits.SetActive(true); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(false);
        }
        if (cinRandomTransform==0&&canMove)
        {
            cin.transform.position = new Vector3(-6f, -1.3815f, -0.33f);
        }
        if (cinRandomTransform == 1 && canMove)
        {
            cin.transform.position = new Vector3(0f, -1.3815f, -0.33f);
        }
        if (cinRandomTransform == 2 && canMove)
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
            cinSpriteRendererRed.color= Color.white;
            cinSpriteRendererYellow.color   = Color.white;
            cinSpriteRendererGreen.color= Color.white;
            cinSpriteRendererHit.color= Color.white;
            cinSpriteRendererSpeak.color= Color.white;
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
            cinSpriteRendererRed.color = Color.white;
            cinSpriteRendererYellow.color = Color.white;
            cinSpriteRendererGreen.color = Color.white;
            cinSpriteRendererHit.color = Color.white;
            cinSpriteRendererSpeak.color = Color.white;
            isDodge = true;
        }
        else
        {
            //Punch Area
            isDodge=false;
            cinSpriteRendererRed.color = Color.red;
            cinSpriteRendererYellow.color = Color.red;
            cinSpriteRendererGreen.color = Color.red;
            cinSpriteRendererHit.color = Color.red;
            cinSpriteRendererSpeak.color = Color.red;
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
