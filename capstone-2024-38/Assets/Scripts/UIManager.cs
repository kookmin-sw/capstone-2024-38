using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;


public class UIManager : MonoBehaviour
{
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    private static UIManager _instance = null;
    public static UIManager Instance {
        get {
            if(_instance == null) {
                _instance = new UIManager();
            }

            return _instance;
        }
    }
    
    public Button postRecieveButton;
    
    public GameObject postPrefab;
    public GameObject postList;

    //public List<Post> _postList;
    
    
    // Start is called before the first frame update
    void Start()
    {
        BackendPost.Instance.PostListCompleted += DisplayPosts;
        BackendPost.Instance.PostListGet(PostType.Admin);
        postRecieveButton.onClick.AddListener(OnButtonClicked);
    }
    
    public void DisplayPosts()
    {
        Debug.Log(BackendPost.Instance._postList.Count);
        for (int i = 0; i < BackendPost.Instance._postList.Count; i++)
        {
            GameObject newPost = Instantiate(postPrefab, null, false);
            newPost.transform.parent = postList.transform;
            Transform senderTransform = newPost.transform.Find("Sender");
            Transform contentTransform = newPost.transform.Find("Item Content");
            Transform inDateTransform = newPost.transform.Find("Post InDate");
            TMP_Text senderText = senderTransform.GetComponent<TMP_Text>();
            TMP_Text contentText = contentTransform.GetComponent<TMP_Text>();
            TMP_Text inDateText = inDateTransform.GetComponent<TMP_Text>();
            senderText.text = BackendPost.Instance._postList[i].author;
            contentText.text = BackendPost.Instance._postList[i].content;
            DateTime dateTime = DateTime.Parse(BackendPost.Instance._postList[i].inDate, null, DateTimeStyles.RoundtripKind);
            inDateText.text = dateTime.ToString("MM-dd");
        }
    }


    void OnButtonClicked()
    {
        BackendPost.Instance.PostReceiveAll(PostType.Admin);
    }
}
