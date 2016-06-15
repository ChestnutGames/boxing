using UnityEngine;
using System.Collections; 
using Assets.Scripts.Common;
using System.Collections.Generic;
using System;

public class StoreMgr : UnitySingleton<StoreMgr>
{ 
    public List<ProductData> dataList;
    private StorePop pop;
    private StoreBuyPop buypop;
    //private LuaState l;


    public int store_refresh_count_max;
    public int goods_refresh_count;
 

    public void OpenPop(StorePop p)
    {
        pop = p;
        GetShopList();
    }

    public void Start()
    {
        InitLua();  
    }

    public void InitLua()
    { 
        //TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Achievement); 
        //l = new LuaState();
        //l.DoString(scriptFile.text);

        //LuaFunction f = l.GetFunction("InitLua");
        //f.Call(this);
    }

    public void SetListByTab(int i)
    {
        pop.tabid = i;
        if (i == 1)
        {

            pop.SetList(normalList);
        }
        else
        {
            pop.SetList(pointList);
        }

    }


    public void SetPopList(List<ProductData> list)
    {
        if (pop.tabid == 1)
        {

            pop.SetList(normalList);
        }
        else
        {
            pop.SetList(pointList);
        } 
    } 

    public void GetShopList()
    { 
            NetworkManager.Instance.ShopList();
            //BagMgr.Instance.BagList(); 
    }

    public void OpenBuyPop(int i)
    {
        if (pop.curFouse != i)
        {
            if (pop.curFouse < pop.viewList.Count)
                pop.viewList[pop.curFouse].ChangeFocus(false);
            pop.viewList[i].ChangeFocus(true);
            pop.curFouse = i;
        }
        if (MainUI.Instance.GetPopState(MainUI.PopType.StoreBuy) != true)
        {
            GameObject obj = Instantiate(pop.popPrefab);
            obj.SetActive(true);
            buypop = obj.GetComponent<StoreBuyPop>();
            buypop.InitData(pop.viewList[i].data, i);
            buypop.transform.parent = pop.transform.parent;
            buypop.transform.localPosition = Vector3.zero;
            buypop.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.StoreBuy, true);
        } 
    }

    List<ProductData> normalList;
    List<ProductData> pointList;

    public void ShopListCallBack(C2sSprotoType.shop_all.response resp)
    {
        normalList = new List<ProductData>();
        pointList = new List<ProductData>();
        dataList = new List<ProductData>();
        for (int i = 0; i < resp.l.Count; i++)
        {
            ProductData d = new ProductData();
            d.data = GameShared.Instance.GetItemData((int)resp.l[i].g_prop_csv_id);
            d.csv_id = (int)resp.l[i].csv_id;
            d.g_prop_csv_id = (int)resp.l[i].g_prop_csv_id;
            d.inventory = (int)resp.l[i].inventory;
            d.currency_type = (int)resp.l[i].currency_type;
            d.currency_num = (int)resp.l[i].currency_num;   
            store_refresh_count_max = (int) resp.store_refresh_count_max;
            goods_refresh_count = (int) resp.goods_refresh_count;
            Debug.Log("id" + resp.l[i].g_prop_csv_id + "time" + resp.l[i].countdown); 
            d.countdown = (int)resp.l[i].countdown; 
            d.refresh_time = DateTime.Now.AddSeconds(d.countdown); 
            switch (d.currency_type)
            {
                case 1:
                    d.currency_icon = Def.DiamondTex;
                    break;
                case 2:
                    d.currency_icon = Def.CoinTex;
                    break;
                case 3:
                    d.currency_icon = "金币";
                    break;
                case 4:
                    d.currency_icon = "金币";
                    break;
            }
            d.isHot = false;

            if (d != null)
            {
                if (resp.l[i].countdown != null)
                {
                    int a = (int)resp.l[i].countdown;
                    d.isShowTime = true;
                }
                else
                {
                    d.isShowTime = false;
                }
                dataList.Add(d);
            }

            if (d.csv_id / 1000 == 1)
            {
                normalList.Add(d);
            }
            else
            {
                pointList.Add(d);
            }
        } 
        SetPopList(dataList);
    }

    public void Purchase(StoreBuyPop p)
    { 
            if (!UserManager.Instance.ResByType(p.data.currency_type, p.data.currency_num * p.count))
            {
                if (p.data.currency_type == 1)
                {
                    MainUI.Instance.DiomandToClick();
                }
            }
            else 
            {
                p.data.buy_count = p.count;
                List<ProductData> l = new List<ProductData>();
                l.Add(p.data);
                buypop = p;
                buypop.buyBtn.isEnabled = false;
                NetworkManager.Instance.ShopPurchase(l);
            } 
    }
 

    public void PurchaseCallBack(C2sSprotoType.shop_purchase.response resp)
    {
        buypop.buyBtn.isEnabled = true;
        if (buypop != null && resp.errorcode == 1)
        { 
            buypop.data.inventory -= buypop.data.buy_count;
            if (resp.ll != null)
            {
                for (int i = 0; i < resp.ll.Count; i++)
                {
                    if (buypop.data.csv_id == resp.ll[i].csv_id)
                    {
                        buypop.data.inventory = (int)resp.ll[i].inventory;
                        buypop.data.countdown = (int)resp.ll[i].countdown; 
                        buypop.data.refresh_time = DateTime.Now.AddSeconds(buypop.data.countdown);
                    }
                }
            } 
            switch (buypop.data.currency_type)
            {
                case 2:
                    UserManager.Instance.SubCoin(buypop.data.currency_num * buypop.count);
                    break;
                case 1:
                    UserManager.Instance.SubDiamond(buypop.data.currency_num * buypop.count);
                    break;
            }
            for (int i = 0; i < resp.l.Count; i++)
            {
                Debug.Log("csv_id" + resp.l[i].csv_id + "num" + resp.l[i].num + "resp.goods_refresh_count" + resp.goods_refresh_count + "store_refresh_count_max" + resp.store_refresh_count_max);
                BagMgr.Instance.AddItemNumById((int)resp.l[i].csv_id, (int)resp.l[i].num);
            }    
            store_refresh_count_max = (int)resp.store_refresh_count_max;
            goods_refresh_count = (int)resp.goods_refresh_count; 
            buypop.CloseClick(); 
        }
    }



    public void ShopRefresh(StoreBuyPop data)
    {
        goods_refresh_count++;
        RefreshCostData r = GameShared.Instance.GetRefreshCostByCount(goods_refresh_count);
        if (store_refresh_count_max < goods_refresh_count)
        {
            ToastManager.Instance.Show("超过刷新次数");
        }
        else if (!UserManager.Instance.ResByType(r.currency_type,r.currency_num))
        {
            MainUI.Instance.DiomandToClick(); 
        }
        else
        {
            buypop = data;
            buypop.refrushBtn.isEnabled = false;
            NetworkManager.Instance.ShopRefresh(buypop.data.csv_id); 
        }
    }

    public void ShopRefreshCallBack(C2sSprotoType.shop_refresh.response resp)
    {
        buypop.buyBtn.isEnabled = true;
        if (buypop != null && resp.errorcode ==1)
        { 
            buypop.data.countdown = (int)resp.l[0].countdown; 
            buypop.data.refresh_count++;
            buypop.data.refresh_time = DateTime.Now.AddSeconds(buypop.data.countdown);

            RefreshCostData r = GameShared.Instance.GetRefreshCostByCount(goods_refresh_count);
            BagMgr.Instance.SubItemNumById(r.currency_type, r.currency_num); 

            buypop.CheckAddSub();
            buypop.CheckRefresh();
            for (int i = 0; i < resp.l.Count; i++)
            {
                Debug.Log("csv_id" + resp.l[i].g_prop_csv_id + "g_prop_num" + resp.l[i].g_prop_num + "resp.goods_refresh_count" + resp.goods_refresh_count + "store_refresh_count_max" + resp.store_refresh_count_max);
            }    
            ProductData p =  GameShared.Instance.GetStoreItem(buypop.data.csv_id);
            store_refresh_count_max = (int)resp.store_refresh_count_max; 
            goods_refresh_count = (int)resp.goods_refresh_count;
            buypop.data.inventory = p.inventory;
            buypop.RestInfo(buypop.data); 
        }
        buypop = null; 
    }

  
 
     
}
