using UnityEngine;
using System.Collections; 
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using System;
using System.Reflection; 

/// <summary>
/// 用于存放读取 从本地获取只读数据 放到内存 
/// 预先载入部分动画 和lua 避免游戏中载入
/// </summary>

public class GameShared : MonoBehaviour
{ 
    private static GameShared _instance;
    public static GameShared Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameShared)) as GameShared;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    //obj.hideFlags = HideFlags.DontSave;
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = (GameShared)obj.AddComponent(typeof(GameShared));
                }
            }
            return _instance;
        }
    }

    public Dictionary<int, AraneRankRewardData> araneRankRewardTable { get; private set; }
    public Dictionary<int, AranePointRewardData> aranePointRewardTable { get; private set; }
    public Dictionary<int, AraneRefreshData> araneRefreshTable { get; private set; }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as GameShared;
        }
        else
        {
            Destroy(gameObject);
        }

        int rand = UnityEngine.Random.Range(100, 9999);
        ObscuredInt.SetNewCryptoKey(rand);
        ObscuredFloat.SetNewCryptoKey(rand); 
    }

    public const string DBName = "create5";

    public class StrData
    {
        public int id;
        public int num;
    } 

    public ConfigData config; 
    //table
    #region
    public Dictionary<string, TextAsset> luaTable; 
    public Hashtable itemDataTable;
    public Hashtable iconTable; 
    public Hashtable achievementTable;
    public Hashtable dailyTable;
    public Hashtable curMonthQianDaoTable;
    public Hashtable nextMonthQianDaoTable;

    public Hashtable storeListTable;
    public Hashtable storeItemTable;
    public Hashtable rechargeTable;
    public Hashtable vipTable; 
    public Hashtable lotteryTable;

    public Hashtable userLevelTable;

    public Hashtable buffTable;
     
    public Hashtable boxingTable;
    public Hashtable boxingLevelTable;

    //装备
    public Hashtable equipmentTable;
    public Hashtable equipmentXiTable;
    public Hashtable equipmentKitTable;
    public Hashtable equipmentIntensifyTable;

    public Hashtable roleTable;
    public Hashtable roleStarTable;

    public Hashtable refreshCostTable;

    public List<DailyCountItem> dailyItemList;

    public List<EquipmentKitData> equipmentKitList;

    public Dictionary<Def.AttrId, XiLianMaxData> xilianMaxTable;

    public Dictionary<int, XiLianConditionData> xilianConditionTable;
     
    public Dictionary<string, SkeletonDataAsset> animTable = new Dictionary<string,SkeletonDataAsset>();
    public Dictionary<int, RoleStarData> roleFragment;

    public Dictionary<int, LiLianHallData> lilianHallTable;

    public Dictionary<int, LiLianEventData> lilianEventTable;

    public Dictionary<int, LiLianCardData> lilianCardTable;

    public Dictionary<int, LiLianLevelData> lilianLevelTable;

    public Dictionary<int, ChapterData> chapterTable;

    public Dictionary<int, LevelData> levelTable;

    public Dictionary<int, LiLianStrengthData> lilianStrengthTable;

    public List<String> LuaList;
    #endregion

    //获得对象
    #region

    public MonsterData GetMonsterById(int id)
    {
        return monsterTable[id];
    }
    public LiLianStrengthData GetLiLianStrengthByCount(int c)
    {
        return lilianStrengthTable[c]; 
    }

    public LevelData GetLevelById(int id)
    {
        return levelTable[id];
    }

    public ChapterData GetChapterById(int id)
    {
        return chapterTable[id];
    }

    public LiLianHallData GetLiLianHallById(int id)
    {
        return lilianHallTable[id];
    }

    public LiLianEventData GetLiLianEventById(int id)
    {
        return lilianEventTable[id];
    }

    public LiLianCardData GetLiLianCardById(int id)
    {
        return lilianCardTable[id];
    }

    public LiLianLevelData GetLiLianLevelById(int id)
    {
        return lilianLevelTable[id];
    }

    public RoleStarData GetRoleInfoByFragment(int id)
    {
        RoleStarData r = null;
        if (roleFragment.ContainsKey(id))
            r = roleFragment[id];
        return r;
    }

    public SkeletonDataAsset GetSkeletonAssetByPath(string p)
    {
        SkeletonDataAsset a = null;
        if (animTable.ContainsKey(p))
        {
            a = animTable[p];
        }
        else
        {
            animTable.Add(p,Resources.Load<SkeletonDataAsset>(p));
        }
        return animTable[p];
    }

    public XiLianConditionData GetXiLianConditionByCount(int id)
    {
        return xilianConditionTable[id];
    }

    public XiLianMaxData GetXiLianByType(Def.AttrId id)
    {
        return xilianMaxTable[id];
    }
  
    public RefreshCostData GetRefreshCostByCount(int i)
    {
        return refreshCostTable[i] as RefreshCostData;
    }

    public RoleData GetRoleById(int id)
    {
        return roleTable[id] as RoleData;
    }

    public RoleStarData GetRoleStarById(int id)
    {
        return roleStarTable[id] as RoleStarData;
    }
    //装备
    public EquipData GetEquipmentById(int i)
    {
        return equipmentTable[i] as EquipData;
    }

    public EquipmentXiData GetEquipmentXiById(int i)
    {
        return equipmentXiTable[i] as EquipmentXiData;
    }

    public EquipmentKitData GetEquipmentKitById(int i)
    {
        return equipmentKitTable[i] as EquipmentKitData;
    }
    /// <summary>
    /// 遍历出当前级数的装备属性
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public EquipmentKitData GetEquipmentKitByLevel(int c)
    { 
        //目前最小的
        EquipmentKitData min = null;
        //遍历
        for (int i = 0; i < equipmentKitList.Count; i++)
        {
            if (equipmentKitList[i].level <= c)
            {
                if(min == null)
                {
                    min = equipmentKitList[i];
                }else if(min.level <= equipmentKitList[i].level)
                {
                    min = equipmentKitList[i];
                }
            } 
        }
        return min; 
    }
    /// <summary>
    /// 遍历出下前级数的装备属性
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public EquipmentKitData GetEquipmentKitNextLevelByLevel(int c)
    {
        //目前最小的
        EquipmentKitData min = null;
        int id = 0;
        //遍历
        for (int i = 0; i < equipmentKitList.Count; i++)
        {
            if (equipmentKitList[i].level < c)
            {
                if (min == null)
                {
                    min = equipmentKitList[i];
                    id = i;
                }
                else if (min.level < equipmentKitList[i].level)
                {
                    min = equipmentKitList[i];
                    id = i;
                }
            }
        }
        if (min != null && id + 1 < equipmentKitList.Count)
        {
            return equipmentKitList[id + 1];
        }
        else 
        {
            return equipmentKitList[0];
        }
        return null;
    }
  
    public EquipLevelData GetEquipmentIntensifyById(int i)
    {
        return equipmentIntensifyTable[i] as EquipLevelData;
    }
    //拳法
    public BoxingData GetBoxingById(int id)
    {
        return boxingTable[id] as BoxingData;
    }
    
    public BoxingLevelData GetBoxingLevelById(int id)
    {
        return boxingLevelTable[id] as BoxingLevelData;
    }

    public BoxingLevelData GetBoxingLevelByIdCon(int id)
    {
        if (boxingLevelTable.Contains(id))
        {
            return boxingLevelTable[id] as BoxingLevelData;
        }
        return null;
    }
    // 日常
    public DailyCountItem GetDailyItemByCount(int c)
    {
        int t = 0;
        for (int i = 0; i < dailyItemList.Count; i++)
        {
            if (dailyItemList[i].count >= c && i > 0)
            {
                t = i;
            } 
        }
        return dailyItemList[t];
    }

    public DailyCountItem GetDailyByRecNum(int n)
    {
        return dailyItemList[n];
    }
    /// <summary>
    /// 下次领取是第几次领取
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public int GetDailyCountByCount(int c)
    {
        int t = 0;
        for (int i = 0; i < dailyItemList.Count; i++)
        {
            if (dailyItemList[i].count <= c && i > 0)
            {
                t++;
            }
        }
        return t;
    } 

    public DailyCountItem GetDailyItemByIndex(int c)
    {
        if (c < dailyItemList.Count && c >= 0)
        {
            return dailyItemList[c];
        }
        return null;
    }
      
    public BuffData GetBuffById(int id)
    {
        return buffTable[id] as BuffData;
    }

    public int GetUserLevelMax()
    {
       return userLevelTable.Count;
    }
    
    public UserLevelData GetUserLevelByLevel(int level)
    {
        return userLevelTable[level] as UserLevelData;
    }
    //抽奖
    public LotteryData GetLotteryById(int id)
    {
        return lotteryTable[id] as LotteryData;
    }
    //充值
    public ReChargeData GetReChargeById(int id)
    {
        return rechargeTable[id] as ReChargeData; 
    }

    public VipData GetVipByLevel(int level)
    {
        return vipTable[level] as VipData; 
    }
    //商店
    public StoreData GetStore(int id)
    {
        return storeListTable[id] as StoreData;
    }

    public ProductData GetStoreItem(int id)
    {
        return storeItemTable[id] as ProductData;
    }
    //日常
    public void CheckMonthDaily()
    {
        //if (UserManager.Instance.curMonth + 1 == DateTime.Now.Month)
        //{
        //    curMonthQianDaoTable = nextMonthQianDaoTable;
        //}
    }

    public QianDaoData GetQianDao(int day)
    { 
        return curMonthQianDaoTable[day] as QianDaoData; 
    }
    
    public DailyData GetDailyData(int id)
    {
        return dailyTable[id] as DailyData;
    }
      
    //道具 
    public string GetIconPathById(int id)
    {
        if (iconTable.Contains(id))
        {
            return iconTable[id] as string;
        }
        return null;
    }
    
    public ItemData GetItemData(int id)
    {
        if (itemDataTable.Contains(id))
        {
            return itemDataTable[id] as ItemData;
        }
        return null;
    }
    //成就
    public AchievementData GetAchievementData(int id)
    {
        return achievementTable[id] as AchievementData;
    }
    #endregion

    //初始化资源
    public void InitAnimData()
    {

    }

    
    /// <summary>
    /// ulua DoFile 文件加载
    /// </summary>
    public void InitLuaData()
    {
    //    LuaMgr.Instance.InitData();
    //    LuaList = new List<string>();
    //    LuaList.Add("def.lua");
    //    LuaList.Add("arena.lua");
    //    for(int i=0;i<LuaList.Count;i++)
    //    {
    //        if (luaTable.ContainsKey(LuaList[i]))
    //        {
    //            luaTable[LuaList[i]] = Resources.Load<TextAsset>(LuaList[i]);
    //        }
    //        else
    //        {
    //            luaTable.Add(LuaList[i], Resources.Load<TextAsset>(LuaList[i]));
    //        }
    //    }  
    }
    /// <summary>
    /// 执行文件
    /// </summary>
    public void GetLuaFile(string str)
    {
        //if (luaTable.ContainsKey(str))
        //{
        //    LuaMgr.Instance.state.DoString(str);
        //}
        //else
        //{
        //    luaTable.Add(str, Resources.Load<TextAsset>(str)); 
        //    LuaMgr.Instance.state.DoString(str);
        //}
    }
    
    public void InitMemeryData()
    {
       
        //attenuationList = getShuaijian(shuaijian);  
        itemDataTable = DBManager.Instance.QueryItemDataTable(); 
        achievementTable = DBManager.Instance.QueryAchievementTable();
        dailyTable = DBManager.Instance.QueryDailyDataTable();

        
        curMonthQianDaoTable = DBManager.Instance.QueryQianDaoDataMonthTable(0);
        nextMonthQianDaoTable = DBManager.Instance.QueryQianDaoDataMonthTable(0);
        storeListTable = DBManager.Instance.QueryStoreListTable();
        storeItemTable = DBManager.Instance.QueryStoreItemTable();
        vipTable = DBManager.Instance.QueryVipTable();
        rechargeTable = DBManager.Instance.QueryReChargeTable();
        iconTable = DBManager.Instance.QueryIconTable();
        lotteryTable = DBManager.Instance.QueryLotteryTable(); 
        userLevelTable = DBManager.Instance.QueryUserLevelTable();
        buffTable = DBManager.Instance.QueryBuffTable(); 
        dailyItemList = DBManager.Instance.QueryDailySignITemDataList();
        boxingTable = DBManager.Instance.QueryBoxingTable();
        boxingLevelTable = DBManager.Instance.QueryBoxingLevelTable();


        equipmentXiTable = DBManager.Instance.QueryEquipmentXiTable();
        equipmentKitTable = DBManager.Instance.QueryEquipmentKitTable();
        equipmentKitList = DBManager.Instance.QueryEquipmentKitList();
        equipmentIntensifyTable = DBManager.Instance.QueryEquipmentIntensifyTable();
        equipmentTable = DBManager.Instance.QueryEquipmentTable();

        roleTable = DBManager.Instance.QueryRoleTable();
        roleStarTable = DBManager.Instance.QueryRoleStarTable();

        refreshCostTable = DBManager.Instance.QueryRefreshCostTable();

        xilianMaxTable = DBManager.Instance.QueryXiLianMaxTable();
        xilianConditionTable = DBManager.Instance.QueryXiLianConditionTable();
        config = DBManager.Instance.QueryConfigData();
        roleFragment = DBManager.Instance.QueryRoleFragmentTable();

        lilianHallTable = DBManager.Instance.QueryLiLianHallTable();
        lilianLevelTable = DBManager.Instance.QueryLiLianLevelTable(); 
        lilianCardTable = DBManager.Instance.QueryLiLianCardTable();
        lilianEventTable = DBManager.Instance.QueryLiLianEventTable();

        chapterTable = DBManager.Instance.QueryChapterTable();
        levelTable = DBManager.Instance.QueryLevelTable();

        lilianStrengthTable = DBManager.Instance.QueryLiLianStrength();

        monsterTable =  DBManager.Instance.QueryMonsterTable();

        aranePointRewardTable = DBManager.Instance.QueryPointReward();
        araneRefreshTable = DBManager.Instance.QueryAraneRefresh();
        araneRankRewardTable = DBManager.Instance.QueryRankReward();

        animTable = new Dictionary<string, SkeletonDataAsset>();
        InitLuaData();
        InitRoleStarTableData();
        InitBoxingLevelTableData();
        InitEquipKitData();
        InitXilianCondition();
        InitLevelTableData();


        InitRoleData();
    }

    public AranePointRewardData GetAranePointReward(int id)
    {
        return aranePointRewardTable[id];
    }
    public AraneRankRewardData GetAraneRankReward(int id)
    { 
        return araneRankRewardTable[id];
    }
    public AraneRefreshData GetAraneRefresh(int id)
    {
        return araneRefreshTable[id];
    }

    //初始化数据
    #region
    public void InitXilianCondition()
    {
        System.Collections.IDictionaryEnumerator enumerator = xilianConditionTable.GetEnumerator();
        foreach (XiLianConditionData d in xilianConditionTable.Values)
        {
            List<StrData> s = GetStrData(d.str);
            d.list = new List<ItemViewData>();
            for (int i = 0; i < s.Count; i++)
            {
                ItemViewData v = new ItemViewData();
                v.curCount = s[i].num;
                v.data = this.GetItemData(s[i].id);
                d.list.Add(v);
            } 
        } 
    }

    public void InitEquipKitData()
    {
        for (int i = 0; i < equipmentKitList.Count;i++)
        {
            EquipmentKitData r = equipmentKitList[i];
            BuffData b1 = GameShared.Instance.GetBuffById(r.effect);
            b1.GetBuffAttr();
            r.attrArr = b1.attrArr;
            r.additionArr = b1.additionArr;
        }
    }

    public void InitEquipData()
    {
        System.Collections.IDictionaryEnumerator enumerator = equipmentIntensifyTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            EquipLevelData r = equipmentIntensifyTable[enumerator.Key] as EquipLevelData;
            r.attrArr[(int)Def.AttrId.FightPower] = r.combat;
            r.attrArr[(int)Def.AttrId.Crit] = r.critical_hit;
            r.attrArr[(int)Def.AttrId.Defense] = r.defense;
            r.attrArr[(int)Def.AttrId.Pray] = r.king;

            r.additionArr[(int)Def.AttrId.FightPower] = r.combat_probability;
            r.additionArr[(int)Def.AttrId.Crit] = r.critical_hit_probability;
            r.additionArr[(int)Def.AttrId.Defense] = r.defense_probability;
            r.additionArr[(int)Def.AttrId.Pray] = r.king_probability;
        }   
    }

    public void InitEquipKitTableData()
    { 
        System.Collections.IDictionaryEnumerator enumerator = equipmentKitTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            EquipmentKitData r = equipmentKitTable[enumerator.Key] as EquipmentKitData;
            BuffData b1 = GameShared.Instance.GetBuffById(r.effect);
            b1.GetBuffAttr();
            r.attrArr = b1.attrArr;
            r.additionArr = b1.additionArr; 
        }  
    }
    //处理 角色表的 buff 数值填进去
    public void InitRoleStarTableData()
    {
        System.Collections.IDictionaryEnumerator enumerator = roleStarTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            RoleStarData r = roleStarTable[enumerator.Key] as RoleStarData; 
            BuffData b1 = GameShared.Instance.GetBuffById(r.gather_buffer_id);
            b1.GetBuffAttr();
            r.attrArr = b1.attrArr;
            r.additionArr = b1.additionArr;
            BuffData b2 = GameShared.Instance.GetBuffById(r.battle_buffer_id);
            b2.GetBuffAttr();
            r.battleAttr = b2.attrArr;
            r.battleAddition = b2.additionArr;
        }   
    }

    public void InitBoxingLevelTableData()
    {
        System.Collections.IDictionaryEnumerator enumerator = boxingLevelTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            BoxingLevelData r = boxingLevelTable[enumerator.Key] as BoxingLevelData;
            BuffData b1 = GameShared.Instance.GetBuffById(r.buff_id);
            b1.GetBuffAttr();
            r.attrArr = b1.attrArr;
            r.additionArr = b1.additionArr;

            BuffData b2 = GameShared.Instance.GetBuffById(r.equip_buff_id);
            b2.GetBuffAttr();
            r.equipAttrArr = b2.attrArr;
            r.equipAdditionArr = b2.additionArr;
        } 
    }

    public void InitLevelTableData()
    {
        foreach (KeyValuePair<int, LevelData> pair in levelTable)
        {
            List<StrData> strs = GetStrData(pair.Value.reward);
             pair.Value.rewardList= new List<ItemViewData>();
            for (int i = 0; i < strs.Count; i++)
            {
                ItemViewData v = new ItemViewData();
                v.data = GameShared.Instance.GetItemData(strs[i].id);
                v.curCount = strs[i].num; 
                pair.Value.rewardList.Add(v); 
            } 
        }  
    }
    #endregion


    public List<int> GetStr(string s)
    {
        List<int> list = new List<int>();
        if (s != null)
        {
            string[] str = s.Split('*'); 
            for (int i = 0; i < str.Length; i++)
            { 
                list.Add(int.Parse(str[i]));  
            }
        }
        return list;
    }
     
    public List<StrData> GetStrData(string s)
    { 
        List<StrData> list = new List<StrData> (); 
        if (s != null)
        {
            string[] str = s.Split('*');
            StrData d = new StrData();
            for (int i = 0; i < str.Length; i++)
            {
                int index = i % 2;
                
                switch (index)
                {
                    case 0:
                        d = new StrData();
                        d.id = int.Parse(str[i]);
                        break;
                    case 1:
                        d.num = int.Parse(str[i]);
                        list.Add(d);
                        break;
                }
                
            }
        }
        return list;
    }
    //将要废弃
    #region

    public class Attenuation
    {
        public int level;
        public int attenuation;
    }

    public class Xishu
    {
        public float powerMax;
        public float curPower;
        public float def;
        public float crit;
        public float critMin;
        public float skill;
        public float skillMin;
        public float critMul;
        public float combo;
    }

    public class WakeItem
    {
        public int curId;
        public int wakeId;
        public int levelMax;
        public int coin;
        public string itemList;
    }

 

    //固定
    public Xishu xishu;//c  
    public List<Attenuation> attenuationList;//sc  
    public List<float> cursorSpeedList;
    public RoleData.StaticBoxRule staticRule;

    public int curLevel;


    public RoleData roleData;
    public RoleData emenyData;
    private Dictionary<int, MonsterData> monsterTable;

    // Use this for initialization
    public void InitRoleData() {
         

        //curLevel = 0;


        //roleData = this.GetRoleById(1);
        //roleData.starData = this.GetRoleStarById(1001);

        //emenyData = this.GetRoleById(1);
        //emenyData.starData = this.GetRoleStarById(1001);

        //string path1 = Application.dataPath + "\\Resources\\DataTest\\staticBox.csv";
        //string path2 = Application.dataPath + "\\Resources\\DataTest\\moveBox.csv";
        //string path3 = Application.dataPath + "\\Resources\\DataTest\\cursor.csv";
        //string path4 = Application.dataPath + "\\Resources\\DataTest\\staticBoxRule.csv";
        //string path5 = Application.dataPath + "\\Resources\\DataTest\\battle.csv";
        //string path6 = Application.dataPath + "\\Resources\\DataTest\\shuaijian.csv";
        //string path7 = Application.dataPath + "\\Resources\\DataTest\\xishu.csv";
        //string path8 = Application.dataPath + "\\Resources\\DataTest\\level.csv";

        TextAsset binAsset = Resources.Load("DataTest/staticBox", typeof(TextAsset)) as TextAsset;
        string path1 = binAsset.text;
        binAsset = Resources.Load("DataTest/moveBox", typeof(TextAsset)) as TextAsset;
        string path2 = binAsset.text;
        binAsset = Resources.Load("DataTest/cursor", typeof(TextAsset)) as TextAsset;
        string path3 = binAsset.text;
        binAsset = Resources.Load("DataTest/staticBoxRule", typeof(TextAsset)) as TextAsset;
        string path4 = binAsset.text;
        binAsset = Resources.Load("DataTest/battle", typeof(TextAsset)) as TextAsset;
        string path5 = binAsset.text;
        binAsset = Resources.Load("DataTest/shuaijian", typeof(TextAsset)) as TextAsset;
        string path6 = binAsset.text;
        binAsset = Resources.Load("DataTest/xishu", typeof(TextAsset)) as TextAsset;
        string path7 = binAsset.text;
        binAsset = Resources.Load("DataTest/level", typeof(TextAsset)) as TextAsset;
        string path8 = binAsset.text;

        List<string[]> staticStr = GetString(path1);
        List<string[]> moveStr = GetString(path2);
        List<string[]> cursorStr = GetString(path3);
        List<string[]> staticRuleStr = GetString(path4);
        List<string[]> battle = GetString(path5);
        List<string[]> shuaijian = GetString(path6);
        List<string[]> x = GetString(path7);
        List<string[]> level = GetString(path8);


        //块
        roleData = new RoleData(); staticStr.RemoveAt(0); moveStr.RemoveAt(0); cursorStr.RemoveAt(0); staticRuleStr.RemoveAt(0); battle.RemoveAt(0); shuaijian.RemoveAt(0); x.RemoveAt(0); level.RemoveAt(0);
        emenyData = new RoleData();
        RoleData.CycleAttr[] staticCycleList = new RoleData.CycleAttr[staticStr.Count];
        RoleData.CycleAttr[] moveCycleList = new RoleData.CycleAttr[moveStr.Count];
        //填充数据 
        roleData.staticCycleList = new List<RoleData.CycleAttr>();
        emenyData.staticCycleList = new List<RoleData.CycleAttr>();
        for (int i = 0; i < staticStr.Count; i++)
        {
            roleData.staticCycleList.Add(getCycleData(staticStr[i]));
            emenyData.staticCycleList.Add(getCycleData(staticStr[i]));
        }
        roleData.moveCycleList = new List<RoleData.CycleAttr>();
        emenyData.moveCycleList = new List<RoleData.CycleAttr>();
        for (int i = 0; i < moveStr.Count; i++)
        {
            roleData.moveCycleList.Add(getCycleData(moveStr[i]));
            emenyData.moveCycleList.Add(getCycleData(moveStr[i]));
        }
        //游标 
        cursorSpeedList = getCursorData(cursorStr);
        //规则
        staticRule = getStaticRule(staticRuleStr);
        //战斗 
        //roleData.wakeLevelAttrList = DBManager.Instance.QueryWakeAttrList(1);
        //emenyData.wakeLevelAttrList = DBManager.Instance.QueryWakeAttrList(2);
        //emenyData.wakeLevelAttrList.RemoveRange(0, 5); 

        //固定
        //衰减
        attenuationList = getShuaijian(shuaijian);
        xishu = GetXishu(x);  
      

        //Role.GetComponent<MonsterMgr>().data = roleData;
        //Emeny.GetComponent<MonsterMgr>().data = emenyData;
    }

    public List<string[]> GetString(string path)
    {
        //读取每一行的内容  
        string a = path.Replace("\n", "");
        string[] lineArray = a.Split("\r"[0]);

        //创建二维数组   
        List<string[]> list = new List<string[]>();
        //把csv中的数据储存在二位数组中  
        for (int i = 0; i < lineArray.Length; i++)
        {
            string[] s = lineArray[i].Split(',');
            list.Add(s);
        }
        list.RemoveAt(lineArray.Length - 1);
        return list;
    }


    List<LevelData> GetLevel(List<string[]> r)
    {
        List<LevelData> list = new List<LevelData>();
        int i = 0;
        
        while(i<r.Count)
        { 
            LevelData data = new LevelData(); 
            //data.data.id = int.Parse(r[i][0]); 
            //data.data.exp = int.Parse(r[i][1]);
            list.Add(data); 
        }
        return list;
    }

    Xishu GetXishu(List<string[]> r)
    {
       Xishu xishu = new Xishu();
        int i = 0;
        float num = 0;
        while (i < r.Count)
        {
            num = float.Parse(r[i][2])/100;
            switch (i)
            {
                case 0:
                    xishu.powerMax = num;
                    break;
                case 1:
                    xishu.curPower = num;
                    break;
                case 2:
                    xishu.def = num;
                    break;
                case 3:
                    xishu.crit = num;
                    break;
                case 4:
                    xishu.critMin = num;
                    break;
                case 5:
                    xishu.skill = num;
                    break;
                case 6:
                    xishu.skillMin = num;
                    break;
                case 7:
                    xishu.critMul = num;
                    break;
                case 8:
                    xishu.combo = num;
                    break; 
            }
            i++;
        }
        return xishu;
 
    }
     
    List<RoleData.WakeAttr> getBattle(List<string[]> r)
    {
        List<RoleData.WakeAttr> list = new List<RoleData.WakeAttr>();
        for (int i = 0; i < r.Count; i++)
        {
            RoleData.WakeAttr a = new RoleData.WakeAttr(); 
            for (int j = 0; j < r[i].Length; j++)
            { 
                switch (j)
                {
                    case 0:
                        a.id = int.Parse(r[i][j]);
                        break;
                    case 1:
                        a.name = r[i][j];
                        break;
                    case 3:
                        a.fightPowerBase = int.Parse(r[i][j]);
                        break;
                    case 4:
                        a.defenseBase = int.Parse(r[i][j]);
                        break;
                    case 5:
                        a.critBase = int.Parse(r[i][j]);
                        break;
                    case 6:
                        a.skillBase = int.Parse(r[i][j]);
                        break;
                    case 7:
                        a.fightLevel = int.Parse(r[i][j]);
                        break;
                    case 8:
                        a.initEqu = int.Parse(r[i][j]);
                        break;
                    case 9:
                        a.initSkill = int.Parse(r[i][j]);
                        break;
                    case 10:
                        a.levelCount = int.Parse(r[i][j]);
                        break;  
                } 
            }
            list.Add(a);
        } 
        return list; 
    }

    public List<Attenuation> getShuaijian(List<string[]> r)
    {  
        List<Attenuation> list = new List<Attenuation>(); 
            
        for (int i = 0; i < r.Count; i++)
        {
            Attenuation a = new Attenuation();
            for (int j = 0; j < r[i].Length; j++)
            {
                switch (j)
                {
                    case 0:
                        a.level = int.Parse(r[i][j]);
                        break;
                    case 1:
                        a.attenuation = int.Parse(r[i][j]);
                        break;
                }
            }
            list.Add(a);
        }
        return list;
    }

    RoleData.StaticBoxRule getStaticRule(List<string[]>r)
    {
        RoleData.StaticBoxRule rule = new RoleData.StaticBoxRule();
        rule.makeBoxCountRate = new List<float>();
        int i = 0;
        float num = 0;
        while(i<r.Count)
        {
            num = float.Parse(r[i][2]);
            switch(i)
            {
                case 0:
                    rule.countMin = (int)num;
                    break;
                case 1:
                    rule.countMax = (int)num;
                    break;
                case 2:
                    rule.eventABRate[0] = (int)num;
                    break;
                case 3:
                    rule.eventABRate[1] = (int)num;
                    break;
                case 4:
                    rule.eventATime = (int)num;
                    break;
                case 5:
                    rule.eventBRate = (int)num;
                    break;
                case 6:
                    rule.minTime = (int)num;
                    break;
                case 7:
                    rule.maxTime = (int)num;
                    break;
                case 8:
                    rule.makeBoxCountRate.Add(num);
                    break;
                case 9:
                    rule.makeBoxCountRate.Add(num);
                    break;
                case 10:
                    rule.makeBoxCountRate.Add(num);
                    break;
                case 11:
                    rule.yellowRate = (int)num;
                    break;
                case 12:
                    rule.fastMaxSpeed = num;
                    break;
                case 13:
                    rule.skillMin = num;
                    break;
                case 14:
                    rule.skillMax = num;
                    break;  
            }
            i++;
        }
        return rule; 
    }

    List<float> getCursorData(List<string[]> r)
    {
        List<float> str = new List<float>();
        for (int i = 0; i < r.Count; i++)
        {
            float num = 0;
            if (float.TryParse(r[i][1], out num))
            {
            }
            else
            {
                //转换失败, 字符串不是只是数字 
                Debug.Log("这个不是数字 " + i);
            } 
            str.Add(num); 
            
        }
        return str;
    }
    
    RoleData.CycleAttr getCycleData(string[] r)
    {
        List<string> str = new List<string>();
        for (int i = 0; i < r.Length; i++)
        {
            if (!r[i].Equals(""))
                str.Add(r[i]);
        }
        int arrylenght = (str.Count - 3) / 6;
        RoleData.CycleAttr cycle = new RoleData.CycleAttr();
        RoleData.BoxAttr[] boxList = new RoleData.BoxAttr[arrylenght];
        for (int i = 0; i < boxList.Length; i++)
        {
            boxList[i] = new RoleData.BoxAttr();
        }
        for (int i = 0; i < str.Count; i++)
        {
            float num = 0;

            if (float.TryParse(str[i], out num))
            { 
            }
            else
            {
                //转换失败, 字符串不是只是数字 
                Debug.Log("这个不是数字 "+ i); 
            } 

            if (i - 2 > 0)
            {
                int temp = i - 3;
                int arrIndex = temp / 6; 
                int index = temp - (arrIndex * 6);
                if (arrIndex >= 5)
                {
                    Debug.Log(arrIndex + "/" + i);
                }
                if (arrIndex < arrylenght)
                {
                    switch (index)
                    {
                        case 0:
                            boxList[arrIndex].type =(int) num;
                            break;
                        case 1:
                            boxList[arrIndex].timeMin = num;
                            break;
                        case 2:
                            boxList[arrIndex].timeMax = num;
                            break;
                        case 3:
                            boxList[arrIndex].makeCount =(int)num;
                            break;
                        case 4:
                            boxList[arrIndex].width = num;
                            break;
                        case 5:
                            boxList[arrIndex].moveSpeed = num;
                            break; 
                    }
                }
            }
            else
            {
                switch (i)
                {
                    case 1:
                        cycle.countMax = (int)num;
                        break;
                    case 2:
                        cycle.intervalMin = num;
                        break;
                }
            }
        } 
        cycle.boxList = new List<RoleData.BoxAttr>(boxList);
        return cycle;
    }
    #endregion


}
