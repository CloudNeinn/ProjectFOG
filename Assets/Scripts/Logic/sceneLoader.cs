using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializeField] private SceneField[] _scenesToLoad;
    [SerializeField] private SceneField[] _scenesToUnload;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // AstarPath.active.UpdateGraphs (characterControl.Instance.coll.bounds); updates grapth according to the collider bounds passed in the function
            // AstarPath.active.Scan(AstarPath.active.data.gridGraph);scans first graph
            // AstarPath.active.ScanAsync(); only pro version
            AstarPath.active.Scan();
            LoadScenes();
            UnloadScenes();
        }
    }

    void LoadScenes()
    {
        for(int i = 0; i < _scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for(int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if(loadedScene.name == _scenesToLoad[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if(!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }

    void UnloadScenes()
    {
        for(int i = 0; i < _scenesToUnload.Length; i++)
        {
            for(int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if(loadedScene.name == _scenesToUnload[i].SceneName)
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }
    }

}
