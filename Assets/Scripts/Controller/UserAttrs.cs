using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//角色属性信息
public class UserAttrs : AttrsBase
{
    /// <summary>
    /// 拳法提升战斗力固定值
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetBoxingAttrByType(int attrtype)
    {
        float num = 0;
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.boxTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            BoxingViewData r = UserManager.Instance.boxTable[enumerator.Key] as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.attrArr[attrtype]; 
        }
        return num; 
    }

    /// <summary>
    /// 拳法装备提升战斗力固定值
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetEquipBoxingAttrByType(int attrtype)
    {
        float num = 0;
        for (int i = 0; i < UserManager.Instance.curRole.boxingList.Count; i++)
        {
            BoxingViewData r = UserManager.Instance.curRole.boxingList[i].viewdata as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.equipAttrArr[attrtype];
        }
        return num;
    }
    /// <summary>
    /// 装备提升战斗力固定值
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetEquipAttrByType(int attrtype)
    {
        float num = 0;
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.equipTable.GetEnumerator();
        int level = 0;
        while (enumerator.MoveNext())
        {
            EquipViewData r = UserManager.Instance.equipTable[enumerator.Key] as EquipViewData;
            if(r.data.levelData!=null)
                num += r.data.levelData.attrArr[attrtype];
            if (r.level == 0 || r.level > level)
            {
                level = r.level;
            }
        }
        EquipmentKitData ekd = GameShared.Instance.GetEquipmentKitByLevel(level);//套装效果
        if (ekd != null)
        {
            num += (int)ekd.attrArr[attrtype];
        } 
        return num;
    } 
    /// <summary>
    /// 角色收集提升战斗力固定值+
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetRoleAttrByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.RoleTable != null)
        {
            System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                RoleData r = UserManager.Instance.RoleTable[enumerator.Key] as RoleData;
                if (r.starData != null && r.is_possessed)
                    num += r.starData.attrArr[attrtype];
            }
        }
        return num;
    } 
    /// <summary>
    /// 拳法手机提升百分比
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetBoxingAdditionByType(int attrtype)
    { 
        float num = 0;
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.boxTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            BoxingViewData r = UserManager.Instance.boxTable[enumerator.Key] as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.additionArr[attrtype];
        }
        return num;
    }

    /// <summary>
    /// 拳法手机提升百分比
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetBoxingEffectPreByType(int attrtype)
    {
        float num = 0;
        for (int i = 0; i < UserManager.Instance.curRole.boxingList.Count; i++)
        {
            BoxingViewData r = UserManager.Instance.curRole.boxingList[i].viewdata as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.effect_pre;
        }
        return num;
    }

    /// <summary>
    /// 拳法装备提升百分比
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetEquipBoxingAdditionByType(int attrtype)
    {
        float num = 0;
        for (int i = 0; i < UserManager.Instance.curRole.boxingList.Count; i++)
        {
            BoxingViewData r = UserManager.Instance.curRole.boxingList[i].viewdata as BoxingViewData;
            if (r.data.levelData != null && r.level > 0)
                num += r.data.levelData.equipAdditionArr[attrtype];
        }
        return num;
    }
    /// <summary>
    /// 装备提升百分比
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetEquipAdditionByType(int attrtype)
    {
        float num = 0;
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.equipTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            EquipViewData r = UserManager.Instance.equipTable[enumerator.Key] as EquipViewData;
            if (r.data.levelData != null)
                num += r.data.levelData.additionArr[attrtype];
        }
        return num;
    } 
    /// <summary>
    /// 角色收集提升
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetRoleAdditionByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.RoleTable != null)
        {
            System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                RoleData r = UserManager.Instance.RoleTable[enumerator.Key] as RoleData;
                if (r.starData != null && r.is_possessed)
                    num += r.starData.additionArr[attrtype];
            }
        } 
        return num;
    }
    /// <summary>
    /// 当前上阵洗练
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns> 
    public float GetXiLianAttrByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.GetCurRole().xilianList != null && UserManager.Instance.level >= GameShared.Instance.config.xilian_level_open)
        {
            for (int i = 0; i < UserManager.Instance.GetCurRole().xilianList.Count; i++)
            {

                int id = (int)UserManager.Instance.GetCurRole().xilianList[i].id;
                if (id == attrtype)
                {
                    num += UserManager.Instance.GetCurRole().xilianList[i].num;
                }
            }
        }
        return num;
    } 
    /// <summary>
    /// 当前上阵洗练%
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns> 
    public float GetXiLianAdditionByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.GetCurRole().xilianList != null && UserManager.Instance.level >= GameShared.Instance.config.xilian_level_open)
        {
            for (int i = 0; i < UserManager.Instance.GetCurRole().xilianList.Count; i++)
            {

                int id = (int)UserManager.Instance.GetCurRole().xilianList[i].id;
                if (id-4 == attrtype)
                {
                    num += UserManager.Instance.GetCurRole().xilianList[i].num;
                }
            }
        }
        return num;
    } 
    /// <summary>
    /// 角色上阵提升
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns> 
    public float GetCurRoleBattleByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.GetCurRole() != null)
        {
            num = UserManager.Instance.GetCurRole().starData.battleAttr[attrtype];
        }
        return num;
    }
    /// <summary>
    /// 角色上阵提升百分比
    /// </summary>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetCurRoleBattleAdditionByType(int attrtype)
    {
        float num = 0;
        if (UserManager.Instance.GetCurRole() != null)
        {
            num = UserManager.Instance.GetCurRole().starData.battleAddition[attrtype];
        }
        return num;
    }
    ////永久提升 
    public float GetForeverAttr(int type)
    {
        float num = 0;
        if (UserManager.Instance.GetCurRole() != null && UserManager.Instance.foreverAttr !=null)
        {
            num = UserManager.Instance.foreverAttr.attrArr[type];
        }
        return num;
    }
    /// <summary>
    /// 玩家提升战斗力固定值
    /// </summary>
    /// <param name="l"></param>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetUserLevelData(int l,int attrtype)
    {
        float num = 0;
        UserLevelData d = GameShared.Instance.GetUserLevelByLevel(l);
        if (d != null)
            num = d.attrArr[attrtype];
        return num;
    }
    /// <summary>
    /// 获得当前角色属性
    /// </summary>
    /// <param name="l"></param>
    /// <param name="attrtype"></param>
    /// <returns></returns>
    public float GetUserAttrByType(int l,int attrtype)
    {
        float num = (GetUserLevelData(l, attrtype) + GetEquipAttrByType(attrtype) + GetBoxingAttrByType(attrtype)+ GetEquipBoxingAttrByType(attrtype) + GetRoleAttrByType(attrtype) + GetCurRoleBattleByType(attrtype) + GetXiLianAttrByType(attrtype) + GetForeverAttr(attrtype)) *
             (1.0f + (GetBoxingAdditionByType(attrtype) + GetEquipBoxingAdditionByType(attrtype) + GetEquipAdditionByType(attrtype) + GetRoleAdditionByType(attrtype) + GetCurRoleBattleAdditionByType(attrtype) + GetXiLianAdditionByType(attrtype))/100);

        if (attrtype == (int)Def.AttrId.FightPower)
        {
            Debug.Log("固定值" + (GetUserLevelData(l, attrtype) + GetEquipAttrByType(attrtype) + GetBoxingAttrByType(attrtype) + GetRoleAttrByType(attrtype) + GetCurRoleBattleByType(attrtype) + GetXiLianAttrByType(attrtype) + GetForeverAttr(attrtype)));
            Debug.Log("提升百分比" + (1.0f + (GetBoxingAdditionByType(attrtype) + GetEquipAdditionByType(attrtype) + GetRoleAdditionByType(attrtype) + GetCurRoleBattleAdditionByType(attrtype) + GetXiLianAdditionByType(attrtype)) / 100));

            Debug.Log("玩家提升战斗力固定值" + attrtype + "值" + GetUserLevelData(l, attrtype));
            Debug.Log("装备提升战斗力固定值+" + attrtype + "值" + GetEquipAttrByType(attrtype));
            Debug.Log("拳法提升战斗力固定值" + attrtype + "值" + GetBoxingAttrByType(attrtype));
            Debug.Log("拳法装备提升战斗力固定值" + attrtype + "值" + GetEquipBoxingAttrByType(attrtype));

            Debug.Log("角色收集提升战斗力固定值" + attrtype + "值" + GetRoleAttrByType(attrtype));
            Debug.Log("角色上阵提升战斗力固定值" + attrtype + "值" + GetCurRoleBattleByType(attrtype));
            Debug.Log("洗练提升战斗力固定值" + attrtype + "值" + GetXiLianAttrByType(attrtype));
            Debug.Log("直接提升战斗力固定" + attrtype + "值" + GetForeverAttr(attrtype));

            Debug.Log("装备提升战斗力%+" + attrtype + "值" + GetEquipAdditionByType(attrtype));
            Debug.Log("拳法提升战斗力%+" + attrtype + "值" + GetBoxingAdditionByType(attrtype));
            Debug.Log("拳法装备拳法提升战斗力%" + attrtype + "值" + GetEquipBoxingAdditionByType(attrtype));
            Debug.Log("角色收集提升战斗力%+" + attrtype + "值" + GetRoleAdditionByType(attrtype));
            Debug.Log("角色上阵提升战斗力%" + attrtype + "值" + GetCurRoleBattleAdditionByType(attrtype));
            Debug.Log("洗练提升战斗力%" + attrtype + "值" + GetXiLianAdditionByType(attrtype));
            }
            return num;
    } 
    /// <summary>
    /// 获得升级提升战斗力
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns> 
    public float GetUpLevelPower(int level)
    {
        float fight = 0;
        fight = GetUserAttrByType(level + 1, (int)Def.AttrType.FightPower) - GetUserAttrByType(level, (int)Def.AttrType.FightPower);
        return fight;
    }    
    /// <summary>
    /// 重置用户信息
    /// </summary>
    /// <param name="level"></param>
    public void RestUserAttr(int level)
    {
        for (int i = 0; i <= Enum.GetNames(typeof(Def.ItemType)).Length; i++)
        {
            attrArr[i] = GetUserAttrByType(level,i);
        } 
    } 
     
}





