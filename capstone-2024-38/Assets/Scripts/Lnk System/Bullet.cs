using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public enum Bulletpattern
    {
        Cube,
        Sphere,
        Capsule,
        Cylinder
    }

    public enum LnkColor
    {
        Red,
        Blue,
        Green,
        White,
        Black
    }
    Bulletpattern Currpattern;
    LnkColor Currlnk;

    Material Red, Blue, Green, White, Black;

    GameObject Cube, Sphere, Capsule, Cylinder;

    GameObject CurrBullet;


    float CubePower, SpherePower, CapsulePower, CylinderPower;



    void Awake()
    {
        // Load Material
        Red = Resources.Load("BulletMaterial/Lava") as Material;
        if (Red == null) Debug.Log("Material Red is not Road!");

        Blue = Resources.Load("BulletMaterial/Sea") as Material;
        if (Blue == null) Debug.Log("Material Blue is not Road!");

        Green = Resources.Load("BulletMaterial/Acid") as Material;
        if (Green == null) Debug.Log("Material Green is not Road!");

        White = Resources.Load("BulletMaterial/Ice") as Material;
        if (White == null) Debug.Log("Material White is not Road!");

        Black = Resources.Load("BulletMaterial/Space") as Material;
        if (Black == null) Debug.Log("Material Black is not Road!");

        // Load GameObject Bullet
        Cube = Resources.Load("BulletPrefab/Cube") as GameObject;
        if (Cube == null) Debug.Log("GameObject Cube is not Road!");

        Sphere = Resources.Load("BulletPrefab/Sphere") as GameObject;
        if (Sphere == null) Debug.Log("GameObject Sphere is not Road!");

        Capsule = Resources.Load("BulletPrefab/Capsule") as GameObject;
        if (Capsule == null) Debug.Log("GameObject Capsule is not Road!");

        Cylinder = Resources.Load("BulletPrefab/Cylinder") as GameObject;
        if (Cylinder == null) Debug.Log("GameObject Cylinder is not Road!");

        Debug.Log("Roading finished!");

        CubePower = 1.1f;
        SpherePower = 1.07f;
        CapsulePower = 1.025f;
        CylinderPower = 1.025f;
    }

    //Scale Up to CurrBullet
    public void ScaleUp()
    {
        if (CurrBullet == null) return;

        if(Currpattern == Bulletpattern.Cube)
        {
            CurrBullet.transform.Rotate(new Vector3(10f,20f,30f) * Time.deltaTime);
            CurrBullet.transform.localScale += new Vector3(CubePower * Time.deltaTime, CubePower * Time.deltaTime, CubePower * Time.deltaTime);
        }

        if(Currpattern == Bulletpattern.Sphere)
        {
            CurrBullet.transform.Rotate(new Vector3(10f, 20f, 30f) * Time.deltaTime);
            CurrBullet.transform.localScale += new Vector3(SpherePower * Time.deltaTime, SpherePower * Time.deltaTime, SpherePower * Time.deltaTime);
        }

        if (Currpattern == Bulletpattern.Capsule)
        {
            CurrBullet.transform.Rotate(new Vector3(10f, 20f, 30f) * Time.deltaTime);
            CurrBullet.transform.localScale += new Vector3(CapsulePower * Time.deltaTime, CapsulePower * Time.deltaTime, CapsulePower * Time.deltaTime);
        }

        if (Currpattern == Bulletpattern.Cylinder)
        {
            CurrBullet.transform.Rotate(new Vector3(10f, 20f, 30f) * Time.deltaTime);
            CurrBullet.transform.localScale += new Vector3(CylinderPower * Time.deltaTime, CylinderPower * Time.deltaTime, CylinderPower * Time.deltaTime);
        }
    }


    // Create Bullet
    public void CreateBullet(Bulletpattern bp, LnkColor lnk, Transform pos)
    {
        Currlnk = lnk;
        Currpattern = bp;

        if (this.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.OnlyCube])
        {
            CurrBullet = Instantiate(Cube, pos);
        }
        else if (this.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.OnlyCapsule])
        {
            CurrBullet = Instantiate(Capsule, pos);
        }
        else if (this.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.OnlyCylinder])
        {
            CurrBullet = Instantiate(Cylinder, pos);
        }
        else if (this.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.OnlySphere])
        {
            CurrBullet = Instantiate(Sphere, pos);
        }
        else
        {
            switch (Currpattern)
            {
                case Bulletpattern.Capsule:
                    CurrBullet = Instantiate(Capsule, pos);
                    break;

                case Bulletpattern.Cube:
                    CurrBullet = Instantiate(Cube, pos);
                    break;

                case Bulletpattern.Cylinder:
                    CurrBullet = Instantiate(Cylinder, pos);
                    break;

                case Bulletpattern.Sphere:
                    CurrBullet = Instantiate(Sphere, pos);
                    break;

                default:
                    Debug.Log("BulletCreate pattern fail");
                    break;
            }
        }


        switch (Currlnk)
        {
            case LnkColor.Black:
                CurrBullet.GetComponent<MeshRenderer>().material = Black;
                break;
            case LnkColor.Blue:
                CurrBullet.GetComponent<MeshRenderer>().material = Blue;
                break;
            case LnkColor.Green:
                CurrBullet.GetComponent<MeshRenderer>().material = Green;
                break;
            case LnkColor.Red:
                CurrBullet.GetComponent<MeshRenderer>().material = Red;
                break;
            case LnkColor.White:
                CurrBullet.GetComponent<MeshRenderer>().material = White;
                break;
            default:
                Debug.Log("BulletCreate lnk fail");
                break;
        }
        CurrBullet.GetComponent<Rigidbody>().useGravity = false;
        SetInitialBulletScale();

    }

    // Shoot Bullet
    public void Shoot()
    {
        CurrBullet.GetComponent<Rigidbody>().useGravity = true;
        CurrBullet = null;
    }


    // Danger!! 
    public GameObject GetCurrBullet()
    {
        return CurrBullet;
    }

    public Color GetCurrBulletColor()
    {
        Color tmp = Color.white;

        switch (Currlnk)
        {
            case LnkColor.Black:
                tmp = Color.black;
                break;
            case LnkColor.Blue:
                tmp = Color.blue;
                break;
            case LnkColor.Green:
                tmp = Color.green;
                break;
            case LnkColor.Red:
                tmp = Color.red;
                break;
            case LnkColor.White:
                tmp = Color.white;
                break;
            default:
                Debug.Log("BulletCreate lnk fail");
                break;
        }

        return tmp;
    }

    public Bulletpattern GetRandomBulletpattern()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(Bulletpattern));
        return (Bulletpattern)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }


    private void SetInitialBulletScale()
    {
        Vector3 initialScale = new Vector3(1f, 1f, 1f);
        CurrBullet.transform.localScale = initialScale;
    }

}
