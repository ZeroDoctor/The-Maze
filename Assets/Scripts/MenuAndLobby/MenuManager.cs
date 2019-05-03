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
        [Header("TM Pro UGUI Texts")]
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
    public Button playBtn;
    public Button exitBtn;
    public Button disconBtn;
    public TMP_InputField nameInput;

    public bool TryConnect;
    public bool TryJoin;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TryConnect = false;
        TryJoin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenu != null)
        {
            mainMenu.gameObject.SetActive(!PhotonNetwork.IsConnected && !TryConnect);
        }
        if (lobby != null)
        {
            lobby.gameObject.SetActive(PhotonNetwork.IsConnected && !TryConnect && !TryJoin);
        }       
    }

    //When multiplaye is clicked, get nickname and connect to master
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

    //On click, start game
    public void OnClickPlay()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }

    //Exits game on click
    public void OnClickExit()
    {
        Debug.Log("Application closing. Thanks for playing!");
        Application.Quit();
    }

    //Disconnect from lobby on click
    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect();
    }

    //When disconnected from master
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TryConnect = false;
        TryJoin = false;
        Debug.Log(cause);
    }

    //When connected to master, auto join random room
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TryConnect = false;
        Debug.Log("Connected to master");
        TryJoin = true;
        PhotonNetwork.JoinRandomRoom();
    }

    //when joined room, players names loaded
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

    //When join random fails, call to create a room, then join the room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 16 });
        Debug.Log("Join room failed, creating room.");
    }

    //If room created failed, major error occurs that can be handled
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        TryJoin = false;
    }

    //New player joins room, loads name into ui
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Player " + newPlayer.NickName + " connected");
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        int index = Array.IndexOf<Player>(photonPlayers, newPlayer);
        ListPlayers(newPlayer, index);
    }

    //Player leaves room, reloads players name into ui
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("Player " + otherPlayer.NickName + " disconnected");
        ResetNames();
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            ListPlayers(photonPlayers[i], i);
        }
    }

    //Calls UI to list player names
    public void ListPlayers(Player player, int i)
    {
        Debug.Log("Player " + player.NickName + " connected");

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

    //Resets ui player names
    public void ResetNames()
    {
        string reset = "Waiting";
        names.playerName0.text = reset;
        names.playerName1.text = reset;
        names.playerName2.text = reset;
        names.playerName3.text = reset;
        names.playerName4.text = reset;
        names.playerName5.text = reset;
        names.playerName6.text = reset;
        names.playerName7.text = reset;
        names.playerName8.text = reset;
        names.playerName9.text = reset;
        names.playerName10.text = reset;
        names.playerName11.text = reset;
        names.playerName12.text = reset;
        names.playerName13.text = reset;
        names.playerName14.text = reset;
        names.playerName15.text = reset;
    }
}
