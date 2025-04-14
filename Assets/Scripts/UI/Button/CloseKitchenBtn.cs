using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseKitchenBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseKitchen();
    }
}
