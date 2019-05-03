using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject templarPrefab;
    public GameObject spawnPoint1, spawnPoint2;

    // Start is called before the first frame update
    void Start()
    {
        if (templarPrefab == null && spawnPoint1 == null && spawnPoint2 == null)
        {
            Debug.Log("Please attach templar prefab too script");
        }
        else
        {
            InstantiatePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiatePlayer()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Player playerName = PhotonNetwork.LocalPlayer;
        int index = Array.IndexOf(players, playerName);

        switch (index)
        {
            case 0:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 1");
                PhotonNetwork.Instantiate(templarPrefab.name, spawnPoint1.transform.position, spawnPoint1.transform.rotation, 0);
                break;
            case 1:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 2");
                PhotonNetwork.Instantiate(templarPrefab.name, spawnPoint2.transform.position, spawnPoint2.transform.rotation, 0);
                break;
            default:
                Debug.Log("Instantiation error");
                break;
        }
    }
}
