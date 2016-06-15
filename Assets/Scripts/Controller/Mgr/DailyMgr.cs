using UnityEngine;
using System.Collections; 
using Assets.Scripts.Common;
using System.Collections.Generic;
using System;
using LuaInterface;

public class DailyMgr : UnitySingleton<DailyMgr>
{
    DailyPop pop;

    private DateTime duanTime;
    private DateTime goldTime;

    private bool isQianDao;
     

    public int monthSign;
    public int totalSign;
    public bool isSign;
    public int recivecCount;
    public int recivecTotalsign;


    public int goldLevel;
    public int duanLevel;


    private DailyView curView;
    LuaState l;
    DailyCountItem curDialyRec;
    DailyMissionView curDuanView;

    public void OpenPop(DailyPop p)
    {
        pop = p;
        pop.SetTab();
        DailyMgr.Instance.DailyList();
        //TabChange(DailyTab.Sign);
        pop.tabList[(int)DailyPop.DailyTab.Sign].SetState(true);
        pop.CheckBtnTime();
    }

    public void Start()
    {
        //InitLua();
    }

    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Daily);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void DailyList()
    { 
        NetworkManager.Instance.DailyList();
    }

    public void DailyListCallBack(C2sSprotoType.checkin.response resp)
    {
        if (pop != null)
        {
            Debug.Log("totalamount:" + resp.totalamount + "ifcheckin_t" + resp.ifcheckin_t + "recivecCount" + resp.rewardnum + "monthamount" + resp.monthamount);
            monthSign = (int)resp.monthamount;
            Debug.Log(resp.monthamount);
            totalSign = (int)resp.totalamount;
            recivecCount = (int)resp.rewardnum;
            isSign = resp.ifcheckin_t;
     
            GetQianDaoList(monthSign);
            GetRecList();
            SetSignNum(totalSign);
            CheckSignBtn();
        }
    }

    public void CheckSignBtn()
    {
        int count = GameShared.Instance.GetDailyCountByCount(totalSign);
        if (recivecCount<=count)
        {
            pop.rewaredBtn.isEnabled = true;
        }
        else
        {
            pop.rewaredBtn.isEnabled = false;
        }
    }

    public void GetQianDaoList(int count)
    {
        GameShared.Instance.CheckMonthDaily();
        List<QianDaoData> list = new List<QianDaoData>();
        int show_count = count;
        if (isSign) show_count++;
        for (int index = 0; index < 31; index++)
        {
            //设置格子
            QianDaoData q = GameShared.Instance.GetQianDao(index + 1);
            q.itemdata = GameShared.Instance.GetItemData(q.item_id);
            if (q != null)
            {
                q.signed = false;
                q.show = false;
                if (index < count)
                {
                    q.signed = true;
                }
                if (index < show_count)
                {
                    q.show = true;
                } 
                q.sort = (q.num + 100);
                list.Add(q);
            } 
        }
        pop.SetQianDao(list);
    }
    
    public void GetRecList()
    {
        //领取多少次
        curDialyRec = GameShared.Instance.GetDailyItemByIndex(recivecCount); 
        //可以领取多少次
        CheckSignBtn();
        List<GameShared.StrData> strs = GameShared.Instance.GetStrData(curDialyRec.item);
        List<ItemViewData> list = new List<ItemViewData>();
        int t = strs.Count;
        if (strs.Count > 3)
            t = 3;
        for (int i = 0; i < t; i++)
        {
            ItemData u = GameShared.Instance.GetItemData(strs[i].id);
            ItemViewData d = new ItemViewData();
            d.data = u;
            d.curCount = strs[i].num;
            list.Add(d);
        }
        curDialyRec.list = list;
        pop.SetRewared(curDialyRec.list);
    }

    public void Sign(DailyView v)
    { 
        curView = v;
        NetworkManager.Instance.DailySign(); 
    }

    public void SetSignNum(int num)
    { 
        //第几次领取
        //int count = GameShared.Instance.GetDailyCountByCount(totalSign);
        //该领第几次的
        recivecTotalsign = GameShared.Instance.GetDailyByRecNum(recivecCount).count;
        pop.qiandaoNum.text = totalSign + "/" + recivecTotalsign;  
    }

    public void SignCallback(C2sSprotoType.checkin_aday.response resp)
    {
        if (curView != null&& resp.errorcode == 1)
        {
            monthSign++;
            totalSign++;
            //pop.SetQianDao(monthSign);  
            SetSignNum(totalSign);
            curView.SetReceive(true);
            CheckSignBtn();
            BagMgr.Instance.AddItemNumById(curView.data.item_id, curView.data.item_count);
            if(UserManager.Instance.vip_level >= curView.data.vip_level)
            {
                BagMgr.Instance.AddItemNumById(curView.data.vip_item_id, curView.data.vip_item_count); 
            }
            curView = null;
        }  
        NetworkManager.Instance.UserInfo();
    }

    public void Receive()
    {
        if (totalSign < recivecTotalsign)
        {
            ToastManager.Instance.Show("签到次数不够");

        }else
        {
            C2sSprotoType.checkin_reward.request obj = new C2sSprotoType.checkin_reward.request();
            obj.totalamount = totalSign;
            obj.rewardnum = recivecCount;
            pop.rewaredBtn.isEnabled = false;
            NetworkManager.Instance.DailyRecivec(obj);  
        }
    }

    public void ReceiveCallback(C2sSprotoType.checkin_reward.response resp)
    {
        pop.rewaredBtn.isEnabled = true; 
        if (pop != null && resp.errorcode == 1)
        {
            for (int i = 0; i < curDialyRec.list.Count; i++)
            {
                BagMgr.Instance.AddItemNumById(this.curDialyRec.list[i].data.id, this.curDialyRec.list[i].curCount);
            }
            MainUI.Instance.GetItemClick(curDialyRec.list);
            recivecCount++;
            GetRecList();
            SetSignNum(totalSign);
            NetworkManager.Instance.UserInfo();
        }

    }
    public void Gold()
    {
        int index = 0;
        for (int i = 0; i < pop.goldCheckList.Length; i++)
        {
            if (pop.goldCheckList[i].toggle.value)
                index = i;
        }
        DailyMissionView v = pop.goldCheckList[index];

        if (v.data == null || UserManager.Instance.diamond < v.data.diamond_count)
        {
            MainUI.Instance.DiomandToClick();
        }else
        {
            C2sSprotoType.c_gold_once.request obj = new C2sSprotoType.c_gold_once.request();
            obj.c_gold_level = goldLevel;
            obj.daily_type = (int)v.data.daily_type;
            obj.c_gold_type = (int)v.data.dialy_sub;
            curGoldView = v;
            pop.goldBtn.isEnabled = false;
            NetworkManager.Instance.Gold(obj); 
        } 
    }
    DailyMissionView curGoldView;
    public void GoldCallback(C2sSprotoType.c_gold_once.response resp)
    {
        pop.goldBtn.isEnabled = true;
        if (curGoldView != null && resp.errorcode == 1)
        {
            UserManager.Instance.SubDiamond(curGoldView.data.diamond_count);
            UserManager.Instance.AddCoin(curGoldView.data.getNum); 

            pop.goldBtn.isEnabled = false;
            DateTime d = DateTime.Now.AddSeconds(resp.lefttime);
            int a = goldLevel + curGoldView.data.level_up;
            pop.goldlevel.text = a.ToString();
            pop.CheckBtnTime();
            pop.SetGoldTime(d); 
            GoldList();  
        }
        curGoldView = null;
    }

    public void GoldList()
    { 
        NetworkManager.Instance.GoldList();
    }

    public void GoldListCallback(C2sSprotoType.c_gold.response resp)
    {
        if (pop != null)
        {
            Debug.Log("resp.exercise_level" + resp.c_gold_level + "resp.lefttime" + resp.lefttime);
            goldLevel = (int)resp.c_gold_level;
            List<DailyData> list = new List<DailyData>();
            DailyData d1 = GameShared.Instance.GetDailyData(4);
            d1.daily_type = DailyData.DailyType.Glod;
            d1.dialy_sub = DailyData.DailySub.Easy; 
            List<GameShared.StrData> strs1 = GameShared.Instance.GetStrData(d1.rewared);
            d1.getNum = strs1[0].num + (d1.level_up * d1.level_rewared);
            list.Add(d1);

            DailyData d2 = GameShared.Instance.GetDailyData(5);
            d2.daily_type = DailyData.DailyType.Glod;
            d2.dialy_sub = DailyData.DailySub.Normal; 
            List<GameShared.StrData> strs2 = GameShared.Instance.GetStrData(d2.rewared);
            d2.getNum = strs2[0].num + (d2.level_up * d2.level_rewared);
            list.Add(d2);

            DailyData d3 = GameShared.Instance.GetDailyData(6);
            d3.daily_type = DailyData.DailyType.Glod;
            d3.dialy_sub = DailyData.DailySub.Hard; 
            List<GameShared.StrData> strs3 = GameShared.Instance.GetStrData(d3.rewared);
            d3.getNum = strs3[0].num + (d3.level_up * d3.level_rewared);
            list.Add(d3);

            pop.SetGold(list); 
            pop.goldBtn.isEnabled = resp.ifc_gold;
            DateTime d = DateTime.Now.AddSeconds(resp.lefttime);
            pop.SetGoldTime(d);
            pop.goldlevel.text = goldLevel.ToString(); 
        }
    }

    public void DuanLian()
    {

        int index = 0;
        for (int i = 0; i < pop.duanlianCheckList.Length; i++)
        {
            if (pop.duanlianCheckList[i].toggle.value)
                index = i;
        }
        DailyMissionView v = pop.duanlianCheckList[index];
         
        if (v.data == null || UserManager.Instance.diamond < v.data.diamond_count)
        {
            MainUI.Instance.DiomandToClick();
        }
        else
        {
            C2sSprotoType.exercise_once.request obj = new C2sSprotoType.exercise_once.request();
            obj.exercise_level = duanLevel;
            obj.daily_type = (int)v.data.daily_type;
            obj.exercise_type = (int)v.data.dialy_sub;
            curDuanView = v;
            pop.duanBtn.isEnabled = false;
            NetworkManager.Instance.Duan(obj);
        }
    }
    
    public void DuanLianCallback(C2sSprotoType.exercise_once.response resp)
    {
        pop.duanBtn.isEnabled = true;
        if (curDuanView != null && resp.errorcode == 1)
        {
            UserManager.Instance.SubDiamond(curDuanView.data.diamond_count);
            UserManager.Instance.AddExp(curDuanView.data.getNum); 
            
            pop.duanBtn.isEnabled = false;
            DateTime d = DateTime.Now.AddSeconds(resp.lefttime);
            pop.SetDuanTime(d);
            int a = duanLevel + curDuanView.data.level_up;
            pop.duanlevel.text = a.ToString();
            pop.CheckBtnTime();
            DuanLianList(); 
        } 
        curDuanView = null;
    }
    public void DuanLianList()
    {
        pop.duanLastTime.gameObject.SetActive(false);
        pop.duanBtn.gameObject.SetActive(false);
        NetworkManager.Instance.DuanList();
    }
     
    public void DuanLianListCallback(C2sSprotoType.exercise.response resp)
    {
        if (pop != null)
        {
            Debug.Log("resp.exercise_level" + resp.exercise_level + "resp.lefttime" + resp.lefttime);
            duanLevel = (int)resp.exercise_level;
            List<DailyData> list = new List<DailyData>();

            DailyData d1 = GameShared.Instance.GetDailyData(1);
            d1.daily_type = DailyData.DailyType.Duan;
            d1.dialy_sub = DailyData.DailySub.Easy;
            List<GameShared.StrData> strs1 = GameShared.Instance.GetStrData(d1.rewared); 
            d1.getNum = strs1[0].num + (d1.level_up * d1.level_rewared);
            list.Add(d1);

            DailyData d2 = GameShared.Instance.GetDailyData(2);
            d2.daily_type = DailyData.DailyType.Duan;
            d2.dialy_sub = DailyData.DailySub.Normal;
             
            List<GameShared.StrData> strs2 = GameShared.Instance.GetStrData(d2.rewared);
            d2.getNum = strs2[0].num + (d2.level_up * d2.level_rewared);
            list.Add(d2);

            DailyData d3 = GameShared.Instance.GetDailyData(3);
            d3.daily_type = DailyData.DailyType.Duan;
            d3.dialy_sub = DailyData.DailySub.Hard; 
            List<GameShared.StrData> strs3 = GameShared.Instance.GetStrData(d3.rewared);
            d3.getNum = strs3[0].num + (d3.level_up * d3.level_rewared);
            list.Add(d3);
             
            pop.SetDuanLian(list);
            pop.duanBtn.isEnabled = resp.ifexercise; 
            DateTime d = DateTime.Now.AddSeconds(resp.lefttime);
            pop.SetDuanTime(d);
            pop.duanlevel.text = duanLevel.ToString(); 
        }
    }
 
    public void SetDuanLian()
    {
        DuanLianList(); 
    }
    public void SetGold()
    {
        pop.goldLastTime.gameObject.SetActive(false);
        pop.goldBtn.gameObject.SetActive(false);
        GoldList(); 
    }


}
