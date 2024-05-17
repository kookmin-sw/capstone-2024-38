using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Animator padeout;

    [SerializeField]
    private Animator title;

    [SerializeField]
    private GameObject stage;

    Animator stageani;
    Transform page_navi;

    [SerializeField]
    private Animator setting;

    [SerializeField]
    private GameObject Language_popup;

    [SerializeField]
    private Animator Ranking;

    [SerializeField]
    private GameObject Fail_popuo;

    [SerializeField]
    private GameObject Mail;

    [SerializeField]
    private GameObject MailCheck;

    [SerializeField]
    private Animator mail_popup;

    private void Start()
    {
        page_navi = stage.transform.Find("Page_navi");
        stageani = stage.GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    void titleAnimation()
    {
        if(title.GetBool("Title"))
        {
            title.SetBool("Title", false);
        }
        else
        {
            title.SetBool("Title", true);
        }
    }

    void stageAnimation()
    {
        if (stageani.GetBool("Stage"))
        {
            stageani.SetBool("Stage", false);
        }
        else
        {
            stageani.SetBool("Stage", true);
        }
    }

    void settingAnimation()
    {
        if (setting.GetBool("check"))
        {
            setting.SetBool("check", false);
        }
        else
        {
            setting.SetBool("check", true);
        }
    }

    void RankingAnimation()
    {
        if (Ranking.GetBool("check"))
        {
            Ranking.SetBool("check", false);
        }
        else
        {
            Ranking.SetBool("check", true);
        }
    }

    void MailAnimation()
    {
        if (mail_popup.GetBool("check"))
        {
            mail_popup.SetBool("check", false);
        }
        else
        {
            mail_popup.SetBool("check", true);
        }
    }

    public void StageRightButtonClick()
    {
        if(page_navi.Find("Page_1").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_1").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_2").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage",2);
        }
        else if(page_navi.Find("Page_2").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_2").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_3").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage", 3);
        }
        else if (page_navi.Find("Page_3").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_3").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_4").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage", 4);
        }
        else if (page_navi.Find("Page_4").Find("On").gameObject.activeSelf == true)
        {

        }
    }

    public void StageLeftButtonClick()
    {
        if (page_navi.Find("Page_4").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_4").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_3").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage", 3);
        }
        else if (page_navi.Find("Page_3").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_3").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_2").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage", 2);
        }
        else if (page_navi.Find("Page_2").Find("On").gameObject.activeSelf == true)
        {
            page_navi.Find("Page_2").Find("On").gameObject.SetActive(false);
            page_navi.Find("Page_1").Find("On").gameObject.SetActive(true);
            stageani.SetInteger("CurStage", 1);
        }
        else if (page_navi.Find("Page_1").Find("On").gameObject.activeSelf == true)
        {

        }
    }

    public void ButtonClick_StageAndTitleSwap()
    {
        titleAnimation();
        stageAnimation();
    }

    public void ButtonClick_SettingTrtleSwap()
    {
        settingAnimation();
        titleAnimation();
    }

    public void ButtonClick_LanguageSetting()
    {
        if(Language_popup.activeSelf == true)
        {
            Language_popup.SetActive(false);
        }
        else
        {
            Language_popup.SetActive(true);
        }
    }

    public void ButtonClick_FailPopup()
    {
        if (Fail_popuo.activeSelf == true)
        {
            Fail_popuo.SetActive(false);
        }
        else
        {
            Fail_popuo.SetActive(true);
        }
    }

    public void ButtonClick_Ranking()
    {
        titleAnimation();
        RankingAnimation();
    }

    public void ButtonClick_MailSucces()
    {
        Mail.SetActive(false);
        MailCheck.SetActive(false);
    }

    public void ButtonClick_mailpopup()
    {
        titleAnimation();
        MailAnimation();
    }

}
