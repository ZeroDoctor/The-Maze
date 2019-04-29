using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
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
}
