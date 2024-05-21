using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public Transform rigenPos;

    public GameObject UserCard;

    public GameObject AICard;

    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("이곳에서 서버 연동 총인원 20명중에 남은 인원은 AI로 해줄것");
        for (int i = 0; i < 20; i++)
        {
            Setiing(Instantiate(AICard, rigenPos));
        }
    }

    private void Setiing(GameObject id,string level = "1", string wins = "1", string lose = "1", string gold = "1", string exp = "1")
    {
        id.GetComponent<Image>().color = getrandomcolor();

        id.transform.Find("Group_Info").Find("Group_Value").Find("Text_Level_Value").GetComponent<TextMeshProUGUI>().text = level;
        id.transform.Find("Group_Info").Find("Group_Value").Find("Text_Wins_Value").GetComponent<TextMeshProUGUI>().text = wins;
        id.transform.Find("Group_Info").Find("Group_Value").Find("Text_Lose_Value").GetComponent<TextMeshProUGUI>().text = lose;
        id.transform.Find("Group_Info").Find("Group_Value").Find("Text_Gold_Value").GetComponent<TextMeshProUGUI>().text = gold;
        id.transform.Find("Group_Info").Find("Group_Value").Find("Text_Gem_Value").GetComponent<TextMeshProUGUI>().text = exp;
    }
    private Color getrandomcolor()
    {
        return Random.ColorHSV();
    }
}
