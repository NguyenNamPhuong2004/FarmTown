using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMissionBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.OpenMission();
    }
}
