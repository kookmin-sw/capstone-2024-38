using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material m = GetComponent<Renderer>().material;

        m.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
