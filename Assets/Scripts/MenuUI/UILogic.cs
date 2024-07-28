using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour, IDataPersistance
{    
    public int[] currentSceneIDs;
    private Scene _sceneToLoad;
    public void LoadData(GameData data)
    {
        this.currentSceneIDs = data.saveSceneIDs;
    }

    public void SaveData(ref GameData data)
    {

    }


    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartGame()
    {
        DataPersistanceManager.Instance.LoadGame();
        //SceneManager.LoadScene("PersistentObjects");
        SceneLoading.Instance.LoadScene(2);
        //SceneManager.UnloadSceneAsync("MainMenu");
        for(int i = 0; i < currentSceneIDs.Length; i++)
        {
            //_sceneToLoad = SceneManager.GetSceneAt(currentSceneIDs[i]);
            //Debug.Log(_sceneToLoad.name);
            //if(_sceneToLoad.name != "PersistentObjects") 
            if(currentSceneIDs[i] != 2) SceneLoading.Instance.LoadScene(currentSceneIDs[i], true);//SceneManager.LoadSceneAsync(currentSceneIDs[i], LoadSceneMode.Additive);
        }
        Time.timeScale = 1f;
    }

    public void toSettings()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void toMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
