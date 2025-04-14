using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGrinderBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseGrinder();
    }
}
