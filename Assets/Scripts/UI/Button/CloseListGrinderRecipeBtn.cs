using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseListGrinderRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseListGrinderRecipe();
    }
}
