using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    public Michsky.MUIP.ButtonManager rankButton;
    public Michsky.MUIP.ButtonManager mailButton;
    public Michsky.MUIP.ButtonManager settingButton;
    public Michsky.MUIP.ButtonManager matchMakingButton;

    public GameObject rankWindow;
    public GameObject mailWindow;
    public GameObject settingWindow;
    
    
    // Start is called before the first frame update
    void Start()
    {
        BackendMatchManager.GetInstance().JoinMatchServer();
        
        Invoke("CreateRoomButtonClicked", 1f);
        
        rankButton.onClick.AddListener(RankButtonClicked);
        mailButton.onClick.AddListener(MailButtonClicked);
        settingButton.onClick.AddListener(SettingButtonClicked);
        
        matchMakingButton.onClick.AddListener(MatchMakingButtonClicked);
    }

    void RankButtonClicked()
    {
        rankWindow.SetActive(true);
    }

    void MailButtonClicked()
    {
        mailWindow.SetActive(true);   
    }

    void SettingButtonClicked()
    {
        settingWindow.SetActive(true);
    }

    void CreateRoomButtonClicked()
    {
        BackendMatchManager.GetInstance().CreateMatchRoom();
    }

    void MatchMakingButtonClicked()
    {
        BackendMatchManager.GetInstance().RequestMatchMaking();
    }
}
