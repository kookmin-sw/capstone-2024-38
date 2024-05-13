using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginUI : MonoBehaviour
{
    private static LoginUI instance;  
    
    public TMP_InputField loginId;
    public TMP_InputField loginPw;
    public TMP_InputField signUpId;
    public TMP_InputField signUpPw;
    public TMP_InputField nickName;
    
    public GameObject nickNameWindow;
    public GameObject signUpWindow;
    
    public Michsky.MUIP.ButtonManager logInButton;
    public Michsky.MUIP.ButtonManager signUpButton;
    public Michsky.MUIP.ButtonManager signUpWindowButton;
    public Michsky.MUIP.ButtonManager nicknameButton;
    
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
        logInButton.onClick.AddListener(LoginButtonClicked);
        signUpButton.onClick.AddListener(SignUpButtonClicked);
        
        signUpWindowButton.onClick.AddListener(SignUpWindowButtonClicked);
        
        nicknameButton.onClick.AddListener(NicknameButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoginButtonClicked()
    {
        BackendLogin.Instance.CustomLogin(loginId, loginPw);
    }

    void SignUpButtonClicked()
    {
        BackendLogin.Instance.CustomSignUp(signUpId, signUpPw);
        signUpWindow.SetActive(false);
    }

    void SignUpWindowButtonClicked()
    {
        signUpWindow.SetActive(true);
    }

    void NicknameButtonClicked()
    {
        BackendLogin.Instance.CreateNickname(nickName);
    }
}
