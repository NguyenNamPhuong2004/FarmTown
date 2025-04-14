using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenListGrinderRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenListGrinderRecipe();
    }
}
