using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource aS;
    public AudioClip[] musicClips;
    private bool introMusicPlaying;
    private float introDecay;
    private const float introDecayConst = 11;
    public bool panicMusic { private get; set; }

    void Start()
    {
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

    void Update()
    {
        if (introMusicPlaying)
        {
            introDecay -= Time.deltaTime;
            if(introDecay <= 0)
            {
                Destroy(this.transform.Find("introMusicSource").gameObject);
                this.GetComponent<AudioSource>().volume = 0.05f;
                introMusicPlaying = false;
            }
            else
            {
                transform.Find("introMusicSource").GetComponentInChildren<AudioSource>().volume = 0.3f * (introDecay / introDecayConst);
                this.GetComponent<AudioSource>().volume = 0.05f * ((introDecayConst - introDecay) / introDecayConst);
            }
        }
    }
}
