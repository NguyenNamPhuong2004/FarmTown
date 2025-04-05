using UnityEngine;
using System.Collections.Generic;

public class ShopSlot
{
    private List<Item> itemList = new List<Item>(); 
    private ItemType shopType;
    private Item[] shopItems;
    private int initialStock = 10;

    public ShopSlot(ItemType type, Item[] items)
    {
        shopType = type;
        shopItems = items;
        InitializeShop();
    }

    private void InitializeShop()
    {
        foreach (Item itemData in shopItems)
        {
            if (itemData.itemType == shopType)
            {
                itemList.Add(new Item
                {
                    id = itemData.id,
                    itemName = itemData.itemName,
                    itemSalePrice = itemData.itemSalePrice,
                    itemBuyPrice = itemData.itemBuyPrice,
                    itemType = itemData.itemType,
                    itemSprite = itemData.itemSprite
                });
                if (DataPlayer.GetItemQuantity(itemData.id) == 0)
                {
                    DataPlayer.AddItemQuantity(itemData.id, initialStock);
                }
            }
        }
    }

    public bool BuyItem(int itemId, int quantity, ref int playerMoney, InventorySystem inventory, PlacementManager placementManager)
    {
        Item item = itemList.Find(x => x.id == itemId); 
        if (item == null) return false;

        int stock = DataPlayer.GetItemQuantity(itemId);
        if (stock < quantity)
        {
            Debug.Log($"Không đủ {item.itemName} trong kho!");
            return false;
        }

        int totalCost = item.itemBuyPrice * quantity;
        if (playerMoney >= totalCost)
        {
            playerMoney -= totalCost;
            DataPlayer.SubItemQuantity(itemId, quantity);

            if (item.itemType == ItemType.Building || item.itemType == ItemType.Animal)
            {
                placementManager.StartPlacing(item, quantity);
                Debug.Log($"Đã mua {quantity} {item.itemName}, hãy đặt nó vào vị trí mong muốn!");
            }
            else
            {
                inventory.AddItem(item.itemName, quantity);
                Debug.Log($"Đã thêm {quantity} {item.itemName} vào inventory");
            }
            return true;
        }
        Debug.Log("Không đủ tiền!");
        return false;
    }

    public bool SellItem(int itemId, int quantity, ref int playerMoney, InventorySystem inventory)
    {
        Item item = itemList.Find(x => x.id == itemId); 
        if (item == null) return false;

        if (!inventory.HasItem(item.itemName, quantity))
        {
            Debug.Log("Không đủ item trong inventory để bán!");
            return false;
        }

        int totalEarn = item.itemSalePrice * quantity;
        playerMoney += totalEarn;
        inventory.SubItem(item.itemName, quantity);
        DataPlayer.AddItemQuantity(itemId, quantity);
        Debug.Log($"Đã bán {quantity} {item.itemName} với giá {totalEarn}");
        return true;
    }

    public void DisplayItems()
    {
        Debug.Log($"=== ShopSlot {shopType} ===");
        foreach (Item item in itemList)
        {
            int stock = DataPlayer.GetItemQuantity(item.id);
            Debug.Log($"{item.itemName} (ID: {item.id}) - Mua: {item.itemBuyPrice} - Bán: {item.itemSalePrice} - Còn: {stock}");
        }
    }

    public List<Item> GetItems()
    {
        return itemList;
    }
}