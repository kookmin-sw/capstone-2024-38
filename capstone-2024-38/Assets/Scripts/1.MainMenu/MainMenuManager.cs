using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Camera WordCamera;
    public Camera ShopCamera;
    float t;

    void Start()
    {
        t = 0;
        WordCamera.enabled = true;
        ShopCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (Input.GetKey(KeyCode.K))
        {
            ShowNextCamera(WordCamera, ShopCamera);
        }

        if (Input.GetKey(KeyCode.L))
        {
            ShowNextCamera(WordCamera, ShopCamera);
        }

    }

    void ShowNextCamera(Camera currCamera, Camera nextCamera)
    {
        currCamera.depth = -1;
        nextCamera.depth = 1;
    }
}
