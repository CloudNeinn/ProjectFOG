using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField] private GameObject _mainWorldMap;
    [SerializeField] private GameObject _otherWorldMap;
    [SerializeField] public List<string> mainWorldVisitedScenes;
    [SerializeField] public List<string> otherWorldVisitedScenes;
    public bool mapIsActive;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _mainWorldMap.SetActive(false);
        _otherWorldMap.SetActive(false);
    }

    public void OpenMap()
    {
        mapIsActive = true;
        string SceneName = SceneLoading.Instance.GetCurrentSceneName();
        if(characterControl.Instance._inOtherWorld)
        {
            if(otherWorldVisitedScenes.Contains(SceneName)) otherWorldVisitedScenes.Remove(SceneName);
            otherWorldVisitedScenes.Add(SceneName);
            foreach(Transform child in _otherWorldMap.transform)
            {
                if(otherWorldVisitedScenes.Contains(child.name)) child.gameObject.SetActive(true);
            }
            _otherWorldMap.SetActive(true);
        }
        else
        {
            if(mainWorldVisitedScenes.Contains(SceneName)) mainWorldVisitedScenes.Remove(SceneName);
            mainWorldVisitedScenes.Add(SceneName);
            foreach(Transform child in _mainWorldMap.transform)
            {
                if(mainWorldVisitedScenes.Contains(child.name)) child.gameObject.SetActive(true);
            }
            _mainWorldMap.SetActive(true);
        }
    }

    public void CloseMap()
    {
        mapIsActive = false;
        if(characterControl.Instance._inOtherWorld)
        {
            _otherWorldMap.SetActive(false);
        }
        else
        {
            _mainWorldMap.SetActive(false);
        }
    }
}
