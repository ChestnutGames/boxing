using UnityEngine;
using System.Collections;
using LuaInterface;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class BagMgr : UnitySingleton<BagMgr>
{ 
    private List<ItemViewData> dataList;
    private ItemView curCallBackView;
    public BagPop pop;
    public BagUsePop usePop;

    public Dictionary<int, ItemViewData> itemTable = new Dictionary<int, ItemViewData>();

    private LuaState l;

    private int curItemIndex;

    private ItemView GetItemView(int i)
    {
        return pop.viewList[i] as ItemView;
    }

    public void OpenPop(BagPop p)
    {
        pop = p;
        BagMgr.Instance.BagList();
        pop.SetTab();
    }

    public void RestEmpty()
    {
        itemTable.Clear();  
    }

    public void Start()
    {
        //InitLua();
    }

    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Bag);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void BagList()
    {
        //if (ClientSockt.Instance.connected)
        //{
            NetworkManager.Instance.BagList();
        //}
        //else
        //{
        //    if (pop != null)
        //    {
        //        pop.SetList(dataList);
        //    }
        //}
    }
    
    public void BagListClientCallback()
    {
        dataList = new List<ItemViewData>(); 
        System.Collections.IDictionaryEnumerator enumerator = this.itemTable.GetEnumerator(); 
        while (enumerator.MoveNext())
        {

            ItemViewData d = itemTable[(int)enumerator.Key];// as ItemViewData;
            dataList.Add(d);
        }    
        if (pop != null)
        {
            pop.SetList(dataList);
        }
    }

    public void BagListCallBack(C2sSprotoType.props.response resp)
    {
        dataList = new List<ItemViewData>();
        itemTable = new Dictionary<int, ItemViewData>();
        for (int i = 0; i < resp.l.Count; i++)
        { 
            ItemViewData d = InitItemViewData((int)resp.l[i].csv_id, (int)resp.l[i].num); 
            if (d.curCount > 0&& d.data!=null)
            {
                dataList.Add(d);
                itemTable.Add(d.data.id, d);
            }
        } 
        //if (pop != null)
        //{
        //    pop.InitBag(); 
        //}
        if (pop != null)
        {
            pop.SetList(dataList);
        }
        //SetList();
    }
    /// <summary>
    /// 初始化用于显示在背包的itemview
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public ItemViewData InitItemViewData(int id, int num)
    { 
        ItemViewData d = new ItemViewData();
        ItemData sql = GameShared.Instance.GetItemData(id);
        d.data = sql;
        d.isUse = true;
        if (sql == null)
        {
            Debug.Log("sql == null"+id);
        }
        if (sql.useType == 0)
        {
            d.isUse = false;
        }
        d.sort = ((int)d.data.bagType).ToString() + (10-(int)d.data.quality) + (int)d.data.useType; 
        switch (d.data.quality)
        {
            case ItemData.QualityType.White:
                d.kuang = "white";
                break;
            case ItemData.QualityType.Green:
                d.kuang = "green";
                break;
            case ItemData.QualityType.Blue:
                d.kuang = "blue";
                break;
            case ItemData.QualityType.Purple:
                d.kuang = "purple";
                break;
            case ItemData.QualityType.Glod:
                d.kuang = "orange";
                break;
            case ItemData.QualityType.Red:
                d.kuang = "red";
                break; 
            default:
                d.kuang = "48sdf";
                break;
        } 

        if (sql != null)
        {
            sql.id = id;
            d.curCount = num;
        }
        return d;
    }


    public void SetList()
    {
        System.Collections.IDictionaryEnumerator enumerator = itemTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ItemViewData r = itemTable[(int)enumerator.Key];// as ItemViewData;  
        }   
    }

    public ItemViewData GetItemViewData(int id)
    {
        if (itemTable.ContainsKey(id))
        {
            return itemTable[id] as ItemViewData;
        }
        return null;
    }

    /// <summary>
    /// 设置物品数量 背包外使用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void SetItemNumById(int id,int num)
    { 
        if(id<5)
        {
            switch(id)
            {
                case 1:
                    UserManager.Instance.SetDiamond(num);
                    break;
                case 2:
                    UserManager.Instance.SetGold(num);
                    break;
                case 3:
                    UserManager.Instance.SetExp(num);
                    break;
                case 4:
                    UserManager.Instance.SetFriendPoint(num);
                    break;
                case 6:
                    UserManager.Instance.SetHonor(num);
                    break;
            }
        }
        else if (itemTable != null && itemTable.ContainsKey(id))
        {
            ItemViewData v = itemTable[id] as ItemViewData;
            v.curCount = num; 
        }
        else
        {

            ItemViewData v = new ItemViewData();
            v.curCount = num;
            v.data = GameShared.Instance.GetItemData(id);
            itemTable.Add(v.data.id, v); 
        } 
    }
    
    /// <summary>
    /// 获得道具数 背包外使用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetItemNumById(int id)
    {
        int num = 0;
        if (id < 5)
        {
            switch (id)
            {
                case 1:
                    num = (int)UserManager.Instance.GetDiamond();
                    break;
                case 2:
                    num = (int)UserManager.Instance.GetCoin();
                    break;
                case 3:
                    num = (int)UserManager.Instance.GetExp();
                    break;
                case 4:
                    num = (int)UserManager.Instance.GetFriendPoint();
                    break;
                case 6:
                    num = (int)UserManager.Instance.GetHonor();
                    break;
            }
        }else if (itemTable!=null && itemTable.ContainsKey(id))
        {
            ItemViewData v = itemTable[id] as ItemViewData;
            num = v.curCount;
        }
        return num;
    }
    /// <summary>
    /// 从缓存增加道具 背包外使用
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <returns>增加后有多少</returns>
    public int AddItemNumById(int id,int num)
    {  
        int i = 0;

        if (id < 5)
        {
            switch (id)
            {
                case 1:
                    UserManager.Instance.AddDiamond(num);
                    break;
                case 2:
                    UserManager.Instance.AddCoin(num);
                    break;
                case 3:
                    UserManager.Instance.AddExp(num);
                    break;
                case 4:
                    UserManager.Instance.AddFriendPoint(num);
                    break;
                case 6:
                    UserManager.Instance.AddHonor(num);
                    break;
            }
        }
        else if (itemTable != null && itemTable.ContainsKey(id))
        {
            ItemViewData v = itemTable[id] as ItemViewData;
            v.curCount = num;
            i = v.curCount;
        }
        else 
        {
            ItemViewData v = new ItemViewData();
            v.curCount = num;
            v.data = GameShared.Instance.GetItemData(id);
            itemTable.Add(v.data.id,v);
            i = v.curCount;
        }
        return i;
    }
    /// <summary>
    /// 从缓存减少道具 背包外使用
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <returns>使用后还有多少</returns>
    public int SubItemNumById(int id,int num)
    {
        int i = 0;
        if (id < 5)
        {
            switch (id)
            {
                case 1:
                    UserManager.Instance.SubDiamond(num);
                    break;
                case 2:
                    UserManager.Instance.SubCoin(num);
                    break;
                case 3:
                    UserManager.Instance.SubExp(num);
                    break;
                case 4:
                    UserManager.Instance.SubFriendPoint(num);
                    break;
                case 6:
                    UserManager.Instance.SubHonor(num);
                    break;
            }
        }else if (itemTable != null && itemTable.ContainsKey(id))
        {
            ItemViewData v = itemTable[id] as ItemViewData;
            if (v.curCount > num)
            {
                v.curCount -= num;
                i = v.curCount;
            } 
        }
        return i;
    }

    public void UseTest()
    { 
        int a = 0;     
        System.Collections.IDictionaryEnumerator enumerator = itemTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ItemViewData r = itemTable[(int)enumerator.Key];// as ItemViewData;  
            a += r.curCount;
        }
        Debug.Log("num" + a);
    }

    public void Inve()
    {
        this.Invoke("UseItem",0.03f);
 
    }

    public void OpenUsePop(ItemView v)
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.BagUse) != true)
        {
            GameObject obj = Instantiate(pop.usePrefab);
            obj.SetActive(true);
            usePop = obj.GetComponent<BagUsePop>();
            usePop.InitData(v.data);
            usePop.transform.parent = pop.transform.parent;
            usePop.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.BagUse, true);
        } 
    }

    public void UseItem()
    { 
            ItemView v = GetItemView(curItemIndex); 
            curCallBackView = v;
            if (v.data != null && v.data.curCount > 1 && v.data.data.useType != 0)
            {
                OpenUsePop(v);
            }
            else if (v.data != null && v.data.curCount > 0 && v.data.data.useType != 0)
            {
                NetworkManager.Instance.ItemUse(v.data.data, -1, UserManager.Instance.battleRoleID);//UserManager.Instance.GetCurRole().roleId);  
                pop.useBtn.isEnabled = false;  
            } 
    }

    public void UseItem(int count)
    { 
            ItemView v = GetItemView(curItemIndex);
            if (v.data != null && v.data.curCount > 0 && v.data.data.useType != 0)
            {
                curCallBackView = v;
                pop.useBtn.isEnabled = false;
                NetworkManager.Instance.ItemUse(v.data.data, -count, UserManager.Instance.battleRoleID);//UserManager.Instance.GetCurRole().roleId);  
            } 
    }

    public ItemView GetTableChild(int i)
    {
        return pop.grid.GetChildList()[i].gameObject.GetComponent<ItemView>();
    }


    public void UseItemCallBack(C2sSprotoType.use_prop.response resp)
    {
        pop.useBtn.isEnabled = true;
        if (curCallBackView != null && resp.errorcode == 1)
        {
            if (resp.errorcode == 3)
            {
                print("不能使用");
                return;
            }

            List<ItemViewData> addl = new List<ItemViewData>();
            
            //改变背包内道具
            if(resp.props.Count >0)
            { 
                for (int i = 0; i < resp.props.Count; i++)
                {
                    ItemViewData add = null;
                    //Debug.Log("csv_id" + resp.props[i].csv_id + "num" + resp.props[i].num); 
                    if(itemTable.ContainsKey((int)resp.props[i].csv_id))
                    {
                        ItemViewData v = itemTable[(int)resp.props[i].csv_id];
                        if(resp.props[i].csv_id<5)//加金币 经验
                        {
                            switch (resp.props[i].csv_id)
                            {
                                case 1:
                                    UserManager.Instance.SetDiamond((int)resp.props[i].num);
                                    break;
                                case 2:
                                    UserManager.Instance.SetGold((int)resp.props[i].num);
                                    break;
                                case 3:
                                    UserManager.Instance.SetExp((int)resp.props[i].num);
                                    break;
                                case 4:
                                    UserManager.Instance.SetFriendPoint((int)resp.props[i].num);
                                    break;
                                case 6:
                                    UserManager.Instance.SetHonor((int)resp.props[i].num);
                                    break;
                            }
                            if (v.curCount < resp.props[i].num)
                            {
                                add = new ItemViewData();
                                add.curCount = (int)resp.props[i].num - v.curCount;
                                add.data = GameShared.Instance.GetItemData((int)resp.props[i].csv_id);
                                addl.Add(add);
                            } 
                        } 
                        else if (v != null)//原来就有道具
                        { 
                            //获得增加多少道具
                            if(v.curCount<resp.props[i].num)
                            {
                                add = new ItemViewData();
                                add.curCount = (int)resp.props[i].num-v.curCount; 
                                add.data = v.data;
                                addl.Add(add);
                            } 
                            if ((int)resp.props[i].num < 1)
                            {
                                RemoveItem(v.view); 
                            }
                            else if(v.view != null) 
                            {
                                v.view.SetCount((int)resp.props[i].num);  
                            } 
                        }
                    } 
                    else//新增道具
                    { 
                        ItemViewData d = InitItemViewData((int)resp.props[i].csv_id, (int)resp.props[i].num);  
                        if (d.curCount > 0 && d.data.id != null)
                        {
                            itemTable.Add(d.data.id, d);
                        }
                        pop.AddItem(d);

                        //获得增加多少道具 
                            add = new ItemViewData();
                            add.curCount = (int)resp.props[i].num ;
                            add.data = d.data;
                            addl.Add(add); 
                    }
                } 
            }
            //是否用完 设置描述
            Debug.Log("csv_id" + resp.props[0].csv_id + "num" + resp.props[0].num);
            if (resp.props[0].num > 0)
            {
                pop.SetCount((int)resp.props[0].num);
            }
            else
            {
                //变到第一个 
                SelectItem(GetTableChild(0));
            } 
            curCallBackView = null;
            pop.useBtn.isEnabled = true;
            if (usePop != null)
            {
                usePop.CloseClick();
                usePop = null;
            } 
            MainUI.Instance.GetItemClick(addl);
            if (addl.Count == 0)
            {
                Debug.Log("null");
            }
        }
    }
 
    public void RemoveItem(ItemView v)
    {
        dataList.Remove(v.data);
        v.data.view = null; 
        v.name = "None";
        v.RestEmpty();
        v.data = null;
        pop.grid.Reposition();
        pop.grid.repositionNow = true; 
    }

    public void RefreshBag()
    { 
        pop.grid.Reposition();
        pop.grid.repositionNow = true; 
    }

    public void SelectItem(ItemView v)
    {
        if (pop != null)
        {
            ((ItemView)pop.viewList[curItemIndex]).SetFous(false);
            pop.SetItemDesc(v);
            curItemIndex = v.index;
        }
    }
    
}
