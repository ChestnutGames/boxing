using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;

public class DBManager : UnitySingleton<DBManager>
{
    public const int RoleConstant = 1000;

    public const string DBName = "create5.db";
     
    private  DbAccess db;

    public void Awake()
    { 
    }

    public void InitDB()
    {
        if (db == null)
        {

            string appDBPath;
            //如果运行在编辑器中  
#if UNITY_EDITOR
            //通过路径找到第三方数据库  
            appDBPath = Application.dataPath + "/StreamingAssets/" + DBName;
            db = new DbAccess("URI=file:" + appDBPath);
            //如果运行在Android设备中  

#elif UNITY_ANDROID  
  
        //将第三方数据库拷贝至Android可找到的地方  
            appDBPath = Application.persistentDataPath + "/" + DBName;  
        //如果已知路径没有地方放数据库，那么我们从Unity中拷贝  
            Debug.Log("test"+appDBPath);
        if(!File.Exists(appDBPath))  
        {  
            //用www先从Unity中下载到数据库  
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DBName);   
            bool boo=true;  
            while(boo)  
            {  
                if(loadDB.isDone)  
                 {  
                     //拷贝至规定的地方  
                    File.WriteAllBytes(appDBPath, loadDB.bytes);  
                    boo =false;
                    Debug.Log("test" + "done");
                 }  
            }  
              
        }  
        //在这里重新得到db对象。  
        db = new DbAccess("URI=file:" + appDBPath);  
#endif  
//            //数据库文件储存地址
//            //android ios
            
//            //android
            
//#if UNITY_ANDROID && !UNITY_EDITOR
//            Debug.Log("androidDB");
//            string appDBPath = Application.persistentDataPath + "/"+DBName+".db";  
//            db = new DbAccess("URI=file:" + appDBPath);
//#endif
//#if UNITY_EDITOR
//            Debug.Log("windDB");
//            //ios
//            //DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
//            string appDBPath = Application.dataPath + "/" + DBName + ".db";
//            db = new DbAccess(@"Data Source=" + appDBPath);
//#endif
        }  
    }
 

     
    //获得角色基础属性
    public Hashtable QueryRoleTable()
    {
        InitDB();
        Hashtable table = new Hashtable();
        SqliteDataReader sqReader = db.ReadFullTable("g_role"); 
        while (sqReader.Read())
    	{
            RoleData role = new RoleData();
            role.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            role.initWakeLevel = sqReader.GetInt32(sqReader.GetOrdinal("star"));
            role.sharp = sqReader.GetInt32(sqReader.GetOrdinal("sharp"));
            role.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            role.us_prop_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("us_prop_csv_id")); 
            table.Add(role.csv_id,role);
    	}
        return table;
    }
    //角色升星
    public Hashtable QueryRoleStarTable()
    {
        InitDB();
        Hashtable table = new Hashtable();
        SqliteDataReader sqReader = db.ReadFullTable("g_role_star");
        while (sqReader.Read())
        { 
            RoleStarData role = new RoleStarData();
            role.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            role.g_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("g_csv_id"));
            role.us_prop_num = sqReader.GetInt32(sqReader.GetOrdinal("us_prop_num"));
            role.us_prop_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("us_prop_csv_id"));
            role.star_init = sqReader.GetInt32(sqReader.GetOrdinal("star_init"));
            role.sharp = sqReader.GetInt32(sqReader.GetOrdinal("sharp"));
            role.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            role.anim = sqReader.GetString(sqReader.GetOrdinal("anim"));
            role.strs = sqReader.GetString(sqReader.GetOrdinal("strs"));
            role.skill_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("skill_csv_id"));
            role.gather_buffer_id = sqReader.GetInt32(sqReader.GetOrdinal("gather_buffer_id"));
            role.battle_buffer_id = sqReader.GetInt32(sqReader.GetOrdinal("battle_buffer_id"));
            table.Add(role.g_csv_id, role);
        }
        return table;
    }

    

    

    //道具
    public Hashtable QueryItemDataTable()
    {
        InitDB(); 
        SqliteDataReader sqReader = db.ExecuteQuery(
        "select * FROM ((g_prop INNER JOIN item_desc ON g_prop.intro=item_desc.id)INNER JOIN item_icon ON g_prop.icon_id=item_icon.id)INNER JOIN item_type ON g_prop.sub_type=item_type.id");
        List<ItemData> list = new List<ItemData>();
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            ItemData item = new ItemData();
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.useType = sqReader.GetInt32(sqReader.GetOrdinal("use_type")); 
            item.subType = sqReader.GetInt32(sqReader.GetOrdinal("sub_type"));
            item.bagType = sqReader.GetInt32(sqReader.GetOrdinal("group_id"));
            item.isShow = sqReader.GetInt32(sqReader.GetOrdinal("show"));
            item.quality = (ItemData.QualityType)sqReader.GetInt32(sqReader.GetOrdinal("level"));
            item.trace = sqReader.GetInt32(sqReader.GetOrdinal("zs"));

            item.attrArr[(int)Def.AttrType.FightPower] = sqReader.GetInt32(sqReader.GetOrdinal("combat"));
            item.attrArr[(int)Def.AttrType.Defense] = sqReader.GetInt32(sqReader.GetOrdinal("defense"));
            item.attrArr[(int)Def.AttrType.Crit] = sqReader.GetInt32(sqReader.GetOrdinal("critical_hit"));
            item.attrArr[(int)Def.AttrType.Pray] = sqReader.GetInt32(sqReader.GetOrdinal("pray"));

//            item.useArg1 = sqReader.GetString(sqReader.GetOrdinal("pram1"));
            //item.useArg2 = sqReader.GetString(sqReader.GetOrdinal("pram2"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name")); 
            item.path = sqReader.GetString(sqReader.GetOrdinal("path"));
            item.desc = sqReader.GetString(sqReader.GetOrdinal("desc"));
            item.typeName = sqReader.GetString(sqReader.GetOrdinal("typeName"));

            table.Add(item.id, item);
        }
        return table;
    }
    //图标
    public Hashtable QueryIconTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("item_icon");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        { 
            int id = sqReader.GetInt32(sqReader.GetOrdinal("id"));
            string path = sqReader.GetString(sqReader.GetOrdinal("path"));

            table.Add(id, path);
        }
        return table;
    }  
    
 
    //成就
    public Hashtable QueryAchievementTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_achievement");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            AchievementData item = new AchievementData();
            item.icon = sqReader.GetString(sqReader.GetOrdinal("icon_id"));
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.type = sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.condition = sqReader.GetInt32(sqReader.GetOrdinal("c_num"));
            item.desc = sqReader.GetString(sqReader.GetOrdinal("describe"));
            item.curStar = sqReader.GetInt32(sqReader.GetOrdinal("star"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward"));
            item.unlockId = sqReader.GetString(sqReader.GetOrdinal("unlock_next_csv_id"));
            table.Add(item.id, item);
        }
        return table;
    }
    //签到
    public Hashtable QueryQianDaoDataMonthTable(int month)
    {
        InitDB();
        SqliteDataReader sqReader = db.SelectWhere("g_checkin",
           new string[] { "month" }, new string[] { "==" },
           new string[] { month .ToString()});
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            QianDaoData item = new QianDaoData();
            item.item_count = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.num = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            item.month = sqReader.GetInt32(sqReader.GetOrdinal("month"));
            item.item_id = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_csv_id"));
            item.diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_num"));
            item.vip_item_id = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_csv_id"));
            item.vip_item_count = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_num"));
            item.vip_level = sqReader.GetInt32(sqReader.GetOrdinal("vip"));
            item.vip_diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_num"));
            table.Add(item.id, item);
        }
        return table;
    }

    public Hashtable QueryQianDaoDataTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_checkin");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            QianDaoData item = new QianDaoData();
            item.item_count = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.num = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            item.month = sqReader.GetInt32(sqReader.GetOrdinal("month"));
            item.item_id = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_csv_id"));
            item.diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_num"));
            item.vip_item_id = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_csv_id"));
            item.vip_item_count = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_num"));
            item.vip_level = sqReader.GetInt32(sqReader.GetOrdinal("vip"));
            item.vip_diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("vip_g_prop_num"));
            table.Add(item.id, item);
        }
        return table;
    }

   

    public Hashtable QueryDailyDataTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_daily_task");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            DailyData item = new DailyData();
            item.refresh_time = sqReader.GetString(sqReader.GetOrdinal("update_time"));
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("id"));
            item.daily_type = (DailyData.DailyType)sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("task_name"));
            item.diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("cost_dioment"));
            item.icon = sqReader.GetString(sqReader.GetOrdinal("iconid"));
            item.rewared = sqReader.GetString(sqReader.GetOrdinal("basic_reward"));
            item.level_rewared = sqReader.GetInt32(sqReader.GetOrdinal("levelup_reward"));
            item.level_up = sqReader.GetInt32(sqReader.GetOrdinal("level_up")); 
            table.Add(item.id, item);
        }
        return table;
    }

    

    public List<DailyCountItem> QueryDailySignITemDataList()
    {
        InitDB();
        SqliteDataReader sqReader = db.ExecuteQuery("SELECT * FROM checkin_total ORDER BY totalamount ASC");
        List<DailyCountItem> list = new List<DailyCountItem>();
        while (sqReader.Read())
        {
            DailyCountItem item = new DailyCountItem();
            item.count = sqReader.GetInt32(sqReader.GetOrdinal("totalamount"));
            item.item = sqReader.GetString(sqReader.GetOrdinal("prop_id_num"));
            list.Add(item);
        }
        return list;
    }


    //商店
    public Hashtable QueryStoreItemTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_goods"); 
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            ProductData item = new ProductData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.g_prop_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_csv_id"));
            item.inventory = sqReader.GetInt32(sqReader.GetOrdinal("inventory_init"));
            item.currency_type =sqReader.GetInt32(sqReader.GetOrdinal("currency_type"));
            item.currency_num = sqReader.GetInt32(sqReader.GetOrdinal("currency_num"));
            item.cd = sqReader.GetInt32(sqReader.GetOrdinal("cd"));
            item.g_prop_num = sqReader.GetInt32(sqReader.GetOrdinal("g_prop_num")); 
            //item.stock = sqReader.GetInt32(sqReader.GetOrdinal("stock"));
            //item.store_type = (ProductData.ProductType)sqReader.GetInt32(sqReader.GetOrdinal("type"));
            //item.count = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            //item.cd = sqReader.GetInt32(sqReader.GetOrdinal("cd"));
            item.refresh_count = sqReader.GetInt32(sqReader.GetOrdinal("cd")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Hashtable QueryStoreListTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_shop");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            StoreData item = new StoreData();
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.type = sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.count = sqReader.GetInt32(sqReader.GetOrdinal("num"));
            item.product = sqReader.GetString(sqReader.GetOrdinal("group_id"));
            item.list = GameShared.Instance.GetStrData(item.product); 
            table.Add(item.id, item);
        }
        return table;
    }
    //充值
    public Hashtable QueryReChargeTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_recharge");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            ReChargeData item = new ReChargeData();
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            //item.icon = sqReader.GetString(sqReader.GetOrdinal("icon_id"));
            //item.diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("diamond"));
            //item.frist_send = sqReader.GetInt32(sqReader.GetOrdinal("first"));goods_id
            item.buy_diamond = sqReader.GetInt32(sqReader.GetOrdinal("diamond"));
            item.rmb = sqReader.GetInt32(sqReader.GetOrdinal("rmb"));
            item.before_desc = sqReader.GetString(sqReader.GetOrdinal("recharge_before"));
            item.after_desc = sqReader.GetString(sqReader.GetOrdinal("recharge_after"));  
            
            table.Add(item.id, item);
        }
        return table;
    }
    //充值
    public Hashtable QueryVipTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_recharge_vip_reward");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            VipData item = new VipData();
            item.vip_level = sqReader.GetInt32(sqReader.GetOrdinal("vip"));
            item.diamond_count = sqReader.GetInt32(sqReader.GetOrdinal("diamond"));
            item.diamond_show = sqReader.GetInt32(sqReader.GetOrdinal("purchasable_diamond"));
            item.diamond = sqReader.GetInt32(sqReader.GetOrdinal("diamond"));
            item.coin_up = sqReader.GetInt32(sqReader.GetOrdinal("gold_up_p"));
            item.exp_up = sqReader.GetInt32(sqReader.GetOrdinal("exp_up_p"));
            item.coin_max = sqReader.GetInt32(sqReader.GetOrdinal("gold_max_up_p"));
            item.exp_max = sqReader.GetInt32(sqReader.GetOrdinal("exp_max_up_p"));
            item.equipment_enhance_success_rate_up_p = sqReader.GetInt32(sqReader.GetOrdinal("equipment_enhance_success_rate_up_p"));
            item.prop_refresh_reduction_p = sqReader.GetInt32(sqReader.GetOrdinal("prop_refresh_reduction_p"));
            item.arena_frozen_time_reduction_p = sqReader.GetInt32(sqReader.GetOrdinal("arena_frozen_time_reduction_p"));
            item.purchase_hp_count = sqReader.GetInt32(sqReader.GetOrdinal("purchase_hp_count"));
            item.SCHOOL_reset_count = sqReader.GetInt32(sqReader.GetOrdinal("SCHOOL_reset_count")); 
            item.swared = sqReader.GetString(sqReader.GetOrdinal("rewared"));
            item.gift_swared = sqReader.GetString(sqReader.GetOrdinal("purchasable_gift"));
            item.store_refresh = sqReader.GetInt32(sqReader.GetOrdinal("store_refresh_count_max"));
            item.vip_attrdesc = sqReader.GetString(sqReader.GetOrdinal("vip_desc"));
            table.Add(item.vip_level, item);
        }
        return table;
    }
    //抽奖
    public Hashtable QueryLotteryTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lottery");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            LotteryData item = new LotteryData();
            item.id = sqReader.GetInt32(sqReader.GetOrdinal("id"));
            item.type = (LotteryData.MoneyType)sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.money = sqReader.GetInt32(sqReader.GetOrdinal("money"));
            item.cd = sqReader.GetInt32(sqReader.GetOrdinal("cd")); 
            table.Add(item.id, item);
        }
        return table;
    }
    //用户
    public Hashtable QueryUserLevelTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_user_level");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            UserLevelData item = new UserLevelData();
            item.level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            item.exp = sqReader.GetInt32(sqReader.GetOrdinal("exp"));
            item.attrArr[(int)Def.AttrType.FightPower] = sqReader.GetInt32(sqReader.GetOrdinal("combat"));
            item.attrArr[(int)Def.AttrType.Defense] = sqReader.GetInt32(sqReader.GetOrdinal("defense"));
            item.attrArr[(int)Def.AttrType.Crit] = sqReader.GetInt32(sqReader.GetOrdinal("critical_hit"));
            item.attrArr[(int)Def.AttrType.Pray] = sqReader.GetInt32(sqReader.GetOrdinal("skill"));
            table.Add(item.level, item);
        }
        return table;
    }
    //角色 
    public Hashtable QueryBuffTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("buff");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            BuffData item = new BuffData();
            item.buffid = sqReader.GetInt32(sqReader.GetOrdinal("buffer_id"));
            item.id1 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id1"));
            item.v1 = sqReader.GetFloat(sqReader.GetOrdinal("value1"));
            item.id2 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id2"));
            item.v2 = sqReader.GetFloat(sqReader.GetOrdinal("value2"));
            item.id3 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id3"));
            item.v3 = sqReader.GetFloat(sqReader.GetOrdinal("value3"));
            item.id4 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id4"));
            item.v4 = sqReader.GetFloat(sqReader.GetOrdinal("value4"));
            item.id5 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id5"));
            item.v5 = sqReader.GetFloat(sqReader.GetOrdinal("value5"));
            item.id6 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id6"));
            item.v6 = sqReader.GetFloat(sqReader.GetOrdinal("value6"));
            item.id7 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id7"));
            item.v7 = sqReader.GetFloat(sqReader.GetOrdinal("value7"));
            item.id8 = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("property_id8"));
            item.v8 = sqReader.GetFloat(sqReader.GetOrdinal("value8"));
            item.GetBuffAttr();
            table.Add(item.buffid, item);
        }
        return table;
    }
 
    //拳法
    public Hashtable QueryBoxingLevelTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_kungfu");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            BoxingLevelData item = new BoxingLevelData();
            item.g_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("g_csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.skill_level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            item.skill_icon = sqReader.GetString(sqReader.GetOrdinal("icon"));
            item.skill_desc = sqReader.GetString(sqReader.GetOrdinal("desc"));
            item.skill_effect = sqReader.GetString(sqReader.GetOrdinal("effect"));
            item.skill_type = (Def.SkillType)sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.skill_hurt = (Def.HurtType)sqReader.GetInt32(sqReader.GetOrdinal("harm_type"));
            item.trigger_pre = sqReader.GetInt32(sqReader.GetOrdinal("arise_probability"));
            item.trigger_num = sqReader.GetInt32(sqReader.GetOrdinal("arise_count"));
            item.trigger_type = (Def.TriggerType)sqReader.GetInt32(sqReader.GetOrdinal("arise_type"));
            item.trigger_arg = sqReader.GetInt32(sqReader.GetOrdinal("arise_param"));
            item.formula_type = (Def.FormulaType)sqReader.GetInt32(sqReader.GetOrdinal("attack_type"));
            item.effect_pre = sqReader.GetInt32(sqReader.GetOrdinal("effect_pre"));
            item.add_effect_type = (Def.AddEffectType)sqReader.GetInt32(sqReader.GetOrdinal("attch_type"));
            item.add_state_pre = sqReader.GetInt32(sqReader.GetOrdinal("attch_state"));
            item.buff_id = sqReader.GetInt32(sqReader.GetOrdinal("buff_id")); 
            item.item_id = sqReader.GetInt32(sqReader.GetOrdinal("prop_csv_id"));
            item.item_num = sqReader.GetInt32(sqReader.GetOrdinal("prop_num"));
            item.coin_type = sqReader.GetInt32(sqReader.GetOrdinal("currency_type"));
            item.coin = sqReader.GetInt32(sqReader.GetOrdinal("currency_num"));
            item.equip_buff_id = sqReader.GetInt32(sqReader.GetOrdinal("quip_buff_id"));
            table.Add(item.g_csv_id, item);
        }
        return table;
    }

    public Hashtable QueryBoxingTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("boxing");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            BoxingData item = new BoxingData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.comm = sqReader.GetInt32(sqReader.GetOrdinal("comm"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    //装备
    public Hashtable QueryEquipmentXiTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("equipment_xi");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            EquipmentXiData item = new EquipmentXiData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.currency_type = sqReader.GetInt32(sqReader.GetOrdinal("currency_type"));
            item.currency_num = sqReader.GetInt32(sqReader.GetOrdinal("currency_num"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Hashtable QueryEquipmentKitTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("equipment_kit");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            EquipmentKitData item = new EquipmentKitData();
            item.effect = sqReader.GetInt32(sqReader.GetOrdinal("effect"));
            item.level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            table.Add(item.level, item);
        }
        return table;
    }

    public List<EquipmentKitData> QueryEquipmentKitList()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("equipment_kit");
        List<EquipmentKitData> list = new List<EquipmentKitData>();
        while (sqReader.Read())
        {
            EquipmentKitData item = new EquipmentKitData();
            item.effect = sqReader.GetInt32(sqReader.GetOrdinal("effect"));
            item.level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            list.Add(item);
        }
        return list;
    }

    public Hashtable QueryEquipmentIntensifyTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ExecuteQuery("select * FROM g_equipment_enhance INNER JOIN item_icon ON g_equipment_enhance.icon_id=item_icon.id");
        //SqliteDataReader sqReader = db.ReadFullTable("g_equipment_enhance");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            EquipLevelData item = new EquipLevelData();
            item.g_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("g_csv_id"));
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.icon_id = sqReader.GetInt32(sqReader.GetOrdinal("icon_id"));
            item.path = sqReader.GetString(sqReader.GetOrdinal("path"));
            item.attrArr[(int)Def.AttrType.FightPower] = sqReader.GetInt32(sqReader.GetOrdinal("combat"));
            item.attrArr[(int)Def.AttrType.Defense] = sqReader.GetInt32(sqReader.GetOrdinal("defense"));
            item.attrArr[(int)Def.AttrType.Crit] = sqReader.GetInt32(sqReader.GetOrdinal("critical_hit"));
            item.attrArr[(int)Def.AttrType.Pray] = sqReader.GetInt32(sqReader.GetOrdinal("king"));

            item.additionArr[(int)Def.AttrType.FightPower] = sqReader.GetInt32(sqReader.GetOrdinal("combat_probability"));
            item.additionArr[(int)Def.AttrType.Defense] = sqReader.GetInt32(sqReader.GetOrdinal("defense_probability"));
            item.additionArr[(int)Def.AttrType.Crit] = sqReader.GetInt32(sqReader.GetOrdinal("critical_hit_probability"));
            item.additionArr[(int)Def.AttrType.Pray] = sqReader.GetInt32(sqReader.GetOrdinal("king_probability")); 

            item.enhance_success_rate = sqReader.GetInt32(sqReader.GetOrdinal("enhance_success_rate"));
            item.currency_type = sqReader.GetInt32(sqReader.GetOrdinal("currency_type")); 
            item.currency_num = sqReader.GetInt32(sqReader.GetOrdinal("currency_num"));
            table.Add(item.g_csv_id, item);
        }
        return table;
    }

    public Dictionary<int, RoleStarData> QueryRoleFragmentTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ExecuteQuery("SELECT * FROM g_role JOIN g_role_star ON g_role.us_prop_csv_id=g_role_star.us_prop_csv_id WHERE g_role.star == g_role_star.star");
        //SqliteDataReader sqReader = db.ReadFullTable("g_equipment_enhance");
        Dictionary<int, RoleStarData> table = new Dictionary<int, RoleStarData>();
        while (sqReader.Read())
        { 
            RoleStarData role = new RoleStarData();
            role.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            role.g_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("g_csv_id"));
            role.us_prop_num = sqReader.GetInt32(sqReader.GetOrdinal("us_prop_num"));
            role.us_prop_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("us_prop_csv_id"));
            role.star_init = sqReader.GetInt32(sqReader.GetOrdinal("star_init"));
            role.sharp = sqReader.GetInt32(sqReader.GetOrdinal("sharp"));
            role.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            role.anim = sqReader.GetString(sqReader.GetOrdinal("anim"));
            role.strs = sqReader.GetString(sqReader.GetOrdinal("strs"));
            role.skill_csv_id = sqReader.GetInt32(sqReader.GetOrdinal("skill_csv_id"));
            role.gather_buffer_id = sqReader.GetInt32(sqReader.GetOrdinal("gather_buffer_id"));
            role.battle_buffer_id = sqReader.GetInt32(sqReader.GetOrdinal("battle_buffer_id"));
            table.Add(role.us_prop_csv_id, role);
        }
        return table;
    }

    public Hashtable QueryRefreshCostTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_refresh_cost");
        //SqliteDataReader sqReader = db.ReadFullTable("g_equipment_enhance");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            RefreshCostData item = new RefreshCostData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.currency_num = sqReader.GetInt32(sqReader.GetOrdinal("currency_num"));
            item.currency_type = sqReader.GetInt32(sqReader.GetOrdinal("currency_type"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Hashtable QueryEquipmentTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_equipment");
        Hashtable table = new Hashtable();
        while (sqReader.Read())
        {
            EquipData item = new EquipData(); 
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<Def.AttrId, XiLianMaxData> QueryXiLianMaxTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("xilian_max");
        Dictionary<Def.AttrId, XiLianMaxData> table = new Dictionary<Def.AttrId, XiLianMaxData>();
        while (sqReader.Read())
        {
            XiLianMaxData item = new XiLianMaxData();
            item.type = (Def.AttrId)sqReader.GetInt32(sqReader.GetOrdinal("attrid"));
            item.min = sqReader.GetInt32(sqReader.GetOrdinal("min"));
            item.max = sqReader.GetInt32(sqReader.GetOrdinal("max"));
            table.Add(item.type, item);
        }
        return table;
    }

    public Dictionary<int, XiLianConditionData> QueryXiLianConditionTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("xilian_condition");
        Dictionary<int, XiLianConditionData> table = new Dictionary<int, XiLianConditionData>();
        while (sqReader.Read())
        {
            XiLianConditionData item = new XiLianConditionData();
            item.count = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.str = sqReader.GetString(sqReader.GetOrdinal("condition"));
            table.Add(item.count, item);
        }
        return table;
    } 

    public ConfigData QueryConfigData()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("config");
        Dictionary<int, XiLianConditionData> table = new Dictionary<int, XiLianConditionData>();
        ConfigData item = new ConfigData();
        while (sqReader.Read())
        { 
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.user_level_max = sqReader.GetInt32(sqReader.GetOrdinal("user_level_max"));
            item.user_vip_max = sqReader.GetInt32(sqReader.GetOrdinal("user_vip_max")); 
            item.xilian_level_open = sqReader.GetInt32(sqReader.GetOrdinal("xilian_level_open")); 
        }
        return item;
    }

    public Dictionary<int, LevelData> QueryLevelTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_level");
        Dictionary<int, LevelData> table = new Dictionary<int, LevelData>(); 
        while (sqReader.Read())
        {
            LevelData item = new LevelData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            
            item.chapter = sqReader.GetInt32(sqReader.GetOrdinal("chapter"));
            item.combat = sqReader.GetInt32(sqReader.GetOrdinal("combat"));
            item.level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.checkpoint = sqReader.GetInt32(sqReader.GetOrdinal("checkpoint"));
            item.type = (Def.levelType)sqReader.GetInt32(sqReader.GetOrdinal("type"));
            item.cd = sqReader.GetInt32(sqReader.GetOrdinal("cd"));

            item.gain_gold = sqReader.GetInt32(sqReader.GetOrdinal("gain_gold"));
            item.gain_exp = sqReader.GetInt32(sqReader.GetOrdinal("gain_exp"));
            item.drop = sqReader.GetInt32(sqReader.GetOrdinal("drop"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward")); 
            item.monster_csv_id1 = sqReader.GetInt32(sqReader.GetOrdinal("monster_csv_id1"));
            item.monster_csv_id2 = sqReader.GetInt32(sqReader.GetOrdinal("monster_csv_id2"));
            item.monster_csv_id3 = sqReader.GetInt32(sqReader.GetOrdinal("monster_csv_id3"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, MonsterData> QueryMonsterTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_monster");
        Dictionary<int, MonsterData> table = new Dictionary<int, MonsterData>(); 
        while (sqReader.Read())
        {
            MonsterData item = new MonsterData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id")); 
            item.combat = sqReader.GetInt32(sqReader.GetOrdinal("combat"));
            item.defense = sqReader.GetInt32(sqReader.GetOrdinal("defense"));
            item.critical_hit = sqReader.GetInt32(sqReader.GetOrdinal("critical_hit"));
            item.blessing = sqReader.GetInt32(sqReader.GetOrdinal("blessing")); 
            item.boxing_id = sqReader.GetString(sqReader.GetOrdinal("boxing_id"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, LiLianLevelData> QueryLiLianLevelTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lilian_level");
        Dictionary<int, LiLianLevelData> table = new Dictionary<int, LiLianLevelData>();
        while (sqReader.Read())
        {
            LiLianLevelData item = new LiLianLevelData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.phy_pwoer = sqReader.GetInt32(sqReader.GetOrdinal("phy_pwoer"));
            item.experience = sqReader.GetInt32(sqReader.GetOrdinal("experience"));
            item.queue = sqReader.GetInt32(sqReader.GetOrdinal("queue"));
            item.dec_lilian_time = sqReader.GetInt32(sqReader.GetOrdinal("dec_lilian_time"));
            item.dec_weikun_time = sqReader.GetInt32(sqReader.GetOrdinal("dec_weikun_time"));
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, LiLianCardData> QueryLiLianCardTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lilian_card");
        Dictionary<int, LiLianCardData> table = new Dictionary<int, LiLianCardData>();
        while (sqReader.Read())
        {
            LiLianCardData item = new LiLianCardData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, LiLianHallData> QueryLiLianHallTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lilian_kungfu");
        Dictionary<int, LiLianHallData> table = new Dictionary<int, LiLianHallData>();
        while (sqReader.Read())
        {
            LiLianHallData item = new LiLianHallData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.belong_zone = sqReader.GetInt32(sqReader.GetOrdinal("belong_zone"));
            item.open_level = sqReader.GetInt32(sqReader.GetOrdinal("open_level"));
            item.time = sqReader.GetInt32(sqReader.GetOrdinal("time"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward"));
            item.day_finish_time = sqReader.GetInt32(sqReader.GetOrdinal("day_finish_time"));
            item.need_phy_power = sqReader.GetInt32(sqReader.GetOrdinal("need_phy_power"));
            item.reward_exp = sqReader.GetInt32(sqReader.GetOrdinal("reward_exp"));
            item.trigger_event_prop = sqReader.GetInt32(sqReader.GetOrdinal("trigger_event_prop"));
            item.trigger_event = sqReader.GetString(sqReader.GetOrdinal("trigger_event")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, LiLianEventData> QueryLiLianEventTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lilian_event");
        Dictionary<int, LiLianEventData> table = new Dictionary<int, LiLianEventData>();
        while (sqReader.Read())
        {
            LiLianEventData item = new LiLianEventData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.cd_time = sqReader.GetInt32(sqReader.GetOrdinal("cd_time"));
            item.description = sqReader.GetString(sqReader.GetOrdinal("description")); 
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, ChapterData> QueryChapterTable()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("chapter");
        Dictionary<int, ChapterData> table = new Dictionary<int, ChapterData>();
        while (sqReader.Read())
        {
            ChapterData item = new ChapterData();
            item.csv_id = sqReader.GetInt32(sqReader.GetOrdinal("csv_id"));
            item.name = sqReader.GetString(sqReader.GetOrdinal("name"));
            item.bg = sqReader.GetString(sqReader.GetOrdinal("bg"));
            item.typeMax[(int)Def.levelType.Normal] = sqReader.GetInt32(sqReader.GetOrdinal("type0_max"));
            item.typeMax[(int)Def.levelType.Hard] = sqReader.GetInt32(sqReader.GetOrdinal("type1_max"));
            item.typeMax[(int)Def.levelType.Hell] = sqReader.GetInt32(sqReader.GetOrdinal("type2_max"));
            item.level = sqReader.GetInt32(sqReader.GetOrdinal("level"));
            item.power = sqReader.GetInt32(sqReader.GetOrdinal("combat")); 
            table.Add(item.csv_id, item);
        }
        return table;
    }

    public Dictionary<int, LiLianStrengthData> QueryLiLianStrength()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("lilian_Strength");
        Dictionary<int, LiLianStrengthData> table = new Dictionary<int, LiLianStrengthData>();
        while (sqReader.Read())
        {
            LiLianStrengthData item = new LiLianStrengthData();
            item.count = sqReader.GetInt32(sqReader.GetOrdinal("count"));
            item.num = sqReader.GetInt32(sqReader.GetOrdinal("num"));
            item.diamond = sqReader.GetInt32(sqReader.GetOrdinal("diamond"));
            table.Add(item.count, item);
        }
        return table;
    }


    public Dictionary<int, AranePointRewardData> QueryPointReward()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_arane_point_reward");
        Dictionary<int, AranePointRewardData> table = new Dictionary<int, AranePointRewardData>();
        while (sqReader.Read())
        {
            AranePointRewardData item = new AranePointRewardData();
            item.point = sqReader.GetInt32(sqReader.GetOrdinal("point"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward")); 
            table.Add(item.point, item);
        }
        return table;
    }

    public Dictionary<int, AraneRankRewardData> QueryRankReward()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_arane_rank_reward");
        Dictionary<int, AraneRankRewardData> table = new Dictionary<int, AraneRankRewardData>();
        while (sqReader.Read())
        {
            AraneRankRewardData item = new AraneRankRewardData();
            item.rank = sqReader.GetInt32(sqReader.GetOrdinal("rank"));
            item.reward = sqReader.GetString(sqReader.GetOrdinal("reward"));
            table.Add(item.rank, item);
        }
        return table;
    }

    public Dictionary<int, AraneRefreshData> QueryAraneRefresh()
    {
        InitDB();
        SqliteDataReader sqReader = db.ReadFullTable("g_arane_refresh");
        Dictionary<int, AraneRefreshData> table = new Dictionary<int, AraneRefreshData>();
        while (sqReader.Read())
        {
            AraneRefreshData item = new AraneRefreshData();
            item.num = sqReader.GetInt32(sqReader.GetOrdinal("num"));
            item.buy_count = sqReader.GetInt32(sqReader.GetOrdinal("buy_count"));
            item.refresh_spend = sqReader.GetInt32(sqReader.GetOrdinal("refresh_spend"));
            table.Add(item.num, item);
        }
        return table;
    }


}
