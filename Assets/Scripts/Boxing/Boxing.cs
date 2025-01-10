using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boxing : MonoBehaviour
{
    public GameObject leftArea,meteors,leftCin,rightCin,middleCin, meteors2, meteors3, meteors4, meteors5;
    public GameObject MiddleArea;
    public GameObject RightArea;
    public GameObject Gloves;
    public GameObject LeftGlove;
    public GameObject RightGlove;
    public bool isPunchedRight=false;
    public bool isPunchedLeft=false;
    bool isDodge = false, canMove=true,cinHittedSoundBoolean=false; 
    public GameObject cin,playerText,enemyText,cinRed,cinYellow,cinGreen,cinHits,cinSpeak;
    string cinTransform, playerTransform,cinToDo;
    int cinRandomTransform = 0,cinRandomInvoke=0,cinRandomPunchInvoke=0,cinToDoRandom=0;
    int cinHealth = 1000,playerHealth=3;
    Text playerTextTxt,enemyTextTxt;
    SpriteRenderer cinSpriteRendererRed, cinSpriteRendererYellow, cinSpriteRendererGreen, cinSpriteRendererHit, cinSpriteRendererSpeak;
    public GameObject sound1, sound2, sound3, sound4, sound5, sound6, sound7, sound8, sound9, sound10, sound11, sound12, sound13, sound14, sound15;

    void Start()
    {
        sound1.SetActive(true);
        Invoke("Sfx", 6f);
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
        cinHits.SetActive(false); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(true); leftCin.SetActive(false); rightCin.SetActive(false);middleCin.SetActive(false);
    }
    void Sfx()
    {
        //5 6 7 8
        sound5.SetActive(false); sound6.SetActive(false); sound7.SetActive(false); sound8.SetActive(false);
        int randomSound = Random.Range(0,4);
        if(randomSound==0)
        sound5.SetActive(true);
        if (randomSound == 1)
            sound6.SetActive(true);
        if (randomSound == 2)
            sound7.SetActive(true);
        if (randomSound == 3)
            sound8.SetActive(true);
        Invoke("Sfx1", 6f);
    }
    void Sfx1()
    {
        //5 6 7 8
        sound5.SetActive(false); sound6.SetActive(false); sound7.SetActive(false); sound8.SetActive(false);
        int randomSound = Random.Range(0, 4);
        if (randomSound == 0)
            sound5.SetActive(true);
        if (randomSound == 1)
            sound6.SetActive(true);
        if (randomSound == 2)
            sound7.SetActive(true);
        if (randomSound == 3)
            sound8.SetActive(true);
        Invoke("Sfx", 6f);
    }
    void CinRandomPunch1()
    {
        //leftCin.SetActive(false); rightCin.SetActive(false);
        middleCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        leftCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        rightCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        cin.SetActive(true);
        meteors.SetActive(false); meteors2.SetActive(false); meteors3.SetActive(false); meteors4.SetActive(false); meteors5.SetActive(false);
        meteors.transform.position = new Vector3(0, 0, 0);
        meteors2.transform.position = new Vector3(0, 0, 0);
        meteors3.transform.position = new Vector3(0, 0, 0);
        meteors4.transform.position = new Vector3(0, 0, 0);
        meteors5.transform.position = new Vector3(0, 0, 0);
        //cin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        if (cinToDo=="Hit")
        {
            cinRandomPunchInvoke = Random.Range(1, 3);
            if (cinTransform == playerTransform && (isDodge == false))
            {
                cinHits.SetActive(true); cinRed.SetActive(false); cinYellow.SetActive(false); cinGreen.SetActive(false); cinSpeak.SetActive(false);
                if (playerTextTxt.text == "Player: 3")
                {
                    playerHealth = 2;
                    playerTextTxt.text = "Player: 2";
                }
                if (playerTextTxt.text == "Player: 2")
                {
                    playerHealth = 1;
                    playerTextTxt.text = "Player: 1";
                }
                if (playerTextTxt.text == "Player: 1")
                {
                    playerHealth = 0;
                    playerTextTxt.text = "Player: 0";
                }
               // playerHealth -= 1;
            }
            cin.transform.localScale = new Vector3(5, 5, 5);
            canMove = false;
            Invoke("CinRandomPunch2", cinRandomPunchInvoke);
        }
        if (cinToDo=="Red")
        {
            sound12.SetActive(true);
            Invoke("SoundController1", 4.8f);
            canMove = true;
            cin.SetActive(false);
            int meteorRandom =Random.Range(0, 5);
            if(meteorRandom==0)
            meteors.SetActive(true);
            if (meteorRandom == 1)
                meteors2.SetActive(true);
            if (meteorRandom == 2)
                meteors3.SetActive(true);
            if (meteorRandom == 3)
                meteors4.SetActive(true);
            if (meteorRandom == 4)
                meteors5.SetActive(true);
            Invoke("CinRandomPunch2", 6f);
        }
        if (cinToDo == "Green")
        {
            sound14.SetActive(true);
            Invoke("SoundController1", 3f);
            canMove = true;
            cinHealth += 200;
            Invoke("CinRandomPunch2", 9f);
        }
        if (cinToDo == "Yellow")
        {
            sound13.SetActive(true);
            Invoke("SoundController3", 2.4f);
            leftCin.SetActive(true);rightCin.SetActive(true);middleCin.SetActive(true);
            middleCin.transform.localScale = new Vector3(5, 5, 5);
            leftCin.transform.localScale = new Vector3(5, 5, 5);
            rightCin.transform.localScale = new Vector3(5, 5, 5);
            Invoke("CinRandomPunch2", 1f);
            Invoke("CinCloneDown", 5f);
        }
       
    }
    void SoundController1()
    {
        sound12.SetActive(false);
    }
    void SoundController2()
    {
        sound14.SetActive(false);
    }
    void SoundController3()
    {
        sound13.SetActive(false);
    }
    void CinRandomPunch2()
    {
        middleCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        leftCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
        rightCin.transform.localScale = new Vector3(3.217957f, 3.217957f, 3.217957f);
       // leftCin.SetActive(false); rightCin.SetActive(false);
        cin.SetActive(true);
        meteors.SetActive(false); meteors2.SetActive(false); meteors3.SetActive(false); meteors4.SetActive(false); meteors5.SetActive(false);
        meteors.transform.position = new Vector3(0, 0, 0);
        meteors2.transform.position = new Vector3(0, 0, 0);
        meteors3.transform.position = new Vector3(0, 0, 0);
        meteors4.transform.position = new Vector3(0, 0, 0);
        meteors5.transform.position = new Vector3(0, 0, 0);
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
            sound12.SetActive(true);
            Invoke("SoundController1", 4.8f);
            canMove = true;
            cin.SetActive(false);
            int meteorRandom = Random.Range(0, 5);
            if (meteorRandom == 0)
                meteors.SetActive(true);
            if (meteorRandom == 1)
                meteors2.SetActive(true);
            if (meteorRandom == 2)
                meteors3.SetActive(true);
            if (meteorRandom == 3)
                meteors4.SetActive(true);
            if (meteorRandom == 4)
                meteors5.SetActive(true);

            Invoke("CinRandomPunch1", 6f);
        }
        if (cinToDo == "Green")
        {
            sound14.SetActive(true);
            Invoke("SoundController1", 3f);
            canMove = true;
            cinHealth += 200;
            Invoke("CinRandomPunch1", 9f);
        }
        if (cinToDo == "Yellow")
        {
            sound13.SetActive(true);
            Invoke("SoundController3", 2.4f);
            leftCin.SetActive(true); rightCin.SetActive(true); middleCin.SetActive(true);
            middleCin.transform.localScale = new Vector3(5, 5, 5);
            leftCin.transform.localScale = new Vector3(5, 5, 5);
            rightCin.transform.localScale = new Vector3(5, 5, 5);
            Invoke("CinRandomPunch1", 1f);
            Invoke("CinCloneDown", 5f);
        }
    }
    void CinCloneDown()
    {
        leftCin.SetActive(false); rightCin.SetActive(false); middleCin.SetActive(false);
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
        if (playerTextTxt.text == "Player: 3")
        {
            playerHealth = 3;
        }
        if (playerTextTxt.text == "Player: 2")
        {
            playerHealth = 2;
        }
        if (playerTextTxt.text == "Player: 1")
        {
            playerHealth = 1;
        }
        if (playerTextTxt.text == "Player: 0")
        {
            playerHealth = 0;
        }
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
            cin.transform.position = new Vector3(-6f, -1.3815f, -0.30f);
        }
        if (cinRandomTransform == 1 && canMove)
        {
            cin.transform.position = new Vector3(0f, -1.3815f, -0.30f);
        }
        if (cinRandomTransform == 2 && canMove)
        {
            cin.transform.position = new Vector3(6f, -1.3815f, -0.30f);
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
            if (cinHittedSoundBoolean==false)
            {
                cinHittedSoundBoolean = true;
                int cinSoundRandom = Random.Range(0, 3);
                if (cinSoundRandom == 0)
                    sound2.SetActive(true);
                if (cinSoundRandom == 1)
                    sound3.SetActive(true);
                if (cinSoundRandom == 2)
                    sound4.SetActive(true);
                Invoke("CinHittedSoundCoolDown", 2.4f);
            }
            cinHealth -= 1;
       
            //isPunchedLeft= false;
           // isPunchedRight= false;
        }
    }
    void CinHittedSoundCoolDown()
    {
        sound2.SetActive(false); sound3.SetActive(false); sound4.SetActive(false);
        cinHittedSoundBoolean = false;
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
