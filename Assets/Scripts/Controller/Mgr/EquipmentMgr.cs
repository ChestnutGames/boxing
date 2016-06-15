using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;

public class EquipmentMgr :  UnitySingleton<EquipmentMgr>
{ 

    private LuaState l;
    EquipmentPop pop;

    EquipViewData curEquip;

    EquipViewData curLevelEquip;

    List<EquipViewData> equipList;

    public Hashtable equipTable;

    private EquipKitPop kitPop;

    public void OpenPop(EquipmentPop p)
    {
        pop = p;
        EquipmentMgr.Instance.EquipList();
    }

    public void Start()
    {
#if Lua
       InitLua();
#endif
    }
     
    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Equip);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this); 
    }

    public void EquipList()
    {
        NetworkManager.Instance.EquipList();
    }


    public void EquipListClientCallback(C2sSprotoType.equipment_all.response resp)
    {
#if Lua
        LuaFunction f = l.GetFunction("EquipListCallback");
        object[] obj = f.Call(resp); 
#else
        if (resp.l != null)
        {
            equipTable = UserManager.Instance.equipTable;
            pop.SetEquipList(equipTable);
            GetCurEquip(ref equipTable);
            SetEquipInfo(ref curLevelEquip);
            curEquip = curLevelEquip;
        }
#endif
    }

    public void EquipListCallback(C2sSprotoType.equipment_all.response resp)
    {
#if Lua
        LuaFunction f = l.GetFunction("EquipListCallback");
        object[] obj = f.Call(resp); 
#else
        if (resp.l != null)
        {
            GetEquipList(resp.l);
            pop.SetEquipList(equipTable);
            GetCurEquip(ref equipTable);
            SetEquipInfo(ref curLevelEquip);
            RestTotalNum();
            curEquip = curLevelEquip;
        }
#endif
    }

    public void GetEquipList(List<C2sSprotoType.equipment> list)
    {
        equipTable = new Hashtable();
#if Lua
        LuaFunction f = l.GetFunction("GetEquipList");
        object[] obj = f.Call(list,equipTable); 
#else 
        for (int i = 0; i < list.Count; i++)
        {
            EquipViewData e = new EquipViewData();
            int id = (int)list[i].csv_id;
            e.level = (int)list[i].level;
            if (id > 999)
                id = id / 1000; 
            e.data = GameShared.Instance.GetEquipmentById(id);
            e.data.levelData = GetEquipLevelData(e.data.csv_id, e.level);

            Debug.Log("id" + list[i].csv_id + "power" + list[i].combat + "powerp" + list[i].combat_probability);
             
            EquipLevelData item = e.data.levelData;
            item.attrArr[(int)Def.AttrType.FightPower] = list[i].combat;
            item.attrArr[(int)Def.AttrType.Defense] = list[i].defense;
            item.attrArr[(int)Def.AttrType.Crit] = list[i].critical_hit;
            item.attrArr[(int)Def.AttrType.Pray] = list[i].king;

            item.additionArr[(int)Def.AttrType.FightPower] = list[i].combat_probability;
            item.additionArr[(int)Def.AttrType.Defense] = list[i].defense_probability;
            item.additionArr[(int)Def.AttrType.Crit] = list[i].critical_hit_probability;
            item.additionArr[(int)Def.AttrType.Pray] = list[i].king_probability;
            item.enhance_success_rate = (int)list[i].enhance_success_rate;

            equipTable.Add(e.data.csv_id, e);
            if (UserManager.Instance.equipTable.Contains(e.data.csv_id))
            {
                UserManager.Instance.equipTable[e.data.csv_id] = e;
            }
            else
            {
                UserManager.Instance.equipTable.Add(e.data.csv_id, e);
            } 
        }
#endif
    }

    public EquipLevelData GetEquipLevelData(int id,int level)
    {
        int i = (id*1000)+level;
        EquipLevelData l = GameShared.Instance.GetEquipmentIntensifyById(i);
        return l;
    }

    //获得当前升级装备
    public EquipViewData GetCurEquip(ref Hashtable table)
    {
#if Lua
        LuaFunction f = l.GetFunction("GetCurEquip");
        object[] obj = f.Call(table); 
        curEquip = obj[0];
#else
        System.Collections.IDictionaryEnumerator enumerator = table.GetEnumerator();
        curLevelEquip = null;
        while (enumerator.MoveNext())
        {
            if (curLevelEquip != null)
            {
                EquipViewData a = table[enumerator.Key] as EquipViewData;
                if (a.level < curLevelEquip.level)
                {
                    curLevelEquip = a;
                }
            }else
            {
                curLevelEquip = table[enumerator.Key] as EquipViewData;
            } 
        } 
#endif
        return curLevelEquip; 
    }
  
    public void KitDescShow()
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.EquipKit) != true)
        {
            GameObject obj = Instantiate(pop.kit);
            obj.SetActive(true);
            kitPop = obj.GetComponent<EquipKitPop>();
            //计算
            //取得
            EquipmentKitData d1 = GameShared.Instance.GetEquipmentKitByLevel(curLevelEquip.level);
            EquipmentKitData d2 = GameShared.Instance.GetEquipmentKitNextLevelByLevel(curLevelEquip.level);

            List<string> l = new List<string>();
            for (int i = 1; i < 4;i++)
            { 
                string s = "";
                string sa = "";
                switch(i)
                {
                    case 1:
                       s +="战力";
                        break;
                    case 2:
                        s += "防御";
                        break;
                    case 3:
                        s += "暴击";
                        break;
                    case 4:
                        s += "祈祷";
                        break;
                }
                sa += s;
                if(d1!=null)
                {
                    s += d1.attrArr[i] + ">";
                    sa += d1.additionArr[i];
                }
                if(d2!=null)
                {
                     s += d2.attrArr[i];
                    sa+=d2.additionArr[i] ;
                }
                if (!s.Equals("0"))
                {
                    l.Add(s);
                }
                if (!sa.Equals("0"))
                {
                    l.Add(sa);
                }
            }  
            
            kitPop.SetInfo(l);
            kitPop.transform.parent = pop.transform.parent;
            kitPop.transform.localScale = Vector3.one;
            kitPop.transform.localPosition = Vector3.zero;
            MainUI.Instance.SetPopState(MainUI.PopType.EquipKit, true);
        }
        else 
        {
            if (kitPop != null)
            {
                MainUI.Instance.SetPopState(MainUI.PopType.EquipKit, false);
                kitPop.gameObject.SetActive(false);
                Destroy(kitPop);
            }
        }
        
    
}
    ///
    public void RestTotalNum()
    {
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.equipTable.GetEnumerator();
        int p = 0;
        int d = 0;
        int c = 0;
        int y = 0;
        while (enumerator.MoveNext())
        { 
                EquipViewData a = UserManager.Instance.equipTable[enumerator.Key] as EquipViewData; 
                p += (int)a.data.levelData.attrArr[(int)Def.AttrType.FightPower];
                d += (int)a.data.levelData.attrArr[(int)Def.AttrType.Defense];
                c += (int)a.data.levelData.attrArr[(int)Def.AttrType.Crit];
                y += (int)a.data.levelData.attrArr[(int)Def.AttrType.Pray];
        }

        EquipmentKitData ekd = GameShared.Instance.GetEquipmentKitByLevel(curLevelEquip.level);
        if (ekd != null)
        {
            p += (int)ekd.attrArr[(int)Def.AttrType.FightPower];
            d += (int)ekd.attrArr[(int)Def.AttrType.Defense];
            c += (int)ekd.attrArr[(int)Def.AttrType.Crit];
            y += (int)ekd.attrArr[(int)Def.AttrType.Pray];
        }

        pop.SetTotalNum(p.ToString(), d.ToString(), c.ToString(), y.ToString());
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="d1">当前</param>
    /// <param name="d2">下一</param>
    public void SetEquipInfo(ref EquipViewData d1)
    { 
        #if Lua
        LuaFunction f = l.GetFunction("SetEquipInfo");
        object[] obj = f.Call(d1); 
        curEquip = obj[0];
#else
        EquipLevelData d2 = null;
        EquipViewData aaa = UserManager.Instance.equipTable[(int)EquipData.EquipType.Boxing] as EquipViewData;
        EquipViewData bbb = UserManager.Instance.equipTable[(int)EquipData.EquipType.Cloth] as EquipViewData;
        EquipViewData ccc = UserManager.Instance.equipTable[(int)EquipData.EquipType.Belt] as EquipViewData;
        EquipViewData ddd = UserManager.Instance.equipTable[(int)EquipData.EquipType.Shoe] as EquipViewData;
        Debug.Log("a" + aaa.data.levelData.currency_num + "b" + bbb.data.levelData.currency_num + "c" + ccc.data.levelData.currency_num + "d" + ddd.data.levelData.currency_num);

        if (d1.level < Def.EquipLevelMax)
        {
            int id = d1.data.csv_id + 1 % 4;  
            d2 = GetEquipLevelData(d1.data.csv_id, d1.level+1);
        }
         
            pop.level.text = d1.level + "/" + Def.EquipLevelMax;
            pop.name.text = d1.data.levelData.name;
            if(d1.data.levelData.path!=null)
                pop.itemView.spriteName = d1.data.levelData.path;

            string str = "";
            string num = "";
            if (d2 != null)
            {
                for (int i = 0; i < d1.data.levelData.attrArr.Length; i++)
                {
                    if (d1.data.levelData.attrArr[i] > 0)
                    { 
                        switch (i)
                        {
                            case 1:
                                str += "战力";
                                break;
                            case 2:
                                str += "防御";
                                break;
                            case 3:
                                str += "暴击";
                                break;
                            case 4:
                                str += "王者";
                                break;
                        }
                        pop.attrType.text = str + "提升";
                        num += d1.data.levelData.attrArr[i] + "-" + d2.attrArr[i];
                    }
                }
            } 

            if (d1 != null)
            {
                pop.intensifylevel.text = d1.level + "-" + (d1.level+1);
                string ss = "";
                switch (d1.data.levelData.currency_type)
                {
                    case 2:
                        ss = "金币";
                        break;
                    case 3:
                        ss = "钻石";
                        break;
                } 

                if (!UserManager.Instance.ResByType(curLevelEquip.data.levelData.currency_type, curLevelEquip.data.levelData.currency_num))
                {
                    pop.use.text = "[FF0000]"+d2.currency_num.ToString() + ss+"[-]";
                    pop.use.color = new Color(255, 0, 0);
                    
                }else
                {
                    pop.use.text = "[00FF00]"+d2.currency_num.ToString() + ss+"[-]";
                    pop.use.color = new Color(0, 255, 0);
                }
                
                pop.pre.text = (d2.enhance_success_rate + UserManager.Instance.curVipdata.equipment_enhance_success_rate_up_p).ToString() + "%";
                pop.attr.text = num;
            }
            else
            {
                pop.intensifylevel.text = d1.level.ToString();
                pop.use.text = "";
                pop.pre.text = "";
                pop.attr.text = num;
            }
        int temp = 0;
        temp = BagMgr.Instance.GetItemNumById(d1.data.levelData.currency_type); 
 
#endif
    }

    public void SelectEquip(EquipViewData d) 
    {
        curEquip = d;
        SetEquipInfo(ref d);
    }

    public void DiamonPop()
    {
 
    }

    public void Intensify()
    {
#if Lua
        LuaFunction f = l.GetFunction("Intensify");
        object[] obj = f.Call();
#else 
        if (curLevelEquip.data.csv_id != curEquip.data.csv_id)
        {
            ToastManager.Instance.Show("当前装备不能升级");
 
        } 
        else if (curLevelEquip.level >= UserManager.Instance.level)
        {
            ToastManager.Instance.Show("装备等级不能大于玩家等级");
        }
        else if (!UserManager.Instance.ResByType(curLevelEquip.data.levelData.currency_type, curLevelEquip.data.levelData.currency_num))
        {
            ToastManager.Instance.Show("需要" + curLevelEquip.data.levelData.currency_num+"金币");
            DiamonPop();  
        }
        else
        {
            C2sSprotoType.equipment_enhance.request obj = new C2sSprotoType.equipment_enhance.request();
            obj.csv_id = curEquip.data.csv_id;
            Debug.Log("csvid" + obj.csv_id);
            pop.intensifyBtn.isEnabled = false;
            NetworkManager.Instance.EquipIntensify(obj); 
        }
        
        Debug.Log(UserManager.Instance.level);
#endif
    }
    public void IntensifyCallback(C2sSprotoType.equipment_enhance.response resp)
    {
#if Lua
        LuaFunction f = l.GetFunction("IntensifyCallback");
        object[] obj = f.Call(resp);
#else
        Debug.Log(UserManager.Instance.level);
        pop.intensifyBtn.isEnabled = true;
        if (resp.errorcode == Def.Er_FailEquip)
        {
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.IntensifyFailAmin),
            (state, trackIndex) =>
            {
                MainUI.Instance.effect.gameObject.SetActive(false);
            });
            ToastManager.Instance.Show("装备强化失败");
        }
        else if(resp.errorcode == 1)
        {
            //升级
            curLevelEquip.level++;
            curLevelEquip.data.levelData = GetEquipLevelData(curLevelEquip.data.csv_id, curLevelEquip.level);
            UserManager.Instance.SubResByType(curLevelEquip.data.levelData.currency_type, curLevelEquip.data.levelData.currency_num);
             
            //EquipLevelData item = curEquip.data.levelData;
            //item.attrArr[(int)Def.AttrType.FightPower] = resp.e.combat;
            //item.attrArr[(int)Def.AttrType.Defense] = resp.e.defense;
            //item.attrArr[(int)Def.AttrType.Crit] = resp.e.critical_hit;
            //item.attrArr[(int)Def.AttrType.Pray] = resp.e.king;

            //item.additionArr[(int)Def.AttrType.FightPower] = resp.e.combat_probability;
            //item.additionArr[(int)Def.AttrType.Defense] = resp.e.defense_probability;
            //item.additionArr[(int)Def.AttrType.Crit] = resp.e.critical_hit_probability;
            //item.additionArr[(int)Def.AttrType.Pray] = resp.e.king_probability;
            //item.enhance_success_rate = (int)resp.e.enhance_success_rate; 
            GetCurEquip(ref equipTable);
            curEquip = curLevelEquip;
            SetEquipInfo(ref curLevelEquip);
            RestTotalNum(); 
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.IntensifySuccessAmin),
            (state, trackIndex) =>
            {
                MainUI.Instance.effect.gameObject.SetActive(false);
            });
        }
#endif
    }
 
    public void EquipPopClose(GameObject obj)
    {
#if Lua
        LuaFunction f = l.GetFunction("EquipPopClose");
        object[] obj = f.Call(obj);
#else
        pop = null;
        MainUI.Instance.SetPopState(MainUI.PopType.Equip, false);
        obj.SetActive(false);
        Destroy(obj);
        if (kitPop != null)
        {
            MainUI.Instance.SetPopState(MainUI.PopType.EquipKit, false);
            kitPop.gameObject.SetActive(false);
            Destroy(kitPop);
        }
#endif
    }

}
