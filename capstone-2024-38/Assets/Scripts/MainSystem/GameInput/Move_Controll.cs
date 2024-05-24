using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class move_Controll : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 12.0f;
    public CinemachineFreeLook freeLookCamera;

    Rigidbody rigid;
    Vector3 moveVector;
    Animator player_animation;

    public LayerMask groundLayer;
    public float groundCheckDistance = 1.1f;
    private bool isGrounded = false;
    private bool isMoving = false;
    public float pushForce = 10.0f;

    private bool isJumping = false;
    private AudioSource audioSource;
    private AudioClip jumpSound;
    //private AudioClip moveSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        groundLayer = LayerMask.GetMask("Default");
        player_animation = this.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        jumpSound = Resources.Load<AudioClip>("Sound/InGame/Jump/Classic Jump 4");
        //moveSound = Resources.Load<AudioClip>("Sound/InGame/Movement/Footsteps/Builder_Game_Footstep_Run_Dirt_2");
    }

    void Update()
    {
        float x = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Horizontal");

        float Fx = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value;

        if (x != 0 || z != 0)
        {
            float angle = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
            angle += Fx;
            if (angle < 0f)
            {
                angle += 360f;
            }

            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveVector = moveDirection * (moveSpeed * 1.5f);
            }
            else
            {
                moveVector = moveDirection * moveSpeed;
            }
            isMoving = true;
        }
        else
        {
            moveVector = Vector3.zero;
            isMoving = false;
        }

        if (isMoving)
        {
            player_animation.SetBool("IsMove", true);
        }
        else
        {
            player_animation.SetBool("IsMove", false);
            player_animation.SetBool("IsRun", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isMoving)
        {
            player_animation.SetBool("IsMove", false);
            player_animation.SetBool("IsRun", true);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (!isGrounded)
        {
            if (!isJumping) 
            {
                isJumping = true;
                player_animation.SetBool("IsMove", false);
                player_animation.SetBool("IsRun", false);
                player_animation.SetTrigger("Jump");
                StartCoroutine(PauseJumpAnimation());
            }
        }
        else
        {
            if (isJumping)
            {
                isJumping = false;
                player_animation.ResetTrigger("Jump");
                player_animation.speed = 1f; 
            }

            if (isMoving)
            {
                player_animation.SetBool("IsMove", true);
            }
            else
            {
                player_animation.SetBool("IsMove", false);
                player_animation.SetBool("IsRun", false);
            }

            if (Input.GetKey(KeyCode.LeftShift) && isMoving)
            {
                player_animation.SetBool("IsMove", true);
                player_animation.SetBool("IsRun", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && isMoving)
            {
                player_animation.SetBool("IsMove", true);
                player_animation.SetBool("IsRun", false);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            player_animation.SetTrigger("Push");
            PushPlayer();
        }

    

    }

    void FixedUpdate()
    {
        if (moveVector != Vector3.zero)
        {
            rigid.MovePosition(rigid.position + moveVector * Time.fixedDeltaTime);

            Quaternion directionQuat = Quaternion.LookRotation(moveVector);
            Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, directionQuat, 0.3f);
            rigid.MoveRotation(moveQuat);
        }
    }

    private void Jump()
    {
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySound(jumpSound);
    }

    private void PushPlayer()
    {
        Vector3 pushDirection = transform.forward;
        Vector3 pushPosition = transform.position + transform.forward * 1.5f;
        Collider[] colliders = Physics.OverlapSphere(pushPosition, 0.5f);

        foreach (Collider collider in colliders)
        {
            Rigidbody otherRigidbody = collider.GetComponent<Rigidbody>();
            if (otherRigidbody != null && otherRigidbody != rigid)
            {
                otherRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator PauseJumpAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isGrounded) 
        {
            player_animation.speed = 0f; 
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}