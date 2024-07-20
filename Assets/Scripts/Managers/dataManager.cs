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
            SceneManager.LoadSceneAsync("PersistentObjects");
            SceneManager.LoadSceneAsync(data.saveSceneID, LoadSceneMode.Additive);

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
        data.saveSceneID = SceneManager.GetActiveScene().buildIndex;
        data.totalCurrency = CurrencyManager.Instance.totalCurrency;
    }
}
