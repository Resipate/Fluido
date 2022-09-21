using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource aS;
    public AudioClip[] musicClips;
    private bool introMusicPlaying;
    private float introDecay;
    private const float introDecayConst = 11;
    public bool panicMusic { private get; set; }

    void Start()
    {
        panicMusic = false;
        aS = this.GetComponent<AudioSource>();
        aS.clip = musicClips[1];
        aS.volume = 0;
        aS.loop = true;
        aS.Play();
        GameObject temporaryAudio = new GameObject();
        temporaryAudio.name = "introMusicSource";
        temporaryAudio.transform.parent = this.transform;
        temporaryAudio.AddComponent<AudioSource>();
        temporaryAudio.GetComponent<AudioSource>().clip = musicClips[0];
        temporaryAudio.GetComponent<AudioSource>().volume = 0.3f;
        temporaryAudio.GetComponent<AudioSource>().Play();
        introMusicPlaying = true;
        introDecay = introDecayConst;
    }

    public bool temp;
    void Update()
    {
        aS.enabled = true;
        if (panicMusic && aS.clip == musicClips[1]) { aS.clip = musicClips[2]; aS.enabled = false; }
        else if(!panicMusic && aS.clip == musicClips[2]) { aS.clip = musicClips[1]; aS.enabled = false; }
        temp = aS.isPlaying;
        /*
         * Initial Music Fade (Only played for first "introDecayConst" seconds)
         * Start() generates a new object housing intro music
         * each Update(), volumes gradually balance out to flood the intro music with current state music
         */
        if (introMusicPlaying)
        {
            introDecay -= Time.deltaTime;
            if(introDecay <= 0)
            {
                //After decay is complete, subset object is destroyed && volume is finalised
                Destroy(this.transform.Find("introMusicSource").gameObject);
                this.GetComponent<AudioSource>().volume = 0.05f;
                introMusicPlaying = false;
            }
            else
            {
                //Volume siphoning
                transform.Find("introMusicSource").GetComponentInChildren<AudioSource>().volume = 0.3f * (introDecay / introDecayConst);
                this.GetComponent<AudioSource>().volume = 0.05f * ((introDecayConst - introDecay) / introDecayConst);
            }
        }

    }
}
