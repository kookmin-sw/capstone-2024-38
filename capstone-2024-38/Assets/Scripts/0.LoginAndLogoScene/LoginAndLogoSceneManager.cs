using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginAndLogoSceneManager : MonoBehaviour
{
    [SerializeField]
    private Animator LoginPopUp;

    [SerializeField]
    private Animator Logo;

    [SerializeField]
    private Animator SingUp;

    [SerializeField]
    private Animator ForgotPassword;

    bool first;
    void Start()
    {
        first = true;
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

        SceneManager.LoadScene("1.MainMenuScene");
    }
}
