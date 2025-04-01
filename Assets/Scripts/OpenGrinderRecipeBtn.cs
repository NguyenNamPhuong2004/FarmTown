using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGrinderRecipeBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenGrinderRecipe();
    }
}
