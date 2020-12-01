using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class WaitRival : MonoBehaviour
{
    OnlineConnecter connecter;

    public void SetConnecter(OnlineConnecter connect)
    {
        connecter = connect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
