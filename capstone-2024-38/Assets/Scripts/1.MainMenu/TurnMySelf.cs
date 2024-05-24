using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMySelf : MonoBehaviour
{

    // 회전 속도를 조절하는 변수
    public float minSpeed = 50f;
    public float maxSpeed = 5f;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 회전 각도
        float currentZ = transform.rotation.eulerAngles.z;

        // 회전 각도에 따라 속도를 선형적으로 조절
        rotationSpeed = Mathf.Lerp(maxSpeed, minSpeed, Mathf.InverseLerp(0, 360, currentZ));

        // z축을 기준으로 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 각도가 360도를 넘으면 다시 0도로 초기화
        if (currentZ >= 360)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }
}
