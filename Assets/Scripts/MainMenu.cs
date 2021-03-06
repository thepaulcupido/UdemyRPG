﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;
    public string loadGameScene;
    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        bool hasKey = PlayerPrefs.HasKey("Current_Scene");
        continueButton.SetActive(hasKey);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {  
        // Will only work on build
        Application.Quit();
    }
}
