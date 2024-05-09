using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingScenemanager : MonoBehaviour
{
    public static string nextScene;
    
    [SerializeField]
    Image progressBar;
    [SerializeField]
    GameObject[] RodingImg;

    private void Start()
    {
        foreach (GameObject i in RodingImg)
        {
            i.SetActive(false);
        }
        RodingImg[Random.Range(0,RodingImg.Length)].SetActive(true);
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LodingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime; if (op.progress < 0.9f) { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); if (progressBar.fillAmount >= op.progress) { timer = 0f; } } else { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); if (progressBar.fillAmount == 1.0f) { op.allowSceneActivation = true; yield break; } }
        }
    }
}
