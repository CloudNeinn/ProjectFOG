using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour, IDataPersistance
{    
    public int currentSceneID;
    public void LoadData(GameData data)
    {
        this.currentSceneID = data.saveSceneID;
    }

    public void SaveData(ref GameData data)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(currentSceneID);
        Time.timeScale = 1f;
    }

    public void toSettings()
    {
        SceneManager.LoadScene(1);
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
