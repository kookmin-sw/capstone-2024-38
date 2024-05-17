using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyBoardMovement : MonoBehaviour
{
    public float movement_weight = 5.0f;
    public float jumpForce = 5.0f;


    Animator player_animation;
    Rigidbody rb;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    private bool isGrounded;
    private bool isMoving = false;

    public Transform cameraTransform;
    

    void Start()
    {
        player_animation = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
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

        isMoving = moveDirection.magnitude > 0;

        if (Input.GetKey(KeyCode.LeftShift) && isMoving)
        {
            power *= 1.5f;
        }

        transform.position += moveDirection * power;

        if (Input.GetKey(KeyCode.W))
        {
            player_animation.SetBool("IsFront", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player_animation.SetBool("IsLeft", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player_animation.SetBool("IsBack", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player_animation.SetBool("IsRight", true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            player_animation.SetBool("IsFront", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player_animation.SetBool("IsLeft", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            player_animation.SetBool("IsBack", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            player_animation.SetBool("IsRight", false);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
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
