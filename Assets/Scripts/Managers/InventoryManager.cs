using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private List<Item> inventoryItems = new List<Item>();
    [field: SerializeField] public GameObject _checkpointInventoryMenu { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    
    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);
    }

    public void SetInventory()
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].itemName == null) return;
            _checkpointInventoryMenu.transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
            _checkpointInventoryMenu.transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
            _checkpointInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = inventoryItems[i].itemName;
            _checkpointInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = inventoryItems[i].icon;
        }
    }
}
