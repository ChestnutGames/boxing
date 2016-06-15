using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;

public class BoxingMgr : UnitySingleton<BoxingMgr>
{
    private BoxingPop pop;

    private List<RoleData> roleList;

    private RoleData curRoleData;

    private  LuaState l;

    public void OpenPop(BoxingPop p)
    {
        pop = p;
        InitBoxingList();
        
    }

    public BoxingPop GetPop()
    {
        return pop;
    }

    public void Start()
    {
        //InitLua();
    }

    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Boxing);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }


    //选中
    public void SetBoxingFocus(BoxingView v)
    {
        if (pop != null && v!=null && pop.curBoxView != v)
        {
            pop.curBoxView.SetFous(false);
            v.SetFous(true);
            pop.curBoxView = v;
            SetSelectBoxing(v);
        }
    }

    //选中显示
    public void SetSelectBoxing(BoxingView view)
    {  
        BoxingViewData v = view.data;
        Debug.Log(v.sort + "/" + v.data.csv_id);
        if (v.frag_num < 1)
            v.frag_num = BagMgr.Instance.GetItemNumById(view.data.data.levelData.item_id);

        int ind = view.data.data.levelData.g_csv_id;
        BoxingLevelData bld = null;
        if (v.level < Def.BoxLevelMax)
        { 
            //查找背包有多少item  
            ind++;
            bld = GameShared.Instance.GetBoxingLevelByIdCon(ind); 
            if (v.frag_num >= bld.item_num)
            { 
                v.fra_value = 1.0f;
            }
            else
            {
                float a = (float)v.frag_num / (float)bld.item_num;
                v.fra_value = a;
            } 
        }else 
        {
            bld = GameShared.Instance.GetBoxingLevelByIdCon(ind); 
            v.fra_value = 1.0f;
        } 

        pop.boxingIcon.RestItem(v);
        pop.SetBoxingInfo(v,GetAttr(v.data));
        pop.curBoxView.SetFous(true);
        pop.SetFragNum(v.frag_num + "/" + bld.item_num);
        //CheckBtn();
    }
     
    public void InitBoxingList()
    { 
        NetworkManager.Instance.BoxingList();
    }
    public int GetSort(BoxingViewData r)
    {
        int tt = 1;
        if (r.level<1)
            tt = 2;
        return (tt * 1000000) + ((100-r.level) * 1000) + r.data.csv_id;
    }

    public void InitBoxingListClientCallback()
    {   
        List<RoleData> roleList = new List<RoleData>();
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            RoleData r = UserManager.Instance.RoleTable[enumerator.Key] as RoleData; 
            roleList.Add(r); 
        }  

        if (roleList != null)
        {
            RoleListCallBack(ref roleList);
            SetBoxingList();
            SetEquipBoxingList(ref pop.curRoleView.data);
        }
        else
        {
            Debug.Log("用户空");
        }
    }

    public void InitBoxingListCallback(C2sSprotoType.kungfu.response resp)
    {
        
            //用户拳法表
            if (resp.k_list != null)
            {
                for (int i = 0; i < resp.k_list.Count; i++)
                {
                    int id = (int)resp.k_list[i].csv_id;
                    C2sSprotoType.kungfu_content c = resp.k_list[i];
                    Debug.Log("id"+resp.k_list[i].csv_id+"num"+resp.k_list[i].k_sp_num+"level"+resp.k_list[i].k_level);
                    if (UserManager.Instance.boxTable.Contains(id))
                    {
                        BoxingViewData v = UserManager.Instance.boxTable[id] as BoxingViewData;
                        v.level = (int)c.k_level;
                        v.frag_num = (int)c.k_sp_num;
                        v.type = (int)c.k_type; 
                        int levelid = (1000 * v.data.csv_id) + v.level;
                        v.data.levelData = GameShared.Instance.GetBoxingLevelById(levelid);
                        v.sort = GetSort(v); 
                        UserManager.Instance.boxTable[v.data.csv_id] = v; 
                    }
                    else
                    { 
                        BoxingViewData v = new BoxingViewData();
                        v.level = (int)c.k_level;
                        v.frag_num = (int)c.k_sp_num;
                        v.type = (int)c.k_type; 
                        v.data = GameShared.Instance.GetBoxingById((int)c.csv_id); 
                        int levelid = (1000*v.data.csv_id)+v.level;
                        v.sort = GetSort(v); 
                        v.data.levelData = GameShared.Instance.GetBoxingLevelById(levelid);
                        UserManager.Instance.boxTable.Add(id,v);
                    }
                }
            }
            //用户角色表
            if (resp.role_kid_list != null)
            {

                roleList = new List<RoleData>();
                for (int i = 0; i < resp.role_kid_list.Count; i++)
                {
                    int id = (int)resp.role_kid_list[i].r_csv_id;
                    C2sSprotoType.kungfu_role_list c = resp.role_kid_list[i];
                    Debug.Log(c.r_csv_id);
                    RoleData r = UserManager.Instance.GetRoleById((int)c.r_csv_id);  
                    if (r != null && resp.role_kid_list[i].pos_list!=null)
                    {
                        r.boxingList = new List<BoxingEquipData>();
                        for (int j = 0; j < c.pos_list.Count; j++)
                        { 
                            int dd = (int)c.pos_list[j].k_csv_id/1000; 
                            Debug.Log("roleid" + resp.role_kid_list[i].r_csv_id + "boxid" + c.pos_list[j].k_csv_id + "pos" + c.pos_list[j].position);
                            BoxingViewData b = UserManager.Instance.GetBoxingById(dd); 
                            BoxingEquipData eq = new BoxingEquipData();
                            eq.viewdata = b; 
                            eq.position = (int)c.pos_list[j].position;
                            r.boxingList.Add(eq); 
                        } 
                    } 
                    roleList.Add(r); 
                }
            }
            if (pop !=null && roleList != null)
            {
                RoleListCallBack(ref roleList);
                SetBoxingList();
                SetEquipBoxingList(ref pop.curRoleView.data);
            }
            else
            {
                Debug.Log("用户空");
            } 
    }

    public void SetBoxingList()
    {
        BoxingViewData min = null;
        //筛选第一个
        Hashtable h = new Hashtable();
        System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.boxTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            BoxingViewData r = UserManager.Instance.boxTable[enumerator.Key] as BoxingViewData;
            r.sort = GetSort(r); 
            if (min == null)
            {
                min = r;
            }
            else
            {
                if (r.sort < min.sort)
                {
                    min = r;
                }
            }
            if (r.data.comm == 2 && r.data.csv_id == pop.curRoleView.data.starData.skill_csv_id)
            {
                h.Add(r.data.csv_id, r);
            }
            else
            {
                h.Add(r.data.csv_id, r);
            }
        }   
        pop.SetBoxingList(h);  
        pop.curBoxView = pop.GetBoxView(min.data.csv_id);
        SetSelectBoxing(pop.curBoxView); 
    }
    //设置装备技能
    public void SetEquipBoxingList(ref RoleData r)
    {
        for (int i = 0; i < pop.activeArr.Length; i++)
        {
            pop.activeArr[i].RestEquipEmpty(i+1);
        }
        for (int i = 0; i < pop.passiveArr.Length; i++)
        {
            pop.passiveArr[i].RestEquipEmpty(i+6);
        } 

        if (r.boxingList != null && r.boxingList.Count > 0)
        {
            for (int i = 0; i < r.boxingList.Count; i++)
            {
                BoxingEquipData v = r.boxingList[i];
                int pos = r.boxingList[i].position;
                Debug.Log(v.viewdata.data.csv_id + "pos" + r.boxingList[i].position);
                if (pos <= 5)
                {
                    pop.activeArr[pos - 1].InitData(v, pos);
                }
                else
                {
                    pop.passiveArr[pos - 6].InitData(v,pos);
                }
            }
        } 
    }


    //选中
    public void SetRoleFocus(HeadRole r)
    {
        if (pop!=null&&pop.curRoleView != r)
        { 
            pop.curRoleView.SetFous(false);
            pop.curRoleView = r;
            r.SetFous(true);
            SetBoxingList();
            SetEquipBoxingList(ref r.data);
            //CheckBtn();
            Debug.Log(r.data.sort + "/" + r.data.csv_id);
        } 
    }
     
 
    //删除重置拳法表
    public void RemoveBoxingList(BoxingView v1,BoxingView v2)
    { 
        v2.RestItem(v1.data); 
        pop.boxViewTable.Remove(v1);
        v1.gameObject.SetActive(false);
        NGUITools.Destroy(v1);  
        pop.boxTable.Reposition();
        pop.boxScrollView.ResetPosition();
        pop.boxTable.repositionNow = true;
    }

    public bool CheckHaveBoxing(BoxingViewData d)
    {
        bool flag = false;
        for (int i = 0; i < pop.passiveArr.Length; i++)
        {
            if (pop.passiveArr[i].data!=null &&
                pop.passiveArr[i].data.data.csv_id == d.data.csv_id)
            {
                flag = true;
            }
        }
        for (int i = 0; i < pop.activeArr.Length; i++)
        {
            if (pop.activeArr[i].data!=null &&
                pop.activeArr[i].data.data.csv_id == d.data.csv_id)
            {
                flag = true;
            }
        }
        return flag; 
    }

    //public bool BoxingCanSwap(BoxingView cur)
    //{
    //    bool flag = false;
    //    if (cur != null && cur.data != null && cur.data.level > 0 && pop.curRoleView.data.id)
    //    {

    //    }

    //    return flag;
    //}
    //设置技能
    public bool SwapBoxingList(ref BoxingView v1, ref BoxingView v2, GameObject sourceParent, Vector3 oriPos)
    {
        bool flag = false;//是否回归原位
        if (v1 != null && v1.data != null )
        {
            bool have = CheckHaveBoxing(v1.data);
            if (!have
                && v1.tag != Def.BoxingSave
                )
            {
               UIWidget w =  v1.GetComponent<UIWidget>();
               Debug.Log(w.depth);
                //
                if (v2 != null && v1.data.data.levelData.skill_type == v2.type && v1.tag != Def.BoxingSave && v2.tag == Def.BoxingSave)
                {
                    //移入装备技能 
                    v2.RestItem(v1.data);
                    flag = true; 
                }
                else if (v2 != null && v2.tag == Def.BoxingSave)//从上面移入装备栏 但类型不对
                {
                    ToastManager.Instance.Show("不可以放这个类型");
                }
                else
                {
                    //在上面的表移动
                    flag = true;
                }
            }
            else if (!have)//不用和才可移入装备
            {
                //交换两个
                BoxingDragView dr = v1.GetComponent<BoxingDragView>();
                if (v2 != null && v2.tag == Def.BoxingSave && dr != null)
                {
                    //在装备技能中移动
                    if (v2.data == null)
                    {
                        v2.RestItem(v1.data);
                        v1.RestEmpty();
                    }
                    else
                    {
                        BoxingViewData temp = v2.data;
                        v2.RestItem(v1.data);
                        v1.RestItem(temp);
                    }
                    v1.gameObject.transform.localPosition = dr.oriPos;
                } //移除
                else
                {//从一个移入一个空的
                    v1.RestEmpty();
                    v1.gameObject.transform.localPosition = dr.oriPos;
                }
            }
            else
            {
                //移除移入的不是下面的装备栏
                if ((v2 == null || v2.tag!=Def.BoxingSave) && v1.tag == Def.BoxingSave)
                {
                    v1.RestEmpty();
                    flag = true;
                    v1.gameObject.transform.localPosition = oriPos;
                }
                else if (v2 != null && v1.type == v2.type && v1.tag == Def.BoxingSave && v2.tag == Def.BoxingSave) //同类互换
                { 
                    flag = true;
                    v1.gameObject.transform.localPosition = oriPos;
                }
                else
                {
                    //非同类互换
                    if (v1.tag == Def.BoxingSave && v2.tag == Def.BoxingSave && v1.type != v2.type)
                    {
                        flag = true;
                        v1.gameObject.transform.localPosition = oriPos;
                        ToastManager.Instance.Show("不可以放这个类型");
                    }
                    else
                    {
                        //从上面移入装备栏 以存在
                        if ((v2.data == null || v1.data.data.csv_id != v2.data.data.csv_id) && !have)
                        { 
                            v1.RestEmpty();
                            flag = true;
                            v1.gameObject.transform.localPosition = oriPos;
                        }
                    } 
                } 
            }
        }
        else
        { 
        }
        pop.boxTable.Reposition(); 
        pop.boxTable.repositionNow = true;
        return flag;
    }

    public void RefreshTable()
    {
        pop.boxTable.Reposition();
        pop.boxTable.repositionNow = true;
    }

    //获得角色
    public void RoleListCallBack(ref List<RoleData> list)
    {
        if (list == null) return;
        RoleData min = null; 
        
        //筛选第一个 
        for (int i = 0; i < list.Count;i++ )
        {
            RoleData r = list[i];
            int tt = 2;
            if (r.wakeLevel < 1)
                tt = 1;
            r.sort = (tt * 10000) + ((10 - r.wakeLevel) * 1000) + r.csv_id; 
            if (min == null)
            {
                min = r;
            }
            else
            {
                if (r.sort < min.sort)
                {
                    min = r;
                }
            }
        }  
        pop.SetRoleList(ref list);
        pop.curRoleView = pop.GetRoleView(min.csv_id); 
        pop.curRoleView.SetFous(true);  
    }

    public void UpLevel()
    {
        if (pop.curBoxView.data.level < Def.BoxLevelMax)
        {
            int ind = pop.curBoxView.data.data.levelData.g_csv_id + 1;
            BoxingLevelData l = GameShared.Instance.GetBoxingLevelByIdCon(ind);
            if (!UserManager.Instance.ResByType(l.coin_type, l.coin))
            {
                ToastManager.Instance.Show("金币不够" + l.coin);
            }
            else if (pop.curBoxView.data.frag_num < l.item_num)
            {
                ToastManager.Instance.Show("碎片不够");
            } 
            else
            {
                C2sSprotoType.kungfu_levelup.request obj = new C2sSprotoType.kungfu_levelup.request();
                obj.csv_id = pop.curBoxView.data.data.csv_id;
                obj.k_level = pop.curBoxView.data.level + 1;
                obj.k_type = (int)pop.curBoxView.data.data.levelData.skill_type;
                pop.upLevelBtn.isEnabled = false;
                NetworkManager.Instance.BoxingUp(obj);
            }
        }
        else
        {
            ToastManager.Instance.Show("不能再升了");
        }
    } 

    public void UpLevelCallback(C2sSprotoType.kungfu_levelup.response resp)
    { 
       //拳法升级
        pop.upLevelBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            BoxingViewData v = pop.curBoxView.data;

            v.level++;
            int id = (v.data.csv_id * 1000) + v.level;
            v.data.levelData = GameShared.Instance.GetBoxingLevelById(id);

            pop.GetBoxView(v.data.csv_id).data.sort = GetSort(v);
            pop.GetBoxView(v.data.csv_id).gameObject.name = GetSort(v).ToString();

            UserManager.Instance.SubResByType(v.data.levelData.coin_type, v.data.levelData.coin);
            BagMgr.Instance.SubItemNumById(v.data.levelData.item_id, v.data.levelData.item_num);
            v.frag_num = BagMgr.Instance.GetItemNumById(v.data.levelData.item_id);
            Debug.Log(v.sort + "/" + v.data.csv_id);
            //获得下一级的碎片数
            int ind = v.data.levelData.g_csv_id + 1;
            BoxingLevelData l = GameShared.Instance.GetBoxingLevelByIdCon(ind);
            if (v.level < Def.BoxLevelMax)
            {
                //查找背包有多少item 
                //v.frag_num = BagMgr.Instance.GetItemNumById(v.data.item_id);
                if (v.frag_num >= l.item_num)
                {
                    v.fra_value = 1.0f;
                }
                else
                {
                    float a = (float)v.frag_num / (float)l.item_num;
                    v.fra_value = a;
                }
            }
            else
            {
                v.fra_value = 1.0f;
            } 
            pop.boxingIcon.RestItem(v);
            pop.SetBoxingInfo(v, GetAttr(v.data));
            pop.SetFragNum(v.frag_num + "/" + l.item_num); 
            pop.boxTable.Reposition();
            pop.boxTable.repositionNow = true;
        }
    }

    public void SaveBoxing()
    { 
        C2sSprotoType.kungfu_chose.request obj = new C2sSprotoType.kungfu_chose.request();

        List<C2sSprotoType.kungfu_pos_and_id> list = new List<C2sSprotoType.kungfu_pos_and_id>();
        for(int i=0;i<pop.activeArr.Length;i++)
        {
            if(pop.activeArr[i].data!=null && pop.activeArr[i].data.data!=null)
            {
                C2sSprotoType.kungfu_pos_and_id d = new C2sSprotoType.kungfu_pos_and_id();
                d.k_csv_id = pop.activeArr[i].data.data.levelData.g_csv_id;
                int p = i + 1;
                d.position = p;
                Debug.Log("id" + d.k_csv_id + "pos" + p);
                list.Add(d);
            }
        }
        for(int i=0;i<pop.passiveArr.Length;i++)
        {
            if(pop.passiveArr[i].data!=null && pop.passiveArr[i].data.data!=null)
            {
                C2sSprotoType.kungfu_pos_and_id d = new C2sSprotoType.kungfu_pos_and_id();
                d.k_csv_id = pop.passiveArr[i].data.data.levelData.g_csv_id;
                int p = i + 6;
                d.position = p;
                list.Add(d);
                Debug.Log("id" + d.k_csv_id + "pos" + p);
            }
        }
        obj.r_csv_id = pop.curRoleView.data.csv_id;
        obj.idlist = list;
        pop.saveBtn.isEnabled = false;
        NetworkManager.Instance.BoxingChose(obj);
    } 

    public void SaveBoxingCallback(C2sSprotoType.kungfu_chose.response resp)
    {
        pop.saveBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            List<BoxingEquipData> list = new List<BoxingEquipData>();
            for (int i = 0; i < pop.activeArr.Length; i++)
            {
                if (pop.activeArr[i].data != null && pop.activeArr[i].data.data != null)
                {
                    pop.activeArr[i].equip.position = 1 + i;
                    list.Add(pop.activeArr[i].equip);
                }
            }
            for (int i = 0; i < pop.passiveArr.Length; i++)
            {
                if (pop.passiveArr[i].data != null && pop.passiveArr[i].data.data != null)
                {
                    pop.passiveArr[i].equip.position = 6 + i;
                    list.Add(pop.passiveArr[i].equip);
                }
            } 
            pop.curRoleView.data.boxingList = list;
            Debug.Log("保存成功");
        }
    }

    public List<string> GetAttr(BoxingData box)
    {
        List<string> l = new List<string>();  
            for(int x=0;x<box.levelData.attrArr.Length;x++)
            { 
                float a = box.levelData.attrArr[x];
                if(a>0)
                {
                    string s = GetAttrStr((Def.AttrId)x); 
                    s+=a;
                    l.Add(s);
                }
                a = 0;
            }
             for(int y=0;y<box.levelData.attrArr.Length;y++)
            { 
                float b = box.levelData.additionArr[y];
                if(b>0)
                {
                    string ss = GetAttrStr((Def.AttrId)y+4); 
                    ss+=b;
                    l.Add(ss);
                }
                b = 0;
            }   
        return l;
    }

    public string GetAttrStr(Def.AttrId type)
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
	 
}
