using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyBoardMovement : MonoBehaviour
{
    public float movement_weight;
    Animator player_animation;

    float charging_animation_temp = 1;
    // Start is called before the first frame update
    void Start()
    {
        player_animation = this.GetComponent<Animator>();

        if (player_animation == null)
        {
            Debug.LogWarning("Animator component is missing on the player game object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player_animation == null)
        {
            return;
        }

        // movement power
        float power = movement_weight * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0,0, power);
            player_animation.SetBool("IsFront", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(-power, 0, 0);
            player_animation.SetBool("IsLeft", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(0, 0, -power);
            player_animation.SetBool("IsBack", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(power, 0, 0);
            player_animation.SetBool("IsRight", true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            player_animation.SetBool("IsFront", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player_animation.SetBool("IsLeft", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            player_animation.SetBool("IsBack", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            player_animation.SetBool("IsRight", false);
        }

        // dancing animation
        if(Input.GetKeyDown(KeyCode.M))
        {
            player_animation.SetTrigger("Hip-Hop Dancing");
        }
        
        /*
        // charging animation
        if(this.GetComponent<Player_SkillandBullet>().getCharging())
        {
            player_animation.SetLayerWeight(1, 1);
            player_animation.SetBool("Charging", true);
        }
        else
        {
            player_animation.SetBool("Charging", false);
            // player charging animaion smooth revalance
            if (player_animation.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f)
            {
                if (charging_animation_temp >= 0)
                {
                    charging_animation_temp -= Time.deltaTime;
                }
                player_animation.SetLayerWeight(1, charging_animation_temp);
            }
        }
        */

        // new charging animation
        if (this.GetComponent<Player_SkillandBullet>().getCharging())
        {
            player_animation.SetLayerWeight(1, 1);

            if(player_animation.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                player_animation.SetFloat("throw", 0);
            }
            else
            {
                player_animation.SetFloat("throw", 1);
            }
            player_animation.SetBool("Charging", true);
        }
        else
        {
            player_animation.SetBool("Charging", false);

            player_animation.SetFloat("throw", 1);
            // player charging animaion smooth revalance
            if (player_animation.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                if (charging_animation_temp >= 0)
                {
                    charging_animation_temp -= Time.deltaTime;
                }
                player_animation.SetLayerWeight(1, charging_animation_temp);
            }
        }
        Debug.Log(player_animation.GetFloat("throw"));
        // Hit animation test must remove this code
        if (Input.GetKeyDown(KeyCode.L))
        {
            player_animation.SetTrigger("Hit");
        }
        // dead animation test must remove this code
        if(Input.GetKeyDown(KeyCode.P))
        {
            player_animation.SetTrigger("dead");
        }

        

    }
}
