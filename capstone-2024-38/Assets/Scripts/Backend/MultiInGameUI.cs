using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiInGameUI : MonoBehaviour
{
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    
    private static MultiInGameUI _instance = null;
    
    public static MultiInGameUI Instance 
    {
        get {
            if(_instance == null) {
                _instance = new MultiInGameUI();
            }

            return _instance;
        }
    }
    
    public GameObject clearWindow;
    public GameObject failWindow;
    public GameObject startCountObject;

    public Animator clearAnim;
    public Animator failAnim;

    public TMP_Text timeText;
    public TMP_Text startCountText;

    public MapManager mapManager;
    
    // Start is called before the first frame update
    void Start()
    {
        clearAnim = clearWindow.GetComponent<Animator>();
        failAnim = failWindow.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeText();
    }
    
    public void SetStartCount(int time, bool isEnable = true)
    {
        startCountObject.SetActive(isEnable);
        if (isEnable)
        {
            if (time == 0)
            {
                startCountText.text = "Game Start!";
            }
            else
            {
                startCountText.text = string.Format("{0}", time);
            }
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
        if (failAnim != null)
        {
            failAnim.SetTrigger("lose");
        }
    }
    
    public void SetClearPopupTrigger()
    {
        if (clearAnim != null)
        {
            clearAnim.SetTrigger("win");
        }
    }
}
