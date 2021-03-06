﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPause : MonoBehaviourPun
{
    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("Menu Buttons")]
    public Button exit;
    public Button resume;

    [Header("Pause Key")]
    public KeyCode pause = KeyCode.Escape;

    [Header("Controller")]
    public CharacterController control;

    [Header("Is Dead")]
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pause))
        {
            if (pauseMenu.activeInHierarchy == true && !isDead)
            {
                Debug.Log("Closing pause menu");
                control.enabled = true;
                pauseMenu.SetActive(false);
            }
            else
            {
                Debug.Log("Opening pause menu");
                control.enabled = false;
                pauseMenu.SetActive(true);
            }
        }
    }

    //On exit button click, leave game and return to main menu
    public void OnExitClick()
    {
        Debug.Log("Exit Clicked");
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    //On resume button click, close menu, lock cursor, and make cursor invisible
    public void OnResumeClick()
    {
        Debug.Log("Resume Clicked");
        control.enabled = true;
        pauseMenu.SetActive(false);
    }
}
