using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    [SerializeField] private float _storeRange;
    [SerializeField] private bool _inRange;
    [SerializeField] private LayerMask _playerLayer;
    private GameObject _storeMenu;
    private bool _storeActive;
    private bool _wasInRange;
    private GameObject _storeInventory;
    private GameObject _storeInventoryContent;
    private int _maxInventorySize;
    public bool storeActive;
    private Item _itemTransfer;
    [SerializeField] public List<Item> inventoryItems = new List<Item>();

    void Start()
    {
        _storeActive = false;
        _wasInRange = false;
        _storeMenu = StoreManager.Instance._storeMenu;
        _storeInventory = _storeMenu.gameObject.transform.GetChild(0).gameObject;
        _storeInventoryContent = _storeMenu.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        _maxInventorySize = _storeInventoryContent.transform.childCount;
        _storeMenu.SetActive(false);
    }

    void Update()
    {
        if(checkIfInRange() && characterControl.Instance._use2Input && !_storeActive)
        {
            _storeActive = true;
            _storeMenu.SetActive(true);
        }
        else if(_storeActive && characterControl.Instance._use2Input)
        {
            _storeActive = false;
            _storeMenu.SetActive(false);
        }

        if(_storeActive && Vector2.Distance(transform.position, characterControl.Instance.transform.position) > 5f)
        {
            _storeActive = false;
            _storeMenu.SetActive(false);
        }

        if(checkIfInRange() && !_wasInRange)
        {
            _wasInRange = true;
            LoadStore();
        }
        else if(!checkIfInRange() && _wasInRange)
        {
            _wasInRange = false;
            UnloadStore();
        }
    }

    bool checkIfInRange()
    {
        return Physics2D.OverlapCircle(transform.position, _storeRange, _playerLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _storeRange);
    }

    void LoadStore()
    {
        StoreManager.Instance.updateStore();
        StoreManager.Instance.currentStore = this;
        for(int i = 0; i < _maxInventorySize; i++)
        {
            if(i >= inventoryItems.Count) break;
            _storeInventoryContent.transform.GetChild(i).gameObject.GetComponent<TraderItemUI>().item = inventoryItems[i];
        }
    }

    void UnloadStore()
    {
        for(int i = 0; i < _maxInventorySize; i++)
        {
            _storeInventoryContent.transform.GetChild(i).gameObject.GetComponent<TraderItemUI>().item = null;
        }
        StoreManager.Instance.UnloadPickedItem();
    }

    public void BuyObject(GameObject ItemNameObject)
    {
        _itemTransfer = inventoryItems.Find(obj => obj.itemDisplayName == ItemNameObject.GetComponent<TextMeshProUGUI>().text);
        RemoveItem(_itemTransfer);
        InventoryManager.Instance.AddItem(_itemTransfer);
        _itemTransfer = null;
        UnloadStore();
        LoadStore();
    }

    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);
    }
}
