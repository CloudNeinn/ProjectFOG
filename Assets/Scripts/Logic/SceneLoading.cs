using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public GameObject loadingScreen;
    public Image loadingBar;

    public void LoadScene(int sceneID, bool isAdditive = false)
    {
        StartCoroutine(LoadSceneAsync(sceneID, isAdditive));
    }

    IEnumerator LoadSceneAsync(int sceneID, bool isAdditive)
    {
        AsyncOperation operation;
        if(isAdditive) operation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
        else operation = SceneManager.LoadSceneAsync(sceneID);
        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progress;
            yield return null;
        }
    }
}
