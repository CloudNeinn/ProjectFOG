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
    public int[] saveSceneIDs;
    public int totalCurrency;
    public string currentCheckpointID;
    public SerializableDictionary<string, bool> doorCondition;
    public List<string> inventoryItemsID;
    public List<string> equippedItemsActiveID;
    public List<string> equippedItemsPassiveID;
    public SerializableDictionary<string, List<string>> storeItems;

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
        saveSceneIDs = new int[] {2, 3};
        totalCurrency = 0;
        //inventoryItemsID = new List<string>();
        //equippedItemsActiveID = new List<string> {null, null, null};
        //equippedItemsPassiveID = new List<string> {null, null, null};
        storeItems = new SerializableDictionary<string, List<string>>();

    }
}
