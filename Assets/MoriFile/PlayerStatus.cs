using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// プレイヤーの情報
[System.Serializable]
public struct StatusData
{
    public int HP;
    public int[] Deck;
}

//プレイヤー情報系
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] ServiceLocator serviceLocator;
    [SerializeField] PhotonView photonView;

    [Header("登録必須項目")]
    [SerializeField] TextAsset Data;
    [SerializeField] Canvas HPvarCanvas;
    [SerializeField] GameObject HPvar;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] float leaveVar = 2.0f;
    [SerializeField] GameObject Rival;
    PlayerStatus rivalStatus;

    Transform RivalEnemySliderPos;

    Slider HPslider;
    Transform sliderPos;

    StatusData parameter;
    [SerializeField] int HP;
    [SerializeField] GameObject HitEffect;

    [Header("操作キャラ用カード組")]
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject HitBox;
    [Header("必殺技用のエフェクト")]
    [SerializeField] GameObject SpecialEffect;
    [SerializeField] Canvas PlayerCanvas;

    [Header("キーコード")] //デフォルトはASDF
    [SerializeField]
    KeyCode[] m_cardKey = new KeyCode[4]
    { KeyCode.Joystick1Button2, KeyCode.Joystick1Button3, KeyCode.Joystick1Button5, KeyCode.Joystick1Button4 };

    private float m_startUpCounter; // 発生カウンタ
    private CardManager.CardData m_currentAttacker; // 発生中の攻撃データ

    /*Sliderセッター*/ public void SetEnemySlider(Transform sliderPos) { RivalEnemySliderPos = sliderPos; }
    /*Sliderゲッター*/ public Transform GetSliderPos() { return sliderPos; }
    /*HPスライダーゲッター*/ public float GetNowHP() { return HPslider != null ? HPslider.value : 100; }

    // Start is called before the first frame update
    void Start()
    {
        //ジョイスティック入力がない場合、強制キー変更
        string[] joyName = Input.GetJoystickNames();
        if (joyName.Length == 0)
        {
            m_cardKey = new KeyCode[4] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V };
        }
        else
        {
            if (Input.GetJoystickNames()[0] == "")
                m_cardKey = new KeyCode[4] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V };

        }

        // パラメータをセット
        parameter = JsonUtility.FromJson<StatusData>(Data.text);

        // 攻撃データ保持変数を初期化
        m_currentAttacker = null;

        if(photonView == null)
        photonView = PhotonView.Get(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine || serviceLocator.systemManager.GetEndFlag())
            return;

        //相手側HPゲージの位置調整
        if (rivalStatus != null)
        {
            RivalEnemySliderPos = rivalStatus.GetSliderPos();
            if (RivalEnemySliderPos != null)
            {
                Vector3 rivalPos = Rival.transform.position;
                Vector3 offset = new Vector3(0, leaveVar, 0);
                rivalPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rivalPos + offset);
                RivalEnemySliderPos.position = new Vector3(rivalPos.x, rivalPos.y, rivalPos.z);
            }
        }
        else
            rivalStatus = Rival.GetComponent<PlayerStatus>();

        // 攻撃トリガーがオンになったら
        if (Attacking())
        {
            // カウンタが硬直時間を上回っている
            if(m_startUpCounter >= m_currentAttacker.startUpTime)
            {
                //GameObject effect = gameManager.GetEffect(m_currentAttacker.cardID);
                Vector3 effectPos = this.transform.position;

                if (HitBox != null)
                {
                    //ダメージ用ヒットボックスを召喚
                    AttackerMaker.MakeAttacker(
                        Rival,
                        this.gameObject,
                        m_currentAttacker,
                        photonView
                        );
                }
                // 攻撃データを開放する
                m_currentAttacker = null;
                // カウンタをリセット
                m_startUpCounter = 0;
            }
            else
                // タイマーを加算
                m_startUpCounter += Time.deltaTime;
        }
    }

    //HP減少
    // [PunRPC]属性をつけると、RPCでの実行が有効になる
    [PunRPC]
    public void DownHP(int damage)
    {
        HP -= damage;

        if(HPslider != null)
        HPslider.value = HP;
    }

    //HPゲージ生成
    [PunRPC]
    public void CreateHPgauge()
    {
        //HPバー出現
        {
            Vector3 thisPos = this.transform.position;
            Vector3 offset = new Vector3(0, leaveVar, 0);

            GameObject obj = GameObject.Instantiate(HPvar);
            obj.transform.parent = HPvarCanvas.transform;
            thisPos = RectTransformUtility.WorldToScreenPoint(Camera.main, thisPos + offset);
            obj.transform.position = new Vector3(thisPos.x, thisPos.y, thisPos.z);

            HPslider = obj.GetComponent<Slider>();
            sliderPos = obj.transform;
        }

        HPReset();
    }

    public void HPReset()
    {
        if (HPslider == null)
            return;

        HP = parameter.HP;

        HPslider.maxValue = parameter.HP;
        HPslider.value = HP;
    }

    // 攻撃中か？
    public bool Attacking()
    {
        return m_currentAttacker != null;
    }

    //カード生成
    public void CreateCard()
    {
        if (CardPrefab == null)
            return;

            Quaternion rotation = PlayerCamera.transform.rotation;
        float width = CardPrefab.GetComponent<RectTransform>().sizeDelta.x;
        Vector3 pos = PlayerCanvas.transform.position;
        pos.x -= width * 2.0f + 200;
        pos.y -= 252.0f;
        pos.z += 1.5f;

        int[] Deck = parameter.Deck; 

        for (int i = 0; i < 4; i++)
        {
            GameObject prefab = GameObject.Instantiate(CardPrefab);
            prefab.transform.parent = PlayerCanvas.transform;
            prefab.transform.position = pos;
            prefab.transform.Translate((width * 2.0F + 10f) * i, 0, 0);

            CardScript card = prefab.GetComponent<CardScript>();
            card.LoadData(serviceLocator.cardManager.GetCardData(Deck[i]), m_cardKey[i], this);
        }
    }

    // 攻撃
    public bool Attack(CardManager.CardData cardData)
    {
        if (!photonView.IsMine || serviceLocator.systemManager.GetEndFlag())
            return false;

        if (m_currentAttacker == null)
        {
            //音をだす
            serviceLocator.soundManager.PlayAudioSE(cardData.cardID);

            // アニメーションを切り替え
            GetComponent<PlayerController>().AnimationTrigger(cardData.AnimetionTrigger);
            if (
                !GetComponent<PlayerController>().CurrentAnimation().IsName("Normal") &&
                !GetComponent<PlayerController>().CurrentAnimation().IsName(cardData.AnimetionTrigger)
                )
                GetComponent<PlayerController>().AnimationTrigger("AnyAttack");
            m_currentAttacker = cardData;
            // 必殺技だったら
            if (m_currentAttacker.CardType == CardManager.CardData.Type.Special)
            {
                // エフェクトを出す
                photonView.RPC(nameof(FlashEffect), RpcTarget.All, cardData.startUpTime + cardData.Life);
            }
            return true;
        }
        return false;
    }
    
    [PunRPC]
    public void FlashEffect(float life)
    {
        GameObject go = GameObject.Instantiate(SpecialEffect);
        go.GetComponent<FlashEffect>().life = life;
        go.transform.position = transform.position;
        go.transform.localScale *= 4;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Attack")
        {
            HitScript hit = other.gameObject.GetComponent<HitScript>();

            if (hit != null && hit.target.gameObject == this.gameObject)
            {
                object[] hitPoses = new object[] {
                    other.gameObject.transform.position.x,
                        other.gameObject.transform.position.y,
                            other.gameObject.transform.position.z};
                
                //当たったエフェクトの生成
                photonView.RPC(nameof(CreateHitEffect), RpcTarget.All, hitPoses);
                //処理
                photonView.RPC(nameof(DownHP), RpcTarget.All, hit.Damage);
                PhotonNetwork.Destroy(other.gameObject);
                Rival.transform.Translate(-Vector3.forward);
                //GetComponent<Rigidbody>().velocity = -transform.forward * 1;
                Debug.Log(this + "damage");
            }
        }
    }

    [PunRPC]
    public void CreateHitEffect(float posX, float posY, float posZ)
    {
        GameObject eff = GameObject.Instantiate(HitEffect);
        eff.transform.position = new Vector3(posX, posY, posZ);
        eff.AddComponent<HitScript>();

        //HitScriptの魔改造
        HitScript scr = eff.GetComponent<HitScript>();
        scr.life = 1.0f;
        scr.isNetwork = false;
    }
}
