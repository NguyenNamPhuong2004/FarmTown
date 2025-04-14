using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseKitchenRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseKitchenRecipe();
    }
}
