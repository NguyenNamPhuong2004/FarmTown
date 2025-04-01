using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int saleQuantity;
    public Text saleQuantityText;
    public Text salePrice;
    public Image Icon;
    public Text productName;
    public Button sell;
    public Button add;
    public Button sub;
    public Button[] selectItems;
    public InventorySlot itemSlotSelected;

    private void Awake()
    {
        itemSlotSelected = slots[0];
        for ( int i = 0; i < slots.Count; i++)
        {
            int index = i;
            selectItems[i].onClick.AddListener(() => SelectItem(slots[index]));
        }
    }
    void Start()
    {
        Debug.Log("Available inventory items:");
        foreach (InventorySlot slot in slots)
        {
            Debug.Log("- " + slot.item.itemName);
        }
    }
    private void Update()
    {
        UpdateItemSelected();
    }
    private void UpdateItemSelected()
    {
        Icon.sprite = itemSlotSelected.item.itemSprite;
        productName.text = itemSlotSelected.item.itemName;
        saleQuantityText.text = saleQuantity.ToString();
        salePrice.text = (itemSlotSelected.item.itemSalePrice * saleQuantity).ToString();
    }
    private void SelectItem(InventorySlot item)
    {
        itemSlotSelected = item;
        saleQuantity = 0;
    }
    public void AddQuantityToSale()
    {
        if (saleQuantity >= itemSlotSelected.quantity) return;
        saleQuantity += 1;
    }public void SubQuantityToSale()
    {
        if (saleQuantity <= 0) return;
        saleQuantity -= 1;
    }
    public void Sell()
    {
        itemSlotSelected.SubItem(saleQuantity);
    }
    public void AddItem(string itemName, int quantityAdd)
    {
        bool itemFound = false;

        // First try to find an existing slot with this item
        foreach (InventorySlot slot in slots)
        {
            if (itemName == slot.item.itemName)
            {
                slot.AddItem(quantityAdd);
                itemFound = true;
                Debug.Log(slots.Count);
            }
        }

        // If item wasn't found in any slot, log an error
        if (!itemFound)
        {
            Debug.LogError("Could not add item to inventory: No slot found with item named '" + itemName + "'");
        }
    }
    public void SubItem(string itemName, int quantitySub)
    {
        foreach (InventorySlot slot in slots)
        {
            if(itemName == slot.item.itemName)
            {
                slot.SubItem(quantitySub);
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
}
