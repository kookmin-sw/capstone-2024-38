using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColision : MonoBehaviour
{
    public Color paintColor = Color.blue;

    public float strength = 1;
    public float hardness = 1;
    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                PaintManager.instance.paint(p, pos, radius, hardness, strength, player.GetComponent<PlayerInputTest>().GetBulletColor());
            }
            
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player");
        }
    }
}
