using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public upLava lava;
    public GameObject player;
    public InGameUI inGameUi;
    public IngameBGMPlayer bgmPlayer;
    public Spawner span;

    private float moveInterval = 5.0f;
    private float elapsedTime = 0.0f;
    private float upHeight = 2.0f;

    public float survivalTime = 100.0f;
    private float remainingTime;
    private bool isGameOver = false;

    public float RemainingTime => remainingTime;

    void Start()
    {
        lava = FindObjectOfType<upLava>();
        if (lava == null)
        {
            Debug.Log("upLava object not found!");
        }

        inGameUi = FindObjectOfType<InGameUI>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player object not found!");
        }
        bgmPlayer = FindObjectOfType<IngameBGMPlayer> ();
        span = FindObjectOfType<Spawner>();

        remainingTime = survivalTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        remainingTime -= Time.deltaTime;

        if (elapsedTime >= moveInterval)
        {
            lava.targetY += upHeight;
            elapsedTime = 0.0f;
        }

        /*float playerY = player.transform.position.y;

        if (playerY - 25 > lava.transform.position.y)
        {
            Debug.Log("Player is above Lava.");
        }
        else
        {
            Debug.Log("Player is below Lava.");
        }*/

        //Debug.Log("Current targetY: " + lava.targetY);

        UpdateTimerText();

        if (remainingTime <= 0)
        {
            GameOver(true);
            WorldManager.instance.SendGameEndOrder();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        //Debug.Log(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    public void GameOver(bool won)
    {
        isGameOver = true;
        span.StopSpawning();
        if (won)
        {
            inGameUi.SetClearPopupTrigger();
            Debug.Log("You survived!");
            bgmPlayer.PlayWinSound();
        }
        else
        {
            inGameUi.SetFailPopupTrigger();
            Debug.Log("Game Over! You died.");
            bgmPlayer.PlayLoseSound();
        }

        StartCoroutine(WaitForKeyPress());
    }

    IEnumerator WaitForKeyPress()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        Time.timeScale = 1f;
        GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);
    }
}
