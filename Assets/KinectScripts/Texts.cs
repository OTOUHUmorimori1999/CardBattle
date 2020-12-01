﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Texts : MonoBehaviour
{

    //変数設定
    float m_X;
    float m_Y;
    float m_Z;
    //知りたい座標のGaeObjectの設定
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //それぞれに座標を挿入
        m_X = target.transform.position.x;
        m_Y = target.transform.position.y;
        m_Z = target.transform.position.z;

        //テキストに表示
        this.GetComponent<Text>().text = "X座標は" + m_X.ToString() + "\nY座標は" + m_Y.ToString() + "\nZ座標は" + m_Z.ToString();
    }
}
