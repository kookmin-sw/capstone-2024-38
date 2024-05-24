using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd.Tcp;
using BackEnd;
using Cinemachine;

public class Player : MonoBehaviour
{
    private SessionId index = 0;
    private string nickName = string.Empty;
    private bool isMe = false;
    
    // 스테이터스
    //public int hp { get; private set; } = 0;
    //private const int MAX_HP = 5;
    public bool isLive = false;
    
    // 이동관련
    public bool isMove { get; private set; }
    public Vector3 moveVector { get; private set; }

    public bool isRotate { get; private set; }
    
    public bool isJump { get; private set; }

    private float rotSpeed = 4.0f;
    private float moveSpeed = 4.0f;
    public float jumpForce = 6.0f;
    
    public LayerMask groundLayer;
    public float groundCheckDistance = 1.1f;
    public bool isGrounded;

    public GameObject playerModelObject;
    private Rigidbody rigidBody;

    public MapManager mapManager;

    public upLava lava;

    public Animator anim;
    
    
    
    void Start()
    {
        if (BackendMatchManager.GetInstance() == null)
        {
            // 매칭 인스턴스가 존재하지 않을 경우 (인게임 테스트 용도)
            Initialize(true, SessionId.None, "testPlayer", 0);
        }
    }
    
    public void Initialize(bool isMe, SessionId index, string nickName, float rot)
    {
        this.isMe = isMe;
        this.index = index;
        this.nickName = nickName;

        if (this.isMe)
        {
            Camera.main.GetComponent<FollowCamera>().target = this.transform;
        }

        this.isLive = true;

        this.isMove = false;
        this.moveVector = new Vector3(0, 0, 0);
        this.isRotate = false;
        this.isJump = false;
        isGrounded = true;
        groundLayer = LayerMask.GetMask("Default");

        //hp

        playerModelObject = this.gameObject;
        playerModelObject.transform.rotation = Quaternion.Euler(0, rot, 0);

        rigidBody = this.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lava = FindObjectOfType<upLava>();

        mapManager = FindObjectOfType<MapManager>();
        
        if (BackendMatchManager.GetInstance().nowModeType == MatchModeType.TeamOnTeam)
        {
            //var teamNumber = BackendMatchManager.GetInstance().GetTeamInfo(index);
            var mySession = Backend.Match.GetMySessionId();
            //var myTeam = BackendMatchManager.GetInstance().GetTeamInfo(mySession);
        }
    }
    
    
    #region 이동관련 함수
    /*
     * 변화량만큼 이동
     * 특정 좌표로 이동
     */
    public void SetMoveVector(float move)
    {
        SetMoveVector(this.transform.forward * move);
    }
    public void SetMoveVector(Vector3 vector)
    {
        moveVector = vector;

        if (vector == Vector3.zero)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
    }

    public void Move()
    {
        Move(moveVector);
    }
    public void Move(Vector3 var)
    {
        if (!isLive)
        {
            return;
        }
        // 회전
        if (var.Equals(Vector3.zero))
        {
            isRotate = false;
        }
        else
        {
            if (Quaternion.Angle(playerModelObject.transform.rotation, Quaternion.LookRotation(var)) > Quaternion.kEpsilon)
            {
                isRotate = true;
            }
            else
            {
                isRotate = false;
            }
        }

        playerModelObject.transform.rotation = Quaternion.LookRotation(var);

        // 이동
        var pos = gameObject.transform.position + playerModelObject.transform.forward * moveSpeed * Time.deltaTime;
        //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, Time.deltaTime * smoothVal);
        SetPosition(pos);
    }
    
    public void Rotate()
    {
        if (moveVector.Equals(Vector3.zero))
        {
            isRotate = false;
            return;
        }
        if (Quaternion.Angle(playerModelObject.transform.rotation, Quaternion.LookRotation(moveVector)) < Quaternion.kEpsilon)
        {
            isRotate = false;
            return;
        }
        playerModelObject.transform.rotation = Quaternion.Lerp(playerModelObject.transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * rotSpeed);
        
        // 마우스 입력에 따라 회전할 각도를 계산합니다.
    }
    
    public void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        anim.SetTrigger("Jump");
    }
    
    void OnTriggerEnter(Collider collider) //충돌 감지
    {
        if (collider.gameObject.tag == "Floor") 
        { 
            isGrounded = true; 
        }

        if (collider.gameObject.tag == "Lava")
        {
            PlayerDie();
            WorldManager.instance.dieEvent(index);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        if (!isLive)
        {
            return;
        }
        gameObject.transform.position = pos;
    }

    // isStatic이 true이면 해당 위치로 바로 이동
    public void SetPosition(float x, float y, float z)
    {
        if (!isLive)
        {
            return;
        }
        Vector3 pos = new Vector3(x, y, z);
        SetPosition(pos);
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetRotation()
    {
        //return gameObject.transform.rotation;
        return gameObject.transform.rotation.eulerAngles;
    }
    #endregion
    
    public bool GetIsLive()
    {
        return isLive;
    }

    private void PlayerDie()
    {
        MultiInGameUI.Instance.SetFailPopupTrigger();
        isLive = false;
        anim.SetTrigger("Dead");
        // 나머지 게임 오브젝트들 false
        mapManager.GameOver(false);
    }

    
    void Update()
    {
        if (BackendMatchManager.GetInstance() == null)
        {
            // 매칭 인스턴스가 존재하지 않는 경우 (인게임 테스트 용도)
            //Move();


            /*if (TESTONLY_attackStick.isInputEnable)
            {
                Vector3 tmp2 = new Vector3(TESTONLY_attackStick.GetHorizontalValue(), 0, TESTONLY_attackStick.GetVerticalValue());
                if (!tmp2.Equals(Vector3.zero))
                {
                    tmp2 += GetPosition();
                    //Attack(tmp2);
                }
            }*/
        }

        if (!isLive)
        {
            return;
        }

        if (isMove)
        {
            anim.SetBool("IsMove", true);
            Move();
        }

        if (!isMove)
        {
            anim.SetBool("IsMove", false);
        }

        if (isRotate)
        {
            Rotate();
        }

        /*if (WorldManager.instance.alivePlayer == 1 && isLive)
        {
            mapManager.GameOver(true);
            MapManager.instance.isGameOver = true;
        }*/
        
    }
    
    public SessionId GetIndex()
    {
        return index;
    }

    public bool IsMe()
    {
        return isMe;
    }

    public string GetNickName()
    {
        return nickName;
    }
}
