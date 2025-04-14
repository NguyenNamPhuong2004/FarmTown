using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGrinderBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenGrinder();
    }
}
