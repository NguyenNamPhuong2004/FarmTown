using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingSlotUI : MonoBehaviour
{
    public GameObject slotObject;
    public GameObject lockSlot;
    public Image productIcon;
    public Text timerText;
    public Text productName;

    public void UpdateSlot(WaitingSlot slot, Action onCollect)
    {
        slotObject.SetActive(true);
        productIcon.sprite = slot.productItem.itemSprite;
        timerText.text = slot.GetRemainingTime();
        productName.text = slot.productItem.itemName;

        slotObject.GetComponent<Button>().onClick.RemoveAllListeners();
        slotObject.GetComponent<Button>().onClick.AddListener(() => onCollect());
    }

    public void HideSlot()
    {
        slotObject.SetActive(false);
    }
    public void UnlockSlot()
    {
        if (lockSlot == null) return;
        lockSlot.SetActive(false);
    }
}
