using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine.SceneManagement;

public partial class BackendMatchManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isSetHost = false;                 // 호스트 세션 결정했는지 여부

    private MatchGameResult matchGameResult;

    // 게임 로그
    private string FAIL_ACCESS_INGAME = "인게임 접속 실패 : {0} - {1}";
    private string SUCCESS_ACCESS_INGAME = "유저 인게임 접속 성공 : {0}";
    private string NUM_INGAME_SESSION = "인게임 내 세션 갯수 : {0}";
    
    // 게임 레디 상태일 때 호출됨
    public void AccessInGameRoom(string roomToken)
    {
        Backend.Match.JoinGameRoom(roomToken);
    }
    
    // 인 게임 서버 접속 종료
    public void LeaveInGameRoom()
    {
        isConnectInGameServer = false;
        Backend.Match.LeaveGameServer();
    }

    public void GameSetUp()
    {
        Debug.Log("게임 시작 메시지 수신. 게임 설정 시작");
        Debug.Log(GameManager.GetInstance().GetGameState());
        if (GameManager.GetInstance().GetGameState() != GameManager.GameState.Ready)
        {
            isHost = false;
            isSetHost = false;
            OnGameReady();
            //LobbyUI.GetInstance().ChangeRoomLoadScene();
        }
    }

    public void OnGameReady()
    {
        if (isSetHost == false)
        {
            isSetHost = SetHostSession();
        }
        Debug.Log("호스트 설정 완료");

        if (isSandBoxGame == true && IsHost() == true)
        {
            //SetAIPlayer();
        }

        if (IsHost() == true)
        {
            // 0.5초 후 ReadyToLoadRoom 함수 호출
            Invoke("ReadyToLoadRoom", 0.5f);
        }
    }
    
    // 현재 룸에 접속한 세션들의 정보
    // 최초 룸에 접속했을 때 1회 수신됨
    // 재접속 했을 때도 1회 수신됨
    private void ProcessMatchInGameSessionList(MatchInGameSessionListEventArgs args)
    {
        sessionIdList = new List<SessionId>();
        gameRecords = new Dictionary<SessionId, MatchUserGameRecord>();

        foreach (var record in args.GameRecords)
        {
            sessionIdList.Add(record.m_sessionId);
            gameRecords.Add(record.m_sessionId, record);
        }
        sessionIdList.Sort();
        Debug.Log(sessionIdList);
    }
    
    // 클라이언트 들의 게임 룸 접속에 대한 리턴값
    // 클라이언트가 게임 룸에 접속할 때마다 호출됨
    // 재접속 했을 때는 수신되지 않음
    private void ProcessMatchInGameAccess(MatchInGameSessionEventArgs args)
    {
        if (isReconnectProcess)
        {
            // 재접속 프로세스 인 경우
            // 이 메시지는 수신되지 않고, 만약 수신되어도 무시함
            Debug.Log("재접속 프로세스 진행중... 재접속 프로세스에서는 ProcessMatchInGameAccess 메시지는 수신되지 않습니다.\n" + args.ErrInfo);
            return;
        }

        Debug.Log(string.Format(SUCCESS_ACCESS_INGAME, args.ErrInfo));

        if (args.ErrInfo != ErrorCode.Success)
        {
            // 게임 룸 접속 실패
            var errorLog = string.Format(FAIL_ACCESS_INGAME, args.ErrInfo, args.Reason);
            Debug.Log(errorLog);
            LeaveInGameRoom();
            return;
        }

        // 게임 룸 접속 성공
        // 인자값에 방금 접속한 클라이언트(세션)의 세션ID와 매칭 기록이 들어있다.
        // 세션 정보는 누적되어 들어있기 때문에 이미 저장한 세션이면 건너뛴다.

        var record = args.GameRecord;
        Debug.Log(string.Format(string.Format("인게임 접속 유저 정보 [{0}] : {1}", args.GameRecord.m_sessionId, args.GameRecord.m_nickname)));
        if (!sessionIdList.Contains(args.GameRecord.m_sessionId))
        {
            // 세션 정보, 게임 기록 등을 저장
            sessionIdList.Add(record.m_sessionId);
            gameRecords.Add(record.m_sessionId, record);

            Debug.Log(string.Format(NUM_INGAME_SESSION, sessionIdList.Count));
        }
    }
    
    public bool PrevGameMessage(byte[] BinaryUserData)
    {
        Protocol.Message msg = DataParser.ReadJsonData<Protocol.Message>(BinaryUserData);
        if (msg == null)
        {
            return false;
        }

        // 게임 설정 사전 작업 패킷 검사 
        switch (msg.type)
        {
            /*case Protocol.Type.AIPlayerInfo:
                Protocol.AIPlayerInfo aiPlayerInfo = DataParser.ReadJsonData<Protocol.AIPlayerInfo>(BinaryUserData);
                ProcessAIDate(aiPlayerInfo);
                return true;*/
            case Protocol.Type.LoadRoomScene:
                GameManager.GetInstance().ChangeState(GameManager.GameState.Ready);
                if (IsHost() == true)
                {
                    Debug.Log("5초 후 게임 씬 전환 메시지 송신");
                    Invoke("SendChangeGameScene", 5f);
                }
                return true;
            case Protocol.Type.LoadGameScene:
                GameManager.GetInstance().ChangeState(GameManager.GameState.Start);
                Debug.Log("SIUU");
                return true;
        }
        return false;
    }

    private void ReadyToLoadRoom()
    {
        /*if (BackendMatchManager.GetInstance().isSandBoxGame == true)
        {
            Debug.Log("샌드박스 모드 활성화. AI 정보 송신");
            // 샌드박스 모드면 ai 정보 송신
            foreach (var tmp in gameRecords)
            {
                if ((int)tmp.Key > (int)SessionId.Reserve)
                {
                    continue;
                }
                Debug.Log("ai정보 송신 : " + (int)tmp.Key);
                SendDataToInGame(new Protocol.AIPlayerInfo(tmp.Value));
            }
        }*/
        
        Debug.Log("1초 후 룸 씬 전환 메시지 송신");
        Invoke("SendChangeRoomScene", 1f);
    }
    
    public void SetPlayerSessionList(List<SessionId> sessions)
    {
        sessionIdList = sessions;
    }
    
    private void SendChangeRoomScene()
    {
        Debug.Log("룸 씬 전환 메시지 송신");
        SendDataToInGame(new Protocol.LoadRoomSceneMessage());
    }

    private void SendChangeGameScene()
    {
        Debug.Log("게임 씬 전환 메시지 송신");
        SendDataToInGame(new Protocol.LoadGameSceneMessage());
    }
    
    // 서버로 게임 결과 전송
    // 서버에서 각 클라이언트가 보낸 결과를 종합
    public void MatchGameOver(Stack<SessionId> record)
    {
        if (nowModeType == MatchModeType.Melee)
        {
            matchGameResult = MeleeRecord(record);
        }

        if (nowModeType == MatchModeType.OneOnOne)
        {
            matchGameResult = OneOnOneRecord(record);
        }
        else
        {
            Debug.LogError("게임 결과 종합 실패 - 알수없는 매치모드타입입니다.\n" + nowModeType);
            return;
        }

        //MatchResultUI.GetInstance().SetGameResult(matchGameResult);
        //RemoveAISessionInGameResult();
        Backend.Match.MatchEnd(matchGameResult);
    }
    
    private MatchGameResult MeleeRecord(Stack<SessionId> record)
    {
        MatchGameResult nowGameResult = new MatchGameResult();
        nowGameResult.m_draws = null;
        nowGameResult.m_losers = null;
        nowGameResult.m_winners = new List<SessionId>();
        int size = record.Count;
        for (int i = 0; i < size; ++i)
        {
            nowGameResult.m_winners.Add(record.Pop());
        }

        return nowGameResult;
    }
    
    private MatchGameResult OneOnOneRecord(Stack<SessionId> record)
    {
        MatchGameResult nowGameResult = new MatchGameResult();

        nowGameResult.m_winners = new List<SessionId>();
        nowGameResult.m_winners.Add(record.Pop());

        nowGameResult.m_losers = new List<SessionId>();
        nowGameResult.m_losers.Add(record.Pop());

        nowGameResult.m_draws = null;

        return nowGameResult;
    }
    
    

    public void SendDataToInGame<T>(T msg)
    {
        var byteArray = DataParser.DataToJsonData<T>(msg);
        Backend.Match.SendDataToInGameRoom(byteArray);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
