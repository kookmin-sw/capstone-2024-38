using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movement_speed = 2.0f;
    public Transform playerT;
    Animator enemy_animation;
    private Vector3 moveDirection;

    void Start()
    {
        enemy_animation = GetComponent<Animator>();
        enemy_animation.SetBool("IsMove", true);
        playerT = GameObject.Find("Player").transform;

        if (playerT != null)
        {
            moveDirection = (playerT.position - transform.position).normalized;
            moveDirection.y = 0f;
            transform.LookAt(playerT.position);

            if (enemy_animation == null)
            {
                Debug.LogWarning("Animator component is missing on the enemy game object.");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    void Update()
    {
        if (enemy_animation == null || playerT == null)
        {
            return;
        }

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        transform.position += moveDirection * movement_speed * Time.deltaTime;
        
    }

    
}
