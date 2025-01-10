using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencingEasterEgg : MonoBehaviour
{
    public AudioSource gojiraMusicAhCaIra;
    public AudioSource normalMusic;
    private string[] sequence = { "g", "o", "j", "i", "r", "a" };
    private int currentIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            string keyPressed = Input.inputString.ToLower();

            if (!string.IsNullOrEmpty(keyPressed) && keyPressed == sequence[currentIndex])
            {
                currentIndex++;

                if (currentIndex == sequence.Length)
                {
                    PlayMusic();
                    currentIndex = 0;
                }
            }
            else
            {
                currentIndex = 0; 
            }
        }
    }

    void PlayMusic()
    {
        if (!gojiraMusicAhCaIra.isPlaying)
        {
            normalMusic.Stop();
            gojiraMusicAhCaIra.Play();
        }
    }
}
