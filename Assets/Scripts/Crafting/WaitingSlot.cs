using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class WaitingSlot
{
    public string finishTime;
    public Item productItem;

    public WaitingSlot(Recipe recipe)
    {
        finishTime = DateTime.Now.AddSeconds(recipe.craftingTime).ToString("yyyy-MM-ddTHH:mm:ss");
        productItem = recipe.resultItem;
    }

    public bool IsCompleted() => DateTime.Now >= DateTime.ParseExact(finishTime, "yyyy-MM-ddTHH:mm:ss", null);
    public string GetRemainingTime() => IsCompleted() ? "00:00:00:00" : (DateTime.ParseExact(finishTime, "yyyy-MM-ddTHH:mm:ss", null) - DateTime.Now).ToString(@"dd\:hh\:mm\:ss");
}
