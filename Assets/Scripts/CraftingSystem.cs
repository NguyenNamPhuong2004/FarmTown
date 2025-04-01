using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private List<Recipe> recipes;
    private List<WaitingSlot> waitingSlots = new List<WaitingSlot>();

    public event Action<List<WaitingSlot>> OnWaitingSlotsUpdated;
    private void Awake() => Instance = this;

    public bool CanCraft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!playerInventory.HasItem(ingredient.item.itemName, ingredient.quantity))
                return false;
        }
        return true;
    }

    public void Craft(Recipe recipe)
    {
        if (!CanCraft(recipe)) return;

        foreach (var ingredient in recipe.ingredients)
            playerInventory.SubItem(ingredient.item.itemName, ingredient.quantity);

        waitingSlots.Add(new WaitingSlot(recipe));
        OnWaitingSlotsUpdated?.Invoke(waitingSlots);
    }

    public void CollectItem(WaitingSlot slot)
    {
        if (!slot.IsCompleted()) return;

        playerInventory.AddItem(slot.productName, 1);
        waitingSlots.Remove(slot);
        OnWaitingSlotsUpdated?.Invoke(waitingSlots);
    }
    public Recipe GetRecipe(int index)
    {
        if (index >= 0 && index < recipes.Count)
            return recipes[index];
        return null;
    }
    public int GetPlayerInventoryAmount(string itemName)
    {
        return playerInventory.GetItemAmount(itemName);
    }
}
