using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Pathfinding;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance;
    public GameObject loadingScreen;
    public Image loadingBar;
    [field: SerializeField] public List<GameObject> Grids;
    [SerializeField] private LayerMask _detectColliderMask;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        Grids = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Grid").ToList();
        AstarPath.active.Scan();
        //UpdatePathfindingGraphs();
    }

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

    public int GetCurrentSceneID()
    {
        Collider2D collider = Physics2D.OverlapCircle(characterControl.Instance.transform.position, 10f, _detectColliderMask);
        return collider.gameObject.scene.buildIndex;
    }

    public string GetCurrentSceneName()
    {
        Collider2D collider = Physics2D.OverlapCircle(characterControl.Instance.transform.position, 10f, _detectColliderMask);
        return collider.gameObject.scene.name;
    }
    // Function is not used because another solution for the pathfinding problem was found
    // public void UpdatePathfindingGraphs()
    // {
    //     for(int i = 0; i < Grids.Count; i++)
    //     {
    //         for(int j = 0; j < Grids[i].transform.childCount; j++)
    //         {
    //             // AstarPath.active.UpdateGraphs(Grids[i].transform.GetChild(j).GetComponent<CompositeCollider2D>().bounds);
    //             CompositeCollider2D collider = Grids[i].transform.GetChild(j).GetComponent<CompositeCollider2D>();
    //             if (collider != null)
    //             {
    //                 AstarPath.active.UpdateGraphs(collider.bounds);
    //             }
    //             // else
    //             // {
    //             //     Debug.LogWarning("CompositeCollider2D not found on " + Grids[i].transform.GetChild(j).name);
    //             // }
    //         }
    //     }
    // }
}
