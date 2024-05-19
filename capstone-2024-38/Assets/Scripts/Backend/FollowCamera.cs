using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float dist = 8.0f;
    public float height = 5.0f;
    public float smoothRotate = 10.0f;
    public float rotateSpeed = 5.0f; // 회전 속도
    public float offsetX = 10f;
    public float offsetY = 10f;
    public float offsetZ = 10f;

    private float mouseX = 0f;

    private void Update()
    {
        Vector3 FixedPos = new Vector3(target.transform.position.x + offsetX, target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = FixedPos;
    }

    /*void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 이동
        var targetPos = target.position - (Vector3.forward * dist) + (Vector3.up * height);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothRotate * Time.deltaTime);

        // 회전
        // 수평 회전
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        Quaternion newRotation = Quaternion.Euler(0, mouseX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * smoothRotate);

        // 수직 회전 (원하는 각도에 따라 조절 필요)
        float mouseY = -Input.GetAxis("Mouse Y") * rotateSpeed;
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x += mouseY;
        transform.rotation = Quaternion.Euler(currentRotation);
        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = transform.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x,-1f,70f);

        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        transform.rotation = (Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z));
        
        // 대상 쪽을 보도록 조정
        //transform.LookAt(target);
    }*/
}