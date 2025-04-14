using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpenKitchenRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenKitchenRecipe();
    }
}