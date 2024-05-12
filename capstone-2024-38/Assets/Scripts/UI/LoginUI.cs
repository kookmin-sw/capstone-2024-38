using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    private static LoginUI instance;  
    void Awake()
    {
        instance = this;
    }

    public static LoginUI GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("LoginUI 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeLobbyScene()
    {
        GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby, (bool isDone) =>
        {
            
        });  
    }
}
