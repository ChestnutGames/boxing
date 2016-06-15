using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System;

public class RolesMgr : UnitySingleton<RolesMgr>
{   
    private LuaState l;
    private RolesPop pop;

    public void OpenPop(RolesPop p)
    {
        pop = p;
        RolesMgr.Instance.GetRoleList();
        //BagMgr.Instance.BagList();
    }

    public void Start()
    {
        //InitLua();
    }

    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Roles);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void SetInfo()
    {
        pop.SetInfo(
            GetCollectAllRoleAttrByType((int)Def.AttrType.FightPower).ToString()+"%",
            GetCollectAllRoleAttrByType((int)Def.AttrType.Defense).ToString() + "%",
            GetCollectAllRoleAttrByType((int)Def.AttrType.Crit).ToString() + "%",
            GetCollectAllRoleAttrByType((int)Def.AttrType.Pray).ToString() + "%",
            GetCollectAllRoleAttrByType((int)Def.AttrType.Pray).ToString() + "%"); 
    }

    public static string GetAttrStr(Def.AttrId type)
    {
        string str = "";
        switch (type)
        {
            case Def.AttrId.FightPower:
                str = "战力";
                break;
            case Def.AttrId.Defense:
                str = "防御";
                break;
            case Def.AttrId.Pray:
                str = "王者";
                break;
            case Def.AttrId.Crit:
                str = "暴击";
                break;
        }
        return str;
    }

    public void SetRoleInfo(ref RoleData d)
    { 
        float c = 0;
        float cu = 0;
        int index = 0;
        d.frgNum = BagMgr.Instance.GetItemNumById(d.starData.us_prop_csv_id);
        for(int i=0;i<pop.GetCurView().data.starData.additionArr.Length;i++)
        { 
            c = GetRoleCollectAttrByType(i);
            cu =  GetCollectRoleUPAttrByType(i);
            if(c>0)
            {
                index =i;
                break;
            } 
        }
        string desc = GetAttrStr((Def.AttrId)index); 
        pop.SetCollectDesc(desc);

        if (d.is_possessed == false)
        {
            int id = (d.starData.csv_id*1000) + 5;
            RoleStarData r = GameShared.Instance.GetRoleStarById(id); 
            float a = d.frgNum / (float)d.starData.us_prop_num;

            pop.SetRoleInfo(d,
            (int)r.battleAddition[(int)Def.AttrType.FightPower] + "%满级",
            (int)c + "%满级",
            "",
            "",
            (int)a,
            d.frgNum + "/" + d.starData.us_prop_num
            );
            pop.SetArrowShow(false);
        }else if (d.wakeLevel >= Def.WakeLevelMax)
        {
            float a = d.frgNum / (float)d.starData.us_prop_num; 
            pop.SetRoleInfo(d,
            (int)GetRoleBattleAttrByType((int)Def.AttrType.FightPower) + "%满级",
            (int)c + "%满级",
            "",
            "",
            (int)a,
            d.frgNum + "/" + d.starData.us_prop_num
            );
            pop.SetArrowShow(false);
        }
        else
        {
            int id = d.starData.g_csv_id + 1;
            RoleStarData r = GameShared.Instance.GetRoleStarById(id);
            float a = d.frgNum / (float)r.us_prop_num; 
            pop.SetRoleInfo(d,
                (int)GetRoleBattleAttrByType((int)Def.AttrType.FightPower)+"%",
                (int)c +"%",
                (int)GetBattleRoleUPAttrByType((int)Def.AttrType.FightPower)+"%",
                (int)cu +"%",  
                (int)a,
                d.frgNum + "/" + r.us_prop_num
            );
            pop.SetArrowShow(true);
        } 
        string[] s = d.starData.strs.Split('*');
        pop.SetTalk(s[0]); 
    }

    /// <summary>
    /// 全部人物的收集加成
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetCollectAllRoleAttrByType(int type)
    {
        //LuaFunction f = l.GetFunction("GetCollectAllRoleAttrByType1");
        //object[] obj = f.Call(type,dataList);
        //return Convert.ToInt32(obj[0]);
        float num = 0;
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            RoleData r = UserManager.Instance.RoleTable[enumerator.Key] as RoleData;
            if(r.starData!=null && r.is_possessed)
                num += r.starData.additionArr[type];
        }   
        return num;
    } 

    /// <summary>
    /// 当前角色上阵增加
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetRoleBattleAttrByType(int type)
    {
        //LuaFunction f = l.GetFunction("GetRoleBattleAttrByType");
        //object[] obj = f.Call(type,level);
        //return (float)obj[0]; r.wakeLevel>0
        float num = 0;
        num += pop.GetCurView().data.starData.battleAddition[type];
        return num; 
    }

    /// <summary>
    /// 当前角色收集增加
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetRoleCollectAttrByType(int type)
    {
        //LuaFunction f = l.GetFunction("GetRoleBattleAttrByType");
        //object[] obj = f.Call(type,level);
        //return (float)obj[0]; r.wakeLevel>0
        float num = 0;
        num += pop.GetCurView().data.starData.additionArr[type];
        return num;
    }

    public void GetRoleList()
    {
        NetworkManager.Instance.RoleAll();
    }
 
    public void RoleListCallback(C2sSprotoType.role_all.response resp)
    {   
        RoleData temp = null; 
        for(int i=0;i<resp.l.Count;i++)
        { 
            RoleData r = GameShared.Instance.GetRoleById((int)resp.l[i].csv_id);
            r.is_possessed = resp.l[i].is_possessed;
            r.wakeLevel = (int)resp.l[i].star;
            if (r.is_possessed)
            {
                r.xilianList = new List<XiLianData>();
                Debug.Log("resp.l[i].property_id1" + resp.l[i].property_id1 + "resp.l[i].value1" + resp.l[i].value1);
                if(resp.l[i].property_id1!=0)
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)resp.l[i].property_id1, (int)resp.l[i].value1));
                if (resp.l[i].property_id2 != 0)
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)resp.l[i].property_id2, (int)resp.l[i].value2));
                if (resp.l[i].property_id3 != 0)
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)resp.l[i].property_id3, (int)resp.l[i].value3));
                if (resp.l[i].property_id4 != 0)
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)resp.l[i].property_id4, (int)resp.l[i].value4));
                if (resp.l[i].property_id5 != 0)
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)resp.l[i].property_id5, (int)resp.l[i].value5));
            } 

            int id = (r.csv_id*1000) + r.wakeLevel;
            r.sort = GetSort(r);
            Debug.Log(r.sort +"/" +(int)r.csv_id);
            r.starData = GameShared.Instance.GetRoleStarById(id);
            r.frgNum = (int)resp.l[i].u_us_prop_num;
            if(UserManager.Instance.RoleTable.Contains(r.csv_id))
            {
                UserManager.Instance.RoleTable[r.csv_id] = r;
            }else 
            {
                UserManager.Instance.RoleTable.Add(r.csv_id, r);
            }
            if (temp != null && r.sort < temp.sort)
            {
                temp = r;
            }
            if(temp == null)
            {
                temp = r; 
            }
        }
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            RoleData r = UserManager.Instance.RoleTable[enumerator.Key] as RoleData;
            r.sort = GetSort(r);
        }   
        //LuaFunction f = l.GetFunction("RoleListCallback");
        //object[] obj = f.Call(list); 
        pop.SetTable(ref UserManager.Instance.RoleTable);
        pop.SetCurView(pop.GetItemView(temp.sort));
        SetInfo();
        CheckBtn();
        if (UserManager.Instance.RoleTable != null && UserManager.Instance.RoleTable.Count > 0)
        {
            SetRoleInfo(ref pop.GetItemView(temp.sort).data);
        }  
    }

    public int GetSort(RoleData r)
    {
        int tt = 2;
            if (r.is_possessed)
                tt = 1; 
        return (tt * 10000) + ((10 - r.wakeLevel) * 1000) + r.csv_id; 
    }
    
    /// <summary>
    /// 升级后角色属性
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public RoleStarData GetUpLevelRoleAttr(RoleData r)
    {
        int id = (r.csv_id * 1000) + r.wakeLevel+1; 
        RoleStarData starData = GameShared.Instance.GetRoleStarById(id); 
        return starData;
    }

    /// <summary>
    /// 升级后角色收集属性
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public float GetCollectRoleUPAttrByType(int type)
    {
        RoleData r = pop.GetCurView().data;
        RoleStarData s = GetUpLevelRoleAttr(r);
        float num = 0;
        if(s!=null)
        {
            num = s.additionArr[type];
        }
        return num;
    }
    /// <summary>
    /// 升级后角色战斗属性
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public float GetBattleRoleUPAttrByType(int type)
    {
        RoleData r = pop.GetCurView().data;
        RoleStarData s = GetUpLevelRoleAttr(r);
        return s.battleAddition[type];
    }
     
    public BuffData SetNum(BuffData data)
    { 
        return data;
    }

    public void CheckBtn()
    {
        bool wake = true;
        bool battle = true;
        if (pop.GetCurView().data != null && pop.GetCurView().data.wakeLevel < Def.WakeLevelMax && 
            pop.GetCurView().data.frgNum >= pop.GetCurView().data.starData.us_prop_num)
        {
            wake = true;
        }
        else
        {
            wake = false;
        }
        if (pop.GetCurView() !=null && UserManager.Instance.battleRoleID != pop.GetCurView().data.csv_id 
            && pop.GetCurView().data.is_possessed)
        {
            battle = true;
        }
        else
        {
            battle = false;
        }
        pop.SetBtn(wake, battle);
    } 
 
    public void RoleSelect(HeadRole role)
    {
        //LuaFunction f = l.GetFunction("RoleSelect");
        //object[] obj = f.Call(role);
        if (pop != null)
        {
            pop.GetCurView().SetFous(false); 
            pop.SetCurView(role);
            SetRoleInfo(ref role.data);  
            CheckBtn();
            Debug.Log(role.data.sort + "/" +role.data.csv_id);
        } 
    }

    public void WakeUp(HeadRole role)
    {
        int id = pop.curView.data.starData.g_csv_id + 1;
        RoleStarData r = GameShared.Instance.GetRoleStarById(id);
        if (!role.data.is_possessed)
        {
            r = GameShared.Instance.GetRoleStarById(pop.curView.data.starData.g_csv_id);
        }

        if (pop.curView.data.wakeLevel >= Def.WakeLevelMax)
        {
            ToastManager.Instance.Show("达到最大等级");
        }
        else if (pop.curView.data.frgNum < r.us_prop_num)
        {
             ToastManager.Instance.Show("碎片不够");
        }
        else if (pop.curView.data.is_possessed)
        {
            C2sSprotoType.role_upgrade_star.request obj = new C2sSprotoType.role_upgrade_star.request();
            obj.role_csv_id = pop.curView.data.csv_id;
            pop.UpWakeBtn.isEnabled = false;
            NetworkManager.Instance.RoleWake(obj);
        }
        else
        {
            C2sSprotoType.role_recruit.request obj = new C2sSprotoType.role_recruit.request();
            obj.csv_id = pop.curView.data.csv_id;
            pop.UpWakeBtn.isEnabled = false;
            NetworkManager.Instance.RoleRecruit(obj);
        }
    }

    public void WakeUpCallback(C2sSprotoType.role_upgrade_star.response resp)
    {
        //LuaFunction f = l.GetFunction("WakeUpCallback");
        //object[] obj = f.Call(); 
        pop.UpWakeBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            RoleData r = pop.curView.data;
            r.wakeLevel++;
            int id = (r.csv_id * 1000) + r.wakeLevel;
            r.sort = id;
            r.starData = GameShared.Instance.GetRoleStarById(id);
            r.frgNum = (int)resp.r.u_us_prop_num;
            r.sort = GetSort(r);
            pop.curView.name = r.sort.ToString();

            BagMgr.Instance.SubItemNumById(r.starData.us_prop_csv_id, r.starData.us_prop_num);

            SetRoleInfo(ref r);
            SetInfo();
            CheckBtn(); 
            pop.table.Reposition();
            pop.table.repositionNow = true;
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.WakeUpAmin),
                GameShared.Instance.GetSkeletonAssetByPath(r.starData.anim),
                (state, trackIndex) =>
                {
                    MainUI.Instance.effect.gameObject.SetActive(false);
                    MainUI.Instance.roleasset.gameObject.SetActive(false);
                });
        }
    }

    public void RoleBattle(HeadRole v)
    { 
        C2sSprotoType.role_battle.request obj = new C2sSprotoType.role_battle.request();
        obj.csv_id = v.data.csv_id;
        pop.BattleBtn.isEnabled = false;
        NetworkManager.Instance.RoleBattle(obj);
    }

    public void RoleBattleCallback(C2sSprotoType.role_battle.response resp)
    {
        //LuaFunction f = l.GetFunction("RoleBattleCallback");
        //object[] obj = f.Call();  
        pop.BattleBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            UserManager.Instance.curRole = pop.curView.data;
            UserManager.Instance.battleRoleID = pop.curView.data.csv_id;
            SetRoleInfo(ref pop.curView.data);
            SetInfo();
            CheckBtn();
        }
    }

    public void RoleUnlockCallback(C2sSprotoType.role_recruit.response resp)
    {
        //获得角色初始等级
        pop.UpWakeBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            RoleData r = pop.GetRoleView().data;
            r.wakeLevel = 1;
            int id = (r.csv_id * 1000) + r.wakeLevel;
            r.sort = id;
            r.starData = GameShared.Instance.GetRoleStarById(id);
            //获得初始等级角色数据
            r.wakeLevel = r.starData.star_init;
            r.is_possessed = true;
            r.sort = GetSort(r);
            id = (r.csv_id * 1000) + r.wakeLevel;
            r.starData = GameShared.Instance.GetRoleStarById(id);
            BagMgr.Instance.SubItemNumById(r.starData.us_prop_csv_id, r.starData.us_prop_num);

            r.sort = r.sort = GetSort(r);
            pop.GetRoleView().name = r.sort.ToString();
            pop.table.Reposition();
            pop.table.repositionNow = true;

            SetRoleInfo(ref r);
            SetInfo();
            CheckBtn();
            ToastManager.Instance.Show("角色解锁成功");
        }
    }

    public void OpenXiLianPop(RoleData r)
    {
        MainUI.Instance.XiLianLClick(r);
    }
    //洗练 
    XiLianPop xilianPop;
    #region 
    public void OpenXiLianPop(XiLianPop p)
    {
        xilianPop = p;
        XiLianInit();
    }

    public void XiLianInit()
    {
        List<XiLianData> list = new List<XiLianData>();
        for(int i=0;i<xilianPop.data.xilianList.Count;i++)
        {
            XiLianData d = InitXiLianData(xilianPop.data.xilianList[i].id, xilianPop.data.xilianList[i].num);
            list.Add(d);
        }
        xilianPop.SetDangQian(list);
        xilianPop.SetXiLianHoutEmpty();
        xilianPop.InitLock();
        RolesMgr.Instance.SetCondition(0);
    }

    public void XiLian()
    {
        bool flag = true;
        for (int i = 0; i < xilianPop.condition.Length;i++)
        { 
            if (xilianPop.condition[i].data != null &&
                BagMgr.Instance.GetItemNumById(xilianPop.condition[i].data.data.id) < xilianPop.condition[i].data.curCount)
            {
                flag = false;
            }
        }
        if (!flag)
        {
            ToastManager.Instance.Show("金币不够");
        } 
        else
        {
            C2sSprotoType.xilian.request obj = new C2sSprotoType.xilian.request();
            obj.is_locked1 = xilianPop.unLock[0].value;
            obj.is_locked2 = xilianPop.unLock[1].value;
            obj.is_locked3 = xilianPop.unLock[2].value;
            obj.is_locked4 = xilianPop.unLock[3].value;
            obj.is_locked5 = xilianPop.unLock[4].value;
            obj.role_id = pop.GetCurView().data.csv_id;
            NetworkManager.Instance.XiLianList(obj);
        }
    }
     
    public void SetCondition(int c)
    {
        XiLianConditionData d = GameShared.Instance.GetXiLianConditionByCount(c); 
        for(int i=0;i< xilianPop.condition.Length;i++)
        {
            if (i < d.list.Count)
            {
                int cur = BagMgr.Instance.GetItemNumById(d.list[i].data.id);
                xilianPop.condition[i].gameObject.SetActive(true);
                if (d.list[i].data.id > 5)
                {
                    xilianPop.condition[i].InitData(d.list[i], d.list[i].data.name, d.list[i].curCount, cur); 
                }
                else
                {
                    xilianPop.condition[i].InitDataHideBG(d.list[i], d.list[i].data.name,
                        d.list[i].curCount, cur, (Def.CurrencyType)d.list[i].data.id); 
                } 
            }
            else
            {
                xilianPop.condition[i].gameObject.SetActive(false);
            }
        }
    }

    public void XiLianCallBack(C2sSprotoType.xilian.response resp)
    {
        xilianPop.yesOrNo.SetActive(true);
        xilianPop.xiBtn.gameObject.SetActive(false);

        List<XiLianData> list = new List<XiLianData>();
        list.Add(InitXiLianData((Def.AttrId)resp.property_id1, (int)resp.value1));
        list.Add(InitXiLianData((Def.AttrId)resp.property_id2, (int)resp.value2));
        list.Add(InitXiLianData((Def.AttrId)resp.property_id3, (int)resp.value3));
        list.Add(InitXiLianData((Def.AttrId)resp.property_id4, (int)resp.value4));
        list.Add(InitXiLianData((Def.AttrId)resp.property_id5, (int)resp.value5));  
        xilianPop.SetXiLianHou(list);
        for (int i = 0; i < xilianPop.unLock.Length; i++)
        { 
            xilianPop.unLock[i].enabled = false;
        }
    }

    public XiLianData InitXiLianData(Def.AttrId id, int num)
    {
        XiLianData d = new XiLianData();
        d.id = id;
        XiLianMaxData max = GameShared.Instance.GetXiLianByType(id);
        d.min = max.min;
        d.max = max.max; 
        d.name = Comm.GetAttrStr(id);
        d.num = (int)num;
        Debug.Log("id" + id + "num" + num);
        return d;
    }
    private bool xilianSave;
    public void XiLianCancel()
    {
        xilianSave = false;
        C2sSprotoType.xilian_ok.request obj = new C2sSprotoType.xilian_ok.request();
        obj.ok = xilianSave; 
        NetworkManager.Instance.XiLianSave(obj); 
    } 

    public void XiLianOk()
    {
        xilianSave = true;
        C2sSprotoType.xilian_ok.request obj = new C2sSprotoType.xilian_ok.request();
        obj.ok = xilianSave;
        obj.role_id = pop.GetCurView().data.csv_id;
        NetworkManager.Instance.XiLianSave(obj);
    }

    public void XiLianOkCallback(C2sSprotoType.xilian_ok.response resp)
    {
        xilianPop.yesOrNo.SetActive(false);
        xilianPop.xiBtn.gameObject.SetActive(true); 
         
        if (xilianSave)
        {
             List<XiLianData> list = new List<XiLianData>();
             for(int i=0; i<xilianPop.xiLianHou.Length;i++)
             {  
                 xilianPop.dangQian[i].InitData(xilianPop.xiLianHou[i].data); 
                 list.Add(xilianPop.xiLianHou[i].data);
             }
             xilianPop.data.xilianList = list; 
            for (int i = 0; i < xilianPop.condition.Length;i++)
            {
                if (xilianPop.condition[i].data != null)
                {
                    BagMgr.Instance.SubItemNumById(xilianPop.condition[i].data.data.id,
                        xilianPop.condition[i].data.curCount);
                }
            }
            pop.GetCurView().data.xilianList = list;
        }
        else
        { 

        }
        for (int i = 0; i < xilianPop.unLock.Length; i++)
        {
            xilianPop.unLock[i].enabled = true;
        }
        xilianPop.UnlockChange();
        xilianPop.SetXiLianHoutEmpty(); 
    } 
    #endregion 
}
