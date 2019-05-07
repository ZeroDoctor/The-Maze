﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPause : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("Menu Buttons")]
    public Button exit;
    public Button resume;

    [Header("Pause Key")]
    public KeyCode pause = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pause) && !pauseMenu.activeSelf)
        {
            if (pauseMenu.activeInHierarchy == true)
            {
                Debug.Log("Closing pause menu");
                pauseMenu.SetActive(false);
            }
            else
            {
                Debug.Log("Opening pause menu");
                pauseMenu.SetActive(true);
            }
        }
    }

    //On exit button click, leave game and return to main menu
    public void OnExitClick()
    {
        Debug.Log("Exit Clicked");
    }

    //On resume button click, close menu, lock cursor, and make cursor invisible
    public void OnResumeClick()
    {
        Debug.Log("Resume Clicked");
    }
}