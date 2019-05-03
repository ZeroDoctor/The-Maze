using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject templarPrefab;
    
    [Serializable]
    public class SpawnPoints
    {
        public GameObject spawnPoint1;
        public GameObject spawnPoint2;
        public GameObject spawnPoint3;
        public GameObject spawnPoint4;
        public GameObject spawnPoint5;
        public GameObject spawnPoint6;
        public GameObject spawnPoint7;
        public GameObject spawnPoint8;
        public GameObject spawnPoint9;
        public GameObject spawnPoint10;
        public GameObject spawnPoint11;
        public GameObject spawnPoint12;
        public GameObject spawnPoint13;
        public GameObject spawnPoint14;
        public GameObject spawnPoint15;
        public GameObject spawnPoint16;
    }
    [SerializeField]
    public SpawnPoints spawn;

    // Start is called before the first frame update
    void Start()
    {
        if (templarPrefab == null)
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
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint1.transform.position, spawn.spawnPoint1.transform.rotation, 0);
                break;
            case 1:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 2");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint2.transform.position, spawn.spawnPoint2.transform.rotation, 0);
                break;
            case 2:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 3");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint3.transform.position, spawn.spawnPoint3.transform.rotation, 0);
                break;
            case 3:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 4");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint4.transform.position, spawn.spawnPoint4.transform.rotation, 0);
                break;
            case 4:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 5");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint5.transform.position, spawn.spawnPoint5.transform.rotation, 0);
                break;
            case 5:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 6");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint6.transform.position, spawn.spawnPoint6.transform.rotation, 0);
                break;
            case 6:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 7");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint7.transform.position, spawn.spawnPoint7.transform.rotation, 0);
                break;
            case 7:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 8");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint8.transform.position, spawn.spawnPoint8.transform.rotation, 0);
                break;
            case 8:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 9");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint9.transform.position, spawn.spawnPoint9.transform.rotation, 0);
                break;
            case 9:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 10");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint10.transform.position, spawn.spawnPoint10.transform.rotation, 0);
                break;
            case 10:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 11");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint11.transform.position, spawn.spawnPoint11.transform.rotation, 0);
                break;
            case 11:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 12");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint12.transform.position, spawn.spawnPoint12.transform.rotation, 0);
                break;
            case 12:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 13");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint13.transform.position, spawn.spawnPoint13.transform.rotation, 0);
                break;
            case 13:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 14");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint14.transform.position, spawn.spawnPoint14.transform.rotation, 0);
                break;
            case 14:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 15");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint15.transform.position, spawn.spawnPoint15.transform.rotation, 0);
                break;
            case 15:
                Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point 16");
                PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint16.transform.position, spawn.spawnPoint16.transform.rotation, 0);
                break;
            default:
                Debug.Log("Instantiation error");
                break;
        }
    }
}
