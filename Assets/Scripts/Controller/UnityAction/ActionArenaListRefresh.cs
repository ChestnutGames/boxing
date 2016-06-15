using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class ActionArenaList : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false; 
        C2sSprotoType.ara_rfh.response resp = ActParam["resp"] as C2sSprotoType.ara_rfh.response;


        List<ArenaUserData> list = new List<ArenaUserData>();
        for (int i = 0; i < resp.ara_rmd_list.Count; i++)
        {
            ArenaUserData u = new ArenaUserData(); 
            u.csv_id = (int)resp.ara_rmd_list[i].uid;
            u.uname = resp.ara_rmd_list[i].uname;
            u.total_combat = (int)resp.ara_rmd_list[i].total_combat;
            u.ara_rnk = (int)resp.ara_rmd_list[i].ranking;
            u.iconid = (int)resp.ara_rmd_list[i].iconid;
            list.Add(u);
        }
         
        ArenaMgr.Instance.arenaList = list;
        ArenaMgr.Instance.cd = resp.ara_rfh_cd;
        EventManager.Trigger<EventArenaRankList>(new EventArenaRankList(ArenaMgr.Instance.arenaList)); 
        return true;
    }
}
