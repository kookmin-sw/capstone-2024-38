using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkTest : MonoBehaviour
{

    public float inkGauge;

    // Start is called before the first frame update
    void Start()
    {
        inkGauge = 50.0f;
    }

    public void IncrementInkGauge(float incrementAmount)
    {
        inkGauge += incrementAmount;
        Debug.Log("Ink Charged! Current Ink Gauge: " + inkGauge);
    }
}
