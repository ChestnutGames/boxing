using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using CodeStage.AntiCheat.ObscuredTypes;
/// <summary>
/// 存放 从服务器获得的的数据 和会修改的数据
/// </summary>
public class UserManager : UnitySingleton<UserManager> 
{   
    public delegate void ChangeInfoFun();
    public event ChangeInfoFun ChangeInfoEvent;

    public delegate void ChangeStrengthFun();
    public event ChangeStrengthFun StrengthEvent;

    //角色 
    public RoleData curRole;

    public Hashtable RoleTable; 
    public Hashtable boxTable;
    public Hashtable equipTable;
    public Dictionary<int,LiLianViewData> hallTable; 

    //网络
    public long uid;
    public string nickName;
    public string signTxt;


    public ObscuredInt level;
    public ObscuredInt vip_level;


    public ObscuredInt lilian_level;
    public ObscuredLong lilian_exp;
    public ObscuredInt strength; 
     

    public ObscuredLong coin;
    public ObscuredLong exp;
    public ObscuredLong diamond;
    public ObscuredLong friend_point;
      
    public int curDay;
    public int curMonth;

    public bool isMonthCard;
    public bool isVip;
    public int recharge_total;
    public int recharge_progress;
    public int hanging_checkpoint;

    public int battleRoleID;

    public string username;
    public string pwd;

    public bool canModifyName;

    //本地
    public bool onlineState;

    public UserAttrs userAttr; 
    public AttrsBase foreverAttr;
    public UserLevelData constAttr; 

    public VipData curVipdata;
    public LevelData curLeveldata;
    public int curUnLockChapter; 

    public float GetPower()
    {
        return userAttr.attrArr[(int)Def.AttrId.FightPower];
    }

    public void SetVipData(int level)
    {
        vip_level = level;
        curVipdata = GameShared.Instance.GetVipByLevel((int)vip_level);
    }

    public float GetUserAttrByType(Def.AttrId id)
    {
        return userAttr.attrArr[(int)id];
    }

    public EquipViewData GetEquipById(int id)
    {
        return equipTable[id] as EquipViewData;
    }

    public BoxingViewData GetBoxingById(int id)
    {
        return boxTable[id] as BoxingViewData;
    }

    public RoleData GetRoleById(int id)
    {
        return RoleTable[id] as RoleData;
    }

    public RoleData GetCurRole()
    {
        return RoleTable[battleRoleID] as RoleData;
    }

    public int GetCurVipLevelRecharge()
    {  
        int cur = GameShared.Instance.GetVipByLevel((int)vip_level).diamond_count; 
        return cur;
    }

    public void UpLevel()
    {
        level++;
        userAttr.RestUserAttr((int)level);
    }
	// Use this for initialization
	void Start () {    
        NetworkManager.Instance.UserCallBackEvent += Instance_UserCallBackEvent;  
	}

    public long honor_coin;
    //资源是否达到要求
    public bool ResByType(int t, int num)
    {
        bool flag = false;
        switch (t)
        {
            case 1:
                if (this.diamond >= num)
                    flag = true;
                break;
            case 2:
                if (coin >= num)
                    flag = true;
                break;
            case 3:
                if (friend_point >= num)
                    flag = true;
                break;
            case 6:
                if (honor_coin >= num)
                    flag = true;
                break;
        }
        return flag;
    }

    //资源是否达到要求
    public void SubResByType(int t, int num)
    { 
        switch (t)
        {
            case 1:
                SubDiamond(num); 
                break;
            case 2:
                SubCoin(num);
                break;
            case 3:
                SubFriendPoint(num);
                break;
            case 6:
                SubHonor(num);
                break;
        } 
    }

     

    void Instance_UserCallBackEvent(C2sSprotoType.user.response resp)
    {
        SetUser(resp.user);
    }

    public void InitRoleData(C2sSprotoType.user u)
    {
        System.Collections.IDictionaryEnumerator enumerator = RoleTable.GetEnumerator();
        while (enumerator.MoveNext())
        { 
            RoleData r = RoleTable[enumerator.Key] as RoleData;
            r.wakeLevel = r.initWakeLevel;
            int id = (r.csv_id*1000) + r.wakeLevel; 
            r.starData = GameShared.Instance.GetRoleStarById(id);  
        }
        if (u.rolelist != null)
        {
            for (int i = 0; i < u.rolelist.Count; i++)
            {
                RoleData r = GameShared.Instance.GetRoleById((int)u.rolelist[i].csv_id);
                r.is_possessed = u.rolelist[i].is_possessed;
                r.wakeLevel = (int)u.rolelist[i].star;
                int id = (r.csv_id * 1000) + r.wakeLevel;
                r.starData = GameShared.Instance.GetRoleStarById(id);
                if (r.is_possessed)
                {
                    r.xilianList = new List<XiLianData>();
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)u.rolelist[i].property_id1, (int)u.rolelist[i].value1));
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)u.rolelist[i].property_id2, (int)u.rolelist[i].value2));
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)u.rolelist[i].property_id3, (int)u.rolelist[i].value3));
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)u.rolelist[i].property_id4, (int)u.rolelist[i].value4));
                    r.xilianList.Add(this.InitXiLianData((Def.AttrId)u.rolelist[i].property_id5, (int)u.rolelist[i].value5));
                } 
                //Debug.Log(r.sort +"/" +(int)r.csv_id);
                r.frgNum = (int)u.rolelist[i].u_us_prop_num;
                if (UserManager.Instance.RoleTable.Contains(r.csv_id))
                {
                    RoleTable[r.csv_id] = r;
                }
                else
                {
                    RoleTable.Add(r.csv_id, r);
                }

            }
        }
        else
        {
            Debug.Log("rolelist空");
        }
    }

    public XiLianData InitXiLianData(Def.AttrId id, int num)
    {
        XiLianData d = new XiLianData();
        XiLianMaxData max = GameShared.Instance.GetXiLianByType(id);
        d.id = id;
        d.min = max.min;
        d.max = max.max;
        d.name = Comm.GetAttrStr(id);
        d.num = (int)num;
        Debug.Log("id" + id + "num" + num);
        return d;
    }

    public void InitBoxingData(C2sSprotoType.user u)
    { 
        System.Collections.IDictionaryEnumerator enumerator = GameShared.Instance.boxingTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            BoxingViewData v = new BoxingViewData(); 
            v.frag_num = 0; 
            v.level = 0;
            BoxingData r = GameShared.Instance.boxingTable[enumerator.Key] as BoxingData;
            v.data = r;
            int id = (r.csv_id*1000) + v.level;
            r.levelData = GameShared.Instance.GetBoxingLevelById(id);
             
            boxTable.Add(v.data.csv_id, v); 
        }

        if (u.kungfu_list != null)
        {
            for (int i = 0; i < u.kungfu_list.Count; i++)
            {
                int id = (int)u.kungfu_list[i].csv_id;
                C2sSprotoType.kungfu_content c = u.kungfu_list[i];
                if (UserManager.Instance.boxTable.Contains(id))
                {
                    BoxingViewData v = new BoxingViewData();
                    v.level = (int)c.k_level;
                    v.frag_num = (int)c.k_sp_num;
                    v.type = (int)c.k_type;
                    v.data = GameShared.Instance.GetBoxingById((int)c.csv_id);
                    int levelid = (1000 * v.data.csv_id) + v.level;
                    v.data.levelData = GameShared.Instance.GetBoxingLevelById(levelid);
                    UserManager.Instance.boxTable[v.data.csv_id] = v;
                }
                else
                {
                    BoxingViewData v = new BoxingViewData();
                    v.level = (int)c.k_level;
                    v.frag_num = (int)c.k_sp_num;
                    v.type = (int)c.k_type;
                    v.data = GameShared.Instance.GetBoxingById((int)c.csv_id);
                    int levelid = (1000 * v.data.csv_id) + v.level;
                    v.data.levelData = GameShared.Instance.GetBoxingLevelById(levelid);
                    UserManager.Instance.boxTable.Add(id, v);
                }
            }
        }
        else
        {
            Debug.Log("kungfu_list空");
        }
    }

    public void InitEquipData(C2sSprotoType.user u)
    { 
        System.Collections.IDictionaryEnumerator enumerator = GameShared.Instance.equipmentTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            EquipViewData v = new EquipViewData(); 
            v.level = 1;
            EquipData r = GameShared.Instance.equipmentTable[enumerator.Key] as EquipData;
            v.data = r;
            int id = (r.csv_id * 1000) + v.level;
            r.levelData = GameShared.Instance.GetEquipmentIntensifyById(id);
            equipTable.Add(v.data.csv_id, v); 
        }
        if (u.equipment_list != null)
        {
            for (int i = 0; i < u.equipment_list.Count; i++)
            {
                EquipViewData e = new EquipViewData();
                int id = (int)u.equipment_list[i].csv_id;
                e.level = (int)u.equipment_list[i].level;
                if (id > 999)
                    id = id / 1000;
                e.data = GameShared.Instance.GetEquipmentById(id);
                e.data.levelData = EquipmentMgr.Instance.GetEquipLevelData(e.data.csv_id, e.level);

                if (UserManager.Instance.equipTable.Contains(e.data.csv_id))
                {
                    equipTable[e.data.csv_id] = e;
                }
                else
                {
                    equipTable.Add(e.data.csv_id, e);
                }
            }
        }
        else
        {
            Debug.Log("equipment_list空");
        }
    }

    public void InitHallData()
    {
        hallTable = new Dictionary<int, LiLianViewData>();
        foreach (KeyValuePair<int, LiLianHallData> pair in GameShared.Instance.lilianHallTable)
        { 
                LiLianViewData d = new LiLianViewData();
                d.data = pair.Value;
                d.unlock = false;
                d.num =  0;
                if (lilian_level >= d.data.open_level)
                {
                    d.unlock = true;
                }
                if (hallTable.ContainsKey(d.data.csv_id))
                {
                    hallTable[d.data.csv_id] = d;
                }
                else
                {
                    hallTable.Add(d.data.csv_id, d);
                } 
        }  
    }

    public void RestUserAttr()
    {
        if (userAttr == null)
        {
            userAttr = new UserAttrs();
        } 
        userAttr.RestUserAttr((int)level); 
    }  

    public void CheckPointHangingCallback(C2sSprotoType.checkpoint_hanging.response resp)
    {
        if (resp.errorcode == 1)
        {
            for(int i=0;i<resp.props.Count;i++)
            {
                BagMgr.Instance.SetItemNumById((int)resp.props[i].csv_id,(int)resp.props[i].num);
            } 
        } 
    }

    public void SetUser(C2sSprotoType.user u)
    {
        Debug.Log("用户信息获取成 recharge_diamond" + u.recharge_diamond + "level" + level + "hanging_checkpoint" + u.cp_hanging_id + "u.cp_chapter" + u.cp_chapter);
        level = (int)u.level;
        curDay = DateTime.Now.Day;
        curMonth = DateTime.Now.Month;

        
        exp = u.uexp;
        vip_level = (int)u.uviplevel;
        curVipdata = GameShared.Instance.GetVipByLevel((int)vip_level);
         

        coin = u.gold;
        diamond = (int)u.diamond;
        nickName = u.uname;
        friend_point = (int)u.love;
        signTxt = u.sign;
        recharge_total = (int)u.recharge_diamond;

        curUnLockChapter = (int)u.cp_chapter+1;
        battleRoleID = (int)u.c_role_id;
        

        ArenaMgr.Instance.curRank = (int)u.ara_rnk;
        lilian_level = (int)u.lilian_level;
         

        if (u.cp_hanging_id != 0)
            curLeveldata = GameShared.Instance.GetLevelById((int)u.cp_hanging_id); 

        RoleTable = GameShared.Instance.roleTable;
        boxTable = new Hashtable();
        equipTable = new Hashtable(); 

        InitRoleData(u);
        InitBoxingData(u);
        InitEquipData(u);
        InitHallData();

        curRole = GameShared.Instance.GetRoleById(battleRoleID);
        curRole.is_possessed = true;

        RestUserAttr();

        if (ChangeInfoEvent != null)
            ChangeInfoEvent();
    }

    public void SetGold(int i)
    {
        this.coin = i;
        if (ChangeInfoEvent != null)
            ChangeInfoEvent();
    }

    public void SetExp(int i)
    {
        this.exp = i;
        if (ChangeInfoEvent != null)
            ChangeInfoEvent();
    }
    public void SetDiamond(int i)
    {
        Debug.Log(i);
        this.diamond = i;
        if(ChangeInfoEvent != null)
            ChangeInfoEvent();
    }

    public void SetFriendPoint(int i)
    {
        this.friend_point = i;
        if(ChangeInfoEvent != null)
            ChangeInfoEvent();
    }

    public void SetHonor(int i)
    {
        this.honor_coin = i;
        if (ChangeInfoEvent != null)
            ChangeInfoEvent();
    }

    public void AddCoin(int i)
    {
        coin += i;
        SetGold((int)coin);
    }
    public void SubCoin(int i)
    { 
        coin-=i;
        SetGold((int)coin);
    }

    public void AddDiamond(int i)
    {
        diamond += i;
        SetDiamond((int)diamond);
    }
    public void SubDiamond(int i)
    { 
        diamond-=i;
        SetDiamond((int)diamond);
    }

    public void AddFriendPoint(int i)
    {
        friend_point += i;
        SetFriendPoint((int)friend_point);
    }
    public void SubFriendPoint(int i)
    {
        friend_point -= i;
        SetFriendPoint((int)friend_point);
    }

    public void AddExp(int i)
    {
        exp += i;
        SetExp((int)exp);
    }
    public void SubExp(int i)
    {
        exp -= i;
        SetExp((int)exp);
    }

    public void SubHonor(int i)
    {
        honor_coin -= i;
        SetHonor((int)exp); 
    }
    public void AddHonor(int i)
    {
        honor_coin -= i;
        SetHonor((int)exp);
    }

    public long GetCoin()
    {
        return coin;
    }

    public long GetExp()
    {
        return exp;
    }

    public long GetDiamond()
    {
        return diamond;
    }

    public long GetHonor()
    {
        return honor_coin;
    }

    public long GetFriendPoint()
    {
        return friend_point;
    }

    public long GetStrength()
    {
        return strength;
    }

    public void RestUserData()
    {
        RoleTable.Clear();
        boxTable.Clear();
        equipTable.Clear();
        BagMgr.Instance.RestEmpty();
    }
     

    //组合角色数据
    //public void InitUserData(C2sSprotoType.login.response resp)
    //{
    //    if (resp.u != null)
    //    {  
    //        Debug.Log("登录完成");
    //        GameShared.Instance.InitMemeryData();
    //        BagMgr.Instance.BagList();
    //        LevelsMgr.Instance.ChapterList();
    //        SetUser(resp.u);
    //        TimerRest(); 
    //    }  
    //}

    public void InitUserData()
    { 
        Debug.Log("登录完成");
        GameShared.Instance.InitMemeryData();
        BagMgr.Instance.BagList();
        LevelsMgr.Instance.ChapterList(); 
        TimerRest();
        //NetworkManager.Instance.LoginUserInfo();
        LoginMgr.Instance.EntryGame();
    }


    PassiveTimer expTimer;
    PassiveTimer hangingTimer;
    public void TimerRest()
    {
        expTimer = new PassiveTimer(1);
        hangingTimer = new PassiveTimer(Def.HangingRefreshTime);
    }

    public void TimerEmpty()
    { 
        expTimer = null;
        hangingTimer = null;
    }

    public void EndGame()
    {
        TimerEmpty();
    }

     

    void FixedUpdate()
    {
 
        if (expTimer != null && expTimer.Update(Time.deltaTime) > 0 && curLeveldata!=null)
        {
            UserManager.Instance.coin += curLeveldata.gain_gold;
            UserManager.Instance.exp +=curLeveldata.gain_exp;
            if(ChangeInfoEvent != null)
            ChangeInfoEvent();

        }
        if (hangingTimer != null && hangingTimer.Update(Time.deltaTime) > 0)
        {
            NetworkManager.Instance.CheckPointHanging();
        } 
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            //ClientSocket.Instance.SocketQuit();
        }

        //if (Application.runInBackground)
        //{
        //    ToastManager.Instance.Show("进入后台");
        //}
        //else
        //{
        //    ToastManager.Instance.Show("进入前台");
        //}
    }



}
