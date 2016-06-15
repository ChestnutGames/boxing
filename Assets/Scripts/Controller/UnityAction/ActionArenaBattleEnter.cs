using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 竞技场进入战斗
/// </summary>
public class ActionArenaBattleEnter : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        C2sSprotoType.BeginArenaCoreFight.response resp = ActParam["resp"] as C2sSprotoType.BeginArenaCoreFight.response;

        ArenaMgr.Instance.BattleEnterCallback(resp); 
        return true;
    }
}
