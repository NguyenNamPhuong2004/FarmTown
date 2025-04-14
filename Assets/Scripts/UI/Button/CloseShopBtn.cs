using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseShopBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseShop();
    }
}
