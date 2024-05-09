using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Bullet bullet;
    Transform bulletPoint;

    bool isCharging;

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

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bullet.CreateBullet(bullet.GetRandomBulletpattern(), Bullet.LnkColor.Red, bulletPoint);
            isCharging = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            bullet.Shoot();
            isCharging = false;
        }


        if (isCharging) bullet.ScaleUp();
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
