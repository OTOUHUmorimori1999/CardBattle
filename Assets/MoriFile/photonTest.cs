using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class photonTest : MonoBehaviourPunCallbacks
{
    Vector3 defSize;
    float m_speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        defSize = this.transform.localScale;
        photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup!");
    }
    [PunRPC]
    void ChatMessage(string a, string b)
    {
        Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
    }
    // Update is called once per frame
    void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            //上
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.Translate(0, 0, m_speed);
            }
            //左
            if (Input.GetKey(KeyCode.A))
            {
                this.transform.Translate(-m_speed, 0, 0);
            }
            //右
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(m_speed, 0, 0);
            }
            //下
            if (Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(0, 0, -m_speed);
            }
            //上昇
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Translate(0, m_speed, 0);
            }
            //下降
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Translate(0, -m_speed, 0);
            }
            //右回転
            if (Input.GetKey(KeyCode.E))
            {
                this.transform.Rotate(0, m_speed * 2, 0);
            }
            //左回転
            if (Input.GetKey(KeyCode.Q))
            {
                this.transform.Rotate(0, -m_speed * 2, 0);
            }
            //上回転
            if (Input.GetKey(KeyCode.R))
            {
                this.transform.Rotate(-m_speed * 2, 0, 0);
            }
            //下回転
            if (Input.GetKey(KeyCode.F))
            {
                this.transform.Rotate(m_speed * 2, 0, 0);
            }
        }
    }
}
