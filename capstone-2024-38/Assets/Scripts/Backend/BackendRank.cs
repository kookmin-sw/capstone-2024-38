using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using BackEnd;

public class Rank
{
    public string rank;
    public string nickname;
    public string score;

    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {rank}\n";
        result += $"content : {nickname}\n";
        result += $"inDate : {score}\n";

        return result;
    }
}

public class BackendRank : MonoBehaviour
{
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    
    private static BackendRank _instance = null;

    public List<Rank> rankList = new List<Rank>();
    public Rank myRank = new Rank();

    public static BackendRank Instance 
    {
        get 
        {
            if(_instance == null) 
            {
                _instance = new BackendRank();
            }

            return _instance;
        }
    }

    public void RankInsert(int score)
    {
        string rankUUID = "39290b10-fede-11ee-9011-b105362ee41e";

        string tableName = "USER_DATA";
        string rowInDate = string.Empty;
        
        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.Log("데이터 조회 중 문제가 발생했습니다");
            return;
        }
        
        Debug.Log("데이터 조회에 성공했습니다 : " + bro);

        if(bro.FlattenRows().Count > 0) 
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        } 
        else 
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if(bro2.IsSuccess() == false) 
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("내 게임 정보의 rowInDate : " + rowInDate); // 추출된 rowIndate의 값은 다음과 같습니다.  

        Param param = new Param();
        param.Add("rankPoint", score);

        // 추출된 rowIndate를 가진 데이터에 param값으로 수정을 진행하고 랭킹에 데이터를 업데이트합니다.  
        Debug.Log("랭킹 삽입을 시도합니다.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if(rankBro.IsSuccess() == false) 
        {
            Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
            return;
        }

        Debug.Log("랭킹 삽입에 성공했습니다. : " + rankBro);
    }

    public void RankGet()
    {
        string rankUUID = "39290b10-fede-11ee-9011-b105362ee41e";
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.Log("랭킹 조회 중 오류가 발생했습니다");
            return;
        }
        Debug.Log("랭킹 조회에 성공했습니다. " + bro);
        Debug.Log("총 랭킹 등록 유저 수 : " + bro.GetFlattenJSON()["totalCount"].ToString());
        
        foreach(LitJson.JsonData jsonData in bro.FlattenRows()) 
        {
            /*StringBuilder info = new StringBuilder();

            info.AppendLine("순위 : " + jsonData["rank"].ToString());
            info.AppendLine("닉네임 : " + jsonData["nickname"].ToString());
            info.AppendLine("점수 : " + jsonData["score"].ToString());
            info.AppendLine("gamerInDate : " + jsonData["gamerInDate"].ToString());
            info.AppendLine("정렬번호 : " + jsonData["index"].ToString());
            info.AppendLine();
            Debug.Log(info);*/

            Rank rank = new Rank();

            rank.rank = jsonData["rank"].ToString();
            rank.nickname = jsonData["nickname"].ToString();
            rank.score = jsonData["score"].ToString();

            rankList.Add(rank);
        }
    }

    public void GetMyRank()
    {
        string rankUUID = "39290b10-fede-11ee-9011-b105362ee41e";
        var bro = Backend.URank.User.GetMyRank(rankUUID);
        
        LitJson.JsonData jsonData = bro.GetFlattenJSON();
        myRank.nickname = jsonData["rows"][0]["nickname"].ToString();       
        myRank.rank = jsonData["rows"][0]["rank"].ToString();       
        myRank.score = jsonData["rows"][0]["score"].ToString();       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
