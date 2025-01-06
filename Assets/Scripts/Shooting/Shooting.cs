using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject pointTextObj,shootAreaObj;
    public Slider shootingSlider;
    int sliderValue = 0,points,randomSide;
    int sliderRandomValue;
    public int sliderShootRandomValue;
    public GameObject polarBear,handGun,leftSide,rightSide,middleSide;
    SpriteRenderer polarBearSpriteRenderer;
    bool isShootedBear = false,isShooted=false;
    bool a = true, b = false, c = false, d = false, e = false, f = false, g = false;
    Text pointText;
    void Start()
    {
        sliderValue = ((int)shootingSlider.value);
        sliderRandomValue = Random.Range(10, 50);
        sliderShootRandomValue= Random.Range(0, 101);
        polarBearSpriteRenderer = polarBear.GetComponent<SpriteRenderer>();
        pointText= pointTextObj.GetComponent<Text>();
        PolarBearSpawn();
        PolarBearWalk();
    }
    public void PolarBearSpawn()
    {
        randomSide = Random.Range(0, 2);
        if (randomSide == 0)
        {
            polarBear.transform.position = leftSide.transform.position;
        }
        else if (randomSide==1)
        {
            polarBear.transform.position = leftSide.transform.position;
        }
    }
    void PolarBearLast()
    {
        a = true;
    }
    public void PolarBearWalk()
    {
        if (randomSide == 0 && g == true)
        {
            polarBear.transform.position = new Vector3(0, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = false;
            g = false;
            isShootedBear = false;
            Invoke("PolarBearLast", 0.2f);
        }
        if (randomSide == 1 && g == true)
        {
            polarBear.transform.position = new Vector3(0, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = false;
            g = false;
            isShootedBear = false;
            Invoke("PolarBearLast", 0.2f);
        }
        //f
        if (randomSide == 0 && f == true)
        {
            polarBear.transform.position = new Vector3(-1, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = false; g = true;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && f == true)
        {
            polarBear.transform.position = new Vector3(1, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = false; g = true;
            Invoke("PolarBearWalk", 0.2f);
        }
        
        //e
        if (randomSide == 0 && e == true)
        {
            polarBear.transform.position = new Vector3(-2, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = true;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && e == true)
        {
            polarBear.transform.position = new Vector3(2, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = false; f = true;
            Invoke("PolarBearWalk", 0.2f);
        }
        //d
        if (randomSide == 0 && d == true)
        {
            polarBear.transform.position = new Vector3(-3, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = true; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && d == true)
        {
            polarBear.transform.position = new Vector3(3, 0.23f, -0.23f);
            a = false; b = false; c = false; d = false; e = true; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        //c
        if (randomSide == 0 && c == true)
        {
            polarBear.transform.position = new Vector3(-4, 0.23f, -0.23f);
            a = false; b = false; c = false; d = true; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && c == true)
        {
            polarBear.transform.position = new Vector3(4, 0.23f, -0.23f);
            a = false; b = false; c = false; d = true; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        //b
        if (randomSide == 0 && b == true)
        {
            polarBear.transform.position = new Vector3(-5, 0.23f, -0.23f);
            a = false; b = false; c = true; d = false; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && b == true)
        {
            polarBear.transform.position = new Vector3(5, 0.23f, -0.23f);
            a = false; b = false; c = true; d = false; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        //a
        if (randomSide == 0 && a == true)
        {
            polarBear.transform.position = new Vector3(-6, 0.23f, -0.23f);
            a = false; b = true; c = false; d = false; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        if (randomSide == 1 && a == true)
        {
            polarBear.transform.position = new Vector3(6, 0.23f, -0.23f);
            a = false; b = true; c = false; d = false; e = false; f = false;
            Invoke("PolarBearWalk", 0.2f);
        }
        //g
        
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Hýz :" + sliderRandomValue);
           // Debug.Log("Hedef :" + sliderShootRandomValue);
            if (sliderShootRandomValue>=(shootingSlider.value-5) && sliderShootRandomValue<=(shootingSlider.value+5)) // hedef:60 value:65 value fazlasý ve eksisi: 55 ve 65
            {
                if (isShootedBear==false)
                {
                    isShootedBear = true;
                    polarBearSpriteRenderer.color = Color.red;
                    Invoke("PolarBearShooted", 0.5f);
                    
                }
            }
            if (isShooted==false)
            {
                isShooted = true;
                handGun.transform.position = new Vector3(1.142f, -0.3f, -0.17f);
                Invoke("GunShooted", 0.2f);
            }
            shootingSlider.value = 0;
            sliderRandomValue = Random.Range(10, 50);
            
        }
        shootingSlider.value += sliderRandomValue*Time.deltaTime;
        if (shootingSlider.value>=100)
        {
            shootingSlider.value = 0;
        }
        pointText.text = points.ToString();
        shootAreaObj.transform.position= new Vector3((((sliderShootRandomValue*(0.0925f)) + 0.0497f)-4.97f), -4.83f, -0.86f);//value 0 -510 value 100 51
        //Debug.Log((sliderShootRandomValue * (5.61f) - 510));
    }
    void GunShooted()
    {
        handGun.transform.position = new Vector3(1.142f, -0.5f, -0.17f);
        isShooted = false;
    }
    void PolarBearShooted()
    {
        polarBearSpriteRenderer.color = Color.white;
        sliderShootRandomValue = Random.Range(0, 101);
        PolarBearSpawn();
        PolarBearWalk();
        if (sliderShootRandomValue>=((int)shootingSlider.value))
        {
            points += sliderShootRandomValue - ((int)shootingSlider.value);
        }
        else if (sliderShootRandomValue < ((int)shootingSlider.value))
        {
            points -= sliderShootRandomValue - ((int)shootingSlider.value);
        }
        
       // isShootedBear = false;
    }
}
