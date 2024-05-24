using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class InGameUI : MonoBehaviour
{
    public GameObject player;
    //public TMP_Text heightText; 
    public TMP_Text timeText;   
    public MapManager mapManager;

    [SerializeField]
    private GameObject failPopup;
    private Animator failPopupAnimator;
    [SerializeField]
    private GameObject clearPopup;
    private Animator clearPopupAnimator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject rankObject = GameObject.Find("rank");
        //heightText = rankObject.GetComponentInChildren<TMP_Text>();
        timeText = rankObject.GetComponentInChildren<TMP_Text>();
        failPopup = GameObject.Find("Popup_Lose");
        failPopupAnimator = failPopup.GetComponent<Animator>();
        clearPopup = GameObject.Find("Popup_Stage_Complete_1");
        clearPopupAnimator = clearPopup.GetComponent<Animator>();

        mapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        //float playerY = player.transform.position.y - 0.09f;
        //heightText.text = playerY.ToString("F2") + " m";

        UpdateTimeText();

        if (Input.GetKey(KeyCode.P))
        {
            SetFailPopupTrigger();
        }

    }

    private void UpdateTimeText()
    {
        if (mapManager != null)
        {
            float remainingTime = mapManager.RemainingTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void SetFailPopupTrigger()
    {
        if (failPopupAnimator != null)
        {
            failPopupAnimator.SetTrigger("lose");
        }
    }

    public void SetClearPopupTrigger()
    {
        if (clearPopupAnimator != null)
        {
            clearPopupAnimator.SetTrigger("win");
        }
    }

}
