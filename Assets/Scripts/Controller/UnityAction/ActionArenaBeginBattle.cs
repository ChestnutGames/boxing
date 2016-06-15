using UnityEngine;
using System.Collections;

public class ActionArenaBeginBattle : LogicAction
{

    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        C2sSprotoType.BeginArenaCoreFight.response resp = new C2sSprotoType.BeginArenaCoreFight.response();

        ArenaMgr.Instance.BattleEnterCallback(resp);
        return true;
    }
}
