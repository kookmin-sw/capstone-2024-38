using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upLava : MonoBehaviour
{
    public float targetY;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        targetY = transform.position.y + targetY;
        moveSpeed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = Mathf.Lerp(transform.position.y, targetY, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
