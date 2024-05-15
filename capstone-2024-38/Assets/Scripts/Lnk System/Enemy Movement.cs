using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
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
        transform.position += direction * movement_speed * Time.deltaTime;
        transform.LookAt(playerT.position);

        UpdateAnimation(direction);

        if (Input.GetKeyDown(KeyCode.P))
        {
            enemy_animation.SetTrigger("dead");
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
            {
                enemy_animation.SetBool("IsRight", true);
                enemy_animation.SetBool("IsFront", false);
                enemy_animation.SetBool("IsLeft", false);
                enemy_animation.SetBool("IsBack", false);
            }
            else
            {
                enemy_animation.SetBool("IsLeft", true);
                enemy_animation.SetBool("IsFront", false);
                enemy_animation.SetBool("IsRight", false);
                enemy_animation.SetBool("IsBack", false);
            }
        }
        else
        {
            if (direction.z > 0)
            {
                enemy_animation.SetBool("IsFront", true);
                enemy_animation.SetBool("IsLeft", false);
                enemy_animation.SetBool("IsRight", false);
                enemy_animation.SetBool("IsBack", false);
            }
            else
            {

                enemy_animation.SetBool("IsBack", true);
                enemy_animation.SetBool("IsFront", false);
                enemy_animation.SetBool("IsLeft", false);
                enemy_animation.SetBool("IsRight", false);
            }
        }
    }
}
