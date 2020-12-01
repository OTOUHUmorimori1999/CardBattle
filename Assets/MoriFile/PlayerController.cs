using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//プレイヤー操作系統
public class PlayerController : MonoBehaviour
{
    [Header("登録必須項目")]
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Model;
    [SerializeField] PhotonView photonView;
    [SerializeField] AvatarController avatarController;
    [SerializeField] ServiceLocator serviceLocator;

    [Header("移動速度")]
    [SerializeField] float m_speed = 0.06f;
    [SerializeField] float m_avoidSpeed = 0.08f;
    [SerializeField] float DEFAvoidCoolTime = 1.0f;

    [Header("キーコード")] //デフォルトはテンキー配置
    [SerializeField]
    KeyCode[] m_moveKey = new KeyCode[8]
    { KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9,
      KeyCode.Keypad4,                  KeyCode.Keypad6,
      KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3};//移動用

    [SerializeField] string HorizontalAxis = "Horizontal";
    [SerializeField] string VerticalAxis = "Vertical";

    Animator ModelAnimator;
    Collider myCollider;
    float avoidCoolTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ModelAnimator = Model.GetComponent<Animator>();
        avoidCoolTime = DEFAvoidCoolTime;
        myCollider = this.gameObject.GetComponent<BoxCollider>();

        if (photonView == null)
            photonView = PhotonView.Get(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (avoidCoolTime <= DEFAvoidCoolTime)
        {
            avoidCoolTime += Time.deltaTime;
        }

        //敵の方を向く
        if (Enemy != null && ModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
        {
            Quaternion rota = Quaternion.LookRotation(Enemy.transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(0, rota.y, 0, rota.w), 0.1f);
        }

        //強制モデルズレ封じ
        Model.transform.position = this.transform.position;
        Model.transform.rotation = this.transform.rotation;
    }

    //-------------------------------------------------------
  
    //簡単な移動処理
    public void KeyMovePlayer()
    {
        if (!photonView.IsMine || serviceLocator.systemManager.GetEndFlag())
            return;

        //上
        if (Input.GetKey(m_moveKey[1]))
        {
            this.transform.Translate(0, 0, m_speed);
        }
        //左
        if (Input.GetKey(m_moveKey[3]))
        {
            this.transform.Translate(-m_speed, 0, 0);
        }
        //右
        if (Input.GetKey(m_moveKey[4]))
        {
            this.transform.Translate(m_speed, 0, 0);
        }
        //下
        if (Input.GetKey(m_moveKey[6]))
        {
            this.transform.Translate(0, 0, -m_speed);
        }

        //回避行動時、自動移動
        if (ModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("L_Avoid"))
        {
            this.transform.Translate(-m_speed * 2, 0, 0);
            
        }
        if (ModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("R_Avoid"))
        {
            this.transform.Translate(m_speed * 2, 0, 0);

        }
    }

    //パッドでの移動処理
    public void PadMovePlayer()
    {
        if (!photonView.IsMine || serviceLocator.systemManager.GetEndFlag())
            return;

        if (Input.GetKeyDown(KeyCode.Joystick1Button6))
            avatarController.Callblate();

        //TODO:うるせえ動けばいいんだよお！！！
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && Input.GetKey(KeyCode.Joystick1Button7))
            AvoidMove(true);
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetKey(KeyCode.Joystick1Button7))
            AvoidMove(false);

        float horizon = Input.GetAxis(HorizontalAxis);
        float vert = Input.GetAxis(VerticalAxis);
        this.transform.Translate(horizon * m_speed, 0, vert * m_speed * 1.5f);
    }

    //回避
    public void AvoidMove(bool left)
    {
        if (!photonView.IsMine || serviceLocator.systemManager.GetEndFlag())
            return;

        if (avoidCoolTime >= DEFAvoidCoolTime)
        {
            avoidCoolTime = 0.0f;

            if (left)
            {
                AnimationTrigger("KinectAvoid_L");
                Debug.Log("ひだり" + Time.deltaTime);
            }
            else
            {
                AnimationTrigger("KinectAvoid_R");
                Debug.Log("みぎ" + Time.deltaTime);
            }
        }
        Debug.Log("is coolTIme:" + avoidCoolTime);
    }
    
    //キーコンフィグ用
    public void ChangeKeyCode(KeyCode[] code)
    {
        if (code.Length == m_moveKey.Length)
            m_moveKey = code;
    }

    // アニメーション変更
    public void AnimationTrigger(string name)
    {
        ModelAnimator.SetTrigger(name);
    }

    // 現在のステート取得
    public AnimatorStateInfo CurrentAnimation()
    {
        return ModelAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
