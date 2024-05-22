using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS : MonoBehaviour
{

    public Transform characterBody;

    public Transform cameraArm;
    // Start is called before the first frame update
    void Start()
    {
        characterBody = this.GetComponent<Transform>();
        cameraArm = this.transform.Find("Camera Arm");
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        
    }

    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x,-1f,70f);

        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = (Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z));
    }
}
