using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<ShopSlot> slots = new List<ShopSlot>();

    public Text costText;
    public Image Icon;
    public Text productName;
    public Button buy;
    public Button[] selectItems;
    public ShopSlot itemSlotSelected;
    public InventorySystem inventory;

    private void Awake()
    {
        itemSlotSelected = slots[0];
        for (int i = 0; i < slots.Count; i++)
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
        costText.text = itemSlotSelected.item.itemBuyPrice.ToString();
    }
    private void SelectItem(ShopSlot item)
    {
        itemSlotSelected = item;
    }
    public void Buy()
    {
        inventory.AddItem(itemSlotSelected.item.itemName, 1);
    }   
}
