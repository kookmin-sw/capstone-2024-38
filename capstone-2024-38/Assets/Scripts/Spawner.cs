using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject portalPrefab;
    public GameObject portalPos;
    public GameObject inkPrefab;
    public GameObject InkPos;
    private List<Vector4> PortalPoints;
    private List<Vector4> InkPoints;


    void Start()
    {
        portalPrefab = GameObject.Find("Portal");
        inkPrefab = GameObject.Find("InkPotion");

        PortalPoints = new List<Vector4>();
        InkPoints = new List<Vector4>();

        portalPos = GameObject.Find("PortalPostion");
        InkPos = GameObject.Find("InkPostion");

        int num = portalPos.transform.childCount;

        for (int i = 0; i < num; ++i)
        {
            var child = portalPos.transform.GetChild(i);
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            PortalPoints.Add(point);
        }

        num = InkPos.transform.childCount;
        for (int i = 0; i < num; ++i)
        {
            var child = InkPos.transform.GetChild(i);
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            InkPoints.Add(point);
        }

        foreach (Vector4 position in PortalPoints)
        {
            Instantiate(portalPrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0f, position.w, 0f));
        }

        foreach (Vector4 position in InkPoints)
        {
            Instantiate(inkPrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0f, position.w, 0f));
        }
    }
}