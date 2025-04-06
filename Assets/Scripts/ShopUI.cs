using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Shop shop; 
    [SerializeField] private Transform shopContent;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Text moneyText;
    [SerializeField] private Button[] shopTypeButtons; 

    private void Start()
    {
        shop.OnShopUpdated += DisplayShopUI;      
        for (int i = 0; i < shopTypeButtons.Length; i++)
        {
            ItemType type = (ItemType)i;
            shopTypeButtons[i].onClick.AddListener(() => DisplayShopUI(type));
        }

        DisplayShopUI(ItemType.Product);
    }
    private void OnEnable()
    {
        DisplayShopUI(ItemType.Product);
    }
    private void DisplayShopUI(ItemType shopType)
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }

        List<Item> items = shop.GetShopItems(shopType);
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(shopItemPrefab, shopContent);
            slot.transform.Find("ItemSprite").GetComponent<Image>().sprite = item.itemSprite;
            slot.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;
            slot.transform.Find("Buy").Find("MoneyText").GetComponent<Text>().text = item.itemBuyPrice.ToString();
            Button buyButton = slot.transform.Find("Buy").GetComponent<Button>();
            buyButton.onClick.AddListener(() => shop.BuyFromShop(shopType, item.id));
        }
    }
}