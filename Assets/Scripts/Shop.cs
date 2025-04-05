using UnityEngine;
using System;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    private Dictionary<ItemType, ShopSlot> shopSlots = new Dictionary<ItemType, ShopSlot>();
    private InventorySystem inventory;
    private PlacementManager placementManager;
    private int playerMoney = 1000;
    [SerializeField] private Item[] allItems;

    public event Action<ItemType> OnShopUpdated; 
    public event Action<int> OnMoneyUpdated; 

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        placementManager = FindObjectOfType<PlacementManager>();
        InitializeShopSlots();
        OnShopUpdated?.Invoke(ItemType.Crop); 
        OnMoneyUpdated?.Invoke(playerMoney);
    }

    void InitializeShopSlots()
    {
        shopSlots[ItemType.Crop] = new ShopSlot(ItemType.Crop, allItems);
        shopSlots[ItemType.Animal] = new ShopSlot(ItemType.Animal, allItems);
        shopSlots[ItemType.Building] = new ShopSlot(ItemType.Building, allItems);
        shopSlots[ItemType.Food] = new ShopSlot(ItemType.Food, allItems);
        shopSlots[ItemType.Other] = new ShopSlot(ItemType.Other, allItems);
    }

    public bool BuyFromShop(ItemType shopType, int itemId, int quantity = 1)
    {
        if (shopSlots.ContainsKey(shopType))
        {
            bool success = shopSlots[shopType].BuyItem(itemId, quantity, ref playerMoney, inventory, placementManager);
            if (success)
            {
                OnMoneyUpdated?.Invoke(playerMoney);
                OnShopUpdated?.Invoke(shopType);
            }
            return success;
        }
        Debug.Log("ShopSlot không tồn tại!");
        return false;
    }

    public bool SellToShop(ItemType shopType, int itemId, int quantity = 1)
    {
        if (shopSlots.ContainsKey(shopType))
        {
            bool success = shopSlots[shopType].SellItem(itemId, quantity, ref playerMoney, inventory);
            if (success)
            {
                OnMoneyUpdated?.Invoke(playerMoney);
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

    public int GetStock(int itemId)
    {
        return DataPlayer.GetItemQuantity(itemId);
    }

    public int GetPlayerMoney()
    {
        return playerMoney;
    }
}