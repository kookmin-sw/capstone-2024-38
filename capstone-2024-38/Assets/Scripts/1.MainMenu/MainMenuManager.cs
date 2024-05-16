using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Animator padeout;

    [SerializeField]
    private Animator title;

    [SerializeField]
    private Animator stage;

    private void Update()
    {
        if(Input.GetKey(KeyCode.K))
        {
            titleAnimation();
            stageAnimation();
        }
    }

    void titleAnimation()
    {
        if(title.GetBool("Title"))
        {
            title.SetBool("Title", false);
        }
        else
        {
            title.SetBool("Title", true);
        }
    }

    void stageAnimation()
    {
        if (stage.GetBool("Stage"))
        {
            stage.SetBool("Stage", false);
        }
        else
        {
            stage.SetBool("Stage", true);
        }
    }


}
