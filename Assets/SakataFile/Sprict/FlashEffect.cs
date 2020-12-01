using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    // 寿命(秒)
    public float life;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 寿命が尽きたら
        if(life <= 0)
        {
            Destroy(this.gameObject);
        }

        // 回転
        transform.Rotate(Vector3.up, 15);

        //// すこしづつ巨大化
        //gameObject.transform.localScale *= 1.1f;

        // タイマーを進める
        life -= Time.deltaTime;
    }
}
