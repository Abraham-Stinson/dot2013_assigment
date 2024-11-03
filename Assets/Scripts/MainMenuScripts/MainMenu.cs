using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Screen.fullScreen = true;
    }
    public void startButton()
    {
        SceneManager.LoadScene("3D_Gameplay_Scene");
    }
    public void creditsButton()
    {

    }
    public void exitButton()
    {
        Application.Quit();
    }
}
