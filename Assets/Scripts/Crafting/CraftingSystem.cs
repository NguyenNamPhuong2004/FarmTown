using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private List<Recipe> recipes;
    public event Action OnWaitingSlotsUpdated;
    private Dictionary<string, int> craftedItems = new Dictionary<string, int>();

    public int waitingSlotUICount = 3;
    private void Awake() => Instance = this;

    public bool CanCraft(Recipe recipe)
    {
        if (DataPlayer.GetWaitingSlotListCount() >= waitingSlotUICount)
        {
            Debug.Log("Waiting slot is filled");
            return false;
        }
        if (!playerInventory.HasItem(recipe.ingredients[0].item.itemName, recipe.ingredients[0].quantity)) return false;
        if (!playerInventory.HasItem(recipe.ingredients[1].item.itemName, recipe.ingredients[1].quantity)) return false;
        if (recipe.ingredients[0].item == recipe.ingredients[1].item)
        {
            if(!playerInventory.HasItem(recipe.ingredients[1].item.itemName, recipe.ingredients[0].quantity + recipe.ingredients[1].quantity))
            return false;
        }
        return true;
    }

    public void Craft(Recipe recipe)
    {
        if (!CanCraft(recipe)) return;
        
        foreach (var ingredient in recipe.ingredients)
            playerInventory.SubItem(ingredient.item.itemName, ingredient.quantity);

        DataPlayer.AddWaitingSlot(new WaitingSlot(recipe));
        OnWaitingSlotsUpdated?.Invoke();

        string productName = recipe.resultItem.itemName;
        if (!craftedItems.ContainsKey(productName))
            craftedItems[productName] = 0;
        craftedItems[productName]++;
        MissionSystem.Instance.TrackProgress(productName, 1, MissionType.Cook);
        SoundManager.Ins.Proceed();
    }

    public int GetCookedCount(string itemName)
    {
        return craftedItems.ContainsKey(itemName) ? craftedItems[itemName] : 0;
    }

    public void CollectItem(WaitingSlot slot)
    {
        if (!slot.IsCompleted()) return;

        playerInventory.AddItem(slot.productItem.itemName, 1);
        DataPlayer.RemoveWaitingSlot(slot);
        OnWaitingSlotsUpdated?.Invoke();
        SoundManager.Ins.Proceed();
    }
    public Recipe GetRecipe(int index)
    {
        if (index >= 0 && index < recipes.Count)
            return recipes[index];
        return null;
    }
    public int GetPlayerInventoryAmount(string itemName)
    {
        Debug.Log(playerInventory.GetItemAmount(itemName));
        return playerInventory.GetItemAmount(itemName);
    }
}
