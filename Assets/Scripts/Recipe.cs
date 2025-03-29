using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Items/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Item resultItem;
    public int resultAmount = 1;

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int quantity;
    }

    public List<Ingredient> ingredients = new List<Ingredient>();
    public float craftingTime = 1.0f; 
}
