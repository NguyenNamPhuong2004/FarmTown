using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryUI : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private Text totalPriceText;
    [SerializeField] private Text skipCountText;
    [SerializeField] private Button deliverButton;
    [SerializeField] private Button skipButton;

    private DeliverySystem deliverySystem;
    private List<GameObject> itemUIObjects = new List<GameObject>();

    private void Start()
    {
        deliverySystem = DeliverySystem.Instance;
        deliverySystem.OnOrderUpdated.AddListener(UpdateUI);
        deliverButton.onClick.AddListener(OnDeliverButtonClicked);
        skipButton.onClick.AddListener(OnSkipButtonClicked);
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (var obj in itemUIObjects)
        {
            Destroy(obj);
        }
        itemUIObjects.Clear();

        List<Sprite> sprites = deliverySystem.GetItemSprites();
        List<int> currentAmounts = deliverySystem.GetCurrentAmounts();
        List<int> requiredAmounts = deliverySystem.GetRequiredAmounts();

        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, itemContainer);
            itemUIObjects.Add(itemUI);

            Image spriteImage = itemUI.transform.Find("Sprite").GetComponent<Image>();
            Text amountText = itemUI.transform.Find("Amount").GetComponent<Text>();

            spriteImage.sprite = sprites[i];
            amountText.text = $"{currentAmounts[i]}/{requiredAmounts[i]}";
        }

        totalPriceText.text = $"Giá: {deliverySystem.GetTotalPrice()} coin";
        skipCountText.text = $"Number of skips remaining: {deliverySystem.GetSkipCountRemaining()}";
        deliverButton.interactable = deliverySystem.IsOrderDeliverable();
        skipButton.interactable = deliverySystem.CanSkipOrder();
    }

    private void OnDeliverButtonClicked()
    {
        deliverySystem.DeliverOrder();
    }

    private void OnSkipButtonClicked()
    {
        deliverySystem.SkipOrder();
    }
}