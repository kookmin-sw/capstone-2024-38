using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputTest : MonoBehaviour
{

    public GameObject[] BulletList;
    public Transform bulletPoint;


    bool isCharging = false; // 총알을 충전 중인지 여부를 나타내는 변수
    float bulletspeed = 10.0f; // 총알 속도
    float chargeSpeed = 0.5f; // 총알이 커지는 속도
    float minChargeTime = 0.5f; // 최소 충전 시간
    float maxChargeTime = 3.0f; // 최대 충전 시간
    float currentChargeTime = 0.0f; // 현재 충전 시간

    GameObject chargingBullet; // 충전 중인 총알의 참조를 저장하는 변수

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
