using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpenListKitchenRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenListKitchenRecipe();
    }
}