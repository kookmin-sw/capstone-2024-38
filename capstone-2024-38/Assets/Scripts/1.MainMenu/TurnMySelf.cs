using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMySelf : MonoBehaviour
{

    // ȸ�� �ӵ��� �����ϴ� ����
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
        // ���� ȸ�� ����
        float currentZ = transform.rotation.eulerAngles.z;

        // ȸ�� ������ ���� �ӵ��� ���������� ����
        rotationSpeed = Mathf.Lerp(maxSpeed, minSpeed, Mathf.InverseLerp(0, 360, currentZ));

        // z���� �������� ȸ��
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // ������ 360���� ������ �ٽ� 0���� �ʱ�ȭ
        if (currentZ >= 360)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }
}
