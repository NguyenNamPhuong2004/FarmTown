using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSlot
{
    public string productName;
    public Sprite productSprite;    
    public DateTime finishTime;

    public WaitingSlot(Recipe recipe)
    {
        productName = recipe.resultItem.itemName;
        productSprite = recipe.resultItem.itemSprite;
        finishTime = DateTime.Now.AddSeconds(recipe.craftingTime);
    }

    public bool IsCompleted() => DateTime.Now >= finishTime;
    public string GetRemainingTime() => IsCompleted() ? "00:00:00:00" : (finishTime - DateTime.Now).ToString(@"dd\:hh\:mm\:ss");
}
