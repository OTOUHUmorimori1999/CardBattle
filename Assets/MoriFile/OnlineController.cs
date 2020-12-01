using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject PlayerObj;
    [SerializeField] PlayerController pController;
    [SerializeField] PlayerStatus pStatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        OnlineTest();
    }

    public void OnlineTest()
    {
        if (pController != null)
        {
            pController.KeyMovePlayer();
            pController.PadMovePlayer();
        }
    }

    // [PunRPC]属性をつけると、RPCでの実行が有効になる
    [PunRPC]
    public void SetPlayerObj(GameObject player)
    {
        PlayerObj = player;
        pController = PlayerObj.GetComponent<PlayerController>();
        pStatus = PlayerObj.GetComponent<PlayerStatus>();
    }

    // データを送受信するメソッド
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 送信側
            //stream.SendNext(nowHP);
        }
        else
        {
            // 受信側
            //nowHP = (float)stream.ReceiveNext();
        }
    }
}
