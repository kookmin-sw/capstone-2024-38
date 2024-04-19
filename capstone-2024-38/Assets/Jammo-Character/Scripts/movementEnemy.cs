using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementEnemy : MonoBehaviour
{
    public float Velocity;
    [Space]

    [SerializeField] ParticleSystem inkParticle;
    private Animator anim;
    private Camera cam;
    private CharacterController controller;
    public Transform player; // 플레이어를 추적하기 위한 변수

    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;

    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public float stoppingDistance = 2f;


    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    public float verticalVel;
    private Vector3 moveVector;

    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
        }

        InputMagnitude();
        HandleVerticalMovement();
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (!blockRotationPlayer)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), desiredRotationSpeed);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stoppingDistance)
        {
            controller.Move(direction * Time.deltaTime * Velocity);
            inkParticle.Stop();
        }
        else
        {
            inkParticle.Play();
        }
    }

    void HandleVerticalMovement()
    {
        verticalVel -= 1;
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);
    }

    void InputMagnitude()
    {
    }
}
