using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMissionBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.CloseMission();
    }
}
