using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyBoardMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float power = 10.0f * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0,0, power);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(-power, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(0, 0, -power);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(power, 0, 0);
        }
    }
}
