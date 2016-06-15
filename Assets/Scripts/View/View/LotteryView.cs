using UnityEngine;
using System.Collections;
using System;

public class LotteryView : MonoBehaviour {

    public UISprite chouNumTxt;
    public UISprite chouNum;
    public UILabel time;
    public UILabel desc;
    public UISprite icon;
    public UILabel money;
    public UISprite moneyIcon;

    public UILabel aname;
    public UILabel amoney;
    public UILabel anum;
    public UISprite amoneyIcon;
    public UIButton abuyBtn;
    public UISprite ascale;

    public GameObject front;
    public GameObject after;

    public LotteryData data;

    private LotteryPop pop;
    private int index;
    private PassiveTimer timer; 

    public bool isBuyLayer;

    public void InitData(LotteryPop p, LotteryData d, int i)
    {
        pop = p;
        data = d;
        index = i;
        SetInfoAfter();
        SetInfo();
        isBuyLayer = false;
        time.text = Comm.DateDiff(data.refresh_time, DateTime.Now);
        timer = new PassiveTimer(1);
    }

    void SetInfoAfter()
    {
        aname.text = data.name;
        amoney.text = data.money.ToString();
        //anum.text = data.kechou;
        Check();
        amoneyIcon.spriteName = data.money_icon;
        abuyBtn.normalSprite = data.btnnor;
        abuyBtn.disabledSprite = data.btndis; 
        ascale.gameObject.SetActive(data.isScale);  
    }

    void SetInfo()
    {
        chouNumTxt.spriteName = data.num_name;
        chouNum.spriteName = data.num;
        money.text = data.money.ToString();
        icon.spriteName = data.icon;
        desc.text = data.desc;
        time.text = "";
        moneyIcon.spriteName = data.money_icon;  
    }

    public void Rest()
    {
        Check();
    }

    public void Check()
    {
        if (data.id == 1)
        {
            anum.text = "今日友情抽奖可抽次数\n" + (1-data.drawnum) + "/1";
        }
        else if (data.id == 2)
        {
            int a = 0;
            if (data.lefttime < 1)
                a = 1;
            anum.text = "今日免费次数\n" + a + "/1";
            if (data.drawnum == 0)
                data.isShowTime = true;
        }else if(data.id == 3)
        {
            data.isShowTime = false;
        } 
        ShowTime(data.isShowTime);
    }

    void setBtn(string str)
    {
        abuyBtn.normalSprite = str; 
    }

    void ShowTime(bool b)
    {
        if (!b)
        {
            time.gameObject.SetActive(false);
            desc.gameObject.SetActive(true);
        }
        else
        {
            time.gameObject.SetActive(true);
            desc.gameObject.SetActive(false);
        }
    }

    void StartTimer()
    {
        timer = new PassiveTimer(1);

    } 
     

    void ChangeView(bool b)
    {
        if (b)
        {
            front.SetActive(false);
            after.SetActive(true);
        }
        else
        {
            front.SetActive(true);
            after.SetActive(false);
        }
    }   
	 
	void Update () {
        if (timer!= null && timer.Update(Time.deltaTime) > 0)
        {
            string str = Comm.DateDiffHour(DateTime.Now, data.refresh_time);
            time.text = str;
        } 
	}

    public void LotteryClick()
    {
        //todo 抽奖
        LotterMgr.Instance.Chou(index);
    } 
}
