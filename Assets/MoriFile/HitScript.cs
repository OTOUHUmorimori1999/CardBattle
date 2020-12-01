using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitScript : MonoBehaviour
{
    public const int DEF_DAMAGE = 5;
    public int Damage = DEF_DAMAGE;
    public GameObject target;
    public float life;
    public bool isNetwork = true;
    public Vector3 Velocity;

    private void Update()
    {
        // ライフが0以下になったら
        if(life <= 0)
        {
            // オブジェクトを消す
            if (isNetwork)
                PhotonNetwork.Destroy(this.gameObject);
            else
                Destroy(this.gameObject);
        }
        // ライフを減らす
        life -= Time.deltaTime;

        // 加速値が有効なら
        if(Velocity.magnitude > 0)
        {
            //  等速加速
            GetComponent<Rigidbody>().velocity = Velocity;
        }
    }
}
