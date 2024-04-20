using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class move_Controll : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed;
    public CinemachineFreeLook freeLookCamera;

    Rigidbody rigid;
    Vector3 moveVector;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

    }

    void Update()
    {
  
        float x = joystick.Vertical;
        float z = joystick.Horizontal;
        float Fx = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value;// ī�޶� ȸ�� ����

        if (x == 0 && z == 0)
        {
            return;
        }

        float angle = Mathf.Atan2(z, x) * Mathf.Rad2Deg; // ���̽�ƽ �̵��� ������ ��ȯ
        angle += Fx; // ���ؼ� ī�޶���� �̵����� ���
        if (angle < 0f)
        {
            angle += 360f;
        }

        Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        moveVector = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVector);
                
        if (moveVector.sqrMagnitude == 0)
            return; 

        Quaternion directionQuat = Quaternion.LookRotation(moveVector);
        Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, directionQuat, 0.3f);
        rigid.MoveRotation(moveQuat);
    }
}
