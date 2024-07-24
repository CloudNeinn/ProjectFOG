using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TraderItemUI : MonoBehaviour
{
    private Item _item;
    private GameObject _storeMenu;

    // Public property with getter and setter
    public Item item
    {
        get { return _item; }
        set
        {
            if(value == null)
            {
                _item = value;
                UnsetItemInfo();
            }
            else if (_item != value) // Check if the value is actually changing
            {
                _item = value;
                SetItemInfo(); // Call the method when the variable is set
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _storeMenu = GameObject.Find("StoreMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetItemInfo()
    {
        this.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _item.itemName;
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = _item.icon;
    }

    void UnsetItemInfo()
    {
        this.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = null;
    }

    public void PickThisItem()
    {
        if(_item == null) return;
        _storeMenu.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _item.itemName;
        _storeMenu.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite = _item.icon;
        _storeMenu.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = _item.description;
    }
}
