using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameStartGORIOSHI : MonoBehaviour
{
    [SerializeField] ServiceLocator serviceLocator;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.PlayerList.Length <= 1)
            serviceLocator.systemManager.ResetSystem();

        Destroy(this); //自壊
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
