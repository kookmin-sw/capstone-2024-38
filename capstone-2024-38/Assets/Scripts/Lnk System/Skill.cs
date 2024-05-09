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
        Throw8Way
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
        //skill test
        passive_dict[Passive_SKill.BulletSpeedUp] = false;
        active_dict[Active_Skill.Throw4Way] = true;

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
    public void Shoot(GameObject bullet, Transform tr)
    {

        foreach (var active in active_dict)
        {
            if(active.Value)
            {
                switch (active.Key)
                {
                    case Active_Skill.guided:
                        break;
                    case Active_Skill.Throw4Way:
                        Throw4Way(bullet,tr);
                        break;
                    case Active_Skill.Throw8Way:
                        break;
                }
            }
        }
    }

    public void Throw4Way(GameObject bullet, Transform tr)
    {
        GameObject clonebullet = Instantiate(bullet, tr.position, Quaternion.identity);

        clonebullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        clonebullet.GetComponent<Rigidbody>().useGravity = true;
        clonebullet.GetComponent<Rigidbody>().AddForce(Vector3.up * 2, ForceMode.Impulse);
    }
}
