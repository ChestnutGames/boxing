using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionArenaEnter : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false; 
        C2sSprotoType.ara_enter.response resp = ActParam["resp"] as C2sSprotoType.ara_enter.response;
        if (resp.errorcode != 1)
            return false;

        List<ArenaUserData> list = new List<ArenaUserData>();
        for (int i = 0; i < resp.ara_rmd_list.Count; i++)
        {
            ArenaUserData u = new ArenaUserData(); 
            u.csv_id = (int)resp.ara_rmd_list[i].uid;
            u.uname = resp.ara_rmd_list[i].uname;
            u.total_combat = (int)resp.ara_rmd_list[i].total_combat;
            u.ara_rnk = (int)resp.ara_rmd_list[i].ranking;
            u.iconid = (int)resp.ara_rmd_list[i].iconid;
            if (UserManager.Instance.uid == u.csv_id)
            {
                u.IsUser = true;
                ArenaMgr.Instance.user_rank = u.ara_rnk;
                ArenaMgr.Instance.user_power = u.total_combat;
            }
            else
            {
                u.IsUser = false;
            }
            list.Add(u);
        } 
        ArenaMgr.Instance.win_tms = resp.ara_win_tms;
        ArenaMgr.Instance.lose_tms = resp.ara_lose_tms;
        ArenaMgr.Instance.tie_tms = resp.ara_tie_tms;
        ArenaMgr.Instance.clg_tms = resp.ara_clg_tms;
        ArenaMgr.Instance.integral = resp.ara_integral;
        ArenaMgr.Instance.rfh_tms = resp.ara_rfh_tms;
        ArenaMgr.Instance.rfh_cost_tms = resp.ara_rfh_cost_tms;
        ArenaMgr.Instance.clg_cost_tms = resp.ara_clg_cost_tms; 
        ArenaMgr.Instance.cd = resp.ara_rfh_cd; 
        ArenaMgr.Instance.arenaList = list;

        ArenaMgr.Instance.pointRewardList = new List<long>();

        for (int i = 0; i < resp.cl.Count; i++)
        {
            long uname = resp.cl[i].integral;
            ArenaMgr.Instance.pointRewardList.Add(uname);
        }
        ArenaMgr.Instance.rankRewardList = new List<bool>();
        for (int i = 0; i < resp.rl.Count; i++)
        {
            bool bb = resp.rl[i].collected;
            ArenaMgr.Instance.rankRewardList.Add(bb);
        }  

        EventManager.Trigger<EventArenaInfo>(new EventArenaInfo(ArenaMgr.Instance.arenaList)); 
        return true;
    }
} 