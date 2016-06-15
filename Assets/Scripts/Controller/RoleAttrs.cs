using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//角色属性信息
public class RoleAttrs : AttrsBase
{
    public List<BoxingEquipData> boxingList;
    //无用
    #region
    public class WakeAttr 
    {
        public int id;
        public int roleId;
        public string name;
        public string path;
        public float defenseBase; //s 
        public float critBase; // s 
        public float skillBase; // s技巧  
        public float fightPowerBase; //s   
        public int fightLevel;
        public int initEqu;
        public int initSkill;
        public int levelCount;
    } 
    
    public List<WakeAttr> wakeLevelAttrList; //sc 
     
    //角色穿戴装备拳法
    public List<ItemData> equipmentList; //c 
    public List<ItemData> boxList; //c 
    public List<ItemData> fashionList; //c



    //战斗前初始化
   
    public float skillRate;//c
    public float validSkillRate;//c    
    public int defense;//c 
    public float crit;//c 
    public float skill; //c技巧  
    public int fightPower;//c

    public int curFightPower; //c
    public int curFightPowerPre;
      
    public int wakeLevel;//c

    public string title; 
    public string path;//资源地址 
    public bool isUser;
    public long avatar;//头像
    public int csv_id;




    public int ComboDamage(int combolevel)
    {
        return (int)((fightPower * 0.2f) + (curFightPower * 0.1f)) * (int)(1 + combolevel * 0.1f + kingRate + critRate);
    } 
 

#endregion  
    //战斗中
    #region 
        //打拳的伤害
    public double boxingPower;

    public double kingRate;
    public double defenseRate; //c防御几率
    public double critRate;//c 暴击几率

    public float GetBoxingEffectPreByType()
    {
        float num = 0;
        for (int i = 0; i < boxingList.Count; i++)
        {
            BoxingViewData r = this.boxingList[i].viewdata as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.effect_pre;
        }
        return num;
    }
    /// <summary>
    /// 计算上阵属性
    /// </summary>
    public void RestUserAttr()
    {
        SetUserAttr();
        fightPower = (int)attrArr[(int)Def.AttrType.FightPower];
        boxingPower = 1;
        if (UserManager.Instance.userAttr!=null)
            boxingPower += UserManager.Instance.userAttr.GetBoxingAdditionByType((int)Def.AttrId.FightPower);
        defenseRate = GetDefenseRate();
        critRate = GetCritRate();
        kingRate = GetKingRate();
    }

    /// <summary>
    /// 把 userattr的数据给 role attr
    /// </summary>
    public void SetUserAttr()
    {
        int old = UserManager.Instance.curRole.csv_id;
        UserManager.Instance.battleRoleID = this.csv_id;
        UserManager.Instance.userAttr.RestUserAttr(UserManager.Instance.level);
        attrArr[(int)Def.AttrType.FightPower] = UserManager.Instance.GetUserAttrByType(Def.AttrId.FightPower);
        attrArr[(int)Def.AttrType.Defense] = UserManager.Instance.GetUserAttrByType(Def.AttrId.Defense);
        attrArr[(int)Def.AttrType.Crit] = UserManager.Instance.GetUserAttrByType(Def.AttrId.Crit);
        attrArr[(int)Def.AttrType.Pray] = UserManager.Instance.GetUserAttrByType(Def.AttrId.Pray);
        UserManager.Instance.battleRoleID = old;
    }

    public void RestEmenyAttr()
    {

        fightPower = (int)attrArr[(int)Def.AttrType.FightPower];
        boxingPower = 1; 
        defenseRate = GetDefenseRate();
        critRate = GetCritRate();
        kingRate = GetKingRate();
    }

    /// <summary>
    /// 防御率
    /// </summary>
    /// <returns></returns>
    public double GetDefenseRate()
    {
        return (int)attrArr[(int)Def.AttrId.Defense] / ((int)attrArr[(int)Def.AttrId.Defense]  + 100.0 );
    }
    /// <summary>
    /// 暴击率
    /// </summary>
    /// <returns></returns>
    public double GetCritRate()
    {
        return (int)attrArr[(int)Def.AttrId.Crit] / ((int)attrArr[(int)Def.AttrId.Crit] + 100.0);
    }
    ///// <summary>
    ///// 有效暴击率
    ///// </summary>
    ///// <param name="tar"></param>
    ///// <returns></returns>
    //public double GetValidCritRate(float tar)
    //{
    //    return Mathf.Max(GameShared.Instance.config.critMin, Mathf.Max(0, (critRate - tar)));
    //}
    /// <summary>
    /// 王者
    /// </summary>
    /// <returns></returns>
    public double GetKingRate()
    {
        return (int)attrArr[(int)Def.AttrId.Pray] / ((int)attrArr[(int)Def.AttrId.Pray] + 100.0);
    }
    /// <summary>
    /// 有效王者
    /// </summary>
    /// <param name="tar"></param>
    /// <returns></returns>
    //public float GetValidKingRate(float tar)
    //{
    //    return Mathf.Max(GameShared.Instance.config.critMin, Mathf.Max(0, (critRate - tar)));
    //}
    /// <summary>
    /// 普通伤害 普通伤害=（战斗力上限*0.2+当前战斗力*0.1）*拳法伤害加成%*（1-目标防御率）*（1+王者率）
    /// </summary>
    /// <returns></returns>
    public int GetDamage(double tarrate, double boxingPower = 1)
    { 
        //return 0;
        double a = ((attrArr[(int)Def.AttrId.FightPower] * 0.2f) + (curFightPower * 0.1f));
        double b = boxingPower * (1 - tarrate) * (1 + kingRate);

        //Debug.Log("战斗力 : " + attrArr[(int)Def.AttrId.FightPower] + "目标防御率  : " + tarrate  +"暴击率 ： " + critRate + "王者 : " + kingRate + "前部分 : " + a + "后部分 : " + b);
        int num =(int)(((attrArr[(int)Def.AttrId.FightPower] * 0.2f) + (curFightPower * 0.1f)) * boxingPower *
            (1 - tarrate ) * (1+ kingRate));  
        return num;


    }
    /// <summary>
    /// 暴击伤害
    /// </summary>
    /// <returns></returns>
    public int GetCrit(double tarrate, double boxingeffect = 1)
    {
        double a = ((attrArr[(int)Def.AttrId.FightPower] * 0.2f) + (curFightPower * 0.1f));
        double b = (1 - tarrate) * (1 + critRate);

        //Debug.Log("战斗力 : " + attrArr[(int)Def.AttrId.FightPower] + "目标防御率  : " + tarrate  + "暴击率 ： "+ critRate +  "王者 : " + kingRate + "前部分 : " + a + "后部分 : " + b); 
        return (int)(((attrArr[(int)Def.AttrId.FightPower] * 0.2f) + (curFightPower * 0.1f)) * boxingeffect *
            (1 - tarrate) * (1 + critRate));
    } 
    #endregion
}



     
 
