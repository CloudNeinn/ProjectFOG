using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class StoreScript : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate enemy id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
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
    [SerializeField] public List<Item> _shopItems = new List<Item>();
    public List<string> _shopItemsID;// = new List<string>();

    public void LoadData(GameData data)
    {
        data.storeItems.TryGetValue(id, out _shopItemsID);
        // if (data.storeItems.TryGetValue(id, out _shopItemsID))
        // {
        //     Debug.Log($"Loaded {_shopItemsID.Count} items for store {id}");
        // }
        // else
        // {
        //     Debug.LogWarning($"No items found for store {id}");
        // }
    }

    public void SaveData(ref GameData data)
    {
        if(data.storeItems.ContainsKey(id))
        {
            data.storeItems.Remove(id);
        }
        data.storeItems.Add(id, _shopItemsID);
    }

    void Start()
    {
        //foreach (var item in _inventoryItems)
        //{
        //    _inventoryItemsID.Add(item.id);
        //}
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
        SetInventoryByID();
        StoreManager.Instance.updateStore();
        StoreManager.Instance.currentStore = this;
        for(int i = 0; i < _maxInventorySize; i++)
        {
            if(i >= _shopItems.Count) break;
            _storeInventoryContent.transform.GetChild(i).gameObject.GetComponent<TraderItemUI>().item = _shopItems[i];
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
        _itemTransfer = _shopItems.Find(obj => obj.itemDisplayName == ItemNameObject.GetComponent<TextMeshProUGUI>().text);
        RemoveItem(_itemTransfer);
        InventoryManager.Instance.AddItem(_itemTransfer);
        _itemTransfer = null;
        UnloadStore();
        LoadStore();
    }

    public void AddItem(Item item)
    {
        _shopItems.Add(item);
        _shopItemsID.Remove(item.id);
    }

    public void RemoveItem(Item item)
    {
        _shopItems.Remove(item);
        _shopItemsID.Remove(item.id);
    }

    public void SetInventoryByID()
    {
        _shopItems.Clear();

        foreach (string id in _shopItemsID)
        {
            Item item = InventoryManager.Instance._allItems.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                _shopItems.Add(item);
            }
        }
    }
}
