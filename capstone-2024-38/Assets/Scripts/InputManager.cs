using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class InputManager : MonoBehaviour
{
    private bool isMove = false;

    private void Start()
    {
        GameManager.InGame += MobileInput;
        GameManager.InGame += AttackInput;
        GameManager.AfterInGame += SendNoMoveMessage;
    }
    
    void MobileInput()
    {
        int keyCode = 0;
        
        isMove = true;

        keyCode |= KeyEventCode.MOVE;
        
        float power = 10.0f * Time.deltaTime;
        
        Vector3 moveVector = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            moveVector = new Vector3(0,0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector = new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector = new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector = new Vector3(1, 0, 0);
        }
        //Vector3 moveVector = new Vector3(virtualStick.GetHorizontalValue(), 0, virtualStick.GetVerticalValue());
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

    void AttackInput()
    {
        
    }
    
    void SendNoMoveMessage()
    {
        int keyCode = 0;
        if (!isMove && WorldManager.instance.IsMyPlayerMove())
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