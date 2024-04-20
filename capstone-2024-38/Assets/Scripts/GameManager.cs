using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BackEnd;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static bool isCreate = false;
    public GameObject nickNameWindow;
    
    #region Scene
    private const string LOGIN = "0. Login";
    private const string LOBBY = "1. MatchLobby";
    private const string READY = "2. LoadRoom";
    private const string INGAME = "3. InGame";
    #endregion
    
    public enum GameState { Login, MatchLobby, Ready, Start, InGame, Over, Result, Reconnect };
    private GameState gameState;
    
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("GameManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }
    
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        // 60프레임 고정
        Application.targetFrameRate = 60;
        // 게임중 슬립모드 해제
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (isCreate)
        {
            DestroyImmediate(gameObject, true);
            return;
        }
        gameState = GameState.Login;
        isCreate = true;
    }

    public void Login()
    {
        
    }

    public void Lobby()
    {
        if (Backend.UserNickName == "")
        {
            nickNameWindow.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("LobyScene");
        }
    }

    public void GameReady()
    {
        
    }

    public void GameStart()
    {
        
    }

    public void GameOver()
    {
        
    }

    public void GameResult()
    {
        
    }
    
    public void ChangeState(GameState state, Action<bool> func = null)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.Login:
                Login();
                break;
            case GameState.MatchLobby:
                Lobby();
                break;
            case GameState.Ready:
                GameReady();
                break;
            case GameState.Start:
                GameStart();
                break;
            case GameState.Over:
                GameOver();
                break;
            case GameState.Result:
                GameResult();
                break;
            case GameState.InGame:
                // 코루틴 시작
                //StartCoroutine(InGameUpdateCoroutine);
                break;
            case GameState.Reconnect:
                //();
                break;
            default:
                Debug.Log("알수없는 스테이트입니다. 확인해주세요.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
