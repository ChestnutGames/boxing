using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 用来保存缓存
/// </summary>
public class ArenaMgr : UnitySingleton<ArenaMgr> {
    ArenaPop areanPop;
    ArenaPointPop pointPop;
    ArenaRewardPop rewardPop;
    ArenaRulesPop rulePop;
    StorePop stroePop;
     

    public int refreshCount;

    public DateTime refreshTime;

    public int curBattleNum;
    public long cd;

    public List<ArenaUserData> arenaList;

    public List<RoleData> emenyList;

    public List<bool> rankRewardList;
    public List<long> pointRewardList;

    public int user_rank;
    public int user_power;

    public int curRank;
    public long lose_tms;
    public long win_tms;
    public long rfh_tms;
    public long clg_cost_tms;
    public long integral;
    public long clg_tms;
    public long tie_tms;
    public long rfh_cost_tms;

    public void RestData()
    {
        arenaList.Clear();
        //refreshTime = null;
        refreshCount = 0; 
    }

    public void ArenaOpen(ArenaPop p)
    {
        areanPop = p;
        NetworkManager.Instance.ArenaInfo();
    }

    public void PointOpen(ArenaPointPop p)
    {
        pointPop = p;
        
    }

    public void RewardOpen(ArenaRewardPop p)
    {
        rewardPop = p;
    }

    public void RewardOpen(ArenaRulesPop p)
    {
        rulePop = p;
    }

    public void StoreOpen(StorePop p)
    {
        stroePop = p;
    }

    ArenaView curBattleEmeny;

    public void Battle(ArenaView v)
    {
        curBattleEmeny = v;
        UIManager.Instance.ShowWindow<ArenaRolesPop>();  
    }
    ArenaRolesPop rolePop;
    public void RoleChoosePop(ArenaRolesPop v)
    {
        curIndex = 0;
        rolePop = v;
        NetworkManager.Instance.AraRoleChooseEnter(curBattleEmeny.data.csv_id);
    }

    public void RefreshList()
    {
        NetworkManager.Instance.ArenaListRefresh();
    }

    public void BattleEnter()
    {
        NetworkManager.Instance.ArenaBeginBattle(curBattleEmeny.data.csv_id);
    }

    public void BattleEnterCallback(C2sSprotoType.BeginArenaCoreFight.response resp)
    {   
        List<RoleData> userList = new List<RoleData>(); 
        for (int i = 0; i < rolePop.battleViewArr.Length; i++)
        {
            userList.Add(rolePop.battleViewArr[i].data); 
            if (userList[i].boxingList != null && userList[i].boxingList.Count > 0)
            {
                for (int j = 0; i < userList[j].boxingList.Count; j++)
                {
                    if (userList[i].boxingList[j].viewdata.data.levelData.skill_type == Def.SkillType.Active)
                        userList[i].boxingBattleList.Add(userList[i].boxingList[j].viewdata.data.levelData);
                }
            }
        }  
        //队列
        //BattleManager.Instance.InitLevel(UserManager.Instance.curRole, e, rl, el);
        //一次一请求
        BattleManager.Instance.InitArane(userList, emenyList);
        UIManager.Instance.uiRoot.SetActive(false);
        Application.LoadLevelAdditive("game");
    }

    public void ArenaRewared()
    {
        NetworkManager.Instance.ArenaRewardCollected();
    }

    int curIndex;

    public void ChooseBattleRole(RoleData d)
    {
        bool flag = true;
        rolePop.SetBattleRole(d);
        List<long> l = new List<long>();
        for (int i = 0; i < rolePop.battleViewArr.Length; i++)
        {
            if (rolePop.battleViewArr[i].data != null)
            {
                l.Add(rolePop.battleViewArr[i].data.csv_id);
            }
            else
            {
                flag = false;
            }
        }
        if(flag) 
            NetworkManager.Instance.AraRoleChoose(l);
    }

    public void ChooseRoleView(ArenaRoleView v)
    {
        rolePop.SetRoleView(v);
    }

    /***
 * 
 * 借口
 * 信息
 * 1排名
 * 2挑战次数
 * 3刷新倒计时
 * 4排名前十 和 玩家匹配的10个人
 * （名字 战力 排名 上阵角色）
 * **/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
