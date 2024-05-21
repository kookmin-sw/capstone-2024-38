using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BackendLogin : MonoBehaviour 
{
    private static BackendLogin _instance = null;
    
    public GameObject nickNameWindow;

    public static BackendLogin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendLogin();
            }
            return _instance;
        }
    }
    

    public void CustomSignUp(TMP_InputField signUpId, TMP_InputField signUpPw)
    {
        Debug.Log("회원가입을 요청합니다.");

        string id = signUpId.text;
        string pw = signUpPw.text;

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다. : " + bro);
            LoginUI.GetInstance().signUpSuccess();
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
            LoginUI.GetInstance().signUpFailOn();
        }
    }

    public void CustomLogin(TMP_InputField logInId, TMP_InputField logInPw)
    {
        Debug.Log("로그인을 요청합니다.");

        string id = logInId.text;
        string pw = logInPw.text;
        
        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
        }
        else
        {
            LoginUI.GetInstance().fail_popupOn();
            Debug.Log("로그인이 실패했습니다. : " + bro);
            return;
        }
        
        if (Backend.UserNickName == "")
        {
            LoginUI.GetInstance().LoginPopUp.SetTrigger("Cancel");
            LoginUI.GetInstance().nicknameWindow.SetActive(true);
            LoginUI.GetInstance().NicknameUp.SetTrigger("PressAnyKey");
        }
        else
        {
            GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);
            //SceneManager.LoadScene("1. MatchLobby");
        }
    }

    public void CreateNickname(TMP_InputField nickName)
    {
        Debug.Log("닉네임 변경을 요청합니다.");
        string name = nickName.text;
        var bro = Backend.BMember.UpdateNickname(name);
        BackendGameData.Instance.GameDataInsert();
        BackendRank.Instance.RankInsert(0);
        //SceneManager.LoadScene("1. MatchLobby");

        if (bro.IsSuccess())
        {
            Debug.Log("닉네임 변경에 성공했습니다. : " + bro);
            GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);

        }
        else
        {
            Debug.LogError("닉네임 변경에 실패했습니다. : " + bro);
            LoginUI.GetInstance().nicknameFailOn();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
