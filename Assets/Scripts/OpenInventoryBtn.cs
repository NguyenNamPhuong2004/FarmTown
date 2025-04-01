using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventoryBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenInventory();
    }
}
