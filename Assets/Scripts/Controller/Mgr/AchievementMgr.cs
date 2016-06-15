using UnityEngine;
using System.Collections;
using LuaInterface;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class AchievementMgr : UnitySingleton<AchievementMgr>
{  
    private List<AchievementViewData> dataList; 
    private AchievementView curCallBackView; 
    public AchievementPop pop;

    private LuaState l;
     
 

    public void OpenPop(AchievementPop p)
    {
        pop = p;
    }

    public void Start()
    {
        //InitLua();  
    }

    public void InitLua()
    { 
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Achievement); 
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void GetAchievementList()
    { 
            NetworkManager.Instance.AchievementList();
       
           //SetPopList(dataList); 
    }

    public void SetPopList(List<AchievementViewData> list)
    {
       pop.SetItemList(list); 
    }

    public void Revcie(AchievementView view)
    {
        AchievementViewData data = view.data;
        view.receiveBtn.isEnabled = false;
        NetworkManager.Instance.AchievementReceive(data.data.id);
        curCallBackView = view; 
        
    }

    public void RevcieCallBack(C2sSprotoType.achievement_reward_collect.response resp)
    {
        curCallBackView.receiveBtn.isEnabled = true; 
        if (resp.errorcode == 1)
        {
            MainUI.Instance.GetItemClick(curCallBackView.data.data.rewarData);
            for (int i = 0; i < curCallBackView.data.data.rewarData.Count; i++)
            {
                BagMgr.Instance.AddItemNumById(curCallBackView.data.data.rewarData[i].data.id,
                    curCallBackView.data.data.rewarData[i].curCount);
            }

            AchievementViewData view = new AchievementViewData(); 
            AchievementData data = GameShared.Instance.GetAchievementData((int)resp.next.csv_id);
            view.curProgress = (int)resp.next.finished;
            view.data = data;
            if (view.data.condition != 0 && view.curProgress < 100)
            {
                view.data.curStar--;
            } 
            view.sort = GetSort(view);
            view.isReceive = resp.next.reward_collected;
            view.isUnlock = resp.next.is_unlock;  
            
            Unlock(curCallBackView,view);
        }
        curCallBackView = null;
    }

    //vm
    public void AchievementListCallBack(C2sSprotoType.achievement.response resp)
    { 
        List<AchievementViewData> list = new List<AchievementViewData>(); 
        List<int> listid = new List<int>();
        Hashtable temp = new Hashtable();
        for (int i = 0; i < resp.achis.Count; i++)
        {
            AchievementViewData view = new AchievementViewData();
            AchievementData data = GameShared.Instance.GetAchievementData((int)resp.achis[i].csv_id);
            view.curProgress = (int)resp.achis[i].finished;
            view.data = data;
            if (view.data.condition != 0 && view.curProgress < 100)
            {
                view.data.curStar--;
            } 
            view.isReceive = resp.achis[i].reward_collected;
            view.isUnlock = resp.achis[i].is_unlock;

            //if (view.data.unlockId.Equals("0"))
            //{
            Debug.Log("is_unlock" + resp.achis[i].is_unlock + "reward_collected" + resp.achis[i].reward_collected + "csv_id" + resp.achis[i].csv_id + "resp.achis[i].finished" + resp.achis[i].finished);
            //} 
            if (temp.Contains(data.type))
            {//判断解锁未领取的最小id
                AchievementViewData d = temp[data.type] as AchievementViewData; 
                //如果是最后一个就插入
                if (view.isUnlock && view.isReceive == true && view.data.unlockId.Equals("0"))
                {
                    if (view.data.id > d.data.id)
                    {
                        temp[data.type] = view;
                    }
                }
                else //if(!d.data.unlockId.Equals("0"))//如果取得了最后一个就不去最小了 
                {
                    if ((view.isUnlock && view.isReceive == false && view.data.id < d.data.id))
                    {
                        temp[data.type] = view;
                    }
                }

            }
            else
            {
                if (view.isUnlock && view.isReceive == false)
                { 
                    temp.Add(data.type, view);
                    listid.Add(data.type);
                }
                if (view.isUnlock && view.isReceive == true && view.data.unlockId.Equals("0"))
                {
                    temp.Add(data.type, view);
                    listid.Add(data.type);
                }
            } 
        } 
        //筛选出解锁的
        for (int i = 0; i < listid.Count; i++)
        {
            AchievementViewData d = temp[listid[i]] as AchievementViewData;
            if (d.isUnlock)
            {
                d.sort = GetSort(d);
                list.Add(d);
            }
        }
            //LuaFunction f = l.GetFunction("AchievementListCallBack");
            //object[] obj = f.Call(list,resp); 
        dataList = list;
        SetPopList(list);
    }

    public string GetSort(AchievementViewData d)
    {
        int r = 1;
        if (!d.isReceive)
            r = 2;
        int u =1;
        if (!d.isUnlock)
            u = 2;
        return r.ToString() + u + (d.data.id*100).ToString();

    } 
    
    //vm
    public void Unlock(AchievementView v,AchievementViewData data)
    {
        //LuaFunction f = l.GetFunction("Unlock");
        //f.Call(data); 
        List<int> list = GameShared.Instance.GetStr(v.data.data.unlockId);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != 0)
            { 
                AchievementView view = pop.GetViewByTyep(data.data.type); 
                view.RestData(data);
            }
        } 
    }


}
