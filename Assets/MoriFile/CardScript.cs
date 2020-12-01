using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public CardManager.CardData m_data;
    float m_coolTime;

    Image cardImage;

    Sprite cardTex;
    [SerializeField] Sprite fillTex;

    PlayerStatus myPlayer;

    [SerializeField] Image coolTimeGauge;
    [SerializeField] float downCoolTime = 0.05f;
    [SerializeField] Color defColor = Color.white;
    [SerializeField] Color changeColor = Color.black;
    [SerializeField] KeyCode m_key = KeyCode.A;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = this.gameObject.GetComponent<Image>();

        if(m_data == null)
        {
            m_data = new CardManager.CardData();
        }
        cardTex = m_data.cardImage;

        m_coolTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //色を白に戻す
        if (m_coolTime < 0.02f && cardImage.color.r < 1)
        {
            cardImage.color = new Color(cardImage.color.r + downCoolTime, cardImage.color.g + downCoolTime, cardImage.color.b + downCoolTime);
        }

        if (m_coolTime > 0.0f)
        {
            coolTimeGauge.sprite = fillTex;

            coolTimeGauge.fillAmount = m_coolTime / m_data.coolTime;
            m_coolTime -= Time.deltaTime;
            if (Input.GetKeyDown(m_key))
            {
                Debug.Log(m_data.cardID + " :is cool time" + m_coolTime);
            }
        }
        else
        {
            coolTimeGauge.fillAmount = 1;
            coolTimeGauge.sprite = cardTex;

            //攻撃
            if (Input.GetKeyDown(m_key))
            {
                // 攻撃ができたなら
                if(myPlayer.Attack(m_data))
                {
                    Debug.Log(m_data.cardID + ":" + m_data.coolTime);
                    cardImage.color = changeColor;
                    m_coolTime = m_data.coolTime;
                }
                else
                {
                    Debug.Log("Don't make \"Attaker\"");
                }
            }
        }
    }

    //---------------------------------------------------------

    //カードとボタン読み込み
    public void LoadData(CardManager.CardData data, KeyCode code, PlayerStatus player)
    {
        m_data = data;
        m_key = code;
        myPlayer = player;

        if(m_data.cardImage != null)
        {
            cardTex = m_data.cardImage;
        }
    }
}
