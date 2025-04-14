using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem; 
    [SerializeField] private Button[] selectItems; 
    [SerializeField] private Text saleQuantityText;
    [SerializeField] private Text salePriceText;
    [SerializeField] private Image icon;
    [SerializeField] private Text productName;
    [SerializeField] private Button sellBtn;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button subBtn;
    [SerializeField] private Text InventoryLimit;

    private int saleQuantity = 0;

    void Start()
    {
        inventorySystem.OnInventoryUpdated += UpdateInventoryUI;
        inventorySystem.OnItemSelected += UpdateSelectedItemUI;
        selectItems = transform.Find("Viewport").GetComponentsInChildren<Button>();
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
        InventoryLimit.text = "Limit: " + DataPlayer.GetTotalItemQuantity() + "/" + DataPlayer.GetInventoryMax();
        List<InventorySlot> slots = inventorySystem.GetSlots();
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < selectItems.Length)
            {
                Image itemSprite = selectItems[i].transform.GetComponent<Image>();
                if (itemSprite != null)
                {
                    itemSprite.sprite = slots[i].item.itemSprite;
                }
                Text quantityText = selectItems[i].transform.Find("Quantity").GetComponent<Text>();
                if (quantityText != null)
                {
                    quantityText.text = "x " + slots[i].quantity;
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
        SoundManager.Ins.ButtonSound();
    }

    private void SubQuantityToSale()
    {
        if (saleQuantity <= 0) return;
        saleQuantity--;
        InventorySlot selectedSlot = inventorySystem.GetSelectedSlot();
        saleQuantityText.text = saleQuantity.ToString();
        salePriceText.text = (selectedSlot.item.itemSalePrice * saleQuantity).ToString();
        SoundManager.Ins.ButtonSound();
    }

    private void Sell()
    {
        InventorySlot selectedSlot = inventorySystem.GetSelectedSlot();
        if (inventorySystem.SellItem(selectedSlot.item.itemName, saleQuantity))
        {
            saleQuantity = 0;
            UpdateSelectedItemUI(selectedSlot);
            SoundManager.Ins.CollectCoin();
        }
    }
}