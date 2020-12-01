using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class TestEffect : MonoBehaviour
{
    public EffekseerEffectAsset tes;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<EffekseerEmitter>();
        obj.AddComponent<DeadEffect>();
        obj.GetComponent<EffekseerEmitter>().effectAsset = tes;
        obj.GetComponent<EffekseerEmitter>().Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
