using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead = false;
    private MapManager mapManager;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Player has died.");
            Time.timeScale = 0f;

            if (mapManager != null)
            {
                mapManager.GameOver(false);
            }

        }
    }



}
