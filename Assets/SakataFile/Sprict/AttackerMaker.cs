using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackerMaker
{
    public static void MakeAttacker(GameObject target, GameObject me, CardManager.CardData cardData, PhotonView pv)
    {
        // ヒットボックス作成
        GameObject attacker;
        // 小物があれば
        if (cardData.accessory != null)
        {
            // 基礎オブジェクトを作成
            attacker = PhotonNetwork.Instantiate(cardData.accessory.ToString(), Vector3.zero, Quaternion.identity);
        }
        else
        {
            // 空のオブジェクト作成
            attacker = PhotonNetwork.Instantiate("attackOBJ", Vector3.zero, Quaternion.identity);
        }
        // ボックス判定導入
        attacker.AddComponent<BoxCollider>();
        
        // 一旦ボックスを保持する
        BoxCollider box = attacker.GetComponent<BoxCollider>();
        // ステータスセット
        box.transform.position = me.transform.position + (me.transform.rotation * cardData.GenerationPoint);
        box.size = cardData.Size;
        // 判定をトリガー(衝突なし)に変更
        box.isTrigger = true;

        // 攻撃データ導入
        attacker.AddComponent<HitScript>();
        // 一旦攻撃データを保持する
        HitScript hitData = attacker.GetComponent<HitScript>();
        // ターゲット指定
        hitData.target = target;
        // ダメージ値をセット
        hitData.Damage = cardData.Damage;
        // 生存時間
        hitData.life = cardData.Life;
        // 攻撃オブジェクトとしてタグ付け
        attacker.tag = "Attack";

        // 回転
        Vector3 vecTarget = target.transform.position - me.transform.position;
        Quaternion rotTarget = Quaternion.LookRotation(vecTarget);
        attacker.transform.rotation = rotTarget;

        // 移動したなら
        if (cardData.Velocity.magnitude > 0)
        {
            // 重力を消す
            attacker.GetComponent<Rigidbody>().useGravity = false;
            // 移動量をセット
            hitData.Velocity = rotTarget * cardData.Velocity;
        }
    }
}
