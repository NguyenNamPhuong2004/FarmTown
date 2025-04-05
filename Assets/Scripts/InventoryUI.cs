using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem; 
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button[] selectItems; 
    [SerializeField] private Text saleQuantityText;
    [SerializeField] private Text salePriceText;
    [SerializeField] private Image icon;
    [SerializeField] private Text productName;
    [SerializeField] private Button sellBtn;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button subBtn;

    private int saleQuantity = 0;

    void Start()
    {
        inventorySystem.OnInventoryUpdated += UpdateInventoryUI;
        inventorySystem.OnItemSelected += UpdateSelectedItemUI;

        for (int i = 0; i < selectItems.Length; i++)
        {
            int index = i;
            selectItems[i].onClick.AddListener(() => inventorySystem.SelectItem(inventorySystem.GetSlots()[index]));
        }

        sellBtn.onClick.AddListener(Sell);
        addBtn.onClick.AddListener(AddQuantityToSale);
        subBtn.onClick.AddListener(SubQuantityToSale);

        UpdateInventoryUI();
    }

   
    private void UpdateInventoryUI()
    {
        List<InventorySlot> slots = inventorySystem.GetSlots();
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < selectItems.Length)
            {
                Text quantityText = selectItems[i].transform.Find("QuantityText")?.GetComponent<Text>();
                if (quantityText != null)
                {
                    quantityText.text = slots[i].quantity.ToString();
                    selectItems[i].gameObject.SetActive(slots[i].quantity > 0);
                }
            }
        }
    }

    private void UpdateSelectedItemUI(InventorySlot selectedSlot)
    {
        icon.sprite = selectedSlot.item.itemSprite;
        productName.text = selectedSlot.item.itemName;
        saleQuantity = 0;
        saleQuantityText.text = saleQuantity.ToString();
        salePriceText.text = (selectedSlot.item.itemSalePrice * saleQuantity).ToString();
    }

    private void AddQuantityToSale()
    {
        InventorySlot selectedSlot = inventorySystem.GetSelectedSlot();
        if (saleQuantity >= selectedSlot.quantity) return;
        saleQuantity++;
        saleQuantityText.text = saleQuantity.ToString();
        salePriceText.text = (selectedSlot.item.itemSalePrice * saleQuantity).ToString();
    }

    private void SubQuantityToSale()
    {
        if (saleQuantity <= 0) return;
        saleQuantity--;
        InventorySlot selectedSlot = inventorySystem.GetSelectedSlot();
        saleQuantityText.text = saleQuantity.ToString();
        salePriceText.text = (selectedSlot.item.itemSalePrice * saleQuantity).ToString();
    }

    private void Sell()
    {
        InventorySlot selectedSlot = inventorySystem.GetSelectedSlot();
        FindObjectOfType<Shop>().SellToShop(selectedSlot.item.itemType, selectedSlot.item.id, saleQuantity);
        saleQuantity = 0;
        UpdateSelectedItemUI(selectedSlot);
    }
}