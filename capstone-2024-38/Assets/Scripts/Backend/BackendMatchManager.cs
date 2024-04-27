using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using System.Linq;


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
    
    public class KeyMessage
    {
        public Type type;

        public KeyMessage(Type type)
        {
            this.type = type;
        }
    }

    
    
    void Awake()
    {
        // 인스턴스가 하나만 존재하게 함
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public static BackendMatchManager GetInstnace()
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
    }

    public bool IsHost()
    {
        return isHost;
    }

    public bool IsMySeesionId(SessionId session)
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
        };

        Backend.Match.OnMatchInGameAccess += (args) =>
        {
            Debug.Log("OnMatchInGameAccess : " + args.ErrInfo);
        };

        Backend.Match.OnMatchInGameStart += () =>
        {
            GameSetUp();
        };
    }

    private void ExceptionHandler()
    {
        
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
    void Update()
    {
        if (isConnectMatchServer || isConnectInGameServer)
        {
            Backend.Match.Poll();
        }
    }
}























