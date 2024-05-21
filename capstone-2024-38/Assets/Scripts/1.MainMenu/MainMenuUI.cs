using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class MainMenuUI : MonoBehaviour
{
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    
    private static MainMenuUI _instance = null;
    
    public static MainMenuUI Instance {
        get {
            if(_instance == null) {
                _instance = new MainMenuUI();
            }

            return _instance;
        }
    }
    
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

    [SerializeField]
    private Animator myinfo_popup;

    [SerializeField]
    private Animator shop_popup;

    [SerializeField]
    private GameObject Mission;
    private Animator Mission_ani;

    [SerializeField]
    private GameObject serching;

    [SerializeField]
    private float serchingTime;

    public TMP_Text rank1Name;
    public TMP_Text rank2Name;
    public TMP_Text rank3Name;
    public TMP_Text myRankName;

    public TMP_Text rank1Score;
    public TMP_Text rank2Score;
    public TMP_Text rank3Score;
    public TMP_Text myRankScore;

    public TMP_Text myRankNum;

    public TMP_Text myNickname;

    public TMP_Text levelInfo;
    public TMP_Text winsInfo;
    public TMP_Text loseInfo;
    public TMP_Text goldInfo;
    public TMP_Text gemInfo;
    
    public TMP_Text missionLevelText;
    public TMP_Text missionWinsText;
    public TMP_Text missionRPText;
    public TMP_Text missionMatchText;
    
    public GameObject postPrefab;
    public GameObject postList;
    
    public Slider levelSlider;
    public Slider winSlider;
    public Slider rankPointSlider;
    public Slider matchSlider;

    private void Start()
    {
        Mission_ani = Mission.GetComponent<Animator>();
        BackendMatchManager.GetInstance().GetMyData();
        BackendMatchManager.GetInstance().JoinMatchServer();
        Invoke("CreateRoom", 0.5f);
        BackendRank.Instance.RankGet();
        BackendRank.Instance.GetMyRank();
        BackendPost.Instance.PostListGet(PostType.Admin);
        page_navi = stage.transform.Find("Page_navi");
        stageani = stage.GetComponent<Animator>();
        DisplayMail();
        BackendGameData.Instance.GameDataGet();
    }

    private void Update()
    {
        /*if(stageani.GetBool("Stage"))
        {
            temp_time += Time.deltaTime;
            if(temp_time > serchingTime)
            {
                serching.SetActive(false);
            }

        }*/
    }

    void CreateRoom()
    {
        BackendMatchManager.GetInstance().CreateMatchRoom();
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

    void MissionAnimation()
    {
        if (Mission_ani.GetBool("check"))
        {
            Mission_ani.SetBool("check", false);
        }
        else
        {
            Mission_ani.SetBool("check", true);
        }
    }

    void shopAnimation()
    {
        if (shop_popup.GetBool("check"))
        {
            shop_popup.SetBool("check", false);
        }
        else
        {
            shop_popup.SetBool("check", true);
        }
    }

    void myinfoAnimation()
    {
        if (myinfo_popup.GetBool("check"))
        {
            myinfo_popup.SetBool("check", false);
        }
        else
        {
            myinfo_popup.SetBool("check", true);
        }
        Debug.Log(BackendGameData.Instance.userData);
        myNickname.text = BackendMatchManager.GetInstance().myNickName;
        
        levelInfo.text = BackendGameData.Instance.userData.level.ToString();
        goldInfo.text = BackendGameData.Instance.userData.gold.ToString();
        gemInfo.text = BackendGameData.Instance.userData.gem.ToString();
        winsInfo.text = BackendMatchManager.GetInstance().GetMyMatchRecord(0).win.ToString();
        loseInfo.text = (BackendMatchManager.GetInstance().GetMyMatchRecord(0).numOfMatch -
                         BackendMatchManager.GetInstance().GetMyMatchRecord(0).win).ToString();
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
            serching.SetActive(true);
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
        BackendMatchManager.GetInstance().RequestMatchMaking();
    }

    public void ButtonClick_SettingTitleSwap()
    {
        settingAnimation();
        titleAnimation();
    }

    public void ButtonClick_shopTitleSwap()
    {
        shopAnimation();
        titleAnimation();
    }

    public void ButtonClick_MissionTitleSwap()
    {
        MissionAnimation();
        titleAnimation();
        levelSlider.value = BackendGameData.Instance.userData.level;
        rankPointSlider.value = BackendGameData.Instance.userData.rankPoint;
        winSlider.value = BackendMatchManager.GetInstance().GetMyMatchRecord(0).win;
        matchSlider.value = BackendMatchManager.GetInstance().GetMyMatchRecord(0).numOfMatch;

        missionLevelText.text = BackendGameData.Instance.userData.level.ToString() + " / 10";
        missionRPText.text = BackendGameData.Instance.userData.rankPoint.ToString() + " / 100";
        missionWinsText.text = BackendMatchManager.GetInstance().GetMyMatchRecord(0).win + " / 10";
        missionMatchText.text = BackendMatchManager.GetInstance().GetMyMatchRecord(0).numOfMatch + " / 1";
    }

    public void ButtonClick_myInfoTitleSwap()
    {
        myinfoAnimation();
        titleAnimation();
    }

    public void ButtonClick_MatchMakingCancel()
    {
        titleAnimation();
        stageAnimation();
        Debug.Log("���⼭ ���� ����");
    }

    public void GameStartButton()
    {
        Debug.Log("�ӽ� ��ư �ϴ� �ΰ��� Ȯ���� ���� �� ��ư ���� ���Ѽ� �� �̵� �ϸ� �ɵ�");
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
        rank1Name.text = BackendRank.Instance.rankList[0].nickname;
        rank2Name.text = BackendRank.Instance.rankList[1].nickname;
        rank3Name.text = BackendRank.Instance.rankList[2].nickname;
        myRankName.text = BackendRank.Instance.myRank.nickname;

        rank1Score.text = BackendRank.Instance.rankList[0].score;
        rank2Score.text = BackendRank.Instance.rankList[1].score;
        rank3Score.text = BackendRank.Instance.rankList[2].score;
        myRankScore.text = BackendRank.Instance.myRank.score;

        myRankNum.text = BackendRank.Instance.myRank.rank;
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

    void DisplayMail()
    {
        for (int i = 0; i < BackendPost.Instance._postList.Count; i++)
        {
            GameObject newPost = Instantiate(postPrefab, null, false);
            newPost.transform.parent = postList.transform;
            Transform textInfo = newPost.transform.Find("Text_Info");
            Transform senderTransform = textInfo.transform.Find("Sender");
            Transform contentTransform = textInfo.transform.Find("Item Content");
            //Transform inDateTransform = newPost.transform.Find("Post InDate");
            TMP_Text senderText = senderTransform.GetComponent<TMP_Text>();
            TMP_Text contentText = contentTransform.GetComponent<TMP_Text>();
            //TMP_Text inDateText = inDateTransform.GetComponent<TMP_Text>();
            senderText.text = BackendPost.Instance._postList[i].author;
            contentText.text = BackendPost.Instance._postList[i].content;
            //DateTime dateTime = DateTime.Parse(BackendPost.Instance._postList[i].inDate, null, DateTimeStyles.RoundtripKind);
            //inDateText.text = dateTime.ToString("MM-dd");
        }
    }

}
