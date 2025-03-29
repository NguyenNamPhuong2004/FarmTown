using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI cho hiển thị các công thức
public class RecipeButton : MonoBehaviour
{
    [SerializeField] private Image recipeIcon;
    [SerializeField] private Text recipeName;

    private Recipe recipe;

    public event Action<Recipe> OnRecipeClicked;

    public void SetRecipe(Recipe newRecipe)
    {
        recipe = newRecipe;

        if (recipe != null)
        {
            recipeName.text = recipe.recipeName;
            recipeIcon.sprite = recipe.resultItem.itemSprite;
        }
    }

    public void OnButtonClick()
    {
        if (recipe != null && OnRecipeClicked != null)
        {
            OnRecipeClicked.Invoke(recipe);
        }
    }
}
