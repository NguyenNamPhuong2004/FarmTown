using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDeliveryBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenDelivery();
    }
}
