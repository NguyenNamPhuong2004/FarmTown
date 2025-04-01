using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGrinderRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseGrinderRecipe();
    }
}
