using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettingGameBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenSetting();
    }
}
