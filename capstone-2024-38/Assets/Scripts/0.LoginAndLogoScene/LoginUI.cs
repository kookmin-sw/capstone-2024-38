using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    private static LoginUI instance;
    
    [SerializeField]
    private Animator LoginPopUp;

    [SerializeField]
    private Animator Logo;

    [SerializeField]
    private Animator SingUp;
    
    [SerializeField]
    public Animator NicknameUp;

    [SerializeField]
    private Animator ForgotPassword;

    [SerializeField]
    private GameObject nickname_popup;

    [SerializeField]
    private GameObject failpopup;

    bool first;

    public TMP_InputField loginId;
    public TMP_InputField loginPw;
    public TMP_InputField signUpId;
    public TMP_InputField signUpPw;
    public TMP_InputField nickname;

    public Button loginButton;
    public Button signUpButton;
    public Button nicknameButton;

    public GameObject nicknameWindow;

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
    
    void Start()
    {
        first = true;
        nicknameButton.onClick.AddListener(NicknameButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            if(first)
            {
                LoginPopUp.SetTrigger("PressAnyKey");
                Logo.SetTrigger("PressAnyKey");
                first = false;
            }
        }
    }


    public void fail_Check()
    {
        if(failpopup.activeSelf)
        {
            failpopup.SetActive(false);
        }
        else
        {
            failpopup.SetActive(true);
        }
    }
    public void nickname_popupOn()
    {
        nickname_popup.SetActive(true);
    }

    public void nickname_popupOff()
    {

        nickname_popup.SetActive(false);
    }
    public void SingUpButtonClick()
    {
        SingUp.SetTrigger("PressAnyKey");
        LoginPopUp.SetTrigger("Cancel");
    }

    public void SingUpPopUpCancel()
    {
        SingUp.SetTrigger("Cancel");
        LoginPopUp.SetTrigger("PressAnyKey");
    }
    public void SingUpPopUpSignUp()
    {
        Debug.Log("Check Server");
        BackendLogin.Instance.CustomSignUp(signUpId, signUpPw);
    }

    public void ForgotPasswordButtonClick()
    {
        ForgotPassword.SetTrigger("PressAnyKey");
        LoginPopUp.SetTrigger("Cancel");
    }

    public void ForgotPasswordButtonCancel()
    {
        ForgotPassword.SetTrigger("Cancel");
        LoginPopUp.SetTrigger("PressAnyKey");
    }
    public void ForgotPasswordFind()
    {
        Debug.Log("Check Server");
    }

    public void GameStartButtonClick()
    {
        // Check the User for server
        LoginPopUp.SetTrigger("Cancel");
        BackendLogin.Instance.CustomLogin(loginId, loginPw);
        //SceneManager.LoadScene("1.MainMenuScene");
    }

    public void NicknameButtonClicked()
    {
        BackendLogin.Instance.CreateNickname(nickname);
    }
}
