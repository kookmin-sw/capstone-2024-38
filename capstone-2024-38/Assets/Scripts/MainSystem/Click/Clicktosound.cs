using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicktosound : MonoBehaviour
{
    public AudioClip[] audioClips; // 소리 파일들을 저장할 배열
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayRandomSound();
        }
    }
    public void PlayRandomSound()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned.");
            return;
        }

        int randomIndex = Random.Range(0, audioClips.Length); // 0부터 audioClips 배열 길이까지 랜덤 인덱스 선택
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
