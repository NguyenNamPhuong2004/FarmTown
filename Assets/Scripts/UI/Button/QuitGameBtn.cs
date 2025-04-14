using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameBtn : ButtonAbstract
{
    public override void OnClick()
    {
        GameUIManager.Ins.QuitGame();
    }
}
