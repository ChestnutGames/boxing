using UnityEngine;
using System.Collections; 
using Assets.Scripts.Common;
using System.Collections.Generic;
using LuaInterface;



//交易
public class RechargeMgr : UnitySingleton<RechargeMgr>
{
    RechargePop pop; 

    private LuaState l;

    private List<VipData> vipList;

    public void OpenPop(RechargePop p)
    {
        pop = p;
        pop.SetInfo();
        pop.StoreClick();
        RechargeMgr.Instance.RechargeList();
        RechargeMgr.Instance.RewardList();
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


    public void RewardList()
    {
        NetworkManager.Instance.RechargeSwaredList();
    }

    public void RewardListCallback(C2sSprotoType.recharge_vip_reward_all.response resp)
    {

        foreach (C2sSprotoType.recharge_vip_reward o in resp.reward)
        {
            Debug.Log("vip等级:"+ o.vip.ToString() + "领取" + o.collected+"购买"+o.purchased);
        }
        Hashtable table = new Hashtable();
        for (int i = 1; i < GameShared.Instance.vipTable.Count; i++)
        {
            VipData vip = GameShared.Instance.GetVipByLevel(i);
            vip.isRevice = false;
            List<ItemViewData> gf = new List<ItemViewData>();
            List<GameShared.StrData> giftl = GameShared.Instance.GetStrData(vip.swared);
            for (int j = 0; j < giftl.Count; j++)
            {
                ItemData d = GameShared.Instance.GetItemData(giftl[j].id); 
                ItemViewData v = new ItemViewData();
                v.data = d;
                v.curCount = giftl[j].num; 
                gf.Add(v);
            }  
            List<GameShared.StrData> list2 = GameShared.Instance.GetStrData(vip.gift_swared);
            List<ItemViewData> sw = new List<ItemViewData>();
            for (int k = 0; k < list2.Count; k++)
            {
                ItemData d = GameShared.Instance.GetItemData(list2[k].id); 
                ItemViewData v = new ItemViewData();
                v.data = d;
                v.curCount = list2[k].num; 
                sw.Add(v);
            }
            vip.giftList = sw; 
            vip.rewardList = gf;
            table.Add(vip.vip_level,vip);
        }
        if(resp.reward!=null)//领取
        {
            for (int i = 0; i < resp.reward.Count; i++)
            { 
                VipData vip = GameShared.Instance.GetVipByLevel((int)(resp.reward[i].vip));
                vip.isRevice = resp.reward[i].collected;
                vip.isPurchased = resp.reward[i].purchased;

                List<ItemViewData> rl = new List<ItemViewData>();
                vip.rewardList = new List<ItemViewData>();
                for (int j = 0; j < resp.reward[i].props.Count; j++)
                {
                    int id = (int)resp.reward[i].props[j].csv_id;
                    ItemData item = GameShared.Instance.GetItemData(id);
                    ItemViewData data = new ItemViewData();
                    data.data = item;
                    data.curCount = (int)resp.reward[i].props[j].num;
                    vip.rewardList.Add(data); 
                }
                if (table.Contains(vip.vip_level))
                {
                    table[vip.vip_level] = vip; 
                }
                else
                {
                    table.Add(vip.vip_level, vip); 
                }
            } 
        }
        table.Remove(0);
        pop.SetVipList(table); 
        pop.SetVipInfo((int)UserManager.Instance.vip_level); 
    }  

    public void RechargeList()
    {
        NetworkManager.Instance.RechargeList();
    }

    public void RechargeListCallback(C2sSprotoType.recharge_all.response resp)
    {
        List<ReChargeData> list = new List<ReChargeData>();
        if (resp.l != null)
        {
            for (int i = 0; i < resp.l.Count; i++)
            {
                ReChargeData re = new ReChargeData(); //GameShared.Instance.GetReChargeById((int)resp.l[i].csv_id);
                re.id = (int)resp.l[i].csv_id;
                re.icon = GameShared.Instance.GetIconPathById((int)resp.l[i].icon_id);
                re.first = (int)resp.l[i].first;
                re.icon_id = (int)resp.l[i].icon_id;
                re.rmb = (int)resp.l[i].rmb;
                re.gift = (int)resp.l[i].gift;
                re.name = resp.l[i].name;
                re.diamond = (int)resp.l[i].diamond;
                if (re.first < 1)
                {
                    re.Once = false;
                }
                else
                {
                    re.Once = true;
                }
                list.Add(re);
            } 
        }
        pop.SetStore(list); 
    }

    public void VipBuy()
    {
        if (UserManager.Instance.diamond < pop.data.diamond_show)
        {
            MainUI.Instance.DiomandToClick();  
        }
        else 
        {
            C2sSprotoType.recharge_vip_reward_purchase.request obj = new C2sSprotoType.recharge_vip_reward_purchase.request();
            obj.vip = pop.curVipIndex;
            pop.vipBuyBtn.isEnabled = false;
            NetworkManager.Instance.RechargeVipPurchase(obj);
        }
    }
    public void Rewared()
    {
        C2sSprotoType.recharge_vip_reward_collect.request obj = new C2sSprotoType.recharge_vip_reward_collect.request();
        obj.vip = pop.GetVipInfo().vip_level;
        pop.rewaredBtn.isEnabled = false;
        NetworkManager.Instance.RechargeSwared(obj);
    }

    public void RewaredCallback(C2sSprotoType.recharge_vip_reward_collect.response resp)
    {
        pop.rewaredBtn.isEnabled = true;
        if (resp != null && resp.errorcode == 1)
        {
            pop.data.isRevice = true;
            pop.CheckBtn();
            for (int i = 0; i < pop.data.rewardList.Count; i++)
            {
                BagMgr.Instance.AddItemNumById(pop.data.rewardList[i].data.id, pop.data.rewardList[i].curCount);
            }
            MainUI.Instance.GetItemClick(pop.data.rewardList);
        } 
    }

    public void RechargePurchase(RechargeView v)
    { 
        C2sSprotoType.recharge_buy b = new C2sSprotoType.recharge_buy();
        b.csv_id = v.data.id;
        b.num = 1;
        List<C2sSprotoType.recharge_buy> list = new List<C2sSprotoType.recharge_buy>();
        list.Add(b);
        NetworkManager.Instance.RechargePurchase(list); 
    }

    public void RechargeVipPurchaseCallBack(C2sSprotoType.recharge_vip_reward_purchase.response resp)
    {
        pop.vipBuyBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            List<ItemViewData> list = new List<ItemViewData>();
            if (resp.l != null)
            {
                for (int i = 0; i < resp.l.Count; i++)
                {
                    BagMgr.Instance.SetItemNumById((int)resp.l[i].csv_id, (int)resp.l[i].num);
                }
            }
            UserManager.Instance.SubDiamond(pop.data.diamond_show);
            pop.data.isPurchased = true;
            pop.CheckBtn();
            Debug.Log("购买成功");
        }
    } 

    public void RechargePurchaseCallBack(C2sSprotoType.recharge_purchase.response resp)
    {
        List<ItemViewData> list = new List<ItemViewData>();
        if (resp.u != null)
        {
            UserManager.Instance.SetVipData((int)resp.u.uviplevel);
            UserManager.Instance.recharge_total = (int)resp.u.recharge_diamond;
            UserManager.Instance.SetDiamond((int)resp.u.diamond);
        } 
        Debug.Log("购买成功");
    } 

}
