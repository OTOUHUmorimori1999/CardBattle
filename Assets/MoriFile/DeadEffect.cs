using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class DeadEffect : MonoBehaviour
{
    EffekseerEmitter efEmit;

    // Start is called before the first frame update
    void Start()
    {
        efEmit = this.GetComponent<EffekseerEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(efEmit.instanceCount == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
