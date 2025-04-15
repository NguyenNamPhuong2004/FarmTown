using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class DeliveryOrder
{
    public List<OrderItem> items;
    public int totalPrice;

    [Serializable]
    public class OrderItem
    {
        public Item item;
        public int requiredAmount;
    }

    public DeliveryOrder(List<Item> availableItems)
    {
        items = new List<OrderItem>();
        int baseTotal = 0;

        int itemCount = UnityEngine.Random.Range(2, 4);
        List<Item> selectedItems = new List<Item>(availableItems);
        Shuffle(selectedItems);

        for (int i = 0; i < itemCount && i < selectedItems.Count; i++)
        {
            Item item = selectedItems[i];
            int amount = RandomAmount(item);
            items.Add(new OrderItem { item = item, requiredAmount = amount });
            baseTotal += item.itemSalePrice * amount;
        }

        float variation = UnityEngine.Random.Range(-0.2f, 0.2f);
        totalPrice = Mathf.RoundToInt(baseTotal * (1 + variation));
        totalPrice = Mathf.Max(totalPrice, 1);
    }

    private int RandomAmount(Item item)
    {
        int amount = 0;
        switch (item.itemType)
        {
            case ItemType.Plant:
                amount = UnityEngine.Random.Range(5,16);
                break; 
            case ItemType.Product:
                amount = UnityEngine.Random.Range(5, 11);
                break; 
            case ItemType.Food:
                amount = UnityEngine.Random.Range(3, 6);
                break; 
        }
        return amount;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}