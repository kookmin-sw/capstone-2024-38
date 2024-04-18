using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd.Tcp;

public class Protocol : MonoBehaviour
{
    // 이벤트 타입
    public enum Type : sbyte
    {
        Key = 0,        // 키(가상 조이스틱) 입력
        PlayerMove,     // 플레이어 이동
        PlayerRotate,   // 플레이어 회전
        PlayerAttack,   // 플레이어 공격
        PlayerDamaged,  // 플레이어 피격
        PlayerNoMove,   // 플레이어 이동 정지
        PlayerNoRotate, // 플레이어 회전 정지
        bulletInfo,
        
        AIPlayerInfo,   // AI가 존재하는 경우 AI 정보
        LoadRoomScene,  // 룸 씬으로 전환
        LoadGameScene,  // 인게임 씬으로 전환
        StartCount,     // 시작 카운트
        GameStart,      // 게임 시작
        GameEnd,        // 게임 종료
        GameSync,       // 플레이어 재접속 시 현재 게임 상황 싱크
        Max
    }

    public static class KeyEventCode
    {
        public const int NONE = 0;
        public const int MOVE = 1;      // 이동 메시지
        public const int ATTACK = 2;    // 공격 메시지
        public const int NO_MOVE = 4;   // 정지 메시지
    }

    public class Message
    {
        public Type type;

        public Message(Type type)
        {
            this.type = type;
        }
    }

    public class KeyMessage : Message
    {
        public int keyData;
        public float x;
        public float y;
        public float z;

        public KeyMessage(int data, Vector3 pos) : base(Type.Key)
        {
            this.keyData = data;
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xDir;
        public float yDir;
        public float zDir;

        public PlayerMoveMessage(SessionId session, Vector3 pos, Vector3 dir) : base(Type.PlayerMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
            this.xDir = dir.x;
            this.yDir = dir.y;
            this.zDir = dir.z;
        }
    }

    public class PlayerAttackMessage : Message
    {
        public SessionId playerSession;
        public float dir_x;
        public float dir_y;
        public float dir_z;
        public PlayerAttackMessage(SessionId session, Vector3 pos) : base(Type.PlayerAttack)
        {
            this.playerSession = session;
            dir_x = pos.x;
            dir_y = pos.y;
            dir_z = pos.z;
        }
    }

    public class PlayerDamegedMessage : Message
    {
        public SessionId playerSession;
        public float hit_x;
        public float hit_y;
        public float hit_z;
        public PlayerDamegedMessage(SessionId session, float x, float y, float z) : base(Type.PlayerDamaged)
        {
            this.playerSession = session;
            this.hit_x = x;
            this.hit_y = y;
            this.hit_z = z;
        }
    }

    public class PlayerNoMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public PlayerNoMoveMessage(SessionId session, Vector3 pos) : base(Type.PlayerNoMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
        }
    }
    
    public class LoadRoomSceneMessage : Message
    {
        public LoadRoomSceneMessage() : base(Type.LoadRoomScene)
        {

        }
    }

    public class LoadGameSceneMessage : Message
    {
        public LoadGameSceneMessage() : base(Type.LoadGameScene)
        {

        }
    }
}


























