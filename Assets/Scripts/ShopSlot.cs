using UnityEngine;
using System.Collections.Generic;

public class ShopSlot
{
    private List<Item> itemList = new List<Item>(); 
    private ItemType shopType;
    private Item[] shopItems;

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
                itemList.Add(itemData);
            }
        }
    }

    public bool BuyItem(int itemId, int quantity, PlacementManager placementManager)
    {
        Item item = itemList.Find(x => x.id == itemId);
        if (item == null)
        {
            Debug.Log($"Item với ID {itemId} không tồn tại trong ShopSlot {shopType}!");
            return false;
        }

        int totalCost = item.itemBuyPrice * quantity;
        if (DataPlayer.GetCoin() >= totalCost)
        {
            DataPlayer.SubCoin(totalCost);

            if (item.itemType == ItemType.Building || item.itemType == ItemType.Animal)
            {
                placementManager.StartPlacing(item);
                Debug.Log($"Đã mua {quantity} {item.itemName}, hãy đặt nó vào vị trí mong muốn!");
            }
            else
            {
                if (!DataPlayer.IsInventoryMax())
                {
                    DataPlayer.AddItemQuantity(itemId, quantity);
                    Debug.Log($"Đã thêm {quantity} {item.itemName} vào inventory của người chơi");
                }
            }
            return true;
        }
        Debug.Log("Không đủ tiền!");
        return false;
    }

    public List<Item> GetItems()
    {
        return itemList;
    }

}