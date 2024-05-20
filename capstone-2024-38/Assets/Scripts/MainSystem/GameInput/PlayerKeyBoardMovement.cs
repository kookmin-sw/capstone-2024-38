using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyBoardMovement : MonoBehaviour
{
    public float movement_weight = 5.0f;
    public float jumpForce = 12.0f;


    Animator player_animation;
    Rigidbody rb;

    public LayerMask groundLayer;
    public float groundCheckDistance = 1.1f;
    private bool isGrounded;
    private bool isMoving = false;

    public Transform cameraTransform;
    

    void Start()
    {
        player_animation = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Default");
        Transform arm = this.transform.Find("Camera Arm");
        cameraTransform = arm.Find("Main Camera");

        if (player_animation == null)
        {
            Debug.LogWarning("Animator component is missing on the player game object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player_animation == null)
        {
            return;
        }

        float power = movement_weight * Time.deltaTime;
        Vector3 moveDirection = GetMoveDirection();

        isMoving = (moveDirection.magnitude > 0);

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
            power *= 1.5f;
            player_animation.SetBool("IsMove", false);
            player_animation.SetBool("IsRun", true);
        }
       
        

        transform.position += moveDirection * power;

        

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();

        }
        if (!isGrounded)
        {
            player_animation.SetBool("IsMove", false);
            player_animation.SetBool("IsRun", false);
            player_animation.SetTrigger("Jump");
        }
        else
        {
            player_animation.ResetTrigger("Jump");
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
        }
    }

    Vector3 GetMoveDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    
}
