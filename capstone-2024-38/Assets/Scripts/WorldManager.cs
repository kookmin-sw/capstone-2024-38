using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd;
using BackEnd.Tcp;

public class WorldManager : MonoBehaviour
{
    static public WorldManager instance;
    
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
    private List<Vector4> statringPoints;

    private Stack<SessionId> gameRecord;
    public delegate void PlayerDie(SessionId index);
    public PlayerDie dieEvent;
    #endregion
    
    void Awake()
    {
        instance = this;
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
            /*case Protocol.Type.StartCount:
                StartCountMessage startCount = DataParser.ReadJsonData<StartCountMessage>(args.BinaryUserData);
                Debug.Log("wait second : " + (startCount.time));
                InGameUiManager.GetInstance().SetStartCount(startCount.time);
                break;
            case Protocol.Type.GameStart:
                InGameUiManager.GetInstance().SetStartCount(0, false);
                GameManager.GetInstance().ChangeState(GameManager.GameState.InGame);
                break;
            case Protocol.Type.GameEnd:
                GameEndMessage endMessage = DataParser.ReadJsonData<GameEndMessage>(args.BinaryUserData);
                SetGameRecord(endMessage.count, endMessage.sessionList);
                GameManager.GetInstance().ChangeState(GameManager.GameState.Over);
                break;*/

            case Protocol.Type.Key:
                KeyMessage keyMessage = DataParser.ReadJsonData<KeyMessage>(args.BinaryUserData);
                ProcessKeyEvent(args.From.SessionId, keyMessage);
                break;
            case Protocol.Type.PlayerMove:
                PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(moveMessage);
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
}
