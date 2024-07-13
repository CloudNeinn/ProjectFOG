using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dataManager : MonoBehaviour, IDataPersistance
{
    public static dataManager Instance;

    public GameData data;
    public characterControl charCon;
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
            SceneManager.LoadSceneAsync(data.saveSceneID);
            hasToRespawn = false;
        }
        charCon.transform.position = data.playerPosition;
        camMov.transform.position = data.playerPosition;
        charCon.hasDeflectProjectile = data.hasDeflectProjectile;
        charCon.hasSpecialAttack = data.hasSpecialAttack;
        charCon.hasDoubleJump = data.hasDoubleJump;
        charCon.hasHighJump = data.hasHighJump;
        charCon.hasBarrier = data.hasBarrier;
        charCon.hasDash = data.hasDash;
        charCon.hasWallJump = data.hasWallJump;
        charCon.constDJI = data.constDJI;
        Debug.Log(data.playerPosition);
        //Debug.Log(camMov.transform.position.x);
        //Debug.Log(checkpManage.currentCheckpointPosition.x);
        Debug.Log("Data Loaded");
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = charCon.transform.position;
        data.hasDeflectProjectile = charCon.hasDeflectProjectile;
        data.hasSpecialAttack = charCon.hasSpecialAttack;
        data.hasDoubleJump = charCon.hasDoubleJump;
        data.hasHighJump = charCon.hasHighJump;
        data.hasBarrier = charCon.hasBarrier;
        data.hasDash = charCon.hasDash;
        data.hasWallJump = charCon.hasWallJump;
        data.constDJI = charCon.constDJI;
        data.saveSceneID = SceneManager.GetActiveScene().buildIndex;
    }
}
