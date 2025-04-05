using UnityEngine;
using System;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot itemSlotSelected;

    public event Action OnInventoryUpdated; 
    public event Action<InventorySlot> OnItemSelected; 

    private void Awake()
    {
        itemSlotSelected = slots[0];
    }

    void Start()
    {
        Debug.Log("Available inventory items:");
        foreach (InventorySlot slot in slots)
        {
            Debug.Log("- " + slot.item.itemName);
        }
        OnInventoryUpdated?.Invoke();
        OnItemSelected?.Invoke(itemSlotSelected);
    }

    public void SelectItem(InventorySlot item)
    {
        itemSlotSelected = item;
        OnItemSelected?.Invoke(itemSlotSelected);
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
                break;
            }
        }

        if (!itemFound)
        {
            Debug.LogError("Could not add item to inventory: No slot found with item named '" + itemName + "'");
        }
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

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    public InventorySlot GetSelectedSlot()
    {
        return itemSlotSelected;
    }
}