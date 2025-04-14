using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSettingGameBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseSetting();
    }
}
