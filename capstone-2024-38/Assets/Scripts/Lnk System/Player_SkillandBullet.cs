using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

<<<<<<<< HEAD:capstone-2024-38/Assets/Scripts/Lnk System/Player_SkillandBullet.cs
public class Player_SkillandBullet : MonoBehaviour
========
public class Player1 : MonoBehaviour
>>>>>>>> server:capstone-2024-38/Assets/Scripts/Lnk System/Player1.cs
{
    Bullet bullet;
    Skill skill;
    Transform bulletPoint;

    bool isCharging;
    float ChargingTime;

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
        bulletPoint = this.transform.Find("BulletPoint").transform;
        ChargingTime = 0.0f;

        skill = this.GetComponent<Skill>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bullet.CreateBullet(bullet.GetRandomBulletpattern(), Bullet.LnkColor.Red, bulletPoint);
            ChargingTime = 0.0f;
            isCharging = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            skill.Shoot(bullet.GetCurrBullet(), bulletPoint, ChargingTime);
            bullet.Shoot();
            isCharging = false;
        }


        if (isCharging)
        {
            ChargingTime += Time.deltaTime;
            bullet.ScaleUp();
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