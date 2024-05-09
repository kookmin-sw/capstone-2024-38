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

        CubePower = 0.1f;
        SpherePower = 0.07f;
        CapsulePower = 0.025f;
        CylinderPower = 0.025f;
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
        switch (Currpattern)
        {
            case Bulletpattern.Capsule:
                Debug.Log(Capsule);
                CurrBullet = Instantiate(Capsule, pos.position, pos.rotation);
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

    }

    // Shoot Bullet
    public void Shoot()
    {
        // Must Remove this code!
        Destroy(CurrBullet);
        CurrBullet = null;
    }


    // Danger!! 
    public GameObject GetCurrBullet()
    {
        return CurrBullet;
    }

    public Bulletpattern GetRandomBulletpattern()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(Bulletpattern));
        return (Bulletpattern)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }


}
