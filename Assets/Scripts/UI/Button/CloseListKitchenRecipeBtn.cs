using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CloseListKitchenRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseListKitchenRecipe();
    }
}