using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Slider shootingSlider;
    float sliderValue = 0;
    void Start()
    {
        sliderValue = shootingSlider.value;
    }

   
    void Update()
    {
        shootingSlider.value += 5*Time.deltaTime;
    }
}
