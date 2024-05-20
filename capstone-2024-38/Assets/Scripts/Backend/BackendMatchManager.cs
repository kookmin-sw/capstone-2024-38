using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using System.Linq;
using Protocol;


public partial class BackendMatchManager : MonoBehaviour
{
    // 콘솔에서 생성한 매칭 카드 정보
    public class MatchInfo
    {
        public string title;                // 매칭 명
        public string inDate;               // 매칭 inDate (UUID)
        public MatchType matchType;         // 매치 타입
        public MatchModeType matchModeType;    // 매치 모드 타입
        public string headCount;            // 매칭 인원
        public bool isSandBoxEnable;        // 샌드박스 모드 (AI매칭)
    }

    private static BackendMatchManager instance = null;

    public List<MatchInfo> matchInfos { get; private set; } = new List<MatchInfo>(); // 콘솔에서 생성한 매칭 카드들의 리스트
    
    public List<SessionId> sessionIdList { get; private set; } // 매치에 참가중인 유저들의 세션 목록

    public Dictionary<SessionId, MatchUserGameRecord> gameRecords { get; private set; } = null;  // 매치에 참가중인 유저들의 매칭 기록
    private string inGameRoomToken = string.Empty;  // 게임 룸 토큰 (인게임 접속 토큰)
    public SessionId hostSession { get; private set; }  // 호스트 세션
    private ServerInfo roomInfo = null;             // 게임 룸 정보
    public bool isReconnectEnable { get; private set; } = false;

    public bool isConnectMatchServer { get; private set; } = false;
    private bool isConnectInGameServer = false;
    private bool isJoinGameRoom = false;
    public bool isReconnectProcess { get; private set; } = false;
    public bool isSandBoxGame { get; private set; } = false;

    private int numOfClient = 2;    
    
    #region Host
    private bool isHost = false;                    // 호스트 여부 (서버에서 설정한 SuperGamer 정보를 가져옴)
    private Queue<KeyMessage> localQueue = null;    // 호스트에서 로컬로 처리하는 패킷을 쌓아두는 큐 (로컬처리하는 데이터는 서버로 발송 안함)
    #endregion
    
    
    void Awake()
    {
        // 인스턴스가 하나만 존재하게 함
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static BackendMatchManager GetInstance()
    {
        if (!instance)
        {
            Debug.Log("인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
    }

    void OnApplicationQuit()
    {
        if (isConnectMatchServer)
        {
            LeaveMatchServer();
            Debug.Log("ApplicationQuit - LeaveMatchServer");
        }
    }
    
    void Start()
    {
        MatchMakingHandler();
        GetMatchList();
        GameHandler();
    }

    public bool IsHost()
    {
        return isHost;
    }

    public bool IsMySessionId(SessionId session)
    {
        return Backend.Match.GetMySessionId() == session;
    }

    public string GetNickNameBySessionId(SessionId session)
    {
        //return Backend.Match.GetNickNameBySessionId(session);
        return gameRecords[session].m_nickname;
    }

    public bool IsSessionListNull()
    {
        return sessionIdList == null || sessionIdList.Count == 0;
    }

    private bool SetHostSession()
    {
        // 호스트 세션 정하기
        // 각 클라이언트가 모두 수행 ( 호스트 세션 정하는 로직은 모두 같으므로 각각의 클라이언트가 모두 로직을 수행하지만 결과값은 같다.)
        
        Debug.Log("호스트 세션 설정 진입");
        // 호스트 세션 정렬 (각 클라이언트마다 입장 순서가 다를 수 있기 때문에 정렬)
        sessionIdList.Sort();
        isHost = false;
        // 내가 호스트 세션인지
        foreach (var record in gameRecords)
        {
            if (record.Value.m_isSuperGamer == true)
            {
                if (record.Value.m_sessionId.Equals(Backend.Match.GetMySessionId()))
                {
                    isHost = true;
                }

                hostSession = record.Value.m_sessionId;
                break;
            }
        }
        
        Debug.Log("호스트 여부 : "+ isHost);
        
        // 호스트 세션이면 로컬에서 처리하는 패킷이 있으므로 로컬 큐를 생성해준다
        if (isHost)
        {
            localQueue = new Queue<KeyMessage>();
        }
        else
        {
            localQueue = null;
        }
        
        // 호스트 설정까지 끝나면 매치서버와 접속 끊음
        LeaveMatchServer();
        return true;
    }
    
    private void SetSubHost(SessionId hostSessionId)
    {
        Debug.Log("서브 호스트 세션 설정 진입");
        // 누가 서브 호스트 세션인지 서버에서 보낸 정보값 확인
        // 서버에서 보낸 SuperGamer 정보로 GameRecords의 SuperGamer 정보 갱신
        foreach (var record in gameRecords)
        {
            if (record.Value.m_sessionId.Equals(hostSessionId))
            {
                record.Value.m_isSuperGamer = true;
            }
            else
            {
                record.Value.m_isSuperGamer = false;
            }
        }
        // 내가 호스트 세션인지 확인
        if (hostSessionId.Equals(Backend.Match.GetMySessionId()))
        {
            isHost = true;
        }
        else
        {
            isHost = false;
        }

        hostSession = hostSessionId;

        Debug.Log("서브 호스트 여부 : " + isHost);
        // 호스트 세션이면 로컬에서 처리하는 패킷이 있으므로 로컬 큐를 생성해준다
        if (isHost)
        {
            localQueue = new Queue<KeyMessage>();
        }
        else
        {
            localQueue = null;
        }

        Debug.Log("서브 호스트 설정 완료");
    }
    
    public void AddMsgToLocalQueue(KeyMessage message)
    {
        // 로컬 큐에 메시지 추가
        if (isHost == false || localQueue == null)
        {
            return;
        }

        localQueue.Enqueue(message);
    }
    
    public void SetHostSession(SessionId host)
    {
        hostSession = host;
    }
    

    private void MatchMakingHandler()
    {
        Backend.Match.OnJoinMatchMakingServer += (args) =>
        {
            Debug.Log("OnJoinMatchMakingServer : " + args.ErrInfo);
            ProcessAccessMatchMakingServer(args.ErrInfo);
        };

        Backend.Match.OnMatchMakingRoomCreate += (args) =>
        {
            Debug.Log(string.Format("OnMatchMakingRoomJoin : {0} : {1}", args.ErrInfo, args.Reason));
        };

        Backend.Match.OnMatchMakingResponse += (args) =>
        {
            Debug.Log("OnMatchMakingResponse : " + args.ErrInfo + " : " + args.Reason);
            ProcessMatchMakingResponse(args);
        };
    }

    private void GameHandler()
    {
        Backend.Match.OnSessionJoinInServer += (args) =>
        {
            Debug.Log("OnSessionJoinInServer : " + args.ErrInfo);
            
            if (isJoinGameRoom)
            {
                return;
            }

            if (inGameRoomToken == string.Empty)
            {
                Debug.LogError("인게임 서버 접속 성공했으나 룸 토큰이 없습니다.");
                return;
            }
            
            Debug.Log("인게임 서버 접속 성공");
            isJoinGameRoom = true;
            AccessInGameRoom(inGameRoomToken);
        };

        Backend.Match.OnSessionListInServer += (args) =>
        {
            Debug.Log("OnSessionListInServer : " + args.ErrInfo);
            
            ProcessMatchInGameSessionList(args);
        };

        Backend.Match.OnMatchInGameAccess += (args) =>
        {
            Debug.Log("OnMatchInGameAccess : " + args.ErrInfo);
            
            ProcessMatchInGameAccess(args);
        };

        Backend.Match.OnMatchInGameStart += () =>
        {
            GameSetUp();
        };
        
        Backend.Match.OnMatchResult += (args) =>
        {
            Debug.Log("게임 결과값 업로드 결과 : " + string.Format("{0} : {1}", args.ErrInfo, args.Reason));
            // 서버에서 게임 결과 패킷을 보내면 호출
            // 내가(클라이언트가) 서버로 보낸 결과값이 정상적으로 업데이트 되었는지 확인

            if (args.ErrInfo == BackEnd.Tcp.ErrorCode.Success)
            {
                //InGameUiManager.instance.SetGameResult();
                GameManager.GetInstance().ChangeState(GameManager.GameState.Result);
            }
            else if (args.ErrInfo == BackEnd.Tcp.ErrorCode.Match_InGame_Timeout)
            {
                Debug.Log("게임 입장 실패 : " + args.ErrInfo);
                //LobbyUI.GetInstance().MatchCancelCallback();
            }
            else
            {
                //InGameUiManager.instance.SetGameResult("결과 종합 실패\n호스트와 연결이 끊겼습니다.");
                Debug.Log("게임 결과 업로드 실패 : " + args.ErrInfo);
            }
            // 세션리스트 초기화
            sessionIdList = null;
        };
        
        Backend.Match.OnMatchRelay += (args) =>
        {
            // 각 클라이언트들이 서버를 통해 주고받은 패킷들
            // 서버는 단순 브로드캐스팅만 지원 (서버에서 어떠한 연산도 수행하지 않음)

            // 게임 사전 설정
            if (PrevGameMessage(args.BinaryUserData) == true)
            {
                // 게임 사전 설정을 진행하였으면 바로 리턴
                return;
            }
            if (WorldManager.instance == null)
            {
                // 월드 매니저가 존재하지 않으면 바로 리턴
                return;
            }

            WorldManager.instance.OnRecieve(args);
        };
        
        Backend.Match.OnLeaveInGameServer += (args) =>
        {
            Debug.Log("OnLeaveInGameServer : " + args.ErrInfo + " : " + args.Reason);
            if (args.Reason.Equals("Fail To Reconnect"))
            {
                JoinMatchServer();
            }
            isConnectInGameServer = false;
        };
    }

    private void ExceptionHandler()
    {
        
    }
    
    void Update()
    {
        if (isConnectInGameServer || isConnectMatchServer)
        {
            Backend.Match.Poll();

            // 호스트의 경우 로컬 큐가 존재
            // 큐에 있는 패킷을 로컬에서 처리
            if (localQueue != null)
            {
                while (localQueue.Count > 0)
                {
                    var msg = localQueue.Dequeue();
                    WorldManager.instance.OnRecieveForLocal(msg);
                }
            }
        }
    }

    public void GetMatchList()
    {
        // 매칭 카드 정보 초기화
        matchInfos.Clear();

        var callback = Backend.Match.GetMatchList();

        if (!callback.IsSuccess())
        {
            Debug.Log("Backend.Match.GetMatchList Error : " + callback);
        }

        LitJson.JsonData row = callback.FlattenRows();
        Debug.Log("Backend.Match.GetMatchList : " + callback);

        for (int i = 0; i < row.Count; i++)
        {
            MatchInfo matchInfo = new MatchInfo();
            matchInfo.title = row[i]["matchTitle"].ToString();
            matchInfo.inDate = row[i]["inDate"].ToString();
            matchInfo.headCount = row[i]["matchHeadCount"].ToString();
            matchInfo.isSandBoxEnable = row[i]["enable_sandbox"].ToString().Equals("True") ? true : false;

            foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
            {
                if (type.ToString().ToLower().Equals(row[i]["matchType"].ToString().ToLower()))
                {
                    matchInfo.matchType = type;
                }
            }

            foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
            {
                if (type.ToString().ToLower().Equals(row[i]["matchModeType"].ToString().ToLower()))
                {
                    matchInfo.matchModeType = type;
                }
            }
            matchInfos.Add(matchInfo);
        }

        foreach (var matchInfo in row)
        {
            Debug.Log(matchInfo.ToString());
            Debug.Log(matchInfos[0].title);
        }
    }
    
    public MatchInfo GetMatchInfo(string indate)
    {
        var result = matchInfos.FirstOrDefault(x => x.inDate == indate);
        if (result.Equals(default(MatchInfo)) == true)
        {
            return null;
        }
        return result;
    }
    
    public void GetMyMatchRecord(int index, Action<MatchRecord, bool> func)
    {
        var inDate = myIndate;

        SendQueue.Enqueue(Backend.Match.GetMatchRecord, inDate, matchInfos[index].matchType, matchInfos[index].matchModeType, matchInfos[index].inDate, callback =>
        {
            MatchRecord record = new MatchRecord();
            record.matchTitle = matchInfos[index].title;
            record.matchType = matchInfos[index].matchType;
            record.modeType = matchInfos[index].matchModeType;

            if (!callback.IsSuccess())
            {
                Debug.LogError("매칭 기록 조회 실패\n" + callback);
                func(record, false);
                return;
            }

            if (callback.Rows().Count <= 0)
            {
                Debug.Log("매칭 기록이 존재하지 않습니다.\n" + callback);
                func(record, true);
                return;
            }
            var data = callback.Rows()[0];
            var win = Convert.ToInt32(data["victory"]["N"].ToString());
            var draw = Convert.ToInt32(data["draw"]["N"].ToString());
            var defeat = Convert.ToInt32(data["defeat"]["N"].ToString());
            var numOfMatch = win + draw + defeat;
            string point = string.Empty;
            if (matchInfos[index].matchType == MatchType.MMR)
            {
                point = data["mmr"]["N"].ToString();
            }
            else if (matchInfos[index].matchType == MatchType.Point)
            {
                point = data["point"]["N"].ToString() + " P";
            }
            else
            {
                point = "-";
            }

            record.win = win;
            record.numOfMatch = numOfMatch;
            record.winRate = Math.Round(((float)win / numOfMatch) * 100 * 100) / 100;
            record.score = point;

            func(record, true);
        });
    }
    
    public class ServerInfo
    {
        public string host;
        public ushort port;
        public string roomToken;
    }
    
    public class MatchRecord
    {
        public MatchType matchType;
        public MatchModeType modeType;
        public string matchTitle;
        public string score = "-";
        public int win = -1;
        public int numOfMatch = 0;
        public double winRate = 0;
    }
    
}
