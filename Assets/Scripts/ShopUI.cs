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

    void Start()
    {
        shop.OnShopUpdated += DisplayShopUI;
        shop.OnMoneyUpdated += UpdateMoneyUI;

        
        for (int i = 0; i < shopTypeButtons.Length; i++)
        {
            ItemType type = (ItemType)i;
            shopTypeButtons[i].onClick.AddListener(() => DisplayShopUI(type));
        }

        DisplayShopUI(ItemType.Crop);
    }

    void OnDestroy()
    {
        shop.OnShopUpdated -= DisplayShopUI;
        shop.OnMoneyUpdated -= UpdateMoneyUI;
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
            slot.transform.Find("Icon").GetComponent<Image>().sprite = item.itemSprite;
            slot.transform.Find("Name").GetComponent<Text>().text = item.itemName;
            slot.transform.Find("BuyPrice").GetComponent<Text>().text = $"Mua: {item.itemBuyPrice}";
            Button buyButton = slot.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => shop.BuyFromShop(shopType, item.id));
        }
    }

    private void UpdateMoneyUI(int money)
    {
        moneyText.text = $"Tiền: {money}";
    }
}