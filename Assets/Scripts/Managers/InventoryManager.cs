using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [field: SerializeField] public List<Item> _inventoryItems;
    [field: SerializeField] public List<Item> _equippedItemsActive;
    [field: SerializeField] public List<Item> _equippedItemsPassive;
    [field: SerializeField] public List<string> _inventoryItemsID;
    [field: SerializeField] public List<string> _equippedItemsActiveID;
    [field: SerializeField] public List<string> _equippedItemsPassiveID;
    [field: SerializeField] public GameObject _checkpointInventoryMenu { get; private set; }
    [field: SerializeField] public GameObject _equippedInventoryMenu { get; private set; }
    [field: SerializeField] public int _maxEquippedActive { get; private set; }
    [field: SerializeField] public int _maxEquippedPassive { get; private set; }
    [field: SerializeField] public List<Item> _allItems { get; private set; }

    public enum Type 
    {
        Passive,
        Active
    }
    private int _passiveItemCount = 0;
    private int _activeItemCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DisplayEquipped();
        GetEquipped();
    }

    public void AddItem(Item item)
    {
        _inventoryItems.Add(item);
    }

    public void RemoveItem(Item item)
    {
        _inventoryItems.Remove(item);
    }
    public void SetInventory()
    {
        for(int i = 0; i < _inventoryItems.Count; i++)
        {
            if(_inventoryItems[i].itemName == null || _inventoryItems.Count <= _activeItemCount + _passiveItemCount) return;
            if(_inventoryItems[i].type == Type.Active)
            {
                _checkpointInventoryMenu.transform.GetChild(1).GetChild(_activeItemCount).gameObject.SetActive(true);
                _checkpointInventoryMenu.transform.GetChild(1).GetChild(_activeItemCount).gameObject.name = _inventoryItems[i].itemName;
                _checkpointInventoryMenu.transform.GetChild(1).GetChild(_activeItemCount).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _inventoryItems[i].itemDisplayName;
                _checkpointInventoryMenu.transform.GetChild(1).GetChild(_activeItemCount).GetChild(1).gameObject.GetComponent<Image>().sprite = _inventoryItems[i].icon;
                _checkpointInventoryMenu.transform.GetChild(1).GetChild(_activeItemCount).gameObject.GetComponent<InventoryItemUI>()._item = _inventoryItems[i];
                _activeItemCount++;
            }
            else 
            {
                _checkpointInventoryMenu.transform.GetChild(3).GetChild(_passiveItemCount).gameObject.SetActive(true);
                _checkpointInventoryMenu.transform.GetChild(3).GetChild(_passiveItemCount).gameObject.name = _inventoryItems[i].itemName;
                _checkpointInventoryMenu.transform.GetChild(3).GetChild(_passiveItemCount).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _inventoryItems[i].itemDisplayName;
                _checkpointInventoryMenu.transform.GetChild(3).GetChild(_passiveItemCount).GetChild(1).gameObject.GetComponent<Image>().sprite = _inventoryItems[i].icon;
                _checkpointInventoryMenu.transform.GetChild(3).GetChild(_passiveItemCount).gameObject.GetComponent<InventoryItemUI>()._item = _inventoryItems[i];
                _passiveItemCount++;
            }

        }
    }

    public void GetEquipped()
    {
        if (_inventoryItems.FirstOrDefault(item => item.itemName == "totem_of_jumping" && item.isEquipped) != null)
        {
            characterControl.Instance.addConstDJI = 1;
        }
        else characterControl.Instance.addConstDJI = 0;

        if (_inventoryItems.FirstOrDefault(item => item.itemName == "totem_of_speed" && item.isEquipped) != null)
        {
            characterControl.Instance.runSpeed = 14;
            characterControl.Instance.maxRunSpeed = 17;
        }
        else 
        {
            characterControl.Instance.runSpeed = 10;
            characterControl.Instance.maxRunSpeed = 13;
        }
    }

    public void SetEquipped(Item item)
    {
        if (item.type == Type.Active)
        {
            for (int i = 0; i < _maxEquippedActive; i++)
            {
                if (_equippedItemsActive[i] == item)
                {
                    _equippedItemsActive[i] = null;
                    _equippedItemsActiveID[i] = null;
                    item.isEquipped = false;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = null;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                    GetEquipped();
                    return;
                }
            }
            
            for (int i = 0; i < _maxEquippedActive; i++)
            {
                if (_equippedItemsActive[i] == null || i == _maxEquippedActive - 1)
                {
                    _equippedItemsActive[i] = item;
                    _equippedItemsActiveID[i] = item.id;
                    item.isEquipped = true;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = item;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemDisplayName;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = item.icon;
                    GetEquipped();
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < _maxEquippedPassive; i++)
            {
                if (_equippedItemsPassive[i] == item)
                {
                    _equippedItemsPassive[i] = null;
                    _equippedItemsPassiveID[i] = null;
                    item.isEquipped = false;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = null;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                    GetEquipped();
                    return;
                }
            }

            for (int i = 0; i < _maxEquippedPassive; i++)
            {
                if (_equippedItemsPassive[i] == null || i == _maxEquippedPassive - 1)
                {
                    _equippedItemsPassive[i] = item;
                    _equippedItemsPassiveID[i] = item.id;
                    item.isEquipped = true;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = item;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemDisplayName;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = item.icon;
                    GetEquipped();
                    return;
                }
            }
        }
    }

    public void SetUnequipped(Item item)
    {
        if (item.type == Type.Active)
        {
            for (int i = 0; i < _maxEquippedActive; i++)
            {
                if (_equippedItemsActive[i] == item)
                {
                    _equippedItemsActive[i] = null;
                    item.isEquipped = false;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = null;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                    GetEquipped();
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < _maxEquippedPassive; i++)
            {
                if (_equippedItemsPassive[i] == item)
                {
                    _equippedItemsPassive[i] = null;
                    item.isEquipped = false;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = null;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    _equippedInventoryMenu.transform.GetChild(3).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                    GetEquipped();
                    return;
                }
            }
        }
    }

    public void SetInventoryByID()
    {
        _inventoryItems.Clear();

        foreach (var id in _inventoryItemsID)
        {
            Item item = _allItems.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                _inventoryItems.Add(item);
            }
        }

        foreach (var id in _equippedItemsActiveID)
        {
            Item item = _allItems.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                _equippedItemsActive.Add(item);
            }
            else _equippedItemsActive.Add(null);
        }

        foreach (var id in _equippedItemsPassiveID)
        {
            Item item = _allItems.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                _equippedItemsPassive.Add(item);
            }
            else _equippedItemsPassive.Add(null);
        }

        SetInventory();
    }

    public void DisplayEquipped()
    {
        int i = 0, j = 0;
        foreach(var item in _equippedItemsActive)
        {
            if(item == null) continue;
            _equippedInventoryMenu.transform.GetChild(1).GetChild(i).gameObject.GetComponent<InventoryItemUI>()._item = item;
            _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemDisplayName;
            _equippedInventoryMenu.transform.GetChild(1).GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = item.icon;
            i++;
        }

        foreach(var item in _equippedItemsPassive)
        {
            if(item == null) continue;
            _equippedInventoryMenu.transform.GetChild(3).GetChild(j).gameObject.GetComponent<InventoryItemUI>()._item = item;
            _equippedInventoryMenu.transform.GetChild(3).GetChild(j).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemDisplayName;
            _equippedInventoryMenu.transform.GetChild(3).GetChild(j).GetChild(1).gameObject.GetComponent<Image>().sprite = item.icon;
            j++;
        }
    }
}
