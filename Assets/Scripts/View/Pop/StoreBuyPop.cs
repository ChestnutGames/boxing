using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StoreBuyPop : MonoBehaviour
{
    public UIButton addBtn;
    public UIButton subBtn;
    public UIButton refrushBtn;
    public UIButton buyBtn;

    public UILabel desc;
    public UILabel haveCount;
    public UILabel name;
    public UILabel type;
    public UILabel stockCount;
    public UILabel stockTime;
    public UILabel num;
    public UILabel price;

    public GameObject stockObj;

    public UISprite icon;

    public int count;
    public int stockNum;
    public ProductData data;
    private PassiveTimer timer;
     

    public void InitData(ProductData d, int i)
    {
        data = d;
        Debug.Log("id" + d.csv_id + "pid" + d.g_prop_csv_id); 
        timer = new PassiveTimer(1);
        SetInfo();
    }

    public void RestInfo(ProductData d)
    {
        data = d;
        SetInfo();
    }

    void SetInfo()
    {
        if (data != null)
        {
            ItemData item = GameShared.Instance.GetItemData(data.g_prop_csv_id); 
            if (item != null)
            {
                desc.text = item.desc;
                icon.spriteName = item.path;
                type.text = item.typeName;
                name.text = item.name;
                haveCount.text = BagMgr.Instance.GetItemNumById(item.id).ToString();
            }

            stockNum = data.inventory;
            if (stockNum == 99)
            {
                stockObj.SetActive(false);
                count = 1;
                if (stockNum != 99)
                {
                    stockNum--;
                }
                SetNum(count);
                addBtn.isEnabled = true;
            }
            else
            {
                stockObj.SetActive(true);
                if (stockNum > 0)
                {
                    count = 1;
                    if (stockNum != 99)
                    {
                        stockNum--;
                    } 
                    addBtn.isEnabled = true;
                }
                else
                {
                    addBtn.isEnabled = false;
                    count = 0;
                }
                SetNum(count); 
            }
            stockTime.text = "";
            string str = Comm.DateDiffHour(DateTime.Now, data.refresh_time);
            this.stockTime.text = str;
            CheckAddSub();
            CheckRefresh();
        } 
    }

    void SetNum(int c)
    {
        num.text = c.ToString();
        stockCount.text = stockNum.ToString();
    }

    

    public void AddClick()
    {
        if (stockNum > 0)
        {
            if (stockNum != 99)
            {
                stockNum--;
            } 
            count++;
            stockCount.text = (stockNum).ToString();
            SetNum(count);
        }
        CheckAddSub();
    }

    public void SubClick()
    {
        if (count > 0)
        {
            if (stockNum != 99)
            {
                stockNum++;
            } 
            count--;
            stockCount.text = (stockNum).ToString();
            SetNum(count);
        }
        CheckAddSub();
    }

    public void CheckRefresh()
    {
        int temp = data.refresh_count;
        if (data.refresh_count == 0)
        {
            temp = data.refresh_count + 1;
        }

        RefreshCostData a = GameShared.Instance.GetRefreshCostByCount(temp);

        bool b = UserManager.Instance.ResByType(a.currency_type, a.currency_num);
        if (data.inventory < 1)
        {
            stockTime.gameObject.SetActive(true);
            refrushBtn.gameObject.SetActive(true);
            if (b && StoreMgr.Instance.store_refresh_count_max > StoreMgr.Instance.goods_refresh_count)
            {
                refrushBtn.isEnabled = true;
            }
            else
            {
                refrushBtn.isEnabled = false;
            }
        }
        else
        {
            stockTime.gameObject.SetActive(false);
            refrushBtn.gameObject.SetActive(false);
        }
    }
    public void CheckAddSub()
    {
        if (stockNum > 0)
        {
            addBtn.isEnabled = true;
        }
        else
        {
            addBtn.isEnabled = false;
        }

        if (count > 0)
        {
            subBtn.isEnabled = true;
            buyBtn.isEnabled = true;
        }
        else
        {
            subBtn.isEnabled = false;
            buyBtn.isEnabled = false;
        }
        price.text = (data.currency_num * count).ToString();

    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.StoreBuy, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void BuyClick()
    { 
        data.buy_count = count; 
        StoreMgr.Instance.Purchase(this);
    } 

    public void RefreshClick()
    {
        StoreMgr.Instance.ShopRefresh(this); 
    } 

	void Update () {
        if (stockNum != 99)
        {
            if (data != null && timer!=null && timer.Update(Time.deltaTime) > 0)
            {
                string str = Comm.DateDiffHour(DateTime.Now, data.refresh_time);
                this.stockTime.text = str;
            }
        } 
	}
}
