using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine;
using BackEnd;


public class UserData
{
    public int level;
    public int rankPoint;
    public int gold;
    public int gem;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
}

public class BackendGameData : MonoBehaviour
{
    private static BackendGameData _instance = null;
    
    void Awake()
    {
        // 인스턴스가 하나만 존재하게 함
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
        
    }
    public static BackendGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    public UserData userData;

    private string gameDataRowInDate = string.Empty; //스트링 초기화

    public void GameDataInsert()
    {
        if (userData == null)
        {
            userData = new UserData();
        }
        
        Debug.Log("데이터를 초기화합니다.");
        userData.level = 1;
        userData.rankPoint = 0;
        userData.gold = 0;
        userData.gem = 0;

        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("rankPoint", userData.rankPoint);
        param.Add("gold", userData.gold);
        param.Add("gem", userData.gem);
        
        Debug.Log("게임정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 호출에 성공했습니다. : " + bro);

            LitJson.JsonData gameDataJson = bro.FlattenRows();
			

            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
				userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                userData.rankPoint = int.Parse(gameDataJson[0]["rankPoint"].ToString());
                userData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                userData.gem = int.Parse(gameDataJson[0]["gem"].ToString());

                Debug.Log(userData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }

    public void LevelUp()
    {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.level += 1;
    }

    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
        }

        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("rankPoint", userData.rankPoint);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임정보 데이터 수정에 실패했습니다. : " + bro);
        }
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
