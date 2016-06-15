using UnityEngine;
using System.Collections;

public class ActionArenaBuyRefresh : LogicAction {

    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        //todo 关闭购买框
        ArenaPop pop = UIManager.Instance.GetWindow<ArenaPop>();

        ArenaMgr.Instance.refreshCount++;

        C2sSprotoType.ara_clg_tms_purchase.response resp = ActParam["resp"] as C2sSprotoType.ara_clg_tms_purchase.response;
        

        return true;
    }
}
