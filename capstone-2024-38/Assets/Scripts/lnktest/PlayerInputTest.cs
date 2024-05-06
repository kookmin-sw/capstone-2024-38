using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputTest : MonoBehaviour
{

    public GameObject[] BulletList;
    public Transform bulletPoint;


    bool isCharging = false; // �Ѿ��� ���� ������ ���θ� ��Ÿ���� ����
    float bulletspeed = 10.0f; // �Ѿ� �ӵ�
    float chargeSpeed = 0.5f; // �Ѿ��� Ŀ���� �ӵ�
    float minChargeTime = 0.5f; // �ּ� ���� �ð�
    float maxChargeTime = 3.0f; // �ִ� ���� �ð�
    float currentChargeTime = 0.0f; // ���� ���� �ð�

    GameObject chargingBullet; // ���� ���� �Ѿ��� ������ �����ϴ� ����

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCharging();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            FireBullet();
        }

        if (isCharging)
        {
            ChargeBullet();
        }
    }
    void StartCharging()
    {
        isCharging = true;
        currentChargeTime = 0.0f;
        chargingBullet = Instantiate(BulletList[0], bulletPoint.position, bulletPoint.rotation);
        chargingBullet.GetComponent<Rigidbody>().useGravity = false;
    }

    void ChargeBullet()
    {
        currentChargeTime += Time.deltaTime;
        currentChargeTime = Mathf.Clamp(currentChargeTime, minChargeTime, maxChargeTime);

        float chargePercent = (currentChargeTime - minChargeTime) / (maxChargeTime - minChargeTime);
        float chargePower = chargePercent * chargeSpeed;
        chargingBullet.transform.localScale += new Vector3(chargePower * Time.deltaTime, chargePower * Time.deltaTime, chargePower * Time.deltaTime);
    }

    void FireBullet()
    {
        isCharging = false;
        Rigidbody bulletRigidbody = chargingBullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * chargingBullet.transform.localScale.x * bulletspeed;
        chargingBullet.GetComponent<Rigidbody>().useGravity = true;
    }

}
