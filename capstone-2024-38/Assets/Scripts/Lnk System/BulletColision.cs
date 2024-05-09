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

    private bool Isblunce;

    GameObject player;

    private void Start()
    {
        player = this.transform.parent.parent.gameObject;
        Isblunce = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colision!");
        if (other.gameObject.CompareTag("Floor"))
        {
            Paintable p = other.GetComponent<Paintable>();

            if (p != null)
            {
                Vector3 pos = other.ClosestPoint(transform.position);
                float radius = 1;
                PaintManager.instance.paint(p, pos, radius, hardness, strength, player.GetComponent<Bullet>().GetCurrBulletColor());
            }
            Destroy(this);
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
            Debug.Log("Player Colision!");
        }
        else if (player.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.bounce] && other.gameObject.CompareTag("Bullet") && !Isblunce)
        {

            Debug.Log("Bullet Colision!");
            // �浹�� �Ѿ��� Rigidbody�� ������
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

            // �浹�� �Ѿ��� ������ �ݴ�� �����Ͽ� �ñ�
            otherRigidbody.velocity = -otherRigidbody.velocity;

            Isblunce = true;
        }else if(player.GetComponent<Skill>().GetPassiveSkill()[Skill.Passive_SKill.bounce] && !other.gameObject.CompareTag("Bullet"))
        {
            // �浹�� ��ü�� ������ ������
            Vector3 normal = other.ClosestPoint(transform.position) - transform.position;

            // �Ի簢 ���
            Vector3 incomingVector = -transform.forward;
            float angle = Vector3.Angle(incomingVector, normal);
            float reflectionAngle = 90f - angle;

            // �ݻ� ���� ���
            Vector3 reflectDir = Vector3.Reflect(incomingVector, normal);

            // �Ѿ� ���� ����
            transform.forward = reflectDir;

            // �ӵ� �״�� �����ϸ鼭 �Ѿ��� ���� ����
            GetComponent<Rigidbody>().velocity = reflectDir * GetComponent<Rigidbody>().velocity.magnitude;
        }
        else
        {
            Destroy(this);
        }
    }
}
