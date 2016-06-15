using UnityEngine;
using System.Collections;

public class ActionLevelFightBegin : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false; 
            LevelsMgr.Instance.EnterBattleLevelCallback(); 
        return true;
    }
}