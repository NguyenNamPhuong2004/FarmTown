using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliverySystem : MonoBehaviour
{
    public static DeliverySystem Instance;

    [SerializeField] private List<Item> availableItems;
    private DeliveryOrder currentOrder;
    public InventorySystem inventorySystem;
    private int deliveredCount = 0;
    private const int MAX_SKIPS_PER_DAY = 3;

    public UnityEvent OnOrderUpdated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentOrder = DataPlayer.GetCurrentOrder();
        if (currentOrder == null)
        {
            GenerateNewOrder();
        }
        ResetSkipsIfNewDay();
        OnOrderUpdated?.Invoke();
    }

    private void GenerateNewOrder()
    {
        currentOrder = new DeliveryOrder(availableItems);
        DataPlayer.SetCurrentOrder(currentOrder);
        OnOrderUpdated?.Invoke();
    }

    public void DeliverOrder()
    {
        if (currentOrder == null || !CanDeliverOrder())
        {
            Debug.Log("Không thể giao hàng: Thiếu nguyên liệu!");
            return;
        }

        foreach (var orderItem in currentOrder.items)
        {
            DataPlayer.SubItemQuantity(orderItem.item.id, orderItem.requiredAmount);
        }

        DataPlayer.AddCoin(currentOrder.totalPrice);
        deliveredCount++;
        MissionSystem.Instance.TrackProgress("Đơn hàng", 1, MissionType.Deliver);
        Debug.Log($"Giao hàng thành công! Nhận được {currentOrder.totalPrice} coin.");
        GenerateNewOrder();
    }

    public int GetDeliveredCount()
    {
        return deliveredCount;
    }

    public void SkipOrder()
    {
        ResetSkipsIfNewDay();

        int skipCount = DataPlayer.GetSkipCountToday();
        if (skipCount <= 0)
        {
            Debug.Log("Đã hết lượt bỏ qua hôm nay!");
            return;
        }

        DataPlayer.SetSkipCountToday(skipCount - 1);
        GenerateNewOrder();
        Debug.Log($"Đã bỏ qua đơn hàng. Lượt bỏ qua hôm nay còn lại: {DataPlayer.GetSkipCountToday()}");
    }

    private void ResetSkipsIfNewDay()
    {
        DateTime today = DateTime.Now.Date;
        DateTime lastReset = DateTime.Parse(DataPlayer.GetLastResetDate());
        if (today > lastReset)
        {
            DataPlayer.SetSkipCountToday(MAX_SKIPS_PER_DAY);
            DataPlayer.SetLastResetDate(today.ToString("yyyy-MM-dd"));
        }
    }

    private bool CanDeliverOrder()
    {
        foreach (var orderItem in currentOrder.items)
        {
            if (DataPlayer.GetItemQuantity(orderItem.item.id) < orderItem.requiredAmount)
            {
                return false;
            }
        }
        return true;
    }

    public List<Sprite> GetItemSprites()
    {
        List<Sprite> sprites = new List<Sprite>();
        if (currentOrder != null)
        {
            foreach (var orderItem in currentOrder.items)
            {
                sprites.Add(orderItem.item.itemSprite);
            }
        }
        return sprites;
    }

    public List<int> GetCurrentAmounts()
    {
        List<int> amounts = new List<int>();
        if (currentOrder != null)
        {
            foreach (var orderItem in currentOrder.items)
            {
                amounts.Add(DataPlayer.GetItemQuantity(orderItem.item.id));
            }
        }
        return amounts;
    }

    public List<int> GetRequiredAmounts()
    {
        List<int> amounts = new List<int>();
        if (currentOrder != null)
        {
            foreach (var orderItem in currentOrder.items)
            {
                amounts.Add(orderItem.requiredAmount);
            }
        }
        return amounts;
    }

    public int GetTotalPrice()
    {
        return currentOrder != null ? currentOrder.totalPrice : 0;
    }

    public int GetSkipCountRemaining()
    {
        ResetSkipsIfNewDay();
        return DataPlayer.GetSkipCountToday();
    }

    public bool IsOrderDeliverable()
    {
        return CanDeliverOrder();
    }

    public bool CanSkipOrder()
    {
        ResetSkipsIfNewDay();
        return DataPlayer.GetSkipCountToday() > 0;
    }
}