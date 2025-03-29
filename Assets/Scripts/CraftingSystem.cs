using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    // Singleton
    public static CraftingSystem Instance { get; private set; }

    [SerializeField] private List<Recipe> availableRecipes = new List<Recipe>();
    [SerializeField] private InventorySystem playerInventory;

    public event Action<Item, int> OnCraftComplete;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Kiểm tra có thể craft không
    public bool CanCraft(Recipe recipe)
    {
        // Kiểm tra nguyên liệu
        foreach (Recipe.Ingredient ingredient in recipe.ingredients)
        {
            if (!playerInventory.HasItem(ingredient.item.itemName, ingredient.quantity))
            {
                return false;
            }
        }

        return true;
    }

    // Thực hiện craft
    public IEnumerator CraftItem(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.LogWarning("Không thể chế biến: " + recipe.recipeName);
            yield break;
        }

        // Xóa nguyên liệu
        foreach (Recipe.Ingredient ingredient in recipe.ingredients)
        {
            playerInventory.SubItem(ingredient.item.itemName, ingredient.quantity);
        }

        // Thời gian chế biến
        float craftingTimer = 0f;
        while (craftingTimer < recipe.craftingTime)
        {
            craftingTimer += Time.deltaTime;
            yield return null;
        }

        // Thêm sản phẩm vào kho đồ
        playerInventory.AddItem(recipe.resultItem.itemName, recipe.resultAmount);

        // Gọi sự kiện chế biến hoàn thành
        OnCraftComplete?.Invoke(recipe.resultItem, recipe.resultAmount);

        Debug.Log("Đã chế biến thành công: " + recipe.recipeName);
    }

    // Lấy tất cả công thức
    public List<Recipe> GetAllRecipes()
    {
        return availableRecipes;
    }
}
