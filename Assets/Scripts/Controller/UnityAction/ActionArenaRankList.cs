using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using UnityEngine;


public class ActionArenaRankList : LogicAction
{

    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        ArenaRankPop pop = UIManager.Instance.GetWindow<ArenaRankPop>();
        C2sSprotoType.ara_lp.response resp = ActParam["resp"] as C2sSprotoType.ara_lp.response;
        if (resp.errorcode != 1)
            return false;
        pop.rank.text = ArenaMgr.Instance.integral.ToString();
        List<ArenaUserData> list = new List<ArenaUserData>();
        for (int i = 0; i < resp.lp.Count; i++)
        {
            ArenaUserData u = new ArenaUserData();
            u.csv_id = (int)resp.lp[i].uid;
            u.uname = resp.lp[i].uname;
            u.total_combat = (int)resp.lp[i].total_combat;
            u.ara_rnk = (int)resp.lp[i].ranking;
            u.iconid = (int)resp.lp[i].iconid;
            list.Add(u);
        } 
        EventManager.Trigger<ArenaRank100List>(new ArenaRank100List(list));
        return true; 
    }
}
 