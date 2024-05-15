using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    public enum Passive_SKill
    {
        bounce,
        OnlyCube,
        OnlySphere,
        OnlyCapsule,
        OnlyCylinder,
        dmg5Up,
        BulletSpeedUp
    }

    public enum Active_Skill
    {
        Throw4Way,
        guided,
        Shot3
    }

    Dictionary<Passive_SKill, bool> passive_dict;
    Dictionary<Active_Skill, bool> active_dict;

    float bulletSpeed;
    void Start()
    {
        passive_dict = new Dictionary<Passive_SKill, bool>();
        active_dict = new Dictionary<Active_Skill, bool>();
        foreach (Active_Skill dir in Enum.GetValues(typeof(Active_Skill)))
        {
            active_dict.Add(dir, false);
        }
        foreach (Passive_SKill dir in Enum.GetValues(typeof(Passive_SKill)))
        {
            passive_dict.Add(dir, false);
        }
        

        passive_dict[Passive_SKill.bounce] = true;

        if (passive_dict.TryGetValue(Passive_SKill.BulletSpeedUp, out bool value))
        {
            if (value) bulletSpeed = 100f;
            else bulletSpeed = 5f;
        }
    }

    public Dictionary<Passive_SKill, bool> GetPassiveSkill()
    {
        return passive_dict;
    }

    public Dictionary<Active_Skill, bool> GetActiveSkill()
    {
        return active_dict;
    }

    public void SetActiveSkill(Dictionary<Active_Skill, bool> act_dict)
    {
        active_dict = act_dict;
    }
    public void Shoot(GameObject bullet, Transform tr, float Ch)
    {
        bool allFalse = true;
        foreach (var active in active_dict)
        {
            if(active.Value)
            {
                allFalse = false;
                switch (active.Key)
                {
                    case Active_Skill.guided:
                        guided(bullet, tr, Ch);
                        break;
                    case Active_Skill.Throw4Way:
                        Throw4Way(bullet,tr, Ch);
                        break;
                    case Active_Skill.Shot3:
                        shot3(bullet, tr, Ch);
                        break;
                }

            }
        }
        if (allFalse)
        {
            nomal_Shoot(bullet, tr, Ch);
        }
    }

    public void nomal_Shoot(GameObject bullet, Transform tr, float ch)
    {
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * ch;
        bullet.GetComponent<Rigidbody>().useGravity = true;
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
    }
    public void Throw4Way(GameObject bullet, Transform tr, float ch)
    {

        GameObject clonebullet = Instantiate(bullet, tr);

        clonebullet.GetComponent<Rigidbody>().velocity = transform.right * bulletSpeed * ch;
        clonebullet.GetComponent<Rigidbody>().useGravity = true;
        clonebullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);

        GameObject clonebullet2 = Instantiate(bullet, tr);

        clonebullet2.GetComponent<Rigidbody>().velocity = -transform.right * bulletSpeed * ch;
        clonebullet2.GetComponent<Rigidbody>().useGravity = true;
        clonebullet2.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);

        GameObject clonebullet3 = Instantiate(bullet, tr);

        clonebullet3.GetComponent<Rigidbody>().velocity = -transform.forward * bulletSpeed * ch;
        clonebullet3.GetComponent<Rigidbody>().useGravity = true;
        clonebullet3.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);


        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * ch;
        bullet.GetComponent<Rigidbody>().useGravity = true;
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
    }

    public void shot3(GameObject bullet, Transform tr, float ch)
    {
        for (int i = 2; i < 4; i++)
        {
            GameObject clonebullet = Instantiate(bullet, tr);

            clonebullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * i * ch;
            clonebullet.GetComponent<Rigidbody>().useGravity = true;
            clonebullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        }

        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * ch;
        bullet.GetComponent<Rigidbody>().useGravity = true;
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
    }

    public void guided(GameObject bullet, Transform tr, float ch)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform target = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance && player != this.gameObject)
            {
                closestDistance = distance;
                target = player.transform;
            }
        }

        if (target != null)
        {
            bullet.transform.LookAt(target);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed * ch;
            bullet.GetComponent<Rigidbody>().useGravity = true;
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        }
        else
        {
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * ch;
            bullet.GetComponent<Rigidbody>().useGravity = true;
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        }

        
    }
}
