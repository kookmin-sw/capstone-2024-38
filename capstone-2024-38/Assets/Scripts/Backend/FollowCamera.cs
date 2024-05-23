using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /*public Transform target;
    public float dist = 8.0f;
    public float height = 5.0f;
    public float smoothRotate = 10.0f;
    public float rotateSpeed = 5.0f; // 회전 속도
    public float offsetX = 10f;
    public float offsetY = 10f;
    public float offsetZ = 10f;

    private float mouseX = 0f;
    private float mouseY = 0f;

    private void Update()
    {
        Vector3 FixedPos = new Vector3(target.transform.position.x + offsetX, target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = FixedPos;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 이동
        Vector3 targetPos = target.position + (Vector3.up * height) - (transform.forward * dist);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothRotate * Time.deltaTime);

        // 마우스 입력에 따라 회전
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60); // 수직 회전 각도 제한

        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothRotate * Time.deltaTime);

        // 대상 쪽을 보도록 조정
        //transform.LookAt(target.position + Vector3.up * height);
    }*/
    
    public float Yaxis;
    public float Xaxis;
    
    public Transform target;//Player

    public float rotSensitive=3f;//카메라 회전 감도
    public float dis=2f;//카메라와 플레이어사이의 거리
    public float RotationMin=-10f;//카메라 회전각도 최소
    public float RotationMax=80f;//카메라 회전각도 최대
    public float smoothTime=0.12f;//카메라가 회전하는데 걸리는 시간
    //위 5개의 value는 개발자의 취향껏 알아서 설정해주자
    private Vector3 targetRotation;
    private Vector3 currentVel;
    
    void LateUpdate()//Player가 움직이고 그 후 카메라가 따라가야 하므로 LateUpdate
    {
        Yaxis=Yaxis+Input.GetAxis("Mouse X")*rotSensitive;//마우스 좌우움직임을 입력받아서 카메라의 Y축을 회전시킨다
        Xaxis=Xaxis-Input.GetAxis("Mouse Y")*rotSensitive;//마우스 상하움직임을 입력받아서 카메라의 X축을 회전시킨다
        //Xaxis는 마우스를 아래로 했을때(음수값이 입력 받아질때) 값이 더해져야 카메라가 아래로 회전한다 

        Xaxis=Mathf.Clamp(Xaxis,RotationMin,RotationMax);
        //X축회전이 한계치를 넘지않게 제한해준다.

        targetRotation=Vector3.SmoothDamp(targetRotation,new Vector3(Xaxis,Yaxis),ref currentVel,smoothTime);
        this.transform.eulerAngles=targetRotation;
        //SmoothDamp를 통해 부드러운 카메라 회전

        transform.position=target.position-transform.forward*dis;
        //카메라의 위치는 플레이어보다 설정한 값만큼 떨어져있게 계속 변경된다.
    }
}