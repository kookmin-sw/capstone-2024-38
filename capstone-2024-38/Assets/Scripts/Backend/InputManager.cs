using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class InputManager : MonoBehaviour
{
    private bool isMove = false;
    private bool isJump = true;

    public static InputManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.InGame += MobileInput;
        GameManager.InGame += AttackInput;
        GameManager.InGame += JumpInput;
        GameManager.AfterInGame += SendNoMoveMessage;
    }
    
    void MobileInput()
    {
        int keyCode = 0;
        
        isMove = true;

        keyCode |= KeyEventCode.MOVE;
        
        Vector3 moveVector = Vector3.zero;
        
        /*Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveInput.magnitude != 0)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveVector = lookForward * moveInput.y + lookRight * moveInput.x;
        }

        //playerBody.forward = moveVector;*/
        /*float horizontal = Input.GetAxis("Horizontal"); // A, D or Left Arrow, Right Arrow
        float vertical = Input.GetAxis("Vertical"); // W, S or Up Arrow, Down Arrow

        moveVector = new Vector3(horizontal, 0, vertical);
        
        moveVector = moveVector.normalized;
        
        //Vector3 moveVector = new Vector3(virtualStick.GetHorizontalValue(), 0, virtualStick.GetVerticalValue());
        moveVector = Vector3.Normalize(moveVector);*/
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0; // 수평 방향 벡터로 만들기 위해 y 값을 0으로 설정
        cameraRight.y = 0;

        moveVector = cameraForward * vertical + cameraRight * horizontal;
        moveVector = Vector3.Normalize(moveVector);


        if (keyCode <= 0)
        {
            return;
        }

        KeyMessage msg;
        msg = new KeyMessage(keyCode, moveVector);
        if (BackendMatchManager.GetInstance().IsHost())
        {
            BackendMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackendMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }

    void JumpInput()
    {
        int keyCode = 0;
        keyCode |= KeyEventCode.JUMP;

        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Debug.Log("2");
        Vector3 jumpVector = Vector3.zero;

        KeyMessage msg;
        msg = new KeyMessage(keyCode, jumpVector);
        if (BackendMatchManager.GetInstance().IsHost())
        {
            BackendMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackendMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }

    void AttackInput()
    {
        int keyCode = 0;
        keyCode |= KeyEventCode.ATTACK;

        Vector3 aimPos = new Vector3(0, 0, 1);
        aimPos += WorldManager.instance.GetMyPlayerPos();
        
        KeyMessage msg;
        msg = new KeyMessage(keyCode, aimPos);
        if (BackendMatchManager.GetInstance().IsHost())
        {
            BackendMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackendMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }
    
    void SendNoMoveMessage()
    {
        int keyCode = 0;
        if (!isMove && !isJump && WorldManager.instance.IsMyPlayerMove())
        {
            keyCode |= KeyEventCode.NO_MOVE;
        }
        if (keyCode == 0)
        {
            return;
        }
        KeyMessage msg = new KeyMessage(keyCode, Vector3.zero);

        if (BackendMatchManager.GetInstance().IsHost())
        {
            BackendMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackendMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }
    
}