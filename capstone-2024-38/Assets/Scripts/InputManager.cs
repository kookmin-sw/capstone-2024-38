using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class InputManager : MonoBehaviour
{
    public VirtualStick virtualStick;
    private bool isMove = false;

    private void Start()
    {
        
    }
    
    void MobileInput()
    {
        if (!virtualStick)
        {
            return;
        }

        int keyCode = 0;
        isMove = false;

        if (!virtualStick.isInputEnable)
        {
#if !UNITY_EDITOR
			isMove = false;
#endif
            return;
        }

        isMove = true;

        keyCode |= KeyEventCode.MOVE;
        Vector3 moveVector = new Vector3(virtualStick.GetHorizontalValue(), 0, virtualStick.GetVerticalValue());
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

    private void Update()
    {
        MobileInput();
        SendNoMoveMessage();
    }
}
