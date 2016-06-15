using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionArenaBattleOver : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        ArenaPop pop = UIManager.Instance.GetWindow<ArenaPop>();
//        C2sSprotoType.ara_bat_exit.response resp = ActParam["resp"] as C2sSprotoType.ara_bat_exit.response;
         
        //List<ArenaUserData> list = new List<ArenaUserData>();
        //for (int i = 0; i < resp.ara_rmd_list.Count; i++)
        //{
        //    ArenaUserData u = new ArenaUserData();
        //    u.csv_id = (int)resp.ara_rmd_list[i].csv_id;
        //    u.uname = resp.ara_rmd_list[i].uname;
        //    u.total_combat = (int)resp.ara_rmd_list[i].total_combat;
        //    u.ara_rnk = (int)resp.ara_rmd_list[i].ara_rnk;
        //    u.iconid = (int)resp.ara_rmd_list[i].iconid;
        //    list.Add(u);
        //}
        //list = list.OrderBy(a => a.ara_rnk).ToList(); 
        //ArenaMgr.Instance.arenaList = list;
        //EventManager.Trigger<EventArenaRankList>(new EventArenaRankList(ArenaMgr.Instance.arenaList));
        return true;
    }
}
