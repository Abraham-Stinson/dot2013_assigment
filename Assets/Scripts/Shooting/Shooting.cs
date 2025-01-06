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
    int sliderValue = 0,points;
    int sliderRandomValue;
    public int sliderShootRandomValue;
    public GameObject polarBear,handGun;
    SpriteRenderer polarBearSpriteRenderer;
    bool isShootedBear = false,isShooted=false;
    Text pointText;
    void Start()
    {
        sliderValue = ((int)shootingSlider.value);
        sliderRandomValue = Random.Range(10, 50);
        sliderShootRandomValue= Random.Range(0, 101);
        Debug.Log("Hýz :"+sliderRandomValue);
        Debug.Log("Hedef :"+sliderShootRandomValue);
        polarBearSpriteRenderer = polarBear.GetComponent<SpriteRenderer>();
        pointText= pointTextObj.GetComponent<Text>();
    }

   
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            
            Debug.Log("Hýz :" + sliderRandomValue);
            Debug.Log("Hedef :" + sliderShootRandomValue);
            if (sliderShootRandomValue>=(shootingSlider.value-9) && sliderShootRandomValue<=(shootingSlider.value+9)) // hedef:60 value:65 value fazlasý ve eksisi: 55 ve 65
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
        shootAreaObj.transform.position= new Vector3((((sliderShootRandomValue*(0.080f)) + 0.0497f)-4.97f), -4.83f, -0.86f);//value 0 -510 value 100 51
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
        if (sliderShootRandomValue>=((int)shootingSlider.value))
        {
            points += sliderShootRandomValue - ((int)shootingSlider.value);
        }
        else if (sliderShootRandomValue < ((int)shootingSlider.value))
        {
            points -= sliderShootRandomValue - ((int)shootingSlider.value);
        }
        
        isShootedBear = false;
    }
}
