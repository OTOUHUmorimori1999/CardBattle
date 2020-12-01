using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OnlineConnecter : MonoBehaviourPunCallbacks
{
    static string waitRoomName = "waitRoom";
    static string battleRoomName = "battleRoom";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Photonに接続する
    public void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public bool JoinLobby()
    {
        bool flag = PhotonNetwork.JoinLobby();
        return flag;
    }

    public bool ReaveNetwork()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.Disconnect();
            return true;
        }

        return false;
    }

    public Player[] GetPlayers()
    {
        return PhotonNetwork.PlayerList;
    }

    public bool GetConnected()
    {
        return PhotonNetwork.IsConnected;
    }
    //-------------------------------------------------------------

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        // ウェイトルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom(waitRoomName, new RoomOptions(), TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {

    }
}
