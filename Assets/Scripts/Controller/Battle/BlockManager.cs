using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlockManager : MonoBehaviour 
{

    public const int GroundClickMax = 3;

    enum CycleType
    {
        Once = 0,
        Loop = 1

    } 
    

    //保存线程信息
    class TimerThearData
    {
        public List<float> curTime;
        public List<float> createTime;
        public List<float> totalCreateTime;
        public List<float> curMakeCount;
        public List<float> curMakeBoxCount; 
        public List<bool> isRun;
        public List<float> startTime;
        public List<float> endTime;

        public CycleType type; 

        public void Reset(List<RoleData.BoxAttr> data)
        {
            curTime = new List<float>();
            createTime = new List<float>();
            totalCreateTime = new List<float>();
            curMakeCount = new List<float>();
            curMakeBoxCount = new List<float>();
            isRun = new List<bool>();
            startTime = new List<float>();
            endTime = new List<float>();
            type = CycleType.Loop;
            for (int i = 0; i < data.Count; i++)
            {
                curTime.Add(0);
                createTime.Add(0);
                totalCreateTime.Add(0);
                curMakeCount.Add(0);
                curMakeBoxCount.Add(0);
                isRun.Add(false);
                startTime.Add(data[i].timeMin);
                endTime.Add(data[i].timeMax); 
            } 
        }
    }
     
     
    //在数组中位置
    public const int Blue = 0;
    public const int Yellow = 1;
    public const int Red = 2;
    public const int Ground = 3;
    public const int Bomb = 4;
    public const int Fast = 5;
    public const int Combo = 6;

    public const int MoveList = 0;
    public const int StaticList = 1;
    public const int SkillList = 2; 

	public GameObject[] prefabs; 

    TimerThearData[] createTimer;
      


    // 10个区间 方块的个数
    public int[] blockCount;
    public float[] blockSize;

    public int level;//难度控制
     

    public GameObject centerPos;
      
    private bool _isSpawning;
	public bool IsSpawning { get { return _isSpawning; } }
     
	private float _time;


    public float eventAKeepTime;


    private int yellowBoxIndex;
    private int blueBoxIndex;
    private int redBoxIndex; 
    private int comboBoxIndex;
    private int bombBoxIndex;
    private int fastBoxIndex;
    private int groundBoxIndex;
    private int bossBoxIndex;

    public int attackBoxCount;
    public int moveBoxCount;
    public int clickCount;

    public int yellowBoxCount;
    public int blueBoxCount;
    public int redBoxCount;
    public int comboBoxCount;
    public int bombBoxCount;
    public int fastBoxCount;
    public int groundBoxCount;
    public int bossBoxCount;

    private float centerY;

	public List<GameObject> _yellowObjects = new List<GameObject>();
    public List<GameObject> _blueObjects = new List<GameObject>();
    public List<GameObject> _redObjects = new List<GameObject>();
    public List<GameObject> _comboObjects = new List<GameObject>();
    public List<GameObject> _bombObjects = new List<GameObject>();
    public List<GameObject> _fastObjects = new List<GameObject>();
    public List<GameObject> _groundObjects = new List<GameObject>();
    public List<GameObject> _bossObjects = new List<GameObject>();


	private Action<GameObject> _onSpawnAction;
     
    public float timeScle = 1;
       

    RoleData role;
    RoleData emeny; 

    RoleData.CycleAttr staticAttr; 
    RoleData.CycleAttr moveAttr;
    public RoleData.StaticBoxRule staticMakeData;

    List<RoleData.CycleAttr> staticCycleList; 
    List<RoleData.CycleAttr> moveCycleList;
     


    void ChangeLevel(int i)
    {
        level = i;
        ChangeStaticCycle(i);
        ChangeMoveCycle(i);
        RestSkill();
        RestCreateTimer();
    } 

    void ChangeStaticCycle(int i)
    {
        if (i < staticCycleList.Count)
        {
            staticAttr = staticCycleList[i]; 
            //RestStaticCycle();
        }
    }

    void ChangeMoveCycle(int i)
    {
        if (i < moveCycleList.Count)
        {
            moveAttr = moveCycleList[i];
            RestMoveCycle();
        }
        
    }

    void RestStaticCycle()
    { 
        createTimer[StaticList] = new TimerThearData();
        createTimer[StaticList].Reset(staticAttr.boxList); 
    }

    void RestMoveCycle()
    {
        createTimer[MoveList] = new TimerThearData(); 
        createTimer[MoveList].Reset(moveAttr.boxList); 
    }



	void Start () 
	{ 
        //确定块生成的y轴位置
        centerY = centerPos.transform.position.y; 
	}

 

    public void InitData()
    {
        //todo 手动块生成规则 需要改
        this.staticCycleList = GameShared.Instance.roleData.staticCycleList;
        this.moveCycleList = GameShared.Instance.emenyData.moveCycleList;
        staticMakeData = GameShared.Instance.staticRule; 

        createTimer = new TimerThearData[3]; 
        ChangeLevel(0); 
    }

    //重置生成器
    public void RestCreateTimer()
    {
        for (int typeid = 0; typeid < createTimer.Length; typeid++)
        {
            if (createTimer[typeid] != null)
            {
                for (int thearid = 0; thearid < createTimer[typeid].createTime.Count; thearid++)
                {
                    if (createTimer[typeid].type != CycleType.Once)
                    {
                        RandTotalMake(typeid, thearid);
                    }
                }
            }
        } 
    }
    //生成一开始的静态块 静态块生成起点
    public void OnceStaticMake()
    { 
        for (int thearid = 0; thearid < staticAttr.boxList.Count; thearid++)
        {
            for (int i = 0; i < staticAttr.boxList[thearid].makeCount; i++)
            {
                CreateBox(staticAttr.boxList[thearid].type, staticAttr.boxList[thearid].moveSpeed, staticAttr.boxList[thearid].width);
            } 
        }
        RestOnceStaticThread();
        CheckCycle();
    }
    //生成一个固定的 静态块生成线程
    public void RestOnceStaticThread()
    {
        int rand = UnityEngine.Random.Range(0,100);
        int count = 0;
        if(rand<staticMakeData.makeBoxCountRate[2])
        {
            count = 3;
        }else if(rand<staticMakeData.makeBoxCountRate[1])
        {
            count = 2;
        }else if(rand<staticMakeData.makeBoxCountRate[0])
        {
            count = 1;
        }  
        //生成一个 块生成线程
        RoleData.BoxAttr box = new RoleData.BoxAttr();
        box.makeCount = count;
        box.timeMax = staticMakeData.maxTime;
        box.timeMin = staticMakeData.minTime;  

        List<RoleData.BoxAttr> list = new List<RoleData.BoxAttr>();
        list.Add(box);

        RoleData.CycleAttr cycle = new RoleData.CycleAttr();
        cycle.countMax = staticMakeData.countMax;
        cycle.intervalMin = 0; 
        cycle.boxList = list;
        staticAttr = cycle;  
  
        TimerThearData a = new TimerThearData();
        a.Reset(list); 
        a.isRun[0] = true;
        a.curMakeCount[0] = count;
        a.curMakeBoxCount[0] = count;
        a.totalCreateTime[0] = staticMakeData.countMax;
        a.type = CycleType.Loop;
        createTimer[StaticList] = a;  
         
    }

    void RestSkill()
    {
        RoleData.BoxAttr box = new RoleData.BoxAttr();
        box.makeCount = 1;
        box.timeMin = staticMakeData.skillMin;
        box.timeMax = staticMakeData.skillMax;

        List<RoleData.BoxAttr> list = new List<RoleData.BoxAttr>();
        list.Add(box);

        RoleData.CycleAttr cycle = new RoleData.CycleAttr();
        cycle.countMax = staticMakeData.countMax;
        cycle.intervalMin = 1;
        cycle.boxList = list;
        staticAttr = cycle;

        TimerThearData a = new TimerThearData();
        a.Reset(list);
        a.isRun[0] = true;
        a.curMakeCount[0] = 1;
        a.totalCreateTime[0] = staticMakeData.skillMax;
        a.type = CycleType.Loop;
        createTimer[SkillList] = a;
    }

    public void MakeEventAB()
    {
        if (eventAKeepTime <= 0)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < staticMakeData.eventABRate[0])
            {
                eventAKeepTime = staticMakeData.eventATime;
            }
            else
            {
                EventB();
            }
        }
        else {
            EventA();
        }
    }

    public void EventA()
    { 
       CreateStaticBox(); 
    } 

    public void EventB()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand <staticMakeData.eventBRate)
            CreateStaticBox();
    }

    public void CreateStaticBox()
    { 
       int rand = UnityEngine.Random.Range(0, 100);
       if (rand < staticMakeData.yellowRate)
       { 
           CreateBox(Yellow, 0, 1);
       }
       else
       {
           CreateBox(Blue, 0, 1);
       }
    }


    public void Restart(RoleData r, RoleData e)
    {
        role = r;
        emeny = e;
        yellowBoxIndex = 0;
        blueBoxIndex = 0;
        redBoxIndex = 0;
        comboBoxIndex = 0;
        bombBoxIndex = 0;
        fastBoxIndex = 0;
        groundBoxIndex = 0;
        bossBoxIndex = 0;

        yellowBoxCount = 0;
        blueBoxCount = 0;
        redBoxCount = 0;
        comboBoxCount = 0;
        bombBoxCount = 0;
        fastBoxCount = 0;
        groundBoxCount = 0;
        bossBoxCount = 0;

        attackBoxCount = 0;
        moveBoxCount = 0;
        clickCount = 0;
         
        InitData();

        float size = BattleManager.Instance.pop.rightPos.transform.position.x - BattleManager.Instance.pop.leftPos.transform.position.x;
        float a = size / blockCount.Length;
        for (int j = 0; j < blockCount.Length; j++)
        {
            blockSize[j] = BattleManager.Instance.pop.leftPos.transform.position.x + a*j;
            blockCount[j] = 0;
        } 

        _time = 0;
        timeScle = 1;  
        RemoveAll();
    }
    //击退效果
    public void BoxAttrBackAction()
    { 
        BackMoveList(_redObjects);
        BackMoveList(_bombObjects);
        BackMoveList(_fastObjects);
        BackMoveList(_groundObjects);
        BackMoveList(_bossObjects); 
    }  

    void BackMoveList(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] != null)
            {
                ClickMoveBox move = objects[i].GetComponent<ClickMoveBox>();
                move.GoRepel();
            } 
        } 
    }

    public void StopRed()
    {
        for(int i=0;i<_redObjects.Count;i++)
        {
            _redObjects[i].GetComponent<ClickMoveBox>().StopRed(); 
        } 
        for(int i=0;i<_fastObjects.Count;i++)
        {
            _fastObjects[i].GetComponent<ClickMoveBox>().StopRed(); 
        } 
        for(int i=0;i<_bombObjects.Count;i++)
        {
            _bombObjects[i].GetComponent<ClickMoveBox>().StopRed(); 
        }
            for(int i=0;i<_groundObjects.Count;i++)
        {
            _groundObjects[i].GetComponent<ClickMoveBox>().StopRed(); 
        }   
    }
    public void ResumeRed()
    {
        for (int i = 0; i < _redObjects.Count; i++)
        {
            _redObjects[i].GetComponent<ClickMoveBox>().ResumeRed();
        }
        for (int i = 0; i < _fastObjects.Count; i++)
        {
            _fastObjects[i].GetComponent<ClickMoveBox>().ResumeRed();
        }
        for (int i = 0; i < _bombObjects.Count; i++)
        {
            _bombObjects[i].GetComponent<ClickMoveBox>().ResumeRed();
        }
        for (int i = 0; i < _groundObjects.Count; i++)
        {
            _groundObjects[i].GetComponent<ClickMoveBox>().ResumeRed();
        }  
    }
      
	void Update () 
	{
		if (!_isSpawning) 
		{
			return;
		}
        

        if (BattleManager.Instance.isBattle)
        {

            _time += Time.deltaTime;
            float a = _time / BomobMakeMax; 
            eventAKeepTime -=Time.deltaTime;
            //用来初始化循环和生成第一批静态块
            if (level == 0)
            {
                this.OnceStaticMake();
            }

            //保证 己方方块最小值
            if (attackBoxCount < staticMakeData.countMin)
            {
               CreateStaticBox();
            } 

            for (int typeid = 0; typeid < createTimer.Length; typeid++)
            { 
                for (int thearid = 0; thearid < createTimer[typeid].createTime.Count; thearid++)
                { 
                    float temp = (createTimer[typeid].curTime[thearid] * timeScle) - (createTimer[typeid].createTime[thearid] * timeScle);

                    createTimer[typeid].totalCreateTime[thearid] -= Time.deltaTime;

                    if (_time > createTimer[typeid].startTime[thearid] || CycleType.Loop == createTimer[typeid].type)
                    { 
                        createTimer[typeid].curTime[thearid] += Time.deltaTime; 
                        createTimer[typeid].curMakeBoxCount[thearid]--;
                        

                        //生成 如果挂起就不生成
                        if (createTimer[typeid].isRun[thearid] != false && temp > 0 && createTimer[typeid].type != CycleType.Once)
                        {
                            if (typeid == MoveList)
                            {
                                CreateBox(moveAttr.boxList[thearid].type, moveAttr.boxList[thearid].moveSpeed, moveAttr.boxList[thearid].width);
                            }
                            else if (typeid == StaticList)
                            {
                                //如果是 固定循环就执行随机类型创建
                                if (CycleType.Loop == createTimer[typeid].type)
                                {
                                    CreateStaticBox();
                                }
                                else
                                {
                                    CreateBox(staticAttr.boxList[thearid].type, staticAttr.boxList[thearid].moveSpeed, staticAttr.boxList[thearid].width);
                                }
                            }
                            else if (typeid == SkillList)
                            {
                                //如果是 固定循环就执行随机类型创建 
                                CreateBox(Combo, 0, 0);
                            }
                            //如果生成完成就 挂起  如果==0 执行到这就说明生成了最后一个
                            if (createTimer[typeid].curMakeBoxCount[thearid] <= 0 && createTimer[typeid].type != CycleType.Once)
                            { 
                                createTimer[typeid].isRun[thearid] = false; 
                            }
                            
                        } 
                        if (createTimer[typeid].curMakeCount[thearid] > 0 && temp > 0 && createTimer[typeid].type != CycleType.Once)
                        {
                            RandMake(typeid, thearid);
                        }
                        //if (typeid == 1 && thearid == 0)
                        //{
                        //   Debug.Log(createTimer[typeid].isRun[thearid]);
                        //}

                        //重置生成时间
                        if (createTimer[typeid].totalCreateTime[thearid] <= 0 && createTimer[typeid].type != CycleType.Once)
                        { 
                            RandTotalMake(typeid, thearid);
                        }
                        
                    }
                     
                }
            }
        }
	}

    //切换循环判断
    public void CheckCycle()
    {
        if (level == 0)
        {
            level++; 
            //ChangeLevel(level);
        }
 
    }
    
    public void RandMake(int typeid,int thearid)
    { 
        float interval = 0;
        if (typeid == StaticList)//静态块
        {
            interval = staticAttr.intervalMin; 
        }
        else if(typeid == MoveList)//动态块
        {
            interval = moveAttr.intervalMin; 
        }
        else if (typeid == SkillList)
        {
            interval = 1;
        }
        //的到生成间隔
        float range = createTimer[typeid].totalCreateTime[thearid] - (createTimer[typeid].curMakeCount[thearid] * interval);
        if (range < 0)
        {
            range = 0;
        }
        
        float time = UnityEngine.Random.Range(0, range);
        createTimer[typeid].createTime[thearid] = time + interval; 
        createTimer[typeid].curMakeCount[thearid]--;
        createTimer[typeid].curTime[thearid] = 0;  

        //Debug.Log(" z" + this._time + "块" + createTimer[typeid].totalCreateTime[thearid] + "name" + name + "make" + createTimer[typeid].curMakeCount[thearid]); 
    }

    public void RandTotalMake(int typeid, int thearid)
    {
        //获得生成时间和个数  
        float interval = 0;
        float maxTime = 0;
        if (typeid == StaticList)//静态块
        {
 
            createTimer[typeid].curMakeCount[thearid] = staticAttr.boxList[thearid].makeCount;
            createTimer[typeid].curMakeBoxCount[thearid] = staticAttr.boxList[thearid].makeCount;
            interval = staticAttr.intervalMin;
            maxTime = staticAttr.boxList[thearid].timeMax; 
        }
        else if (typeid == MoveList)//动态块
        {
             
            createTimer[typeid].curMakeCount[thearid] = moveAttr.boxList[thearid].makeCount;
            createTimer[typeid].curMakeBoxCount[thearid] = moveAttr.boxList[thearid].makeCount; 
            interval = moveAttr.intervalMin;
            maxTime = moveAttr.boxList[thearid].timeMax;


        }
        else if (typeid == SkillList)
        {
            createTimer[typeid].curMakeCount[thearid] = 1;
            interval = staticMakeData.skillMin;
            maxTime = staticMakeData.skillMax;
        }
        //如果生成时间不符合 最小间隔 就用最大时间
        if (createTimer[typeid].totalCreateTime[thearid] < interval * createTimer[typeid].curMakeCount[thearid])
        {
            createTimer[typeid].totalCreateTime[thearid] = maxTime;
        } 
        createTimer[typeid].isRun[thearid] = true; 

       // Debug.Log(" z" + this._time + "块" + createTimer[typeid].totalCreateTime[thearid] + "name" + name); 

        RandMake(typeid, thearid);
         
                
            
    }

    public void EnableSpawn(bool enable)
    {
        _isSpawning = enable;
    }

    int BomobMakeMax= 0;
    //生成快初始化
    public void CreateBox(int i, float speed,float width)
    { 
        switch (i)
        {
            case Blue://蓝快 
                if (blueBoxCount < staticAttr.countMax && attackBoxCount < staticAttr.countMax)
                {
                    int index = 0;//prefabs的位置
                    int size = UnityEngine.Random.Range(0, 3);
                    Spawn(prefabs[index + size], ClickBox.BoxType.BlueBox, width); 
                }
                break;
            case Yellow://黄 
                if (yellowBoxCount < staticAttr.countMax && attackBoxCount < staticAttr.countMax)
                {
                    int index2 =3;//prefabs的位置
                    int size2 = UnityEngine.Random.Range(0, 3);
                    Spawn(prefabs[index2 + size2], ClickBox.BoxType.YellowBox, width);
                }
                break; 
            case Red: //红
                if (redBoxCount < moveAttr.countMax && moveBoxCount < moveAttr.countMax)
                {
                    int index3 = 6;
                    int size3 = UnityEngine.Random.Range(0, 2);
                    Spawn(prefabs[index3 + size3], BattleManager.Instance.pop.initMoveBoxPos.transform.position.x,
                        ClickBox.BoxType.RedBox, speed, width, false);
                }
                break;
            case Ground://盾牌 
                if (groundBoxCount < moveAttr.countMax && moveBoxCount < moveAttr.countMax)
                {
                    Spawn(prefabs[8], BattleManager.Instance.pop.initMoveBoxPos.transform.position.x,
                        ClickBox.BoxType.GroundBox, speed, width, false, GroundClickMax);
                }
                break;
            case Bomb://炸弹
                if (bombBoxCount < moveAttr.countMax && moveBoxCount < moveAttr.countMax)
                {
                    Spawn(prefabs[9], BattleManager.Instance.pop.initMoveBoxPos.transform.position.x,
                        ClickBox.BoxType.BombBox, speed, width, false);
                    
                }
                break;
            case Fast: //加速
                if (fastBoxCount < moveAttr.countMax && moveBoxCount < moveAttr.countMax)
                {
                    Spawn(prefabs[10], BattleManager.Instance.pop.initMoveBoxPos.transform.position.x,
                        ClickBox.BoxType.FastBox, speed, width, true);
                    
                }
                break;
            case Combo: //combo
                if (comboBoxCount < 1)
                {
                    clickCount = 0;
                    Spawn(prefabs[11], ClickBox.BoxType.ComboBox, width); 
                }
                break;
            case 7: //boss

                break; 
        } 
    } 
    //检测方块是否重叠
    public float staticBoxMakeCheck(float x, ClickBox box)
    {
        float temp = x;
        float min = blockSize[0];
        float max = 0;
        int index = 0;
        //在哪个位置
        for (int i = 1; i < blockCount.Length-1; i++)
        {
            if (x < blockSize[i])
            {
                max = blockSize[i];
                min = blockSize[i - 1];
                index = i;
                break;
            }
        } 
        if (blockCount[index] > 0)
        {
            //重新找一个位置
            for (int i = index; i < blockCount.Length; i++)
            {
                if (blockCount[i] < 1)
                { 
                    temp = BattleManager.Instance.pop.leftPos.transform.position.x + (i * (max - min)); 
                    index = i;
                    break;
                } 
            }
        }
        else
        {
            temp = x;      
        } 
        blockCount[index]++;
        box.blockIndex = index;
//        Debug.Log("create"+index);
        return temp;
    }

    //移动块
    public void Spawn(GameObject prefab,float initx, ClickBox.BoxType type,float minSpeed,float width,
         bool isAcc = false, int click = 0)
    {
         
       // if (!hasMove) return;
        float y; 

        Vector3 pos = transform.position; 
        y = centerY; 

        GameObject go = Instantiate(prefab) as GameObject;  
        int index = 0;
        int layer = 0;
        switch (type)
        {
            case ClickBox.BoxType.RedBox:
                redBoxIndex++;
                index = redBoxIndex;
                moveBoxCount++;
                redBoxCount++;
                layer = 2000;
                _redObjects.Add(go);
                break;
            case ClickBox.BoxType.GroundBox:
                groundBoxIndex++;
                index = groundBoxIndex;
                moveBoxCount++;
                groundBoxCount++;
                layer = 3000;
                _groundObjects.Add(go);
                break;
            case ClickBox.BoxType.BombBox:
                bombBoxIndex++;
                index = bombBoxIndex;
                moveBoxCount++;
                bombBoxCount++;
                layer = 4000;
                _bombObjects.Add(go);
                break;
            case ClickBox.BoxType.FastBox:
                fastBoxIndex++;
                index = fastBoxIndex;
                moveBoxCount++;
                fastBoxCount++;
                layer = 5000;
                _fastObjects.Add(go);
                break; 
            case ClickBox.BoxType.BossBox:
                bossBoxIndex++;
                index = bossBoxIndex;
                moveBoxCount++;
                bossBoxCount++;
                layer = 6000;
                _bossObjects.Add(go);
                break;
        } 
        ClickMoveBox box = go.GetComponent<ClickMoveBox>(); 
        go.transform.parent = this.transform;
        go.transform.localScale = new Vector3(1, 1, 1);
        //控制在横条内生成
        float half = go.GetComponent<Collider2D>().bounds.size.x / 2;
        float temp = initx - half;
        box.offsetX = half; 
        box.Init(half, layer + index);
        box.InitMoveBox(minSpeed,minSpeed+1, isAcc ,click);
        go.transform.position = new Vector3(temp, y, 0);   
    }
    //固定块
	public void Spawn(GameObject prefab,ClickBox.BoxType type,float width)
	{ 
       // if(!hasStatic) return; 

        float min;
        float max;

        float x;
        float y;

        Vector3 pos = transform.position;

        min = BattleManager.Instance.pop.leftPos.transform.position.x;
        max = BattleManager.Instance.pop.rightPos.transform.position.x; 
            x = Mathf.Lerp(min, max, UnityEngine.Random.value);
            y = centerY;
        

		GameObject go = Instantiate(prefab) as GameObject; 
        int index = 0;
        int layer = 0;
        switch (type)
        { 
            case ClickBox.BoxType.BlueBox:
                blueBoxIndex++;
                index = blueBoxIndex;
                blueBoxCount++;
                attackBoxCount++;
                layer = 100;
                _blueObjects.Add(go);
                break;
            case ClickBox.BoxType.YellowBox:
                yellowBoxIndex++;
                index = yellowBoxIndex;
                yellowBoxCount++;
                attackBoxCount++;
                layer = 1000;
                _yellowObjects.Add(go);
                break;
            case ClickBox.BoxType.ComboBox:
                comboBoxIndex+=3;
                index = comboBoxIndex;
                comboBoxCount++;
                layer = 10;
                _comboObjects.Add(go);
                break; 
        }    
        ClickBox box = go.GetComponent<ClickBox>();
		go.transform.parent = this.transform;
        go.transform.localScale = new Vector3(1, 1, 1);

        x = staticBoxMakeCheck(x, box);
        //控制在横条内生成
        Bounds bound = go.GetComponent<Collider2D>().bounds;
        float half = bound.size.x / 2;
        if (type == ClickBox.BoxType.ComboBox)
            half += half*0.3f;
        float temp = min + half; 
        if (x < temp)
            x = temp;
        temp = max - half;
        if (x > temp)
            x = temp;
        box.Init(half, layer + index);

        if (type == ClickBox.BoxType.ComboBox)
        {
            float tt = max + Math.Abs(min);
            x = min + (tt / 2);
        }
        //技能条重叠解决
        //if (_comboObjects.Count > 0 && type != ClickBox.BoxType.ComboBox)
        //{
        //    foreach (GameObject d in _comboObjects)
        //    {
        //        DragBox drag = d.GetComponent<DragBox>();
        //        float haf = drag.childBox.bounds.size.x/2;
        //        if (drag.childBox.bounds.Intersects(bound))
        //        {
        //            x += drag.childBox.bounds.size.x;
        //        } 
        //    } 
        //} 
        go.transform.position = new Vector3(x, y, 0); 
	}    


    public void Remove(GameObject go)
    {
        
        if (go == null)
        {
            Debug.Log("Remove null");
            return;
        }
        else
        { 
            ClickBox box = go.GetComponent<ClickBox>();
            box.OverAnimPlay();
            ClickBox.BoxType type = box.type;
            //处理点击事件a b  
            if(type == ClickBox.BoxType.YellowBox || type == ClickBox.BoxType.BlueBox)
            { 
                MakeEventAB(); 
                blockCount[box.blockIndex]--;
            } 
            Remove(go, type);
            clickCount++;
        }
    }

    public void Remove(GameObject go,ClickBox.BoxType type)
    { 
   
        ClickBox box = go.GetComponent<ClickBox>();  
        go.SetActive(false);
        switch (type)
        {
            case ClickBox.BoxType.YellowBox:
                yellowBoxCount--;
                attackBoxCount--; 
                _yellowObjects.Remove(go);
                break;
            case ClickBox.BoxType.RedBox:
                redBoxCount--;
                moveBoxCount--;
                _redObjects.Remove(go);
                break;
            case ClickBox.BoxType.BombBox:
                bombBoxCount--;
                moveBoxCount--;
                _bombObjects.Remove(go);
                if (bombBoxCount < 0)
                    bombBoxCount = 0;
                break;
            case ClickBox.BoxType.BlueBox:
                blueBoxCount--;
                attackBoxCount--;
                _blueObjects.Remove(go);
                break;
            case ClickBox.BoxType.ComboBox:
                comboBoxCount--; 
                _comboObjects.Remove(go);
                break;
            case ClickBox.BoxType.BossBox:
                bossBoxCount--;
                moveBoxCount--;
                _bossObjects.Remove(go);
                break;
            case ClickBox.BoxType.FastBox:
                fastBoxCount--;
                moveBoxCount--;
                _fastObjects.Remove(go);
                break;
            case ClickBox.BoxType.GroundBox:
                groundBoxCount--;
                moveBoxCount--;
                _groundObjects.Remove(go);
                break; 
        }
        if (moveBoxCount < 0)
            moveBoxCount = 0;
        Destroy(go);
    }

    public void RemoveAllRed()
    {
        moveBoxCount = 0;
        redBoxCount = 0;
        fastBoxCount = 0;
        bombBoxCount = 0;
        groundBoxCount = 0;
        bossBoxCount = 0;
        foreach (GameObject go in _redObjects)
        {
            Destroy(go);
        } 
        foreach (GameObject go in _bombObjects)
        {
            Destroy(go);
        }
        foreach (GameObject go in _fastObjects)
        {
            Destroy(go);
        }
        foreach (GameObject go in _groundObjects)
        {
            Destroy(go);
        }
        foreach (GameObject go in _bossObjects)
        {
            Destroy(go);
        }
      
        _redObjects.Clear(); 
        _bombObjects.Clear();
        _fastObjects.Clear();
        _groundObjects.Clear();
        _bossObjects.Clear();
    }

	public void RemoveAll()
	{ 
		foreach(GameObject go in _yellowObjects)
		{
			Destroy(go);
		}
        foreach(GameObject go in _blueObjects)
		{
			Destroy(go);
		}
		foreach(GameObject go in _redObjects)
		{
			Destroy(go);
		}
		foreach(GameObject go in _comboObjects)
		{
            
			Destroy(go);
		}
		foreach(GameObject go in _bombObjects)
		{
			Destroy(go);
		}
		foreach(GameObject go in _fastObjects)
		{
			Destroy(go);
		}
		foreach(GameObject go in _groundObjects)
		{
			Destroy(go);
		}
		foreach(GameObject go in _bossObjects)
		{
			Destroy(go);
		} 
	    _yellowObjects.Clear();
        _blueObjects.Clear();
        _redObjects.Clear();
        _comboObjects.Clear();
        _bombObjects.Clear();
        _fastObjects.Clear();
        _groundObjects.Clear();
        _bossObjects.Clear(); 
	}
}
