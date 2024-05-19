using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead = false;
    private MapManager mapManager;
    private Animator player_animation;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        player_animation = GetComponent<Animator>();
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Player has died.");
            

            PlayerKeyBoardMovement playerMovement = GetComponent<PlayerKeyBoardMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            if (mapManager != null)
            {
                mapManager.GameOver(false);
            }

            player_animation.SetTrigger("Dead");
            StartCoroutine(StopGame(1f));
        }
    }

    IEnumerator StopGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;
    }
}
