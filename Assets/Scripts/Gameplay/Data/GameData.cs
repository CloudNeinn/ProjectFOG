using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool hasDoubleJump;
    public bool hasHighJump;
    public bool hasDash;
    public bool hasSpecialAttack;
    public bool hasBarrier;
    public bool hasDeflectProjectile;
    public bool hasWallJump;
    public int constDJI;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> doorCondition;
    public int saveSceneID;
    public int totalCurrency;

    // values defined in the construcor will be the initial values for the game
    public GameData()
    {
        this.playerPosition = new Vector3(-56f, 39.1f, 1.8f);
        doorCondition = new SerializableDictionary<string, bool>();
        hasHighJump = false;
        hasDash = false;
        hasWallJump = false;
        hasSpecialAttack = false;
        hasBarrier = false;
        hasDeflectProjectile = false;
        constDJI = 0;
        saveSceneID = 2;
        totalCurrency = 0;
    }
}
