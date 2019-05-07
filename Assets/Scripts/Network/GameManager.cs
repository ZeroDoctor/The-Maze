using Photon.Pun;
using Photon.Realtime;
using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject templarPrefab;

    [Serializable]
    public class SpawnPoints
    {
        public GameObject spawnPoint;
    }

    [SerializeField]
    public SpawnPoints spawn;

    private static Random getrandom = new Random();

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

        int x = getrandom.Next(50, 400);
        float y = 0.8f;
        int z = getrandom.Next(50, 400);

        spawn.spawnPoint = new GameObject("Spawn");
        spawn.spawnPoint.transform.position = new Vector3((float)x, y, (float)z);
        Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point " + index);
        PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint.transform.position, spawn.spawnPoint.transform.rotation, 0);
    }
}
