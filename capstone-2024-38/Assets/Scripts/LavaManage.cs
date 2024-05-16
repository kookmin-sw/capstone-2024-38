using UnityEngine;

public class LavaManage : MonoBehaviour
{
    public upLava lava;
    public GameObject player;

    private float moveInterval = 30.0f; 

    private float elapsedTime = 0.0f;

    void Start()
    {
        lava = FindObjectOfType<upLava>();
        if (lava == null)
        {
            Debug.Log("upLava object not found!");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player object not found!");
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= moveInterval)
        {
            lava.targetY += 5.0f;

            elapsedTime = 0.0f;
        }

        if (player.transform.position.y - 25 > lava.transform.position.y)
        {
            Debug.Log("Player is above Lava.");
        }
        else
        {
            Debug.Log("Player is below Lava.");
        }

        Debug.Log("Current targetY: " + lava.targetY);
    }
}
