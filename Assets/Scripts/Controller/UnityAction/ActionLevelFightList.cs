using UnityEngine;
using System.Collections;

public class ActionLevelFightList : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        //todo 关闭购买框  
        C2sSprotoType.GuanQiaBattleList.response resp = ActParam["resp"] as C2sSprotoType.GuanQiaBattleList.response;
        if(resp.errorcode == 112)
        {
            BattleManager.Instance.BattleDataError();
            return false;
        }
        else if(resp.errorcode == 1){ 

            if (BattleManager.Instance.roleData.curFightPower < 0)
            {
                BattleManager.Instance.BattleOver(BattleManager.Instance.roleData.csv_id);
            }
            else if (BattleManager.Instance.emenyData.curFightPower < 0)
            {
                BattleManager.Instance.BattleOver(BattleManager.Instance.emenyData.csv_id);
            }
        }
        return true;
    }
}
