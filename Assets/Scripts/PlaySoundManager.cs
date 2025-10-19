using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundManager : MonoBehaviour
{
    public AudioClip toiletFlush;
    public AudioClip KnockSound;
    public AudioClip KnockSound2;
    public AudioClip KnockSound3;
    public AudioClip RadioSoundMilitary;
    public AudioClip RadioSoundWeirdChimes;
    public AudioClip RadioSoundStaticNoise;
    private AudioSource source;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayToiletFlush()
    {
        source.PlayOneShot(toiletFlush);
    }

    public void PlayKnockSound()
    {
        int rand = Random.Range(1, 4);
        if (rand == 1)
        {
            source.PlayOneShot(KnockSound);
        }
        else if (rand == 2)
        {
            source.PlayOneShot(KnockSound2);
        }
        else
        {
            source.PlayOneShot(KnockSound3);
        }
    }

    public void PlayRadioMilitary()
    {
        source.PlayOneShot(RadioSoundMilitary);
    }

    public void PlayRadioWeirdChimes()
    {
        source.PlayOneShot(RadioSoundWeirdChimes);
    }

    public void PlayRadioStaticNoise()
    {
        source.PlayOneShot(RadioSoundStaticNoise);
    }
}
