using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI Instance;

    [Header("Recipe Selection UI")]
    [SerializeField] private List<Button> recipeButtons;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Text resultName;
    [SerializeField] private Button craftButton;
    private Recipe selectedRecipe;

    [SerializeField] private Image ingredientIcon1, ingredientIcon2;
    [SerializeField] private Text ingredientName1, ingredientName2;
    [SerializeField] private Text ingredientAmount1, ingredientAmount2;

    [Header("Waiting List UI")]
    [SerializeField] private List<WaitingSlotUI> waitingSlotsUI;

    private void Awake() => Instance = this;

    private void Start()
    {
        UpdateWaitingSlots();
        CraftingSystem.Instance.OnWaitingSlotsUpdated += UpdateWaitingSlots;

        for (int i = 0; i < recipeButtons.Count; i++)
        {
            int index = i;
            recipeButtons[i].onClick.AddListener(() => SelectRecipe(CraftingSystem.Instance.GetRecipe(index)));
        }
        craftButton.onClick.AddListener(CraftSelectedRecipe);
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < DataPlayer.allData.waitingSlotList.Count; i++)
        {
            int index = i;
            WaitingSlot slot = DataPlayer.GetWaitingSlot(index);          
            waitingSlotsUI[i].timerText.text = slot.GetRemainingTime();
        }
    }
    private void SelectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;
        resultIcon.sprite = recipe.resultItem.itemSprite;
        resultName.text = $"{recipe.resultItem.itemName} x{recipe.resultAmount}";

        var ingredient1 = recipe.ingredients[0];
        ingredientIcon1.sprite = ingredient1.item.itemSprite;
        ingredientName1.text = ingredient1.item.itemName;
        int playerAmount1 = DataPlayer.GetItemQuantity(ingredient1.item.id);
        ingredientAmount1.text = $"{playerAmount1}/{ingredient1.quantity}";
        ingredientAmount1.color = playerAmount1 >= ingredient1.quantity ? Color.green : Color.red;

        var ingredient2 = recipe.ingredients[1];
        ingredientIcon2.sprite = ingredient2.item.itemSprite;
        ingredientName2.text = ingredient2.item.itemName;
        int playerAmount2 = DataPlayer.GetItemQuantity(ingredient2.item.id);
        ingredientAmount2.text = $"{playerAmount2}/{ingredient2.quantity}";
        ingredientAmount2.color = playerAmount2 >= ingredient2.quantity ? Color.green : Color.red;
        if (ingredient1.item == ingredient2.item) 
        {
            ingredientAmount2.text = playerAmount2 >= ingredient1.quantity + ingredient2.quantity || playerAmount1 < ingredient1.quantity ? $"{playerAmount2}/{ingredient2.quantity}": $"{playerAmount2 - ingredient1.quantity}/{ingredient2.quantity}";
            ingredientAmount2.color = playerAmount2 >= ingredient1.quantity + ingredient2.quantity ? Color.green : Color.red; 
        }

        craftButton.interactable = CraftingSystem.Instance.CanCraft(recipe);
    }

    private void CraftSelectedRecipe()
    {
        if (selectedRecipe != null)
        {
            CraftingSystem.Instance.Craft(selectedRecipe);
            SelectRecipe(selectedRecipe);
        }
    }

    private void UpdateWaitingSlots()
    {
        for (int i = 0; i < waitingSlotsUI.Count; i++)
        {
            if (i < DataPlayer.GetWaitingSlotListCount())
            {
                int index = i;
                waitingSlotsUI[i].UpdateSlot(DataPlayer.GetWaitingSlot(i), () => CraftingSystem.Instance.CollectItem(DataPlayer.GetWaitingSlot(index)));
            }
            else
            {
                waitingSlotsUI[i].HideSlot();
            }
        }
    }
    public void UnLockSlot()
    {
        int nextUnlockSlot = CraftingSystem.Instance.waitingSlotUICount;
        waitingSlotsUI[nextUnlockSlot].UnlockSlot();
        CraftingSystem.Instance.waitingSlotUICount += 1;
    }
}
