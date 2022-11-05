using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Background Music")]
    public AudioSource backgroundMusic;

    [Header("Sound FX")]
    public AudioSource chopSound;
    public AudioSource craftingSound;
    public AudioSource dropItemSound;
    public AudioSource toolSwingSound;
    public AudioSource pickUpItemSound;
    public AudioSource grassWalkSound;

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

    public void PlaySound(AudioSource soundToPlay, float delay)
    {
        if (!soundToPlay.isPlaying)
        {
            StartCoroutine(DelaySound(soundToPlay, delay));
        }
    }

    IEnumerator DelaySound(AudioSource soundToPlay, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundToPlay.Play();
    }
}
