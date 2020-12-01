using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] SoundManager SEMna;
    [SerializeField] GameManager GMMna;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && Input.GetKey(KeyCode.Joystick1Button1))
        {
            SEMna.PlayAudioSE(0);
            GMMna.SceneChange("SScene");
        }
    }
}
