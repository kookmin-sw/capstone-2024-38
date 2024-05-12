using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("ink_Charge");
            InkTest inkTestComponent = other.gameObject.GetComponent<InkTest>();
            inkTestComponent.IncrementInkGauge(10f);

        }


    }
}
