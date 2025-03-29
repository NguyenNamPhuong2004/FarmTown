using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        foreach (InventorySlot slot in slots)
        {
            if(itemName == slot.item.itemName)
            {
                slot.AddItem(quantityAdd);
            }
        }
    } 
    public void SubItem(string itemName, int quantitySub)
    {
        foreach (InventorySlot slot in slots)
        {
            if(itemName == slot.item.itemName)
            {
                slot.SubItem(quantitySub);
            }
        }
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
