using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_SkillandBullet : MonoBehaviour
{
    Bullet bullet;
    Skill skill;
    Transform bulletPoint;

    bool isCharging;
    public Transform playerT;
    float ChargingTime;
    float shootingDistance = 10.0f;
    float chargeDuration = 7.0f;

    Dictionary<Skill.Passive_SKill, bool> passive_dict;
    Dictionary<Skill.Active_Skill, bool> active_dict;

    int CurrPlayerHp;

    private void Awake()
    {
        //Player Data Roding Server
        RoadingPlayerState();
    }
    void Start()
    {
        bullet = this.GetComponent<Bullet>();
        playerT = GameObject.Find("Player").transform;
        bulletPoint = this.transform.Find("BulletPoint").transform;
        ChargingTime = 0.0f;

        skill = this.GetComponent<Skill>();

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerT.position);


        if (distanceToPlayer <= shootingDistance)
        {
            if (!isCharging)
            {
                bullet.CreateBullet(bullet.GetRandomBulletpattern(), Bullet.LnkColor.Blue, bulletPoint);
                ChargingTime = 0.0f;
                isCharging = true;
            }

            if (isCharging)
            {
                ChargingTime += Time.deltaTime;
                //bullet.ScaleUp();

                if (ChargingTime >= chargeDuration)
                {
                    skill.Shoot(bullet.GetCurrBullet(), bulletPoint, ChargingTime);
                    bullet.Shoot();
                    isCharging = false;
                }
            }
        }

        else
        {
            if (isCharging)
            {
                skill.Shoot(bullet.GetCurrBullet(), bulletPoint, ChargingTime);
                bullet.Shoot();
            }
            isCharging = false;
        }


    }

    public bool getCharging()
    {
        return isCharging;
    }

    //Player Data Roding Server
    void RoadingPlayerState()
    {
        if(!PlayerPrefs.HasKey("PlayerHp"))
        {
            Debug.Log("Fail Roding Player");
            CurrPlayerHp = 100;
        }
    }

}
