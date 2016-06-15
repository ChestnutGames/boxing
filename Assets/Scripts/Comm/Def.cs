using UnityEngine;
using System.Collections;


//定义战斗中用到的各模块属性的类型枚举
public class Def  {

    public static int BattleSendTime =4;
    //服务器IP
    public static string IP = "192.168.1.116";
    //public static string IP = "192.168.228.133";

   // public static string IP = "192.168.1.239";

    //战斗中谁死了
    public enum BattleDeadType
    {
        None = 0,
        User = 1,
        Emeny = 2
    }

    public enum KFID
    {
        Normal= 90000,
        Combo = 100000
    }
    
    //攻击方式（拳、组合、普通）
    public enum BattleAttackType
    {
        Boxing =1,
        Combo = 2,
        Normal =3
    }
   
    public enum MsgStyle
    {
        Yes = 0,
        YesAndNo = 1,
        YesAndNoAndCancel = 2
    }

    public delegate void MessageBoxResultDelegate(MsgResult result);

    public enum MsgResult
    {
        Yes = 0,
        No = 1,
        Cancel = 2
    }

    public enum LogicActionDefine
    {
        LevelFightBegin,
        ArenaFightList,
        ArenaBattleRoleList,
        SingleLevelFightBegin,
        AraConvertPts,
        LevelFightList,
        Worship,
        ArenaEnter,
        ArenaRewardCollected,//竞技场积分
        ArenaListRefresh,
        ArenaBuyRefresh,  //购买刷新
        ArenaPointList,     //获得竞技场积分
        ArenaSwaredList,     //获得竞技场领奖
        ArenaBattleEnter,      //进入竞技场核心战斗
        ArenaBattleOver,              //核心竞技场战斗结束
        LevelBattleEnter,      //进入过关核心战斗
        LevelBattleOver,                //核心过关战斗结束 
        ArenaRankList
    }
    //战斗类型（自动或手动控制）
    public enum FightType
    {
        Auto = 1,
        Manually = 2
    }

   //历练的类型
    public enum LiLianType
    {
        Lilian =1,
        Event = 2,
        LiLianFinish = 3,
        EventFinish = 4
    }
    //关卡难度级别
    public enum levelType
    {
        Normal = 0,
        Hard = 1,
        Hell = 2,
    }

    //游戏结束原因
    public enum BattleOverType
    {
        Fail = 0,
        Success = 1
    }

    public enum CurrencyType
    {
        Diamond = 1,
        Gold = 2,
        Exp = 3,
        Heart = 4
    }

    public enum AttrId
    {
        FightPower = 1,
        Defense = 2,
        Crit = 3,
        Pray = 4,
        FightPowerAdd = 5,
        DefenseAdd = 6,
        CritAdd = 7,
        PrayAdd = 8,
    }
    //查 属性数组用
    public enum AttrType
    {
        None = 0,
        FightPower = 1,
        Defense = 2,
        Crit = 3,
        Pray = 4, 
    }
    public enum ItemUseType
    {
        None = 0,
        Boxing = 1,
        Fashion = 2,
        Equipment = 3,
        Invitation = 6
    }
    

    public enum ItemType
    {
        None = 0,
        Boxing = 1,
        Fashion = 2,
        Equipment = 3
    }

    public enum SkillType
    {
        Active = 1,
        Passive = 2
    }

    public enum HurtType
    {
        None = 0,
        Normal = 1,
        Crit = 2,
    }

    public enum TriggerType
    {
        None = 0,
        User = 1,
        Emeny = 2,
        Attack = 3
    }

    public enum FormulaType
    {
        None = 0,
        Normal = 1,
        Crit = 2,
        Combo = 4,
    }

    public enum AddEffectType
    {
        None = 0,
        Seal = 1,
        AbsorbHit = 2,
        AbsorbMeFight = 3,
        Rebound = 4,
        HitMeFight = 5,
        AbsorbEmenyFight = 6
    }

    public static int port = 3301;

    public const string Lua_Achievement = "lua/AchievementVM.lua";
    public const string Lua_Email = "lua/AchievementVM.lua";
    public const string Lua_Store = "lua/AchievementVM.lua";
    public const string Lua_Daily = "lua/AchievementVM.lua";
    public const string Lua_Friend = "lua/AchievementVM.lua";
    public const string Lua_Bag = "lua/AchievementVM.lua";
    public const string Lua_Recharge = "lua/AchievementVM.lua";
    public const string Lua_Lottery = "lua/AchievementVM.lua";
    public const string Lua_Role = "lua/AchievementVM.lua";
    public const string Lua_Billing = "lua/AchievementVM.lua";
    public const string Lua_UserInfo = "lua/userinfo.lua";
    public const string Lua_Roles = "lua/roles.lua";
    public const string Lua_Boxing = "lua/boxing.lua";
    public const string Lua_Equip = "lua/equip.lua";


    public const int ItemMax = 99;
    public  const string Cours = "Cours"; 
    public  const string Box = "Box";
    public  const string DragBox = "DragBox";


    public  const string SortRole = "Role";
    public  const string SortEmeny = "Emeny";


    public const string BoxingSave = "BoxingSave";

    public const string NoneSprite = "sefq1";


    public const int HangingRefreshTime = 360;
    public const int WakeLevelMax = 5;
    public const int BoxLevelMax = 9;
    public const int RecHeart = 10;
    public const int EquipLevelMax = 50; 
        
    public const int ModifyNameDimand = 100;

    public const int MaxUserItem = 4;
    

    public const string DiamondTex = "D";
    public const string CoinTex = "金币";


    public const int Er_NotFindFrind = 64;
    public const int Er_FailEquip = 24;
    public const int Er_Scuesss = 1;

    public const string LotteryAmin = "SpineData/Effect/getcat/New SkeletonData";
    public const string WakeUpAmin = "SpineData/Effect/catRankup/New SkeletonData"; 
    public const string UpgradeAmin = "SpineData/Effect/levelup/New SkeletonData";
    public const string IntensifyFailAmin = "SpineData/Effect/qhsbg/New SkeletonData";
    public const string IntensifySuccessAmin = "SpineData/Effect/qhcg/New SkeletonData";


    public const string RoleStanby = "standby";


      
}
