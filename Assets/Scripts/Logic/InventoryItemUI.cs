using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Item _item;

    public void SetAsEquipped()
    {
        if(_item != null) InventoryManager.Instance.SetEquipped(_item);
    }

    public void SetAsUnequipped()
    {
        if(_item != null) InventoryManager.Instance.SetUnequipped(_item);
    }
}
