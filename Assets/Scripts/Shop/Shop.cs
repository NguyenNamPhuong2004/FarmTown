using UnityEngine;
using System;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    private Dictionary<ItemType, ShopSlot> shopSlots = new Dictionary<ItemType, ShopSlot>();   
    private PlacementManager placementManager;
    [SerializeField] private Item[] allItems;

    public event Action<ItemType> OnShopUpdated;

    void Start()
    {
        placementManager = FindObjectOfType<PlacementManager>();
        InitializeShopSlots();
        OnShopUpdated?.Invoke(ItemType.Plant);
    }

    void InitializeShopSlots()
    {
        shopSlots[ItemType.Plant] = new ShopSlot(ItemType.Plant, allItems);
        shopSlots[ItemType.Animal] = new ShopSlot(ItemType.Animal, allItems);
        shopSlots[ItemType.Building] = new ShopSlot(ItemType.Building, allItems);
        shopSlots[ItemType.Tree] = new ShopSlot(ItemType.Tree, allItems);
    }

    public bool BuyFromShop(ItemType shopType, int itemId, int quantity = 1)
    {
        if (shopSlots.ContainsKey(shopType))
        {
            bool success = shopSlots[shopType].BuyItem(itemId, quantity, placementManager);
            if (success)
            {

                OnShopUpdated?.Invoke(shopType);
            }
            return success;
        }
        Debug.Log("ShopSlot không tồn tại!");
        return false;
    } 

    public List<Item> GetShopItems(ItemType shopType)
    {
        return shopSlots.ContainsKey(shopType) ? shopSlots[shopType].GetItems() : new List<Item>();
    }

}