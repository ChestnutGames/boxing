using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



//抽奖
public class LotterMgr : UnitySingleton<LotterMgr>
{
    //private List<LotteryView> viewList;
    //private List<LotteryData> dataList;

    public LotteryPop pop;
    public int showRoleIndex; 
    //private LuaState l;

    public void OpenPop(LotteryPop p)
    {
        pop = p;
        NetworkManager.Instance.LotteryList();
    }

    public void LotteryListCallback(List<LotteryData> list)
    { 
        LotteryData d = list[0];
        d.id = 1;
        d.name = "抽取1次";
        d.money = 100;
        d.type = LotteryData.MoneyType.Friend;
        d.money_icon = GetIconByType(d.type);
        d.desc = "友情抽奖";
        d.icon = "宝箱";
        d.btnnor = "购买一个1";
        d.btndis = "购买一个2";
        d.num_name = "抽取一次";
        d.num = "宝箱数1";
        d.kechou = "今日友情抽奖可抽次数\n";
        d.isScale = false;  
        LotteryData b = list[1];
        b.id = 2;
        b.name = "抽取1次";
        b.money = 240;
        b.type = LotteryData.MoneyType.Diamond;
        b.money_icon = GetIconByType(b.type);
        b.desc = "5次后逼出金色拳击手";
        b.icon = "宝箱";
        b.btnnor = "购买一个1";
        b.btndis = "购买一个2";
        b.kechou = "今日免费次数";
        b.num_name = "抽取一次";
        b.num = "宝箱数1";
        b.isScale = false;
        LotteryData c = new LotteryData();
        c.id = 3;
        c.name = "抽取10次";
        c.money = 2400;
        c.type = LotteryData.MoneyType.Diamond;
        c.money_icon = GetIconByType(c.type);
        c.desc = "10次后逼出金色拳击手";
        c.icon = "宝箱";
        c.btnnor = "购买十个1";
        c.btndis = "购买十个2";
        c.num_name = "抽取十次";
        c.num = "宝箱数10";
        c.isScale = true;
        //c.money =(int)(c.money * 0.9f);
        list.Add(c);
        pop.SetList(list);  
    }

    string GetIconByType(LotteryData.MoneyType t)
    {
        string str = "";
        switch (t)
        {
            case LotteryData.MoneyType.Coin:
                str = "金币";
                break;
            case LotteryData.MoneyType.Diamond:
                str = "D";
                break;
            case LotteryData.MoneyType.Friend:
                str = "友情点";
                break;
        }
        return str;
    } 
    private LotteryView curChouView;
    public void Chou(int index)
    {  
        int id = pop.viewList[index].data.id;
        curChouView = pop.viewList[index];
        
        bool mian = false;
        if (id == 2)
        {
            if (pop.viewList[index].data.lefttime < 1)
            {
                mian = true;
            }
            pop.cur = pop.viewList[index];
            if (UserManager.Instance.ResByType((int)pop.cur.data.type, pop.cur.data.money) || mian)
            {
                curChouView.abuyBtn.isEnabled = false;
                NetworkManager.Instance.LotteryChou(id, mian);
            }
            else
            {
                MainUI.Instance.DiomandToClick();
            }
        }
        else if (id == 1)
        {
            if (pop.viewList[index].data.drawnum < 1)
            {   
                pop.cur = pop.viewList[index];
                mian = true;
                if (UserManager.Instance.ResByType((int)pop.cur.data.type, pop.cur.data.money))
                {
                    curChouView.abuyBtn.isEnabled = false;
                    NetworkManager.Instance.LotteryChou(id, mian);
                }
                else
                {
                    ToastManager.Instance.Show("好友点数不够");
                }
            }
            else
            {
                ToastManager.Instance.Show("只能抽一次");
            }
        }
        else if (id == 3)
        {
            pop.cur = pop.viewList[index];
            if (UserManager.Instance.ResByType((int)pop.cur.data.type, pop.cur.data.money))
            {
                curChouView.abuyBtn.isEnabled = false;
                NetworkManager.Instance.LotteryChou(id, mian);
            }
            else
            {
                MainUI.Instance.DiomandToClick();
            }
        }

    }
    List<RoleStarData> roleList;
    public void ChouCallback(C2sSprotoType.applydraw.response resp)
    {
        curChouView.abuyBtn.isEnabled = true;
        List<ItemViewData> list = new List<ItemViewData>();
        roleList = new List<RoleStarData>();
        if (resp.errorcode == 1)
        {
            if (resp.list != null)
            {
                for (int i = 0; i < resp.list.Count; i++)
                {
                    ItemViewData re = new ItemViewData();
                    re.curCount = (int)resp.list[i].propnum;
                    re.time = (int)resp.lefttime;
                    re.isRole = (int)resp.list[i].proptype;
                    re.data = GameShared.Instance.GetItemData((int)resp.list[i].propid);
                    RoleStarData r = GameShared.Instance.GetRoleInfoByFragment((int)resp.list[i].propid);
                    Debug.Log("r.us_prop_csv_id" + resp.list[i].propid + "resp.list[i].proptype" + resp.list[i].proptype);
                    if (r != null && resp.list[i].proptype == 1)
                    {
                        roleList.Add(r);
                    }
                    list.Add(re);
                }

                if (pop.cur != null)
                {
                    if (pop.cur.data.id == 2)
                    {
                        pop.cur.data.isShowTime = true;
                        pop.cur.data.drawnum++;
                        if (pop.cur.data.drawnum > 1)
                        {
                            UserManager.Instance.SubDiamond(pop.cur.data.money);
                        }
                    }
                    else if (pop.cur.data.id == 1)
                    {
                        UserManager.Instance.SubFriendPoint(pop.cur.data.money);
                    }
                    else if (pop.cur.data.id == 3)
                    {
                        UserManager.Instance.SubDiamond(pop.cur.data.money);
                    }
                    pop.cur.data.lefttime = (int)list[0].time;
                    int a = (int)list[0].time;
                    pop.cur.data.drawnum++;
                    pop.cur.data.refresh_time = DateTime.Now.AddSeconds(a);
                    pop.cur.Check();
                    pop.cur = null;
                }
                if (list.Count > 0)
                {
                    OpenBiling(list); 
                }
            }
            else
            {
                if (pop.cur.data.id > 1)
                {
                    MainUI.Instance.DiomandToClick();
                }
                else
                {
                    Debug.Log("好友点数不够");
                }
            }
        }
    }

    public void AnimNext() 
    {
        
        if (roleList!=null && showRoleIndex < roleList.Count-1)
        {
            showRoleIndex++;
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.LotteryAmin),
                GameShared.Instance.GetSkeletonAssetByPath(roleList[showRoleIndex].anim)); 
        }
        else
        {
            MainUI.Instance.effect.gameObject.SetActive(false);
            MainUI.Instance.roleasset.gameObject.SetActive(false);
            roleList.Clear();
        } 
    }
    
    public void OpenBiling(List<ItemViewData> list)
    { 
        if (MainUI.Instance.GetPopState(MainUI.PopType.Biling) != true)
        {
            GameObject obj = Instantiate(pop.bilingPrefab);
            obj.SetActive(true);
            BillingPop b = obj.GetComponent<BillingPop>();
            b.InitData(list);
            b.transform.parent = pop.transform.parent;
            b.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.Biling, true);
        }
        if (roleList.Count > 0)
        {
            for (int i = 0; i < roleList.Count;i++ )
            {
                if (UserManager.Instance.RoleTable.ContainsKey(roleList[i].csv_id))
                {
                    RoleData r = UserManager.Instance.RoleTable[roleList[i].csv_id] as RoleData;
                    r.is_possessed = true; 
                } 
            }
            showRoleIndex = 0;
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.LotteryAmin),
                GameShared.Instance.GetSkeletonAssetByPath(roleList[showRoleIndex].anim),
                (aa, bb) =>
                {
                    AnimNext();
                });
        }
    } 
}
