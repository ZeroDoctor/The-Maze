using Photon.Pun;
using Photon.Realtime;
using System;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;
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

        Vector3 randPos = Vector3.zero;
        int myCheck = 0;
        do
        {
            myCheck = 0;
            randPos = new Vector3(UnityRandom.Range(50f, 400f), 6f, UnityRandom.Range(50f, 400f));
            Collider[] hitColliders = Physics.OverlapSphere(randPos, 7f);
            for (int j = 0; j < hitColliders.Length; j++)
            {
                if (hitColliders[j].name == "Terrain")
                {
                    myCheck++;
                }
            }

            // verify that position is above terrain
            RaycastHit hit;
            if (Physics.Raycast(randPos, Vector3.up, out hit, 250))
            {
                float distanceToGround = hit.distance;
                if (hit.transform.name == "Terrain")
                {
                    // there is something else above us
                    myCheck = 1;
                }
                else
                {
                    // all is good
                    randPos.y -= (distanceToGround - 0.8f);
                }
            }
        } while (myCheck > 0);
        Vector3 location = randPos;

        spawn.spawnPoint = new GameObject("Spawn");
        spawn.spawnPoint.transform.position = location;
        Debug.Log("Instantiating LocalPlayer: " + playerName.NickName + " at spawn point " + index);
        PhotonNetwork.Instantiate(templarPrefab.name, spawn.spawnPoint.transform.position, spawn.spawnPoint.transform.rotation, 0);
    }
}
