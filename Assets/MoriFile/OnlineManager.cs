using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// MonoBehaviourではなくMonoBehaviourPunCallbacksを継承して、Photonのコールバックを受け取れるようにする
public class OnlineManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject PlayerOne;
    [SerializeField] GameObject PlayerTwo;
    [Space(10)]
    [SerializeField] Camera POneCamera;
    [SerializeField] Camera PTwoCamera;
    [Space(10)]
    [SerializeField] GameObject OnlinePlayerObj;
    [SerializeField] GameObject OnlineGalleryObj;

    [SerializeField] Player[] test;

    // Start is called before the first frame update
    void Awake()
    {
        ConnectionRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ConnectionRoom()
    {
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    public bool DisConnectPhotonRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            return true;
        }
        return false;
    }

    public bool GetOwnerPlayer(int playerNumber)
    {
        switch(playerNumber)
        {
            case 1:
                return PlayerOne.GetPhotonView().AmOwner;
            case 2:
                return PlayerTwo.GetPhotonView().AmOwner;
        }

        return false;
    }
    //-------------------------------------------------------------

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room2", new RoomOptions(), TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        SetUsePlayer();
    }

    void SetUsePlayer()
    {
        Vector3 test = new Vector3();

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            //オーナー権限を確保
            PlayerOne.GetPhotonView().RequestOwnership();
            //オンライン用のコントローラを出現させる
            GameObject obj = PhotonNetwork.Instantiate(OnlinePlayerObj.name, test, Quaternion.identity);
            obj.GetComponent<OnlineController>().SetPlayerObj(PlayerOne);
            PhotonNetwork.LocalPlayer.NickName = "PlayerOne";
            //カメラ切り替え
            PTwoCamera.enabled = false;
            POneCamera.enabled = true;
            //ステータスから色々リセット
            PlayerStatus pStatus = PlayerOne.GetComponent<PlayerStatus>();
            PlayerOne.GetPhotonView().RPC(nameof(pStatus.CreateHPgauge), RpcTarget.AllBuffered);
            pStatus.HPReset();
            pStatus.CreateCard();
        }
        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            PlayerTwo.GetPhotonView().RequestOwnership();

            GameObject obj2 = PhotonNetwork.Instantiate(OnlinePlayerObj.name, test, Quaternion.identity);
            obj2.GetComponent<OnlineController>().SetPlayerObj(PlayerTwo);
            PhotonNetwork.LocalPlayer.NickName = "PlayerTwo";

            POneCamera.enabled = false;
            PTwoCamera.enabled = true;

            PlayerStatus pStatus = PlayerTwo.GetComponent<PlayerStatus>();
            PlayerTwo.GetPhotonView().RPC(nameof(pStatus.CreateHPgauge), RpcTarget.AllBuffered);
            pStatus.HPReset();
            pStatus.CreateCard();
        }
        else
        {
            GameObject obj = PhotonNetwork.Instantiate(OnlineGalleryObj.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.NickName = "Gallery";
            obj.AddComponent<Camera>();
        }
    }
}
