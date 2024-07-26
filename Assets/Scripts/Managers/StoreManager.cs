using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public StoreScript currentStore;
    public StoreScript[] checkIfStoreActive;
    private GameObject _storeMenu;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        checkIfStoreActive = FindObjectsOfType<StoreScript>();
        _storeMenu = GameObject.Find("StoreMenu");
    }

    public void updateStore()
    {
        foreach(StoreScript obj in checkIfStoreActive)
        {
            if(obj.storeActive)
            {
                obj.storeActive = false;
            }
        }        
    }

    public void BuyObject(GameObject ItemNameObject)
    {
        currentStore.BuyObject(ItemNameObject);        
    }

    public void UnloadPickedItem()
    {
        _storeMenu.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
        _storeMenu.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
        _storeMenu.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
}
