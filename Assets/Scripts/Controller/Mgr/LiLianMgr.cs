using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LiLianMgr : UnitySingleton<LiLianMgr>
{
    LiLianPop pop;

    LiLianHanPop hanPop;

    LiLianLevelData data;
     
    private List<LiLianRunData> runList;

    private LiLianRunData runEvent;

    public LiLianRunData curRun;

    public LiLianRunData curSwared;

    public List<ItemViewData> swardList;

    PassiveTimer tiliTimer;

    DateTime t;
    DateTime tt;
    PassiveTimer timer;

    private YesOrNoPop yesPop;

    public void OpenPop(LiLianPop p)
    {
        pop = p;
        LiLianInfo(); 
    }

    public void OpenHanPop(LiLianHanPop p)
    {
        hanPop = p;
        LiLianCardInfo();
    } 

    public void OpenCard()
    {
        MainUI.Instance.LilianHanPop();
    }

    public void LiLianCardInfo()
    {
        LiLianCardInfoCallback();
    }
     
    public void LiLianCardInfoCallback()
    {
        List<ItemViewData> list = new List<ItemViewData>();
        foreach (KeyValuePair<int, ItemViewData> pair in BagMgr.Instance.itemTable)
        {
            if (pair.Value.data.subType == (int)Def.ItemUseType.Invitation)
            { 
                list.Add(pair.Value);
            }
        }
        if (hanPop.data != null)
        {
            hanPop.SetLiLianList(list);
        }
        else
        {
            hanPop.SetLiLianList(list,false);
        }
    }

    public void LiLianInfo()
    {
        NetworkManager.Instance.LiLianInfo(); 
    }
   
    public void ChangeStrength(long time,long cd = 0)
    {
        if (pop != null)
        {
            //体力条
            float a = (float)UserManager.Instance.strength / data.phy_pwoer;
            pop.strengthBar.value = a;
            pop.strength.text = UserManager.Instance.strength.ToString();
            pop.strengthBarTxt.text = UserManager.Instance.strength + "/" + data.phy_pwoer;
            //体力倒计时
            
            long at = 0;
            if (present_phy_power_num < data.phy_pwoer)
            {
                long n = data.phy_pwoer - present_phy_power_num;
                at = ((n - 1) * GameShared.Instance.config.strength_time) + time;
                t = DateTime.Now.AddSeconds((int)time);
                //总的倒计时
                tt = DateTime.Now.AddSeconds(at);
            }
            else
            {

            }  
            if(time!=0)
                tiliTimer = new PassiveTimer(time);  

        }
        if (cd > 0 && UserManager.Instance.strength < data.phy_pwoer)
        {
            tiliTimer = new PassiveTimer(cd);
        }
        //else if (UserManager.Instance.strength >= data.phy_pwoer)
        //{
        //    tiliTimer = null;
        //}
    }
    int purch_phy_power_num = 0;
    int present_phy_power_num = 0;


    public LiLianRunData InitLiLianRunData()
    {
        LiLianRunData d = new LiLianRunData();
        d.quanguan_id = 0;
        d.card_id = 0;
        d.triggr_id = 0; 
        d.type = Def.LiLianType.Lilian;
        d.state = 0;  
        return d;
    }

    public void LiLianInfoCallback(C2sSprotoType.get_lilian_info.response resp)
    {
        Debug.Log("get_lilian_info"+"phy_power:" + resp.phy_power + "level:" + resp.level + "lilian_exp:" + resp.lilian_exp
            + "resp.lilian_exp:" + resp.lilian_exp + "purch_phy_power_num:" + resp.purch_phy_power_num + "present_phy_power_num:" + resp.present_phy_power_num);
        present_phy_power_num = (int)resp.present_phy_power_num;
        //信息
        UserManager.Instance.strength = (int)resp.phy_power;
        UserManager.Instance.lilian_level = (int)resp.level;
        UserManager.Instance.lilian_exp = (int)resp.lilian_exp;
        purch_phy_power_num = (int)resp.purch_phy_power_num;
        pop.NumTxt.text = purch_phy_power_num + "/" + UserManager.Instance.curVipdata.purchase_hp_count;
        pop.totalTimeTxt.text = "";
        pop.timeTxt.text = "";
        data = GameShared.Instance.GetLiLianLevelById(UserManager.Instance.lilian_level);

        //正在历练de 
        runList = new List<LiLianRunData>();
        for (int r = 0; r < data.queue; r++)
        {
            runList.Add(InitLiLianRunData());
        } 
        runEvent = InitLiLianRunData();
        runEvent.view = pop.eventView.time;
        pop.eventView.RestView(runEvent);

        

        SetExpPrograss();
        ChangeStrength(resp.phy_power_left_cd_time);
        pop.name.text = "";  
         

        data = GameShared.Instance.GetLiLianLevelById(UserManager.Instance.lilian_level); 
        pop.levelBar.value = (float)UserManager.Instance.lilian_exp / data.experience;
        pop.level.text = UserManager.Instance.lilian_level.ToString();
        pop.levelBarTxt.text = UserManager.Instance.lilian_exp + "/" + data.experience;



        //拳官
        List<LiLianViewData> hallList = new List<LiLianViewData>();
        foreach (KeyValuePair<int, LiLianViewData> pair in UserManager.Instance.hallTable)
        {
            pair.Value.unlock = false;
            if (UserManager.Instance.lilian_level >= pair.Value.data.open_level)
            {
                pair.Value.unlock = true;
            }
            hallList.Add(pair.Value);
        }

        if (resp.lilian_num_list != null)
        {
            for (int r = 0; r < resp.lilian_num_list.Count; r++)
            {
                Debug.Log("get_lilian_info--lilian_num_list:[" + r + "]: quanguan_id" + resp.lilian_num_list[r].quanguan_id +
                    "num ; " + resp.lilian_num_list[r].num + "reset_num" + resp.lilian_num_list[r].reset_num); 
                LiLianViewData d = new LiLianViewData();
                d.data = GameShared.Instance.GetLiLianHallById((int)resp.lilian_num_list[r].quanguan_id);
                d.num = (int)resp.lilian_num_list[r].num;
                d.rest_num = (int)resp.lilian_num_list[r].reset_num;

                if (UserManager.Instance.hallTable.ContainsKey(d.data.csv_id))
                {
                    UserManager.Instance.hallTable[d.data.csv_id] = d;
                }
                else
                {
                    UserManager.Instance.hallTable.Add(d.data.csv_id, d);
                }
            }
        } 
       
        timer = new PassiveTimer(1);
        pop.SetLiLianList(hallList);
        pop.SetLiLianRun(runList);


        int ind = 0;
        swardList = new List<ItemViewData>();
        if (resp.basic_info != null)
        {
            for (int i = 0; i < resp.basic_info.Count; i++)
            { 
                

                LiLianRunData d = new LiLianRunData();
                d.quanguan_id = (int)resp.basic_info[i].quanguan_id;
                d.card_id = (int)resp.basic_info[i].invitation_id;
                d.triggr_id = (int)resp.basic_info[i].eventid;
                d.hall = GameShared.Instance.GetLiLianHallById((int)resp.basic_info[i].quanguan_id);
                d.time = DateTime.Now.AddSeconds(resp.basic_info[i].left_cd_time);
                 
                d.if_event_reward = (int)resp.basic_info[i].if_event_reward;  
                d.if_lilian_reward = (int)resp.basic_info[i].if_lilian_reward; 
                d.type = (Def.LiLianType)resp.basic_info[i].delay_type;
                d.swaredList = new List<ItemViewData>();

                string debugss = "";
                if (resp.basic_info[i].reward != null)
                {
                    for (int r = 0; r < resp.basic_info[i].reward.Count; r++)
                    {
                        ItemViewData v = new ItemViewData();
                        v.data = GameShared.Instance.GetItemData((int)resp.basic_info[i].reward[r].propid);
                        debugss += resp.basic_info[i].reward[r].propid.ToString()+"-";
                        v.curCount = (int)resp.basic_info[i].reward[r].propnum;
                        d.swaredList.Add(v);
                    }
                }

                Debug.Log("get_lilian_info--basic_info:[" + i + "]: quanguan_id：" + resp.basic_info[i].quanguan_id +
                    "left_cd_time" + resp.basic_info[i].left_cd_time + "delay_type " + resp.basic_info[i].delay_type + "if_lilian_reward"
    + resp.basic_info[i].if_lilian_reward + "if_event_reward" + resp.basic_info[i].if_event_reward + "if_trigger_event" + resp.basic_info[i].if_trigger_event +
    "invitation_id" + resp.basic_info[i].invitation_id + "reward:" + debugss);

                if (d.if_lilian_reward == 1)
                { 
                    d.card = GameShared.Instance.GetLiLianCardById((int)resp.basic_info[i].invitation_id); 
                    List<GameShared.StrData> str2 = GameShared.Instance.GetStrData(d.card.reward);
                    for (int j = 0; j < str2.Count; j++)
                    {
                        ItemViewData item = new ItemViewData();
                        item.data = GameShared.Instance.GetItemData(str2[j].id);
                        item.curCount = str2[j].num;
                        d.swaredList.Add(item);
                    } 
                }
                if (d.if_event_reward == 1)
                {
                    runEvent = d;
                    runEvent.view = pop.eventView.time;
                }
                

                if(d.type == Def.LiLianType.Lilian)
                    ind++;

                if (resp.basic_info[i].left_cd_time > 0)
                {
                    d.time = DateTime.Now.AddSeconds(resp.basic_info[i].left_cd_time); 
                    //runList.Add(d.quanguan_id, d);
                    d.runid = ind;
                    
                    AddLiLianRun(d);
                }
                else
                {   
                    d.runid = ind;
                    RemoveInitLiLianRun(d); 
                }
            } 
        }  
    } 
      
    public void LiLianOpen(LiLianViewData d)
    {
        bool flag = false;
        int runCount = 0;
        if (runList != null && runList.Count > 0)
        {
            for(int i=0;i<runList.Count;i++)
            {
                if (runList[i].hall != null && runList[i].state != 0)
                {
                    runCount++;
                    if(runList[i].hall.csv_id == d.data.csv_id)
                        flag = true;
                }
                    
            }
        }
        pop.name.text = "csv——id" + d.data.csv_id;
        if (UserManager.Instance.GetStrength() < d.data.need_phy_power)
        {
            ToastManager.Instance.Show("体力不够");
        }
        else if (data.queue <= runCount)
        {
            ToastManager.Instance.Show("队列以满");
        }
        else if (d.num >= d.data.day_finish_time)
        { 
            //todo
            RestHall(d);
        }
        else if (this.runEvent.runTime !=null)
        {
            ToastManager.Instance.Show("不能在事件进行时开始历练");
        }
        else if (flag)
        {
            ToastManager.Instance.Show("不可再历练");
        } 
        else
        {
            //LiLianSend(d);
            MainUI.Instance.LilianHanPop(d.data);
        }
    }
    /// <summary>
    /// 发送历练请求
    /// </summary>
    /// <param name="card"></param>
    public void LiLianSend(ItemViewData card)
    {
        if (hanPop!=null && hanPop.data!=null)
        {
            curRun = new LiLianRunData();
            curRun.card = GameShared.Instance.GetLiLianCardById(card.data.id);
            curRun.hall = hanPop.data;
            curRun.quanguan_id = curRun.hall.csv_id;

            C2sSprotoType.start_lilian.request obj = new C2sSprotoType.start_lilian.request();
            obj.invitation_id =  curRun.card.csv_id;
            obj.quanguan_id = curRun.hall.csv_id;  
            NetworkManager.Instance.LiLianStart(obj);
        }
        hanPop.CloseClick();
    }

    public void LiLianSend(LiLianViewData card)
    { 
            curRun = new LiLianRunData();
            curRun.card = GameShared.Instance.GetLiLianCardById(50001);
            curRun.hall = card.data;
            curRun.hallviewdata = card;
            curRun.quanguan_id = curRun.hall.csv_id;
            

            C2sSprotoType.start_lilian.request obj = new C2sSprotoType.start_lilian.request();
            obj.invitation_id = curRun.card.csv_id;
            obj.quanguan_id = curRun.hall.csv_id;
            NetworkManager.Instance.LiLianStart(obj); 
    }
    /// <summary>
    /// 客户端显示新增历练
    /// </summary>
    public void LiLianCallback(C2sSprotoType.start_lilian.response resp)
    {
        if (curRun != null && resp.errorcode == 1)
        { 
            curRun.hall.time = (int)resp.left_cd_time;
            curRun.time = DateTime.Now.AddSeconds(curRun.hall.time);
            pop.GetViewById(curRun.hall.csv_id).data.num++; 
            curRun.type = Def.LiLianType.Lilian;
            curRun.runTime = new PassiveTimer(1); 
            AddLiLianRun(curRun);
            this.SetExpPrograss();
            SubStrength(curRun.hall.need_phy_power);
            ChangeStrength(0);
            NetworkManager.Instance.GetStrength();
            //ChangeStrength(1);
            BagMgr.Instance.SubItemNumById(curRun.card.csv_id, 1);//todo 使用几个卡
        }
        curRun = null; 
    }
     
    /// <summary>
    /// 添加历练倒计时
    /// </summary>
    /// <param name="run"></param>
    public void AddLiLianRun(LiLianRunData run)
    { 
        run.runTime = new PassiveTimer(1);
        int id = 99;
        if (run.type == Def.LiLianType.Lilian)
        {
            for (int i = 0; i < runList.Count; i++)
            {
                if (runList[i].state == 0)
                {
                    id = i;
                    break;
                }
            }
            if (runList[id].state == 0)
            {
                run.runid = id;
                run.state = 1;
                runList[id] = run;
                pop.GetLiLianRunView(run.runid).RestView(run);
            }
            else
            {
                Debug.Log("重复添加历练");
            }
        }
        else if(run.type == Def.LiLianType.Event)
        { 
            runEvent = run;
            run.state = 1;
            pop.eventView.RestView(run);
        }
    }
    /// <summary>
    /// 移除历练倒计时
    /// </summary>
    /// <param name="run"></param>
    public void RemoveLiLianRun(LiLianRunData run)
    {
        if (run.type == Def.LiLianType.Lilian)
        {
            run.runTime = null;
            run.state = 0;
            pop.GetViewById(run.quanguan_id).data.run = run;
            pop.GetLiLianRunView(run.runid).RestView(run);
            pop.GetViewById(run.quanguan_id).SetState(true);
            runList[run.runid] = InitLiLianRunData();
            pop.GetLiLianRunView(run.runid).RestView(run);
        }
        else if (run.type == Def.LiLianType.Event)
        {
            run.runTime = null;
            run.state = 0;
            pop.GetViewById(run.quanguan_id).data.run = run;
            pop.GetViewById(run.quanguan_id).SetState(true);
            pop.eventView.RestView(run);
        }
    }
    
    /// <summary>
    /// 初始化的时候用没有倒计时的情况
    /// </summary>
    /// <param name="run"></param>
    public void RemoveInitLiLianRun(LiLianRunData run)
    {
        run.runTime = null;
        run.state = 0;
        pop.GetViewById(run.quanguan_id).data.run = run; 
        pop.GetViewById(run.quanguan_id).SetState(true); 
    }

    public void GetLiLianSward(LiLianViewData d)
    {
        OpenSwaredPop(d.run); 
    }

    private void GetLiLianSward(LiLianRunData d)
    {
        C2sSprotoType.lilian_get_reward_list.request obj = new C2sSprotoType.lilian_get_reward_list.request();
        obj.quanguan_id = d.quanguan_id;
        if (d.type == Def.LiLianType.EventFinish)
            d.type = Def.LiLianType.Event;
        if (d.type == Def.LiLianType.LiLianFinish)
            d.type = Def.LiLianType.Lilian;
        obj.reward_type = (int)d.type; 
        curSwared = d;
        NetworkManager.Instance.LiLianRewardList(obj);
    }

    LiLianRunData saveRun;
    public void LiLianSaveSward(LiLianRunData d)
    {
        C2sSprotoType.lilian_rewared_list.request obj = new C2sSprotoType.lilian_rewared_list.request();
        obj.quanguan_id = d.hall.csv_id;
        obj.rtype = (int)d.type; 
        saveRun = d;
        NetworkManager.Instance.LiLianSaveRewardList(obj);
    } 
    
    public void GetLiLianSaveSwardListCallback(C2sSprotoType.lilian_rewared_list.response resp)
    {
        if (resp.errorcode == 1)
        {
            saveRun.state = 0; 
            saveRun.swaredList = new List<ItemViewData>();
            if (resp.reward != null)
            {
                for (int i = 0; i < resp.reward.Count; i++)
                {
                    Debug.Log("GetLiLianSaveSwardListCallback " + "propid=" + resp.reward[i].propid + "propnum=" + resp.reward[i].propnum);
                    ItemViewData v = new ItemViewData();
                    v.data = GameShared.Instance.GetItemData((int)resp.reward[i].propid);
                    v.curCount = (int)resp.reward[i].propnum; 
                    saveRun.swaredList.Add(v); 
                } 
            }
            this.RemoveLiLianRun(saveRun);
        }
        else if (resp.errorcode == 81)
        {
            saveRun.time.AddSeconds(resp.left_cd_time); 
        }  
    }

    public void GetLiLianSwardCallback(C2sSprotoType.lilian_get_reward_list.response resp)
    {
        if (curSwared != null && resp.errorcode == 1)
        {
            Debug.Log("GetLiLianSwardCallback  resp.if_lilian_reward" + resp.if_lilian_reward + "resp.if_event_reward" + resp.if_event_reward + "resp.if_trigger_event" + resp.if_trigger_event);
            List<ItemViewData> list = new List<ItemViewData>();
            if (resp.if_lilian_reward ==1)
            { 
                UserManager.Instance.hallTable[curSwared.hall.csv_id].run = null; 
                UserManager.Instance.lilian_exp = (int)resp.lilian_exp;
                UserManager.Instance.lilian_level = (int)resp.lilian_level;
                SetExpPrograss(); 
                 
                if (curSwared.card == null)
                    curSwared.card = GameShared.Instance.GetLiLianCardById(curSwared.card_id);
                List<GameShared.StrData> str2 = GameShared.Instance.GetStrData(curSwared.card.reward);
                for (int j = 0; j < str2.Count; j++)
                { 
                    ItemViewData item = new ItemViewData();
                    item.data = GameShared.Instance.GetItemData(str2[j].id);
                    item.curCount = str2[j].num;
                    list.Add(item);
                }
                if (curSwared.swaredList != null)
                {
                    for (int x = 0; x < curSwared.swaredList.Count; x++)
                    {
                        list.Add(curSwared.swaredList[x]);
                    }
                }

                if (resp.if_trigger_event == 1)
                {
                    LiLianRunData d = new LiLianRunData();
                    d.runTime = new PassiveTimer(1);
                    d.time = DateTime.Now.AddSeconds(resp.left_cd_time);
                    d.hall = curSwared.hall; 
                    d.if_event_reward = 1;
                    d.if_lilian_reward = 1;
                    d.view = pop.eventView.time;
                    d.quanguan_id = curSwared.hall.csv_id;
                    d.type = Def.LiLianType.Event;
                    runEvent = d;
                    d.state = 1;
                    pop.SetLiLianRunEvent(d); 
                }
            } 
            if (resp.if_event_reward == 1)
            {
                //LiLianMgr.Instance.LiLianSaveSward(curSwared); 
                if (curSwared.swaredList != null)
                {
                    for (int x = 0; x < curSwared.swaredList.Count; x++)
                    {
                        
                        list.Add(curSwared.swaredList[x]);
                    }
                }
                this.RemoveLiLianRun(curSwared);
            }


            if (resp.reward != null && curSwared.swaredList != null && curSwared.swaredList.Count<1)
            {
                for (int r = 0; r < resp.reward.Count; r++)
                {
                    Debug.Log("GetLiLianSwardCallback " + "resp.reward[i].propid=" + resp.reward[r].propid + "resp.reward[i].propnum=" + resp.reward[r].propnum);
                    ItemViewData v = new ItemViewData();
                    v.data = GameShared.Instance.GetItemData((int)resp.reward[r].propid);
                    v.curCount = (int)resp.reward[r].propnum;
                    list.Add(v);
                }
            } 
            MainUI.Instance.GetItemClick(list); 
            curSwared.state = 2; 
            pop.GetViewById(curSwared.quanguan_id).SetState(false);
            CheckRunList(false);
        }
        //else if (resp.errorcode == 81)
        //{
        //    curSwared.time.AddSeconds(resp.left_cd_time);
        //} 
    }

    public void OpenSwaredPop(LiLianRunData d)
    { 
        if (MainUI.Instance.GetPopState(MainUI.PopType.YesOrNo) != true)
        {
            MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, true);
            GameObject obj = Instantiate(MainUI.Instance.yesOrNoPop);
            obj.SetActive(true);
            yesPop = obj.GetComponent<YesOrNoPop>();
            yesPop.InitData(d, "奖励");
            yesPop.YesCallBackEvent += SwaredYes;
            yesPop.transform.parent = pop.transform.parent;
            yesPop.transform.position = Vector3.zero;
            yesPop.transform.localScale = Vector3.one;
        }
    }

    public void SwaredYes(object o)
    {  
        LiLianRunData d = o as LiLianRunData;
        GetLiLianSward(d);
    } 
     
    public void OpenBuyStrength()
    {
        if (purch_phy_power_num >= UserManager.Instance.curVipdata.purchase_hp_count)
        {
            ToastManager.Instance.Show("购买次数已满");
        }
        //else if (UserManager.Instance.strength >= data.phy_pwoer)
        //{
        //    ToastManager.Instance.Show("体力以满");
        //} 
        else if (UserManager.Instance.diamond < GameShared.Instance.GetLiLianStrengthByCount(purch_phy_power_num+1).diamond)
        {
            ToastManager.Instance.Show("钻石不够");
        } 
        else if (MainUI.Instance.GetPopState(MainUI.PopType.YesOrNo) != true)
        {
            MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, true);
            GameObject obj = Instantiate(MainUI.Instance.yesOrNoPop);
            obj.SetActive(true);
            yesPop = obj.GetComponent<YesOrNoPop>();
            yesPop.InitData(null, "是用" + GameShared.Instance.config.lilian_buy_strength_damiand + "钻石购买"
                + GameShared.Instance.config.lilian_buy_strength + "体力");
            yesPop.YesCallBackEvent += BuyYes;
            yesPop.transform.parent = pop.transform.parent;
            yesPop.transform.position = Vector3.zero;
            yesPop.transform.localScale = Vector3.one;
        }
    }

    public void BuyYes(object o)
    {
        if (UserManager.Instance.diamond < GameShared.Instance.config.lilian_buy_strength_damiand)
        {
            ToastManager.Instance.Show("钻石不够");
        }
        else
        {
            NetworkManager.Instance.BuyStrength();
           
        }
    } 

    public void SetExpPrograss()
    {
        data = GameShared.Instance.GetLiLianLevelById(UserManager.Instance.lilian_level); 
        pop.levelBar.value = (float)UserManager.Instance.lilian_exp / data.experience;
        pop.level.text = UserManager.Instance.lilian_level.ToString();
        pop.levelBarTxt.text = UserManager.Instance.lilian_exp + "/" + data.experience;
        int c = data.queue - runList.Count;
        if (c > 0)
        {
            for (int r = 0; r < c; r++)
            {
                LiLianRunData a = InitLiLianRunData();
                runList.Add(a);
                pop.AddLiLianRunView(a); 
            }
        }  
    } 

    public void AddExp(int a)
    {
        UserManager.Instance.lilian_exp += a; 
        SetExpPrograss(); 
    }
     
    public void AddStrength(int a)
    {
        UserManager.Instance.strength += a;
        if (UserManager.Instance.strength > data.phy_pwoer)
        {
            UserManager.Instance.strength = data.phy_pwoer;
        }
        if(pop!=null)
        {
            pop.strengthBar.value = (float)UserManager.Instance.strength / data.phy_pwoer; ; 
            pop.strength.text = UserManager.Instance.strength.ToString(); 
            pop.strengthBarTxt.text = UserManager.Instance.strength + "/" + data.phy_pwoer; 
        }
    }

    public bool SubStrength(int a)
    {
        bool flag = false;
        if (UserManager.Instance.strength > a)
        {
            UserManager.Instance.strength -= a;
            flag = true;
        }

        if (present_phy_power_num >= a)
        {
            present_phy_power_num -= a;
        }
        else
        {
            present_phy_power_num = 0;
        }


        if (pop != null)
        {
            pop.strengthBar.value = (float)UserManager.Instance.strength / data.phy_pwoer; ;
            pop.strength.text = UserManager.Instance.strength.ToString();
            pop.strengthBarTxt.text = UserManager.Instance.strength + "/" + data.phy_pwoer;
        }
        return flag;
    }

    public void BuyCallback(C2sSprotoType.lilian_purch_phy_power.response resp)
    {
        if (resp.errorcode == 1)
        { 
            ToastManager.Instance.Show("购买成功");
            purch_phy_power_num++; 
            UserManager.Instance.SubDiamond(GameShared.Instance.GetLiLianStrengthByCount(purch_phy_power_num).diamond);
            UserManager.Instance.strength = (int)resp.phy_power;
            ChangeStrength(resp.left_cd_time);  
            
            pop.NumTxt.text = purch_phy_power_num + "/" + UserManager.Instance.curVipdata.purchase_hp_count;
        }
        yesPop = null;
    }

    public void ClosePop()
    {
        runList.Clear();
        runList = null;
    }

    public void StrengthCallback(C2sSprotoType.lilian_get_phy_power.response resp)
    {
        Debug.Log("resp.left_cd_time" + resp.left_cd_time + "resp.phy_power" + resp.phy_power);
        if (resp.errorcode == 1)
        {
            UserManager.Instance.strength = (int)resp.phy_power;
            ChangeStrength(resp.left_cd_time); 
            t = DateTime.Now.AddSeconds(resp.left_cd_time);
        }
        else if (resp.errorcode == 81 || resp.errorcode == 85)
        {
            tiliTimer = new PassiveTimer(resp.left_cd_time);
            ChangeStrength(resp.left_cd_time); 
            t = DateTime.Now.AddSeconds(resp.left_cd_time);
        }
    }

    LiLianRunData quickRun;

    public void QuickLiLianYes(object o)
    {
        LiLianRunData d = o as LiLianRunData;
        C2sSprotoType.lilian_inc.request obj = new C2sSprotoType.lilian_inc.request();
        obj.quanguan_id = d.hall.csv_id;
        obj.inc_type = (long)d.type;
        quickRun = d;
        NetworkManager.Instance.QuickLiLian(obj);
    } 

    public void QuickLiLian(LiLianRunData d)
    {
        if (UserManager.Instance.diamond < GameShared.Instance.config.diamond_per_sec * Comm.GetSeconds(DateTime.Now, d.time))
        {
            ToastManager.Instance.Show("钻石不够");
        }
        else
        {
            MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, true);
            GameObject obj = Instantiate(MainUI.Instance.yesOrNoPop);
            obj.SetActive(true);
            yesPop = obj.GetComponent<YesOrNoPop>();
            yesPop.InitData(d, "是用" + GameShared.Instance.config.diamond_per_sec * Comm.GetSeconds(DateTime.Now, d.time) + "钻石购买"
                );
            yesPop.YesCallBackEvent += QuickLiLianYes;
            yesPop.transform.parent = pop.transform.parent;
            yesPop.transform.position = Vector3.zero;
            yesPop.transform.localScale = Vector3.one;
        }
    } 

    public void QuickLiLianCallback(C2sSprotoType.lilian_inc.response resp)
    { 
        if (resp.errorcode == 1)
        {
            quickRun.state = 2; 
            UserManager.Instance.SetDiamond((int)resp.diamond_num);
            this.LiLianSaveSward(quickRun);
            this.RemoveLiLianRun(quickRun); 
        }
        else if (resp.errorcode == 91)
        {

        }
    }

    public void RestHallYes(object o)
    {
        LiLianViewData view = o as LiLianViewData;
        C2sSprotoType.lilian_reset_quanguan.request obj = new C2sSprotoType.lilian_reset_quanguan.request();
        obj.quanguan_id = view.data.csv_id;
        restHall = view;
        NetworkManager.Instance.RestHall(obj);
    } 

    LiLianViewData restHall; 
    public void RestHall(LiLianViewData view)
    {
        if (view.rest_num >= UserManager.Instance.curVipdata.SCHOOL_reset_count)
        {
            ToastManager.Instance.Show("达到重置最大次数");
           
        }
        else if (UserManager.Instance.diamond < GameShared.Instance.GetLiLianStrengthByCount(view.num).diamond)
        {
            ToastManager.Instance.Show("钻石不够");
        }
        else
        {
            MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, true);
            GameObject obj = Instantiate(MainUI.Instance.yesOrNoPop);
            obj.SetActive(true);
            yesPop = obj.GetComponent<YesOrNoPop>();
            yesPop.InitData(view, "是用" + GameShared.Instance.GetLiLianStrengthByCount(view.num).diamond + "钻石购买"
                );
            yesPop.YesCallBackEvent += RestHallYes;
            yesPop.transform.parent = pop.transform.parent;
            yesPop.transform.position = Vector3.zero;
            yesPop.transform.localScale = Vector3.one;
        }
    }
    public void RestHallCallback(C2sSprotoType.lilian_reset_quanguan.response resp)
    { 
        if (resp.errorcode == 1)
        {
            UserManager.Instance.SubDiamond(GameShared.Instance.GetLiLianStrengthByCount(restHall.num).diamond);
            restHall.rest_num++;
            restHall.num = 0;
            restHall.view.RestView();  
        }
        restHall = null;
    }

    public void HallUpdate()
    {
        foreach (KeyValuePair<int, LiLianViewData> pair in UserManager.Instance.hallTable)
        {
            pair.Value.num = 0; 
        }  
    }
    
	void FixedUpdate() 
    {
        if (tiliTimer != null && tiliTimer.Update(Time.deltaTime) > 0)
        {
            //todo 时间太长 容易有误差 
            if (present_phy_power_num<data.phy_pwoer)
                present_phy_power_num++;
            NetworkManager.Instance.GetStrength();
        }

        if (timer != null && timer.Update(Time.deltaTime) > 0)
        {
            string strt = Comm.DateDiffHour(DateTime.Now, t);  
            string strtt = Comm.DateDiffHour(DateTime.Now, tt);
            if(!strtt.Equals(""))
                pop.timeTxt.text = strt;
            //if(!strtt.Equals(""))
                pop.totalTimeTxt.text = strtt;
        }
        if (runEvent!=null && runEvent.runTime != null && runEvent.runTime.Update(Time.deltaTime) > 0)
        {
            string str = Comm.DateDiffHour(DateTime.Now, runEvent.time);
            if (str.Equals(""))
            {
                //历练结束
                runEvent.view.text = "";
                LiLianSaveSward(runEvent);
            }
            else
            {
                if (runEvent.view != null)
                    runEvent.view.text = str;
            } 
        }
        if (runList != null && runList.Count>0)
        {
            for (int i = 0; i < runList.Count; i++)
            {
                if (runList[i].state != 0 && runList[i].runTime != null && runList[i].runTime.Update(Time.deltaTime) > 0)
                {
                    string str = Comm.DateDiffHour(DateTime.Now, runList[i].time);
                    if (str.Equals(""))
                    {
                        //历练结束
                        runList[i].view.text = "";
                        LiLianSaveSward(runList[i]);   
                    }
                    else
                    {
                        if (runList[i].view != null)
                            runList[i].view.text = str;
                    } 
                }
            }
        }
	}

    public void CheckRunList(bool b = true)
    {
        for (int i = 0; i < runList.Count; i++)
        { 
            if (runList[i].state == 2)
            {
                if (b)
                {
                    LiLianMgr.Instance.LiLianSaveSward(runList[i]);
                    RemoveLiLianRun(runList[i]);
                }
                else 
                {
                    pop.GetViewById(runList[i].quanguan_id).SetState(false); 
                }
            } 
        }
    } 
}
