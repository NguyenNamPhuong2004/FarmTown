using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private GameObject recipeButtonPrefab;

    [SerializeField] private Transform ingredientContainer;
    [SerializeField] private GameObject ingredientItemPrefab;

    [SerializeField] private UnityEngine.UI.Image resultIcon;
    [SerializeField] private UnityEngine.UI.Text resultName;

    [SerializeField] private UnityEngine.UI.Button craftButton;
    [SerializeField] private UnityEngine.UI.Text craftButtonText;

    private Recipe selectedRecipe;
    private bool isCrafting = false;

    private void Start()
    {
        // Hi?n th? c�c c�ng th?c
        DisplayAllRecipes();

        // V� hi?u h�a n�t craft ban ??u
        craftButton.interactable = false;
        craftButtonText.text = "Ch?n c�ng th?c";
    }

    // Hi?n th? t?t c? c�ng th?c
    private void DisplayAllRecipes()
    {
        // X�a t?t c? recipe button hi?n t?i
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        List<Recipe> recipes = CraftingSystem.Instance.GetAllRecipes();

        foreach (Recipe recipe in recipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeContainer);
            RecipeButton recipeButton = buttonObj.GetComponent<RecipeButton>();

            if (recipeButton != null)
            {
                recipeButton.SetRecipe(recipe);
                recipeButton.OnRecipeClicked += SelectRecipe;
            }
        }
    }

    // X? l� khi ng??i ch?i ch?n m?t c�ng th?c
    public void SelectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;

        // Hi?n th? th�ng tin c�ng th?c
        DisplayRecipeDetails(recipe);

        // C?p nh?t tr?ng th�i n�t craft
        UpdateCraftButton();
    }

    // Hi?n th? th�ng tin chi ti?t c�ng th?c
    private void DisplayRecipeDetails(Recipe recipe)
    {
        // X�a t?t c? item trong ingredient container
        foreach (Transform child in ingredientContainer)
        {
            Destroy(child.gameObject);
        }

        // Hi?n th? nguy�n li?u
        foreach (Recipe.Ingredient ingredient in recipe.ingredients)
        {
            GameObject ingredientObj = Instantiate(ingredientItemPrefab, ingredientContainer);
            IngredientItem ingredientUI = ingredientObj.GetComponent<IngredientItem>();

            if (ingredientUI != null)
            {
                bool hasEnough = CraftingSystem.Instance.GetComponent<InventorySystem>()
                                 .HasItem(ingredient.item.itemName, ingredient.quantity);
                ingredientUI.Setup(ingredient.item, ingredient.quantity, hasEnough);
            }
        }

        // Hi?n th? k?t qu?
        resultIcon.sprite = recipe.resultItem.itemSprite;
        resultName.text = recipe.resultItem.itemName;
    }

    // C?p nh?t tr?ng th�i n�t craft
    private void UpdateCraftButton()
    {
        if (selectedRecipe == null)
        {
            craftButton.interactable = false;
            craftButtonText.text = "Ch?n c�ng th?c";
            return;
        }

        if (isCrafting)
        {
            craftButton.interactable = false;
            craftButtonText.text = "?ang ch? bi?n...";
            return;
        }

        bool canCraft = CraftingSystem.Instance.CanCraft(selectedRecipe);
        craftButton.interactable = canCraft;
        craftButtonText.text = canCraft ? "Ch? bi?n" : "Thi?u nguy�n li?u";
    }

    // X? l� n�t craft ???c nh?n
    public void OnCraftButtonClicked()
    {
        if (selectedRecipe != null && !isCrafting)
        {
            StartCoroutine(CraftWithAnimation());
        }
    }

    // Coroutine ?? x? l� qu� tr�nh ch? bi?n
    private IEnumerator CraftWithAnimation()
    {
        isCrafting = true;
        UpdateCraftButton();

        // B?t ??u craft
        yield return StartCoroutine(CraftingSystem.Instance.CraftItem(selectedRecipe));

        isCrafting = false;

        // C?p nh?t hi?n th?
        DisplayRecipeDetails(selectedRecipe);
        UpdateCraftButton();
    }

    // Hi?n th?/?n panel
    public void ToggleCraftingPanel()
    {
        craftingPanel.SetActive(!craftingPanel.activeSelf);

        if (craftingPanel.activeSelf)
        {
            // Refresh UI khi m?
            if (selectedRecipe != null)
            {
                DisplayRecipeDetails(selectedRecipe);
                UpdateCraftButton();
            }
        }
    }
}
