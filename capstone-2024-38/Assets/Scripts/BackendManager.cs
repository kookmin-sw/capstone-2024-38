using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks;

public class BackendManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 뒤끝 초기화
        var bro = Backend.Initialize(true);
        
        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) 
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else 
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }

        Test();
    }

    async void Test()
    {
        await Task.Run(() =>
        {
            //BackendLogin.Instance.CustomSignUp("user1", "1234");
            
            BackendLogin.Instance.CustomLogin("user1", "1234");
            
            //BackendLogin.Instance.UpdateNickname("원하는 이름");

            BackendGameData.Instance.GameDataGet();

            if (BackendGameData.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();
            }
            
            BackendGameData.Instance.HpUp();
            
            BackendGameData.Instance.GameDataUpdate();
            
            Debug.Log("테스트를 종료합니다");
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
