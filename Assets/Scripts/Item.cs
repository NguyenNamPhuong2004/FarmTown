using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int itemSalePrice;
    public int itemBuyPrice;
    public ItemType itemType;
    public Sprite itemSprite;
}