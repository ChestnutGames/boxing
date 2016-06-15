using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionAraConvertPts : LogicAction
{ 
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        ArenaPointPop pop = UIManager.Instance.GetWindow<ArenaPointPop>();
        C2sSprotoType.ara_convert_pts.response resp = ActParam["resp"] as C2sSprotoType.ara_convert_pts.response;
        ArenaMgr.Instance.pointRewardList = new List<long>();
        if (resp.cl != null)
        {
            for (int i = 0; i < resp.cl.Count; i++)
            {
                long uname = resp.cl[i].integral;
                ArenaMgr.Instance.pointRewardList.Add(uname);
            }
        } 
        pop.SetRewared();
        if (resp.props != null)
        {
            for (int i = 0; i < resp.props.Count; i++)
            {
                BagMgr.Instance.AddItemNumById((int)resp.props[i].csv_id, (int)resp.props[i].num);
            }
        }
        return true;
    }
}
