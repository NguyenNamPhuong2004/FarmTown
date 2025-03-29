using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemSalePrice;
    public int itemBuyPrice;
    public Sprite itemSprite;
}