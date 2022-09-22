using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private bool changedClips;
    public int walking;     //Using int to create 3 states (0 for disabled, 1 for first enabled, 2 for all instances until disabled
    private AudioSource audio;
    public AudioClip[] clips;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0.07f;
        audio.loop = true;
        changedClips = false;
    }
    void Update()
    {
        if(audio.enabled == false) { audio.enabled = true; audio.Play(); }    //Was not working properly without re-enabling the component
        if (walking == 1) { audio.clip = clips[0]; audio.enabled = false; walking = 2; }
    }
}
