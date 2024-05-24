using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd;
using BackEnd.Tcp;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    
    const int START_COUNT = 5;
    
    private SessionId myPlayerIndex = SessionId.None;
    
    #region 플레이어
    public GameObject playerPool;
    public GameObject playerPrefeb;
    public int numOfPlayer = 0;
    public GameObject particle;
    private const int MAXPLAYER = 4;
    public int alivePlayer { get; set; }
    private Dictionary<SessionId, Player> players;
    public GameObject startPointObject;
    private List<Vector4> startingPoints;

    private Stack<SessionId> gameRecord;
    public delegate void PlayerDie(SessionId index);
    public PlayerDie dieEvent;
    #endregion
    
    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        InitializeGame();
        var matchInstance = BackendMatchManager.GetInstance();
        if (matchInstance == null)
        {
            return;
        }
    }
    
    public bool InitializeGame()
    {
        if (!playerPool)
        {
            Debug.Log("Player Pool Not Exist!");
            return false;
        }
        Debug.Log("게임 초기화 진행");
        gameRecord = new Stack<SessionId>();
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameResult += OnGameResult;
        myPlayerIndex = SessionId.None;
        SetPlayerAttribute();
        OnGameStart();
        return true;
    }
    
    public void SetPlayerAttribute()
    {
        // 시작점
        startingPoints = new List<Vector4>();

        int num = startPointObject.transform.childCount;
        for (int i = 0; i < num; ++i)
        {
            var child = startPointObject.transform.GetChild(i);
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            startingPoints.Add(point);
        }

        dieEvent += PlayerDieEvent;
    }

    private void PlayerDieEvent(SessionId index)
    {
        alivePlayer -= 1;
        players[index].gameObject.SetActive(false);
        
        gameRecord.Push(index);
        
        Debug.Log(string.Format("Player Die : " + players[index].GetNickName()));
        
        if (!BackendMatchManager.GetInstance().IsHost())
        {
            return;
        }
        
        if (alivePlayer <= 1)
        {
            MultiInGameUI.Instance.SetClearPopupTrigger();
            SendGameEndOrder();
        }
    }
    
    public void SendGameEndOrder()
    {
        // 게임 종료 전환 메시지는 호스트에서만 보냄
        Debug.Log("Make GameResult & Send Game End Order");
        foreach (SessionId session in BackendMatchManager.GetInstance().sessionIdList)
        {
            if (players[session].GetIsLive() && !gameRecord.Contains(session))
            {
                gameRecord.Push(session);
            }
        }
        GameEndMessage message = new GameEndMessage(gameRecord);
        BackendMatchManager.GetInstance().SendDataToInGame<GameEndMessage>(message);
    }
    
    
    public void SetPlayerInfo()
    {
        if (BackendMatchManager.GetInstance().sessionIdList == null)
        {
            // 현재 세션ID 리스트가 존재하지 않으면, 0.5초 후 다시 실행
            Invoke("SetPlayerInfo", 0.5f);
            return;
        }
        var gamers = BackendMatchManager.GetInstance().sessionIdList;
        int size = gamers.Count;
        if (size <= 0)
        {
            Debug.Log("No Player Exist!");
            return;
        }
        if (size > MAXPLAYER)
        {
            Debug.Log("Player Pool Exceed!");
            return;
        }
        
        Debug.Log(size);
        Debug.Log(GameManager.GetInstance().GetGameState());

        players = new Dictionary<SessionId, Player>();
        BackendMatchManager.GetInstance().SetPlayerSessionList(gamers);
        
        Debug.Log(gamers[0]);
        Debug.Log(gamers[1]);
        int index = 0;
        foreach (var sessionId in gamers)
        {
            GameObject player = Instantiate(playerPrefeb, new Vector3(startingPoints[index].x, startingPoints[index].y, startingPoints[index].z), Quaternion.identity, playerPool.transform);
            players.Add(sessionId, player.GetComponent<Player>());

            if (BackendMatchManager.GetInstance().IsMySessionId(sessionId))
            {
                myPlayerIndex = sessionId;
                players[sessionId].Initialize(true, myPlayerIndex, BackendMatchManager.GetInstance().GetNickNameBySessionId(sessionId), startingPoints[index].w);
            }
            else
            {
                players[sessionId].Initialize(false, sessionId, BackendMatchManager.GetInstance().GetNickNameBySessionId(sessionId), startingPoints[index].w);
            }
            index += 1;
        }
        Debug.Log("Num Of Current Player : " + size);

        // 스코어 보드 설정
        alivePlayer = size;
        //InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);

        if (BackendMatchManager.GetInstance().IsHost())
        {
            StartCoroutine("StartCount");
        }
    }
    
    public void OnGameStart()
    {
        if (BackendMatchManager.GetInstance().IsHost())
        {
            Debug.Log("플레이어 세션정보 확인");

            if (BackendMatchManager.GetInstance().IsSessionListNull())
            {
                Debug.Log("Player Index Not Exist!");
                // 호스트 기준 세션데이터가 없으면 게임을 바로 종료한다.
                foreach (var session in BackendMatchManager.GetInstance().sessionIdList)
                {
                    // 세션 순서대로 스택에 추가
                    gameRecord.Push(session);
                }
                //GameEndMessage gameEndMessage = new GameEndMessage(gameRecord);
                //BackendMatchManager.GetInstance().SendDataToInGame<GameEndMessage>(gameEndMessage);
                return;
            }
        }
        SetPlayerInfo();
    }
    
    IEnumerator StartCount()
    {
        StartCountMessage msg = new StartCountMessage(START_COUNT);

        // 카운트 다운
        for (int i = 0; i < START_COUNT + 1; ++i)
        {
            msg.time = START_COUNT - i;
            BackendMatchManager.GetInstance().SendDataToInGame<StartCountMessage>(msg);
            yield return new WaitForSeconds(1); //1초 단위
        }

        // 게임 시작 메시지를 전송
        GameStartMessage gameStartMessage = new GameStartMessage();
        BackendMatchManager.GetInstance().SendDataToInGame<GameStartMessage>(gameStartMessage);
    }
    
    public void OnGameOver()
    {
        Debug.Log("Game End");
        if (BackendMatchManager.GetInstance() == null)
        {
            Debug.LogError("매치매니저가 null 입니다.");
            return;
        }
        BackendMatchManager.GetInstance().MatchGameOver(gameRecord);
    }
    
    public void OnGameResult()
    {
        Debug.Log("Game Result");
        //BackEndMatchManager.GetInstance().LeaveInGameRoom();

        if (GameManager.GetInstance().IsLobbyScene())
        {
            GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);
        }
    }
    
    public void OnRecieve(MatchRelayEventArgs args)
    {
        if (args.BinaryUserData == null)
        {
            Debug.LogWarning(string.Format("빈 데이터가 브로드캐스팅 되었습니다.\n{0} - {1}", args.From, args.ErrInfo));
            // 데이터가 없으면 그냥 리턴
            return;
        }
        Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
        if (msg == null)
        {
            return;
        }
        if (BackendMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
        {
            return;
        }
        if (players == null)
        {
            Debug.LogError("Players 정보가 존재하지 않습니다.");
            return;
        }
        switch (msg.type)
        {
            case Protocol.Type.StartCount:
                StartCountMessage startCount = DataParser.ReadJsonData<StartCountMessage>(args.BinaryUserData);
                Debug.Log("wait second : " + (startCount.time));
                //InGameUiManager.GetInstance().SetStartCount(startCount.time);
                break;
            case Protocol.Type.GameStart:
                //InGameUiManager.GetInstance().SetStartCount(0, false);
                GameManager.GetInstance().ChangeState(GameManager.GameState.InGame);
                break;
            case Protocol.Type.GameEnd:
                GameEndMessage endMessage = DataParser.ReadJsonData<GameEndMessage>(args.BinaryUserData);
                SetGameRecord(endMessage.count, endMessage.sessionList);
                GameManager.GetInstance().ChangeState(GameManager.GameState.Over);
                break;
            case Protocol.Type.Key:
                KeyMessage keyMessage = DataParser.ReadJsonData<KeyMessage>(args.BinaryUserData);
                ProcessKeyEvent(args.From.SessionId, keyMessage);
                break;
            case Protocol.Type.PlayerMove:
                PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(moveMessage);
                break;
            case Protocol.Type.PlayerJump:
                PlayerJumpMessage jumpMessage = DataParser.ReadJsonData<PlayerJumpMessage>(args.BinaryUserData);
                ProcessPlayerData(jumpMessage);
                break;
            case Protocol.Type.PlayerAttack:
                PlayerAttackMessage attackMessage = DataParser.ReadJsonData<PlayerAttackMessage>(args.BinaryUserData);
                ProcessPlayerData(attackMessage);
                break;
            case Protocol.Type.PlayerDamaged:
                PlayerDamegedMessage damegedMessage = DataParser.ReadJsonData<PlayerDamegedMessage>(args.BinaryUserData);
                ProcessPlayerData(damegedMessage);
                break;
            case Protocol.Type.PlayerNoMove:
                PlayerNoMoveMessage noMoveMessage = DataParser.ReadJsonData<PlayerNoMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(noMoveMessage);
                break;
            case Protocol.Type.GameSync:
                GameSyncMessage syncMessage = DataParser.ReadJsonData<GameSyncMessage>(args.BinaryUserData);
                ProcessSyncData(syncMessage);
                break;
            default:
                Debug.Log("Unknown protocol type");
                return;
        }
    }
    
    public void OnRecieveForLocal(KeyMessage keyMessage)
    {
        ProcessKeyEvent(myPlayerIndex, keyMessage);
    }

    public void OnRecieveForLocal(PlayerNoMoveMessage message)
    {
        ProcessPlayerData(message);
    }

    private void ProcessKeyEvent(SessionId index, KeyMessage keyMessage)
    {
        if (BackendMatchManager.GetInstance().IsHost() == false)
        {
            //호스트만 수행
            return;
        }
        bool isMove = false;
        bool isAttack = false;
        bool isNoMove = false;
        bool isJump = false;

        int keyData = keyMessage.keyData;

        Vector3 moveVecotr = Vector3.zero;
        Vector3 attackPos = Vector3.zero;
        Vector3 playerPos = players[index].GetPosition();
        if ((keyData & KeyEventCode.MOVE) == KeyEventCode.MOVE)
        {
            moveVecotr = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
            moveVecotr = Vector3.Normalize(moveVecotr);
            isMove = true;
        }
        if ((keyData & KeyEventCode.ATTACK) == KeyEventCode.ATTACK)
        {
            attackPos = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
            //players[index].Attack(attackPos);
            isAttack = true;
        }

        if ((keyData & KeyEventCode.JUMP) == KeyEventCode.JUMP)
        {
            players[index].Jump();
            isJump = true;
        }

        if ((keyData & KeyEventCode.NO_MOVE) == KeyEventCode.NO_MOVE)
        {
            isNoMove = true;
        }

        if (isMove)
        {
            players[index].SetMoveVector(moveVecotr);
            PlayerMoveMessage msg = new PlayerMoveMessage(index, playerPos, moveVecotr);
            BackendMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);
        }
        if (isNoMove)
        {
            PlayerNoMoveMessage msg = new PlayerNoMoveMessage(index, playerPos);
            BackendMatchManager.GetInstance().SendDataToInGame<PlayerNoMoveMessage>(msg);
        }
        if (isAttack)
        {
            PlayerAttackMessage msg = new PlayerAttackMessage(index, attackPos);
            BackendMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);
        }

        if (isJump)
        {
            PlayerJumpMessage msg = new PlayerJumpMessage(index);
            BackendMatchManager.GetInstance().SendDataToInGame<PlayerJumpMessage>(msg);
        }
    }

    private void ProcessAttackKeyData(SessionId session, Vector3 pos)
    {
        //players[session].Attack(pos);
        //PlayerAttackMessage msg = new PlayerAttackMessage(session, pos);
        //BackendMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);
    }

    private void ProcessPlayerData(PlayerMoveMessage data)
    {
        if (BackendMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴
            return;
        }
        Vector3 moveVecotr = new Vector3(data.xDir, data.yDir, data.zDir);
        // moveVector가 같으면 방향 & 이동량 같으므로 적용 굳이 안함
        if (!moveVecotr.Equals(players[data.playerSession].moveVector))
        {
            players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
            players[data.playerSession].SetMoveVector(moveVecotr);
        }
    }
    private void ProcessPlayerData(PlayerNoMoveMessage data)
    {
        players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
        players[data.playerSession].SetMoveVector(Vector3.zero);
    }

    private void ProcessPlayerData(PlayerJumpMessage data)
    {
        if (BackendMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴
            return;
        }
        players[data.playerSession].Jump();
    }
    private void ProcessPlayerData(PlayerAttackMessage data)
    {
        if (BackendMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴
            return;
        }
        //players[data.playerSession].Attack(new Vector3(data.dir_x, data.dir_y, data.dir_z));
    }
    private void ProcessPlayerData(PlayerDamegedMessage data)
    {
        //players[data.playerSession].Damaged();
        //EffectManager.instance.EnableEffect(data.hit_x, data.hit_y, data.hit_z);
    }
    
    private void ProcessSyncData(GameSyncMessage syncMessage)
    {
        // 플레이어 데이터 동기화
        int index = 0;
        if (players == null)
        {
            Debug.LogError("Player Poll is null!");
            return;
        }
        foreach (var player in players)
        {
            var y = player.Value.GetPosition().y;
            player.Value.SetPosition(new Vector3(syncMessage.xPos[index], y, syncMessage.zPos[index]));
            //player.Value.SetHP(syncMessage.hpValue[index]);
            index++;
        }
        BackendMatchManager.GetInstance().SetHostSession(syncMessage.host);
    }
    
    public bool IsMyPlayerMove()
    {
        return players[myPlayerIndex].isMove;
    }
    
    public Vector3 GetMyPlayerPos()
    {
        return players[myPlayerIndex].GetPosition();
    }
    
    private void SetGameRecord(int count, int[] arr)
    {
        gameRecord = new Stack<SessionId>();
        // 스택에 넣어야 하므로 제일 뒤에서 부터 스택에 push
        for (int i = count - 1; i >= 0; --i)
        {
            gameRecord.Push((SessionId)arr[i]);
        }
    }
}
