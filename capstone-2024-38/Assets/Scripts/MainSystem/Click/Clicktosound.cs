using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicktosound : MonoBehaviour
{
    public AudioClip[] audioClips; // �Ҹ� ���ϵ��� ������ �迭
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

        int randomIndex = Random.Range(0, audioClips.Length); // 0���� audioClips �迭 ���̱��� ���� �ε��� ����
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
