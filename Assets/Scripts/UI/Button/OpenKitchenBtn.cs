using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKitchenBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenKitchen();
    }
}
