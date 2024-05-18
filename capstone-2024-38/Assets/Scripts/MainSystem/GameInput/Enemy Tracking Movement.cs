using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingMovement : MonoBehaviour
{
    public float movement_speed = 2.0f;
    public float playerDistance = 10.0f;
    public Transform playerT;
    Animator enemy_animation;

    void Start()
    {
        enemy_animation = GetComponent<Animator>();
        playerT = GameObject.Find("Player").transform;

        if (enemy_animation == null)
        {
            Debug.LogWarning("Animator component is missing on the enemy game object.");
        }
    }

    void Update()
    {
        if (enemy_animation == null || playerT == null)
        {
            return;
        }

        Vector3 direction = (playerT.position - transform.position).normalized;
        direction.y = 0f;
        transform.position += direction * movement_speed * Time.deltaTime;
        transform.LookAt(playerT.position);

        if (Input.GetKeyDown(KeyCode.P))
        {
            enemy_animation.SetTrigger("dead");
        }
    }

    
}