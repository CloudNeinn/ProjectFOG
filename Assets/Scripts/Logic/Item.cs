using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create new item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public bool isActive;
}
