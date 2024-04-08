using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

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

    private void OnGameReconnect()
    {
        isHost = false;
        localQueue = null;
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
