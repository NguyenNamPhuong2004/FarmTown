using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDeliveryBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseDelivery();
    }
}
