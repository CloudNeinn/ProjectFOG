using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour, IDataPersistance
{
    public static DataManager Instance;

    public GameData data;
    public cameraMovement camMov;
    public bool hasToRespawn;
    private Scene _sceneToLoad;
    private int[] _sceneIDs;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadData(GameData data)
    {
        if(hasToRespawn) 
        {
            SceneManager.LoadSceneAsync(2);
            for(int i = 0; i < data.saveSceneIDs.Length; i++)
            {
                //_sceneToLoad = SceneManager.GetSceneAt(data.saveSceneIDs[i]);
                //if(_sceneToLoad.name != "PersistentObjects") 
                if(data.saveSceneIDs[i] != 2) SceneManager.LoadSceneAsync(data.saveSceneIDs[i], LoadSceneMode.Additive);
            }
            //SceneManager.LoadSceneAsync("PersistentObjects");
            //SceneManager.LoadSceneAsync(data.saveSceneID, LoadSceneMode.Additive);

            hasToRespawn = false;
        }
        characterControl.Instance.transform.position = data.playerPosition;
        camMov.transform.position = data.playerPosition;
        characterControl.Instance.hasDeflectProjectile = data.hasDeflectProjectile;
        characterControl.Instance.hasSpecialAttack = data.hasSpecialAttack;
        characterControl.Instance.hasDoubleJump = data.hasDoubleJump;
        characterControl.Instance.hasHighJump = data.hasHighJump;
        characterControl.Instance.hasBarrier = data.hasBarrier;
        characterControl.Instance.hasDash = data.hasDash;
        characterControl.Instance.hasWallJump = data.hasWallJump;
        characterControl.Instance.constDJI = data.constDJI;
        CurrencyManager.Instance.totalCurrency = data.totalCurrency;
        Debug.Log(data.playerPosition);
        CurrencyManager.Instance.SetCurrency();
        foreach (var checkpoint in CheckpointManager.Instance.checkIfSaved)
        {
            if (checkpoint.id == data.currentCheckpointID)
            {
                CheckpointManager.Instance.currentCheckpoint = checkpoint;
                checkpoint.isActivated = true;
                break;
            }
        }
        //Debug.Log(camMov.transform.position.x);
        //Debug.Log(checkpManage.currentCheckpointPosition.x);
        Debug.Log("Data Loaded");
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = characterControl.Instance.transform.position;
        data.hasDeflectProjectile = characterControl.Instance.hasDeflectProjectile;
        data.hasSpecialAttack = characterControl.Instance.hasSpecialAttack;
        data.hasDoubleJump = characterControl.Instance.hasDoubleJump;
        data.hasHighJump = characterControl.Instance.hasHighJump;
        data.hasBarrier = characterControl.Instance.hasBarrier;
        data.hasDash = characterControl.Instance.hasDash;
        data.hasWallJump = characterControl.Instance.hasWallJump;
        data.constDJI = characterControl.Instance.constDJI;
        data.currentCheckpointID = CheckpointManager.Instance.currentCheckpoint.id;
        data.saveSceneIDs = GetLoadedSceneIDs();
        data.totalCurrency = CurrencyManager.Instance.totalCurrency;
    }

    int[] GetLoadedSceneIDs()
    {
        int countLoaded = SceneManager.sceneCount;
        int[] loadedScenes = new int[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i).buildIndex;
        }
        return loadedScenes;
    }
}
