using System;
using System.Collections;
using System.Collections.Generic;

public class RoleData : RoleAttrs
{

    public RoleData()
    {
        boxingBattleList = new List<BoxingLevelData>();
        boxingList = new  List<BoxingEquipData>();
        xilianList = new List<XiLianData>();
    }

    public List<BoxingLevelData> boxingBattleList ;
    //数据
    #region
    //sql
    
    //初始化觉醒等级
    public int initWakeLevel;
    public SkeletonDataAsset asset;
    //体积
    public int sharp;
    public string name;
    public int us_prop_csv_id; 
    public RoleStarData starData;

    //网络
    public bool is_possessed; 
    public int wakeLevel;
    public int sort; 
    public int frgNum;
    //装备的技能
   

    public string icon;
    public string anim;
    public string qipao;

    public int attackCount;

    //上阵加成
    public float[] battleAttr = new float[Enum.GetNames(typeof(Def.AttrType)).Length]; 
    public float[] battleAddition = new float[Enum.GetNames(typeof(Def.AttrType)).Length];//增益

    public List<XiLianData> xilianList;


    public ItemData.QualityType quality;    
#endregion
      
    //全是角色战斗信息
    #region
    public class CycleAttr
    {
        public int countMax; 
        public float intervalMin; 
        public List<BoxAttr> boxList;
    }

    public class BoxAttr
    {
        public int id;

        public float timeMin;

        public float timeMax;

        public int type;

        public int makeCount;

        public float width;

        public float moveSpeed; 
    }


    public class StaticBoxRule
    {
        public int countMax;
        public int countMin;
        public int yellowRate;
        public int[] eventABRate = new int[2];
        public float eventATime;
        public int eventBRate;
        public float minTime;
        public float maxTime;
        public List<float> makeBoxCountRate;
        public List<float> makeBoxTypeRate;
        public float[] width = new float[3];
        public float fastMaxSpeed;
        public float skillMin;
        public float skillMax;
    } 

    public List<CycleAttr> staticCycleList; 
    public List<CycleAttr> moveCycleList; 
    public List<float> cursorSpeedList; 
    public StaticBoxRule staticRule; 

    public int endWinCount;
    #endregion
}
