using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject TrackingenemyPrefab;
    public float spawnRadius = 4f;
    private Transform playerTransform;

    public AudioClip spawnSound; 
    private AudioSource audioSource; 

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPrefab = GameObject.Find("Enemy");
        TrackingenemyPrefab = GameObject.Find("TrackingEnemy");
        audioSource = GetComponent<AudioSource>();
        spawnSound = Resources.Load<AudioClip>("Sound/InGame/AI rigen/rigen_AIEnemy");
        InvokeRepeating("SpawnEnemy", 0f, 2f);
    }

    void SpawnEnemy()
    {
        
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0f; 
            Vector3 spawnPosition = playerTransform.position + randomPosition;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0f;
            spawnPosition = playerTransform.position + randomPosition;

            Instantiate(TrackingenemyPrefab, spawnPosition, Quaternion.identity);

            audioSource.PlayOneShot(spawnSound);


    }

    
}
