using UnityEngine;
using System.Collections;
using LuaInterface;
using Assets.Scripts.Common;
using System.Collections.Generic;
using C2sSprotoType;
using System;

public class FriendMgr : UnitySingleton<FriendMgr>
{
    public const int MaxPoint = 100;
    public const int GetPoint = 10;

    private Hashtable viewList;
    private List<FriendData> dataList;
    private FriendView curCallBackView; 
    public FriendPop pop;
    public int point;
    private LuaState l;


    public void OpenPop(FriendPop p)
    {
        pop = p;
        point = 0;
    }

    public void Start()
    {
        //InitLua();  
    }

    public void InitLua()
    { 
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Friend); 
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void GetFriendList()
    { 
            NetworkManager.Instance.FriendList();
        
    }

    public void GetFriendApplyList()
    { 
            NetworkManager.Instance.FriendApplyList();
       
    }

    public void GetFriendAddList()
    {
        
            NetworkManager.Instance.FriendAddList();
       
    }
    public void FriendApplyCallBack(C2sSprotoType.applied_list.response resp)
    {
        if (pop != null)
        {
            List<FriendData> list = RespToList(resp.friendlist);
            SetPopList(list);
        }
    }

    public void FriendAddCallBack(C2sSprotoType.otherfriend_list.response resp)
    {
        if (pop != null)
        {
            List<FriendData> list = RespToList(resp.friendlist);
            SetPopList(list);
        }
    }
        

    public void FriendListCallBack(C2sSprotoType.friend_list.response resp)
    {
        if (pop != null)
        {
            List<FriendData> list = RespToList(resp.friendlist);
            point = (int)resp.today_left_heart;
            Debug.Log("友情点还可" + point);
            pop.SetPoint(point);
            SetPopList(list);
        }
    }

    public List<FriendData> RespToList(List<subuser> friendlist)
    {
        List<FriendData> list = new List<FriendData>();
        
        if (friendlist != null)
        {
            for (int i = 0; i < friendlist.Count; i++)
            { 
                FriendData data = new FriendData();
                Debug.Log("(int)friendlist[i].id"+(int)friendlist[i].id+"data.heartamount" + friendlist[i].heartamount + "friendlist[i].heart" + friendlist[i].heart);
                data.id = (int)friendlist[i].id;
                data.name = friendlist[i].name;
                data.time = friendlist[i].online_time;
                data.qian = friendlist[i].sign;
                data.level = (int)friendlist[i].level;
                data.power = (int)friendlist[i].fightpower; 
                data.vip = (int)friendlist[i].viplevel;
                data.icon = friendlist[i].iconid.ToString();
                data.isApply = friendlist[i].apply;
                data.isReceive = friendlist[i].receive;  
                data.isHeart = friendlist[i].heart;
                data.heartamount = (int)friendlist[i].heartamount;
                data.signtime = (int)friendlist[i].signtime;
                list.Add(data); 
            }
        }
        return list;
    }

    
    public void SetPopList(List<FriendData> list)
    {
        pop.SetList(list);
    }

    
    

    public void ApplyAll()
    {
        viewList = pop.viewList;
        if (viewList.Count > 0 )
        {

            C2sSprotoType.applyfriend.request obj = new C2sSprotoType.applyfriend.request();
            List<C2sSprotoType.friendidlist> l = new List<C2sSprotoType.friendidlist>(); 
            obj.friendlist = l;
            for (int i = 0; i < viewList.Count; i++)
            { 
                FriendView v = viewList[i] as FriendView;
                if (v.data.isApply)
                {
                    C2sSprotoType.friendidlist f = new C2sSprotoType.friendidlist();
                    f.friendid = v.data.id;
                    f.type = 0;
                    f.signtime = v.data.signtime;
                    l.Add(f);
                }
            }
            NetworkManager.Instance.FriendApply(obj);
            for (int i = 0; i < viewList.Count; i++)
            {
                FriendView v = pop.GetView(i);
                if (v != null)
                {
                    if (v.data.isApply != false)
                    {
                        v.data.isApply = false;
                        v.CheckBtn();
                    }
                } 
            }
        }
    }

    public void Apply(FriendView v)
    { 
            C2sSprotoType.applyfriend.request obj = new C2sSprotoType.applyfriend.request();
            List<C2sSprotoType.friendidlist> l = new List<C2sSprotoType.friendidlist>();
            obj.friendlist = l; 
                C2sSprotoType.friendidlist f = new C2sSprotoType.friendidlist();
                f.friendid = v.data.id;
                f.type = 0;
                f.signtime = v.data.signtime;
                l.Add(f); 
            NetworkManager.Instance.FriendApply(obj); 
 
            v.data.isApply = false;
            v.CheckBtn(); 
    }

    public void Swap()
    {  
            NetworkManager.Instance.FriendAddList(); 
    }

   

   


    public List<C2sSprotoType.friendidlist> GetFriendidList(Hashtable list)
    {
        List<C2sSprotoType.friendidlist> l = new List<C2sSprotoType.friendidlist>();
        System.Collections.IDictionaryEnumerator enumerator = list.GetEnumerator(); 
        while (enumerator.MoveNext())
        { 
            C2sSprotoType.friendidlist f = new C2sSprotoType.friendidlist();
            FriendView v = list[enumerator.Key] as FriendView;
            f.friendid = v.data.id;
            f.type = 0;
            f.signtime = v.data.signtime;
            l.Add(f);
        }
        return l;
    }


    public List<C2sSprotoType.friendidlist> GetFriendidList(FriendView v)
    {
        List<C2sSprotoType.friendidlist> l = new List<C2sSprotoType.friendidlist>();
         
            C2sSprotoType.friendidlist f = new C2sSprotoType.friendidlist(); 
            f.friendid = v.data.id;
            f.type = 0;
            f.signtime = v.data.signtime;
            l.Add(f); 
        return l;
    }

    public void Accept(FriendView v)
    { 
            C2sSprotoType.recvfriend.request obj = new C2sSprotoType.recvfriend.request();
            obj.friendlist = GetFriendidList(v);
            NetworkManager.Instance.FriendAccept(obj);

            pop.SetLive(1);
            pop.RemoveFriendView(v); 
    }
    public void AcceptAll()
    { 
        viewList = pop.viewList;
        if (viewList.Count > 0 )
        {
            C2sSprotoType.recvfriend.request obj = new C2sSprotoType.recvfriend.request(); 
            obj.friendlist =  GetFriendidList(viewList);
            NetworkManager.Instance.FriendAccept(obj); 

            pop.RemoveAllView();
            pop.SetLive(1); 
            viewList.Clear();
        }
    }

    public void RefuseAll()
    {
        viewList = pop.viewList;
        if (viewList.Count > 0 )
        {

            C2sSprotoType.refusefriend.request obj = new C2sSprotoType.refusefriend.request();
            obj.friendlist = GetFriendidList(viewList);
            NetworkManager.Instance.FriendRefuse(obj); 

            pop.RemoveAllView();
            viewList.Clear();
        }
    }
    public void Refuse(FriendView v)
    { 
            C2sSprotoType.refusefriend.request obj = new C2sSprotoType.refusefriend.request();
            obj.friendlist = GetFriendidList(v);
            NetworkManager.Instance.FriendRefuse(obj);  

            pop.RemoveFriendView(v); 
    }

    public void SendAll()
    {
        viewList = pop.viewList;
        if (viewList.Count > 0 )
        { 
                C2sSprotoType.sendheart.request obj = new C2sSprotoType.sendheart.request();
                List<C2sSprotoType.heartlist> l = new List<C2sSprotoType.heartlist>();
                DateTime dt = DateTime.Now;
                string t = string.Format("{0:yyyyMMddHHmmss}", dt);
                int total = 0;
                for (int i = 0; i < viewList.Count; i++)
                {
                   
                    FriendView v = viewList[i] as FriendView;
                    if (v.data.isHeart)
                    {
                        C2sSprotoType.heartlist f = new C2sSprotoType.heartlist();
                        f.friendid = v.data.id;
                        f.type = 0;
                        f.signtime = v.data.signtime;
                        f.amount = Def.RecHeart; 
                        f.csendtime = t;
                        l.Add(f);
                        total += Def.RecHeart;
                    }
                } 
                obj.hl = l;
                obj.totalamount = total;
                NetworkManager.Instance.SendHeart(obj); 
                for (int i = 0; i < viewList.Count; i++)
                {
                    FriendView v = pop.GetView(i);
                    if (v != null)
                    {
                        if (v.data.isHeart != false)
                        {
                            v.data.isHeart = false;
                            v.CheckBtn();
                        }
                    }
                } 
        }
    }

    public void Send(FriendView v)
    { 
            C2sSprotoType.sendheart.request obj = new C2sSprotoType.sendheart.request();
            List<C2sSprotoType.heartlist> l = new List<C2sSprotoType.heartlist>();
            DateTime dt = DateTime.Now;
            string t = string.Format("{0:yyyyMMddHHmmss}", dt); 
                C2sSprotoType.heartlist f = new C2sSprotoType.heartlist(); 
                f.friendid = v.data.id;
                f.type = 0;
                f.signtime = v.data.signtime;
                f.amount = Def.RecHeart;
                f.csendtime = t;
                l.Add(f); 
            obj.hl = l;
            obj.totalamount = Def.RecHeart;
            NetworkManager.Instance.SendHeart(obj);

            v.data.isHeart = false;
            v.CheckBtn();  
    }

    public void ReceiveAll()
    {
        viewList = pop.viewList;
        int temp = 0;
        if (viewList.Count > 0 )
        {
            C2sSprotoType.recvheart.request obj = new C2sSprotoType.recvheart.request();
            List<C2sSprotoType.heartlist> l = new List<C2sSprotoType.heartlist>();
            DateTime dt = DateTime.Now;
            string t = string.Format("{0:yyyyMMddHHmmss}", dt); 
            for (int i = 0; i < viewList.Count; i++)
            {  
                FriendView v = viewList[i] as FriendView;
                temp += v.data.heartamount;
                if (v.data.heartamount>0 && temp < point)
                {
                    C2sSprotoType.heartlist f = new C2sSprotoType.heartlist();
                    f.friendid = v.data.id;
                    f.type = 0;
                    f.signtime = v.data.signtime;
                    f.amount = Def.RecHeart; 
                    f.csendtime = t;
                    l.Add(f);
                }
            }
            obj.hl = l;
            obj.totalamount = temp;
           
            if(point > temp)
            {
                UserManager.Instance.AddFriendPoint(temp);
                NetworkManager.Instance.ReceviceHeart(obj);
                point = point - temp;
                pop.SetPoint(point);
                for (int i = 0; i < viewList.Count; i++)
                {
                    FriendView v = pop.GetView(i);
                    if (v != null)
                    {
                        if (v.data.isReceive != false)
                        {
                            v.data.isReceive = false;
                            v.data.heartamount = 0;
                            v.CheckBtn(); 
                        }
                    }
                } 
            }else
            { 
                ToastManager.Instance.Show("达到领取上线");
            }
        }
    }
    public void Receive(FriendView v)
    {
        viewList = pop.viewList;
        if (point>v.data.heartamount && v.data.heartamount >0)
        {
            C2sSprotoType.recvheart.request obj = new C2sSprotoType.recvheart.request();
            List<C2sSprotoType.heartlist> l = new List<C2sSprotoType.heartlist>();
            DateTime dt = DateTime.Now;
            string t = string.Format("{0:yyyyMMddHHmmss}", dt);  
            C2sSprotoType.heartlist f = new C2sSprotoType.heartlist();
            f.friendid = v.data.id;
            f.type = 0;
            f.signtime = v.data.signtime;
            f.amount = v.data.heartamount;
            f.csendtime = t;
            l.Add(f);  
            obj.hl = l;
            obj.totalamount = v.data.heartamount;
            NetworkManager.Instance.ReceviceHeart(obj);

            point = point - v.data.heartamount;
            pop.SetPoint(point);
            UserManager.Instance.AddFriendPoint(v.data.heartamount);
            v.data.isReceive = false;
            v.data.heartamount = 0;
            v.CheckBtn(); 
        }
    }


    public void OpenFriendDel(FriendView v)
    { 
        if (MainUI.Instance.GetPopState(MainUI.PopType.FriendDel) != true)
        { 
            MainUI.Instance.SetPopState(MainUI.PopType.FriendDel, true);
            GameObject obj = Instantiate(pop.popPrefab);
            obj.SetActive(true);
            DelFriendPop p = obj.GetComponent<DelFriendPop>();
            p.InitData(v);
            p.transform.parent = pop.transform.parent;
            p.transform.position = Vector3.zero;
            p.transform.localScale = Vector3.one;
        }
    }

    private YesOrNoPop yesPop;
    
    public void OpenDelYesOrNoPop(DelFriendPop v)
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.YesOrNo) != true)
        {
            MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, true);
            GameObject obj = Instantiate(MainUI.Instance.yesOrNoPop);
            obj.SetActive(true);
            yesPop = obj.GetComponent<YesOrNoPop>();
            yesPop.InitData(v);
            yesPop.YesCallBackEvent += DelYes;
            yesPop.transform.parent = pop.transform.parent;
            yesPop.transform.position = Vector3.zero;
            yesPop.transform.localScale = Vector3.one;
        }
    }

    public void DelYes(object o)
    {
        DelFriendPop v = o as DelFriendPop;
        FriendDel(v);
    }

    private DelFriendPop curDelView;
    public void FriendDel(DelFriendPop v)
    { 
            curDelView = v;
            NetworkManager.Instance.FriendDelete(v.view.data.id); 
       
    }

    public void FriendDelCallBack(C2sSprotoType.deletefriend.response resp)
    {
        pop.RestDelFriend(curDelView.view);
        //yesPop.CloseClick();
        curDelView.CloseClick();
        curDelView = null;
        yesPop = null;
    }

    public void Search(string str)
    { 
            NetworkManager.Instance.FriendFind(int.Parse(str));
     
    }

    public void FriendFindCallBack(C2sSprotoType.findfriend.response resp)
    {
        if (pop != null)
        {
            if (resp.friend == null || resp.errorcode == Def.Er_NotFindFrind)
            {
                ToastManager.Instance.Show("未找到");
            }
            else
            {
                List<FriendData> list = RespToList(resp.friend);
                SetPopList(list);
            }
        }
    } 
     
}
