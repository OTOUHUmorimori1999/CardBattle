using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource BGMAudio;
    [SerializeField] AudioSource SEAudio;
    [Space(5)]
    [SerializeField] AudioClip[] soundSauce;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponent<AudioListener>() == null)
            this.gameObject.AddComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAudioBGM(int id)
    {
        if (soundSauce.Length <= id)
            return;
        
        BGMAudio.PlayOneShot(soundSauce[id]);
    }

    public void PlayAudioSE(AudioClip audio)
    {
        if (audio == null)
            return;

        SEAudio.PlayOneShot(audio);
    }

    public void PlayAudioSE(int id)
    {
        if (soundSauce.Length <= id)
            return;

        SEAudio.PlayOneShot(soundSauce[id]);
    }
    
    public string GetAudioName(int id)
    {
        if (soundSauce.Length <= id)
            return "";

        return soundSauce[id].name;
    }
}
