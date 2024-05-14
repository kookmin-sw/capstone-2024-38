using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomUI : MonoBehaviour
{
    public Michsky.MUIP.ButtonManager matchButton;
    // Start is called before the first frame update
    void Start()
    {
        matchButton.onClick.AddListener(matchButtonClicked);
    }

    void matchButtonClicked()
    {
        BackendMatchManager.GetInstance().RequestMatchMaking();
    }
}
