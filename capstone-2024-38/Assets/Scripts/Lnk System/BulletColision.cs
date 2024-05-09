using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColision : MonoBehaviour
{
    Color paintColor = Color.blue;

    float strength = 1;
    float hardness = 1;
    float minRadius = 0.05f;
    float maxRadius = 0.2f;

    GameObject player;

    private void Start()
    {
        player = this.transform.parent.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Paintable p = other.GetComponent<Paintable>();

            if (p != null)
            {
                Vector3 pos = other.ClosestPoint(transform.position);
                float radius = 1;
                PaintManager.instance.paint(p, pos, radius, hardness, strength, player.GetComponent<Bullet>().GetCurrBulletColor());
            }
            
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject == player)
            {
                Debug.Log("My Player");
            }
            else
            {
                Debug.Log("Other Player");
            }
        }
    }
}
