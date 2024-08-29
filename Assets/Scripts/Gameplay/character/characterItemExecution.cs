using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterItemExecution : MonoBehaviour
{
    public static characterItemExecution Instance;

    void Update()
    {
        foreach(Item item in InventoryManager.Instance._equippedItemsActive)
        {
            if(item != null) item.Execute();
        }
        
        foreach(Item item in InventoryManager.Instance._equippedItemsPassive)
        {
            if(item != null) item.Execute();
        }
    }
}
