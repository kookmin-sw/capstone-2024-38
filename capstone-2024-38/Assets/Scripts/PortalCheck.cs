using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCheck : MonoBehaviour
{
    public GameObject resultUI;

    private void Start()
    {
        resultUI = GameObject.Find("ResultUI"); // 현재 임시화면

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player");
            resultUI.SetActive(true);
            Time.timeScale = 0f;

        }

    }
        

        private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            resultUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
