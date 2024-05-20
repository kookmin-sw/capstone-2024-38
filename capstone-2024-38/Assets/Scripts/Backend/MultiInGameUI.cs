using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
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

    public Animator clearAnim;
    public Animator failAnim;

    public TMP_Text timeText;

    public MapManager mapManager;
    
    // Start is called before the first frame update
    void Start()
    {
        clearAnim = clearWindow.GetComponent<Animator>();
        failAnim = failWindow.GetComponent<Animator>();
=======
>>>>>>> Stashed changes

public class MultiInGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
<<<<<<< Updated upstream
=======
>>>>>>> 833ffab04570b9c96b843d0d489a122fa74ef081
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
<<<<<<< HEAD
        UpdateTimeText();
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
=======
        
>>>>>>> 833ffab04570b9c96b843d0d489a122fa74ef081
>>>>>>> Stashed changes
    }
}
