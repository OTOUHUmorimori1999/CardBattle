using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    // カードマネージャ
    [SerializeField] public CardManager cardManager;
    // サウンドマネージャ
    [SerializeField] public SoundManager soundManager;
    // システムマネージャ
    [SerializeField] public SystemManager systemManager;
    // ゲームマネージャ
    [SerializeField] public GameManager gameManager;
    // キネクトマネージャー
    [SerializeField] public KinectManager kinectManager;
    // オンラインマネージャー
    [SerializeField] public OnlineManager onlineManager;

    private void Start()
    {
       // DontDestroyOnLoad(this);
    }
}
