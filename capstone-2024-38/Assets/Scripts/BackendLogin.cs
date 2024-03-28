using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using TMPro;

public class BackendLogin : MonoBehaviour 
{
    private static BackendLogin _instance = null;

    public TMP_InputField loginId;
    public TMP_InputField loginPw;
    public TMP_InputField signUpId;
    public TMP_InputField signUpPw;

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

    public void CustomSignUp()
    {
        Debug.Log("회원가입을 요청합니다.");

        string id = signUpId.text;
        string pw = signUpPw.text;

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
        }
    }

    public void CustomLogin()
    {
        Debug.Log("로그인을 요청합니다.");

        string id = loginId.text;
        string pw = loginPw.text;
        
        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
        }
        else
        {
            Debug.Log("로그인이 실패했습니다. : " + bro);
        }
    }

    public void UpdateNickname(string nickname)
    {
        Debug.Log("닉네임 변경을 요청합니다.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("닉네임 변경에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("닉네임 변경에 실패했습니다. : " + bro);
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
