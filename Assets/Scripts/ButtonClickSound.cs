using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    private AudioSource Audio;
    public AudioClip clip;
    //AudioSource 컴포넌트 가져옴
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    //소리 재생
    public void PlayAudio()
    {
        Audio.clip = clip;
        Audio.Play();
    }
}
