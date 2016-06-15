using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 竞技场进入角色选择页面
/// </summary>
public class ActionArenaBattleRoleList : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        C2sSprotoType.ara_choose_role_enter.response resp = ActParam["resp"] as C2sSprotoType.ara_choose_role_enter.response;
           
        List<RoleData> list = new List<RoleData>();
        for (int i = 0; i < 3; i++)
        {
            RoleData e = new RoleData();

            e.avatar = resp.e.avatar;
            e.name = resp.e.uname;

           
            e.boxingBattleList = new List<BoxingLevelData>();
            list.Add(e);
         }

        list[0] = FillRoleData((int)resp.e.ara_role_id1, resp.e.ara_role_id1_kf, list[0],
            resp.e.ara_r1_sum_combat, resp.e.ara_r1_sum_defense, resp.e.ara_r1_sum_critical_hit, resp.e.ara_r1_sum_king);
        list[1] = FillRoleData((int)resp.e.ara_role_id2, resp.e.ara_role_id2_kf, list[1],
            resp.e.ara_r2_sum_combat, resp.e.ara_r2_sum_defense, resp.e.ara_r2_sum_critical_hit, resp.e.ara_r2_sum_king);
        list[2] = FillRoleData((int)resp.e.ara_role_id3, resp.e.ara_role_id3_kf, list[2],
            resp.e.ara_r3_sum_combat, resp.e.ara_r3_sum_combat, resp.e.ara_r3_sum_critical_hit, resp.e.ara_r3_sum_king);

        ArenaMgr.Instance.emenyList = list;
         EventManager.Trigger<EventArenaBattleRoleList>(new EventArenaBattleRoleList(resp.bat_roleid));
        return true;
    }

    public RoleData FillRoleData(int id,List<long> boxlist, RoleData d,
        long sum_combat,long sum_defense,long sum_critical_hit,long sum_king)
    {
        d.csv_id = id;
        d.attrArr[(int)Def.AttrType.FightPower] = sum_combat; 
        d.attrArr[(int)Def.AttrType.Defense] = sum_defense;
        d.attrArr[(int)Def.AttrType.Crit] = sum_critical_hit;
        d.attrArr[(int)Def.AttrType.Pray] = sum_king;
        d.path = GameShared.Instance.GetRoleStarById((d.csv_id * 1000) + 1).anim;
        for (int j = 0; j < boxlist.Count; j++)
        {
            if ((int)boxlist[j] != 0)
            {
                BoxingLevelData l = GameShared.Instance.GetBoxingLevelById((int)boxlist[j]);
                if (l.skill_type == Def.SkillType.Active)
                    d.boxingBattleList.Add(l);
            }
        }
        return d;
    }
}
