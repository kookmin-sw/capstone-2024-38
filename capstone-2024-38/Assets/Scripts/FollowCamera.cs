using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform followTarget;
    public float cameraHeight = 5.0f; // 카메라 높이
    public float cameraDist = 5.0f; // 카메라 거리
    public float smoothRotate = 5.0f; // 부드러운 회전 속도

    private Transform camTrans;

    void Start()
    {
        camTrans = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        float currYAngle = Mathf.LerpAngle(camTrans.eulerAngles.y, followTarget.eulerAngles.y, smoothRotate * Time.deltaTime);

        Quaternion quat = Quaternion.Euler(0, currYAngle, 0);

        camTrans.position = followTarget.position - (quat * Vector3.forward * cameraDist) + (Vector3.up * cameraHeight);

        camTrans.LookAt(followTarget);
    }
}
