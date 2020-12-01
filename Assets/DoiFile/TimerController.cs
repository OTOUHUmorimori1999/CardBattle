using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(SpriteRenderer))]

public class TimerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] ServiceLocator serviceLocator;
    [Header("StartTimeをマイナスの数字にした場合、開始時間は無限になる")]
    [SerializeField] int StartTime = 150;
    [SerializeField] Text timerText;

    float totalTime;
    int seconds;

    public bool count = false;
    bool Starting = false;

    // Start is called before the first frame update
    void Start()
    {
        Starting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Starting)
        {
            if (PhotonNetwork.PlayerList.Length >= 2)
            {
                Starting = true;
                SetStartTimer(StartTime);
            }
            else
                return;
        }

        if (count && StartTime > 0)
        {
            if (serviceLocator != null)
            {
                if (!serviceLocator.systemManager.GetEndFlag())
                        this.TimeCount();
            }
        }
    }

    void TimeCount()
    {
        totalTime -= Time.deltaTime;
        seconds = (int)totalTime;
        timerText.text = seconds.ToString();

        if (totalTime <= 0.0f)
        {
            totalTime = 0.0f;
        }
    }

    public void SetStartTimer(int time)
    {
        StartTime = time;

        if (StartTime > 0)
        {
            totalTime = (float)StartTime;
        }
        else
        {
            count = false;
            timerText.text = "∞";
        }
    }

    public int GetStartTime()
    {
        return StartTime;
    }

    public float GetNowTime()
    {
        if (StartTime < 0)
            return 99;

        return totalTime;
    }

    // データを送受信するメソッド
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 送信側
            stream.SendNext(totalTime);
            stream.SendNext(seconds);
        }
        else
        {
            // 受信側
            totalTime = (float)stream.ReceiveNext();
            seconds = (int)stream.ReceiveNext();
        }
    }
}
