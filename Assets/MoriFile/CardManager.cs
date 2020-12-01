using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public class CardData
    {
        public CardData() { }
        public CardData(
            int ID,
            float startUpTime,
            float coolTime,
            float life,
            string animationTrigger,
            CardData.Type type,
            int damage,
            Vector3 pos,
            Vector3 size,
            Sprite image = null,
            string accessory = null
            )
        {
            this.cardID = ID;
            this.startUpTime = startUpTime;
            this.coolTime = coolTime;
            this.Life = life;
            this.AnimetionTrigger = animationTrigger;
            this.CardType = type;
            this.Damage = damage;
            this.GenerationPoint = pos;
            this.Size = size;
            this.accessory = accessory;
            this.cardImage = image;
        }
        public CardData(
            int ID,
            float startUpTime,
            float coolTime,
            float life,
            string animationTrigger,
            CardData.Type type,
            int damage,
            Vector3 pos,
            Vector3 size,
            Vector3 velocity,
            Sprite image = null,
            string accessory = null
            )
        {
            this.cardID = ID;
            this.startUpTime = startUpTime;
            this.coolTime = coolTime;
            this.Life = life;
            this.AnimetionTrigger = animationTrigger;
            this.CardType = type;
            this.Damage = damage;
            this.GenerationPoint = pos;
            this.Size = size;
            this.Velocity = velocity;
            this.accessory = accessory;
            this.cardImage = image;
        }
        // 攻撃タイプ
        public enum Type
        {
            // 飛び道具
            Fire,
            // 通常
            Normal,
            // 必殺
            Special

        }
        public readonly int cardID = 0; //指定されたIDの技
        public readonly float startUpTime; // 発生時間
        public readonly float Life = 1.0f; // 持続時間
        public readonly float coolTime; // クールタイム
        public readonly string AnimetionTrigger; // アニメーション切り替え用のトリガー
        public readonly CardData.Type CardType; // 攻撃の種類
        public readonly int Damage = 0; // 攻撃力
        public readonly Vector3 GenerationPoint; // 出現座標
        public readonly Vector3 Size; // 大きさ
        public readonly Vector3 Velocity = Vector3.zero; // 移動量
        public readonly Sprite cardImage = null; // 画像
        public readonly string accessory = ""; // 小物
    }

    public const int CardNum = 6;

    CardData[] cardList = new CardData[CardNum];
    // カードのイラスト
    [SerializeField] Sprite[] cardImages = new Sprite[CardNum];
    // 攻撃時に出る小物・エフェクトのPrefab名
    [SerializeField] string[] accessorys;

    // Start is called before the first frame update
    void Start()
    {
        DataList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO:未来の俺へ。データが膨大になったらここを外部データにしてね。
    void DataList()
    {
        //最終必殺奥義・手打ち
        cardList[0] = new CardData(0,  1.0f,  1.0f, 0.5f, "Attack1", CardData.Type.Normal,  5, new Vector3(0, 3, 6), new Vector3(2, 2, 12), cardImages[0]);
        cardList[1] = new CardData(1, 1.0f, 3.5f, 5.0f, "Attack3", CardData.Type.Normal, 15, new Vector3(0, 3, 0.5f), new Vector3(1, 1, 1), new Vector3(0, 0, 25), cardImages[1], accessorys[0]);
        cardList[2] = new CardData(2,  1.0f,  5.0f, 0.5f, "Attack2", CardData.Type.Special, 40, new Vector3(0, 1, 4), new Vector3(8, 10, 8), cardImages[2]);
        cardList[3] = new CardData(3,  0.8f, 10.0f, 0.5f, "Attack1", CardData.Type.Normal, 10, new Vector3(0, 3, 1), new Vector3(1, 2, 5), cardImages[3]);
        cardList[4] = new CardData(4,  1.0f, 15.0f, 0.5f, "Attack1", CardData.Type.Normal, 15, new Vector3(0, 3, 1), new Vector3(1, 2, 5), cardImages[4]);
        cardList[5] = new CardData(5,  1.0f, 20.0f, 0.5f, "Attack1", CardData.Type.Normal, 20, new Vector3(0, 3, 1), new Vector3(1, 2, 5), cardImages[5]);
    }

    public CardData GetCardData(int cardID)
    {
        DataList();      

        for (int i = 0; i < cardList.Length; i++)
        {
            if (cardList[i].cardID == cardID)
            {
                return cardList[i];
            }
        }

        //終了しなかった場合、nullを返す
        return null;
    }
}
