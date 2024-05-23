using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameBGMPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip bgmClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        bgmClip = Resources.Load<AudioClip>("Sound/InGame/BGM/the rave partyF");
        winClip = Resources.Load<AudioClip>("Sound/InGame/Win/Woodland Game Winner 1");
        loseClip = Resources.Load<AudioClip>("Sound/InGame/Lose/death");
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayWinSound()
    {
        if (winClip != null)
        {
            audioSource.Stop();
            audioSource.clip = winClip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayLoseSound()
    {
        if (loseClip != null)
        {
            audioSource.Stop();
            audioSource.clip = loseClip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
