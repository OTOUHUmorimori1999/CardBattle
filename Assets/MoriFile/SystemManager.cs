using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SystemManager : MonoBehaviour
{
    //プレイヤー数
    const int playerNum = 2;

    //勝利に必要なバトル数
    public const int BattleNum = 1;
    [SerializeField] ServiceLocator serviceLocator;
    [SerializeField] TimerController timerController;
    [SerializeField] PlayerStatus playerOneStatus;
    [SerializeField] PlayerStatus playerTwoStatus;
    [SerializeField] Canvas playerOneCanvas;
    [SerializeField] Canvas playerTwoCanvas;
    [Space(5)]
    [SerializeField] Sprite timeupTex;
    [SerializeField] Sprite winTex;
    [SerializeField] Sprite loseTex;
    [SerializeField] Sprite drawTex;
    [Space(5)]
    [SerializeField] string EndingSceneName;
    [Header("SystemVoices")]
    [SerializeField] AudioClip SE_GameStart;
    [SerializeField] AudioClip SE_KO;
    [Space(10)]

    //プレイヤーの勝利カウント。一次元目はプレイヤー数
    bool[][] PlayerWinChecker = new bool[playerNum][];

    bool startFlag;
    
    enum EndFlagName
    {
        None,

        P1WIN,
        P2WIN,
        DRAW,

        END
    }
    [SerializeField] EndFlagName endFlagName = EndFlagName.None;

    //エンドフラグが立っていない状態でなければtrueを返す
    public bool GetEndFlag() { return endFlagName != EndFlagName.None ? true : false; }

    public void ResetSystem()
    {
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        startFlag = true;
        endFlagName = EndFlagName.None;

        //チェッカー初期化
        for (int i = 0; i < playerNum; i++)
        {
            PlayerWinChecker[i] = new bool[BattleNum];

            for(int j = 0; j < BattleNum; j++)
            {
                PlayerWinChecker[i][j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startFlag)
        {
            //２人以上入ってればおｋ！！！ｗｗｗｗｗ
            if (PhotonNetwork.PlayerList.Length >= 2 && endFlagName == EndFlagName.None)
            {
                GameStart();
            }
            return;
        }

        if(endFlagName == EndFlagName.None && timerController.GetNowTime() <= 0)
        {
            //タイムアップ
            if(playerOneStatus.GetNowHP() < playerTwoStatus.GetNowHP())
            {
                //プレイヤー１敗北
                endFlagName = EndFlagName.P2WIN;
            }
            else if(playerOneStatus.GetNowHP() > playerTwoStatus.GetNowHP())
            {
                //プレイヤー２敗北
                endFlagName = EndFlagName.P1WIN;
            }
            else
            {
                //ドローゲーム
                endFlagName = EndFlagName.DRAW;
            }
        }

        if(endFlagName == EndFlagName.None && playerOneStatus.GetNowHP() <= 0)
        {
            //プレイヤー１敗北
            endFlagName = EndFlagName.P2WIN;
        }
        if(endFlagName == EndFlagName.None && playerTwoStatus.GetNowHP() <= 0)
        {
            //プレイヤー２敗北
            endFlagName = EndFlagName.P1WIN;
        }

        Ending();
    }

    void GameStart()
    {
        serviceLocator.soundManager.PlayAudioSE(SE_GameStart);
        startFlag = false;
    }

    //そのまま渡すパターンと分割して渡すパターン
    public bool[][] GetPlayerWinChecker()
    {
        return PlayerWinChecker;
    }
    public bool[] GetPlayerWinChecker(int playerNum)
    {
        return PlayerWinChecker[playerNum];
    }
    public bool GetPlayerWinChecker(int playerNum, int winNum)
    {
        return PlayerWinChecker[playerNum][winNum];
    }

    //勝利チェッカーをセット
    public bool SetPlayerWinChecker(int playerNum)
    {
        for(int i = 0; i < BattleNum; i++)
        {
            if(PlayerWinChecker[playerNum][i] == false)
            {
                PlayerWinChecker[playerNum][i] = true;
                return true;
            }
        }

        //ここに突入したらゲームもう終わってんだよな
        return false;
    }

    //エンディング分岐
    void Ending()
    {
        switch(endFlagName)
        {
            case EndFlagName.P1WIN:
                if (!SetPlayerWinChecker(0))
                {
                    //画像出力
                    if(serviceLocator.onlineManager.GetOwnerPlayer(1))
                        SetSystemSprite(playerOneCanvas, winTex);
                    if(serviceLocator.onlineManager.GetOwnerPlayer(2))
                        SetSystemSprite(playerTwoCanvas, loseTex);

                    //退室
                    serviceLocator.onlineManager.DisConnectPhotonRoom();

                    endFlagName = EndFlagName.END;
                }
                return;

            case EndFlagName.P2WIN:
                if (!SetPlayerWinChecker(1))
                {
                    if (serviceLocator.onlineManager.GetOwnerPlayer(1))
                        SetSystemSprite(playerOneCanvas, loseTex);
                    if (serviceLocator.onlineManager.GetOwnerPlayer(2))
                        SetSystemSprite(playerTwoCanvas, winTex);

                    //退室
                    serviceLocator.onlineManager.DisConnectPhotonRoom();

                    endFlagName = EndFlagName.END;
                }
                return;

            case EndFlagName.DRAW:
                {
                    if (serviceLocator.onlineManager.GetOwnerPlayer(1))
                        SetSystemSprite(playerOneCanvas, drawTex);
                    if (serviceLocator.onlineManager.GetOwnerPlayer(2))
                        SetSystemSprite(playerTwoCanvas, drawTex);

                    //退室
                    serviceLocator.onlineManager.DisConnectPhotonRoom();

                    endFlagName = EndFlagName.END;
                }
                return;

            case EndFlagName.END:
                GameEnd();
                return;

            default:
                return;
        }
    }

    void GameEnd()
    {
        //マウス系以外の何でもいいからキーを押す
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            serviceLocator.gameManager.SceneChange(EndingSceneName);
        }
    }

    void SetSystemSprite(Canvas canvas, Sprite sprite, float posY = 0)
    {
        GameObject tex = new GameObject();
        tex.AddComponent<Image>();
        tex.transform.parent = canvas.transform;
        
        Image image = tex.GetComponent<Image>();
        image.sprite = sprite;

        RectTransform rect = tex.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localPosition = new Vector3(0, posY, 0);
        rect.sizeDelta = new Vector2(803, 221);
        rect.localScale = Vector3.one;
    }
}
