using UnityEngine;
using System;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot itemSlotSelected;

    public event Action OnInventoryUpdated; 
    public event Action<InventorySlot> OnItemSelected;
    private Dictionary<string, int> harvestedItems = new Dictionary<string, int>();

    private void Awake()
    {
        itemSlotSelected = slots[0];
    }

    private void Start()
    {
        Debug.Log("Available inventory items:");
        foreach (InventorySlot slot in slots)
        {
            slot.quantity = DataPlayer.GetItemQuantity(slot.item.id);
            Debug.Log("- " + slot.item.itemName);
        }
        OnInventoryUpdated?.Invoke();
        OnItemSelected?.Invoke(itemSlotSelected);
    }
    private void OnEnable()
    {
        Debug.Log("Available inventory items:");
        foreach (InventorySlot slot in slots)
        {
            slot.quantity = DataPlayer.GetItemQuantity(slot.item.id);
            Debug.Log("- " + slot.item.itemName);
        }
        OnInventoryUpdated?.Invoke();
        OnItemSelected?.Invoke(itemSlotSelected);
    }
    public void SelectItem(InventorySlot item)
    {
        itemSlotSelected = item;
        OnItemSelected?.Invoke(itemSlotSelected);
        SoundManager.Ins.ButtonSound();
    }

    public void AddItem(string itemName, int quantityAdd)
    {
        bool itemFound = false;

        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName)
            {
                slot.AddItem(quantityAdd);
                itemFound = true;
                OnInventoryUpdated?.Invoke();

                if (!harvestedItems.ContainsKey(itemName))
                    harvestedItems[itemName] = 0;
                harvestedItems[itemName] += quantityAdd;
                break;
            }
        }

        if (!itemFound)
        {
            Debug.LogError("Could not add item to inventory: No slot found with item named '" + itemName + "'");
        }
    }
    public int GetHarvestedCount(string itemName)
    {
        return harvestedItems.ContainsKey(itemName) ? harvestedItems[itemName] : 0;
    }
    public void SubItem(string itemName, int quantitySub)
    {
        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName)
            {
                slot.SubItem(quantitySub);
                OnInventoryUpdated?.Invoke();
                break;
            }
        }
    }

    public int GetItemAmount(string itemName)
    {
        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName)
            {
                return slot.quantity;
            }
        }
        return 0;
    }

    public bool HasItem(string itemName, int quantity)
    {
        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName && slot.quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }
    public bool SellItem(string itemName, int quantity)
    {
        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName && slot.quantity >= quantity)
            {
                int totalEarn = slot.item.itemSalePrice * quantity;
                DataPlayer.AddCoin(totalEarn);
                slot.SubItem(quantity);
                OnInventoryUpdated?.Invoke();
                Debug.Log($"Đã bán {quantity} {itemName} với giá {totalEarn}");
                return true;
            }
        }
        Debug.Log("Không đủ item trong inventory để bán!");
        return false;
    }
    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    public InventorySlot GetSelectedSlot()
    {
        return itemSlotSelected;
    }
}