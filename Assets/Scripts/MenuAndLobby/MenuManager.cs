using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Serializable]
    public class NameFields
    {
        [Header("--TM Pro UGUI Texts")]
        public TextMeshProUGUI playerName0;
        public TextMeshProUGUI playerName1;
        public TextMeshProUGUI playerName2;
        public TextMeshProUGUI playerName3;
        public TextMeshProUGUI playerName4;
        public TextMeshProUGUI playerName5;
        public TextMeshProUGUI playerName6;
        public TextMeshProUGUI playerName7;
        public TextMeshProUGUI playerName8;
        public TextMeshProUGUI playerName9;
        public TextMeshProUGUI playerName10;
        public TextMeshProUGUI playerName11;
        public TextMeshProUGUI playerName12;
        public TextMeshProUGUI playerName13;
        public TextMeshProUGUI playerName14;
        public TextMeshProUGUI playerName15;
    }
    [SerializeField]
    public NameFields names;

    public Canvas mainMenu;
    public Canvas lobby;
    public Button multiplayBtn;
    public TMP_InputField nameInput;

    public bool TryConnect;
    public bool TryJoin;

    // Start is called before the first frame update
    void Start()
    {
        TryConnect = false;
        TryJoin = false;
    }

    // Update is called once per frame
    void Update()
    {
        mainMenu.gameObject.SetActive(!PhotonNetwork.IsConnected && !TryConnect);
        lobby.gameObject.SetActive(PhotonNetwork.IsConnected && !TryConnect && !TryJoin);
    }

    public void OnClickConnectToMaster()
    {
        var nickname = nameInput.text;

        if(nickname == null)
        {
            Debug.Log("Null name...");
            return;
        }

        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v1";

        TryConnect = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TryConnect = false;
        TryJoin = false;
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TryConnect = false;
        Debug.Log("Connected to master");
        TryJoin = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        TryJoin = false;
        Debug.Log("Joined room successfully.");

        Player[] photonPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            ListPlayers(photonPlayers[i], i);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 16 });
        Debug.Log("Join room failed, creating room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        TryJoin = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        int index = Array.IndexOf<Player>(photonPlayers, newPlayer);
        ListPlayers(newPlayer, index);
    }

    public void ListPlayers(Player player, int i)
    {
        Debug.Log("Player" + player.NickName + " connected");

        switch (i)
        {
            case 0:
                names.playerName0.text = player.NickName;
                break;
            case 1:
                names.playerName1.text = player.NickName;
                break;
            case 2:
                names.playerName2.text = player.NickName;
                break;
            case 3:
                names.playerName3.text = player.NickName;
                break;
            case 4:
                names.playerName4.text = player.NickName;
                break;
            case 5:
                names.playerName5.text = player.NickName;
                break;
            case 6:
                names.playerName6.text = player.NickName;
                break;
            case 7:
                names.playerName7.text = player.NickName;
                break;
            case 8:
                names.playerName8.text = player.NickName;
                break;
            case 9:
                names.playerName9.text = player.NickName;
                break;
            case 10:
                names.playerName10.text = player.NickName;
                break;
            case 11:
                names.playerName11.text = player.NickName;
                break;
            case 12:
                names.playerName12.text = player.NickName;
                break;
            case 13:
                names.playerName13.text = player.NickName;
                break;
            case 14:
                names.playerName14.text = player.NickName;
                break;
            case 15:
                names.playerName15.text = player.NickName;
                break;
            default:
                Debug.Log("Too full");
                break;
        }
    }
}
