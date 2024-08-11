using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create new item")]
public class Item : ScriptableObject
{
    public string id;
    [ContextMenu("Generate item id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public string itemName;
    public string itemDisplayName;
    public string description;
    public int value;
    public Sprite icon;
    public bool isEquipped;
    public InventoryManager.Type type;
}
