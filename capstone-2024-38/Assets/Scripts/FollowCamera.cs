using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform followTarget;
    public float cameraHeight = 5.0f; // ī�޶� ����
    public float cameraDist = 5.0f; // ī�޶� �Ÿ�
    public float smoothRotate = 5.0f; // �ε巯�� ȸ�� �ӵ�

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
