using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 5f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPrefab = GameObject.Find("Enemy");

        InvokeRepeating("SpawnEnemy", 0f, 10f);
    }

    void SpawnEnemy()
    {
        
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0f; 
            Vector3 spawnPosition = playerTransform.position + randomPosition;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

    }

    
}
