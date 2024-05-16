using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject portalPrefab;
    public GameObject portalPos;
    private List<Vector4> PortalPoints;


    void Start()
    {
        portalPrefab = GameObject.Find("Portal");

        PortalPoints = new List<Vector4>();

        portalPos = GameObject.Find("PortalPostion");

        int num = portalPos.transform.childCount;

        for (int i = 0; i < num; ++i)
        {
            var child = portalPos.transform.GetChild(i);
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            PortalPoints.Add(point);
        }



        foreach (Vector4 position in PortalPoints)
        {
            Instantiate(portalPrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0f, position.w, 0f));
        }


    }
}