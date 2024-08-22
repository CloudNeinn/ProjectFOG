using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterActiveItems : MonoBehaviour
{
    public static characterActiveItems Instance;

    void Update()
    {
        foreach(Item item in InventoryManager.Instance._equippedItemsActive)
        {
            if(item != null) item.Execute();
        }
    }
}
