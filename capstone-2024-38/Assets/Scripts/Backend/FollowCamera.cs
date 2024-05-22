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
    private float mouseY = 0f;

    /*private void Update()
    {
        Vector3 FixedPos = new Vector3(target.transform.position.x + offsetX, target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = FixedPos;
    }*/

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
    }
}