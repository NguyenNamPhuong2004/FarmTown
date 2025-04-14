using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseInventoryBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseInventory();
    }
}
