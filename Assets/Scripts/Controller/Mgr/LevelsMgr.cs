using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelsMgr : UnitySingleton<LevelsMgr> {
    private PassiveTimer levelTimer;

    public List<LevelData> levelList;

    public LevelView curView;

    public LevelsPop levelpop;
    public int curFousLevel;
    public int levelmax;
    public Def.levelType curType;

    public ChapterData curChapterData;

    public ChapterPop chapterPop;

    public float moveMax;

    public void OpenChapter(ChapterPop p)
    {
        chapterPop = p;
        chapterPop.InitChapter(GameShared.Instance.GetChapterById(UserManager.Instance.curUnLockChapter)); 
    }

    /// <summary>
    /// 获得章节那些解锁
    /// </summary>
    /// <param name="p"></param>
    /// <param name="d"></param>
    public void ChapterList()
    {
        NetworkManager.Instance.CheckPointChapter();
    }

    public void InitChapterAndLevelData(C2sSprotoType.checkpoint_chapter.response resp)
    {
        if (resp.errorcode == 1 && resp.l != null)
        {
            for (int i = 0; i < resp.l.Count; i++)
            {
                Debug.Log("chapter" + "关" + (i + 1) + ":" + resp.l[i].chapter + " chapter_type0 :" + resp.l[i].chapter_type0 + " chapter_type1:" + resp.l[i].chapter_type1);
                ChapterData d = GameShared.Instance.GetChapterById((int)resp.l[i].chapter);
                d.curLevel[(int)Def.levelType.Normal] = (int)resp.l[i].chapter_type0;
                d.curLevel[(int)Def.levelType.Hard] = (int)resp.l[i].chapter_type1;
                d.curLevel[(int)Def.levelType.Hell] = (int)resp.l[i].chapter_type2;
            }
        }
    } 

    public void OpenLevel(LevelsPop p,ChapterData d)
    { 
        levelpop = p;
        InitLevel(d);
    }

    public void InitLevel(ChapterData d)
    {
        curChapterData = d;
        levelpop.SetTab();
        LevelListCallback();
    }
      
    public void LevelListCallback()
    { 
        for (int i = 0; i < 3; i++)
        {
            if (curChapterData.typeMax[i] < 1)
            {
                levelpop.tabList[i].gameObject.SetActive(false);
            } 
        }  
        levelpop.curFousTab = Def.levelType.Hell;
        levelpop.TabChange(Def.levelType.Normal); 
    }

    public List<LevelViewData> GetLevelListByType(Def.levelType type)
    {
        List<LevelViewData> list = new List<LevelViewData>();
        levelList = new List<LevelData>();
        int id = curChapterData.csv_id * 1000;
        id += (int)type * 100;
        levelmax = curChapterData.typeMax[(int)type];
        
        for (int i = 0; i < levelmax; i++)
        {
            LevelViewData v = new LevelViewData(); 
            v.data = GameShared.Instance.GetLevelById(id + i + 1);
            if (v.data.checkpoint <= curChapterData.curLevel[(int)type])
            {
                v.unlock = true;
            }
            else
            {
                v.unlock = false;
            }
            list.Add(v);
            levelList.Add(v.data);
        }
        return list; 
    }
    /// <summary>
    /// 换难度
    /// </summary>
    /// <param name="type"></param>
    public void SelectDifficulty(Def.levelType type)
    {
        curType = type;
        curFousLevel = curChapterData.curLevel[(int)type];
        if (curFousLevel > curChapterData.typeMax[(int)type])
            curFousLevel = curChapterData.typeMax[(int)type];
        levelpop.SetLevelList(GetLevelListByType(type));
        if (UserManager.Instance.curLeveldata == null)//新账号初始化挂机
        {
            curView = levelpop.levelViewList[0];
        }else 
        {
            if (UserManager.Instance.curLeveldata.chapter != curChapterData.csv_id)
            {
                curView = GetLevelView(curFousLevel);
            }
            else
            {
                curView = GetLevelView(UserManager.Instance.curLeveldata.checkpoint);
            }
        } 
        SelectLevel(curView);
        
    }

    public void CloseLevelPop()
    {
        NetworkManager.Instance.CheckPointExit();
    }

    public void CreateLevelResult()
    { 
        MainUI.Instance.LevelResultClick(levelList);
    }

    public void CreateLevelPop(ChapterData data)
    {
        if (1 < data.level)
        {
            ToastManager.Instance.Show("等级不够");
        }
        else if (UserManager.Instance.GetPower() < data.power)
        {
            ToastManager.Instance.Show("战斗力不够");
        }
        else
        {
            MainUI.Instance.levelsClick(data);
        }
    }

    public void EnterBattleLevel()
    {
        NetworkManager.Instance.SingleLevelBattleBegin(UserManager.Instance.curLeveldata.monster_csv_id1); 
    }
    public void EnterBattleLevelCallback()
    { 
        RoleData e = new RoleData();
        e.csv_id = UserManager.Instance.curLeveldata.monster_csv_id1;
        MonsterData d = GameShared.Instance.GetMonsterById(UserManager.Instance.curLeveldata.monster_csv_id1);
        e.attrArr[(int)Def.AttrType.FightPower] = d.combat;
        e.attrArr[(int)Def.AttrType.Defense] = d.defense;
        e.attrArr[(int)Def.AttrType.Crit] = d.critical_hit;
        e.attrArr[(int)Def.AttrType.Pray] = d.blessing;

        List<BoxingLevelData> rl = new List<BoxingLevelData>();
        if (UserManager.Instance.curRole.boxingList != null && UserManager.Instance.curRole.boxingList.Count > 0)
        {
            for (int i = 0; i < UserManager.Instance.curRole.boxingList.Count; i++)
            {
                if (UserManager.Instance.curRole.boxingList[i].viewdata.data.levelData.skill_type == Def.SkillType.Active)
                    rl.Add(UserManager.Instance.curRole.boxingList[i].viewdata.data.levelData);
            }
        }
        UserManager.Instance.curRole.boxingBattleList = rl;


        List<BoxingLevelData> el = new List<BoxingLevelData>();
        List<int> str = GameShared.Instance.GetStr(d.boxing_id);
        for (int i = 0; i < str.Count; i++)
        {
            BoxingLevelData l = GameShared.Instance.GetBoxingLevelById(str[i]);
            if (l.skill_type == Def.SkillType.Active)
                el.Add(l);
        }
        e.boxingBattleList = el;

        //队列
        //BattleManager.Instance.InitLevel(UserManager.Instance.curRole, e, rl, el);
        //一次一请求
        BattleManager.Instance.InitLevel(UserManager.Instance.curRole, e, rl, el);
        UIManager.Instance.uiRoot.SetActive(false);
        Application.LoadLevelAdditive("game");
        //EnterAraneBattleLevelCallback();
    }


    public void EnterAraneBattleLevelCallback()
    { 

        List<RoleData> rllist = new List<RoleData>();
        RoleData e = new RoleData();
        e.csv_id = UserManager.Instance.curLeveldata.monster_csv_id1;
        MonsterData d = GameShared.Instance.GetMonsterById(UserManager.Instance.curLeveldata.monster_csv_id1);
        e.attrArr[(int)Def.AttrType.FightPower] = d.combat;
        e.attrArr[(int)Def.AttrType.Defense] = d.defense;
        e.attrArr[(int)Def.AttrType.Crit] = d.critical_hit;
        e.attrArr[(int)Def.AttrType.Pray] = d.blessing;

        List<BoxingLevelData> rl = new List<BoxingLevelData>();
        if (UserManager.Instance.curRole.boxingList != null && UserManager.Instance.curRole.boxingList.Count > 0)
        {
            for (int i = 0; i < UserManager.Instance.curRole.boxingList.Count; i++)
            {
                if (UserManager.Instance.curRole.boxingList[i].viewdata.data.levelData.skill_type == Def.SkillType.Active)
                    rl.Add(UserManager.Instance.curRole.boxingList[i].viewdata.data.levelData);
            }
        }
        UserManager.Instance.curRole.boxingBattleList = rl;  
        rllist.Add(UserManager.Instance.curRole);
        rllist.Add(UserManager.Instance.curRole);
        rllist.Add(UserManager.Instance.curRole);

        List<RoleData> ellist = new List<RoleData>();
        List<BoxingLevelData> el = new List<BoxingLevelData>();
        List<int> str = GameShared.Instance.GetStr(d.boxing_id);
        for (int i = 0; i < str.Count; i++)
        {
            BoxingLevelData l = GameShared.Instance.GetBoxingLevelById(str[i]);
            if (l.skill_type == Def.SkillType.Active)
                el.Add(l);
        }
        e.boxingBattleList = el;
        ellist.Add(e);
        ellist.Add(e);
        ellist.Add(e);

        BattleManager.Instance.InitArane(rllist, ellist);
        UIManager.Instance.uiRoot.SetActive(false);
        Application.LoadLevelAdditive("game");
    }

    public void BattleStart()
    {
        EnterBattleLevel();
        ////todo 进入核心战斗 
        //if (levelpop.prograss.value < 1 && curView.data.data.checkpoint == curChapterData.curLevel[(int)levelpop.curFousTab])
        //{
        //    ToastManager.Instance.Show("倒计时不够");
        //}
        //else if (curView.data.data.checkpoint == curChapterData.curLevel[(int)levelpop.curFousTab])
        //{
        //    //todo 进入战斗
        //    this.BattleOver(Def.BattleOverType.Success);
        //}
        //else if (curView.data.data.checkpoint < curChapterData.curLevel[(int)levelpop.curFousTab])
        //{
        //    NextLevel();
        //}
    }
    /// <summary>
    /// 战斗结束
    /// </summary>
    public void BattleOver(Def.BattleOverType over)
    {
        UIManager.Instance.uiRoot.SetActive(true);
        Destroy(UIManager.Instance.battleRoot);

        //C2sSprotoType.checkpoint_battle_exit.request obj = new C2sSprotoType.checkpoint_battle_exit.request();
        //obj.csv_id = GetLevelView(curFousLevel).data.data.csv_id;
        //obj.checkpoint = GetLevelView(curFousLevel).data.data.checkpoint;
        //obj.chapter = GetLevelView(curFousLevel).data.data.chapter;
        //obj.type = (int)levelpop.curFousTab;
        //obj.result = (int)over;
        //Debug.Log("BattleOver obj.checkpoint" + obj.csv_id);
        //NetworkManager.Instance.CheckPointBattleExit(obj);
    }

    public void BattleOverCallback(C2sSprotoType.checkpoint_battle_exit.response resp)
    {
        if (resp.errorcode == 1)
        {
            
            this.curChapterData.curLevel[(int)curType]++;
            if (resp.reward != null)
            {
                List<ItemViewData> list = new List<ItemViewData>();
                for (int i = 0; i < resp.reward.Count; i++)
                {
                    BagMgr.Instance.SetItemNumById((int)resp.reward[i].csv_id, (int)resp.reward[i].num);
                    list.Add(BagMgr.Instance.GetItemViewData((int)resp.reward[i].csv_id));
                }
                MainUI.Instance.GetItemClick(list);
            }
            NextLevel();
        } 
    } 


    //battleEnterCallback方法中包含cd的内容注释掉了--6.15
    public void BattleEnterCallback(C2sSprotoType.checkpoint_battle_enter.response resp)
    {
        //Debug.Log("cd" + resp.cd);
        Debug.Log("cd");
        if (resp.errorcode == 1)
        {
            //curView.data.cd = resp.cd;            
            curView.data.time = DateTime.Now.AddSeconds(curView.data.cd);// curView.data.cd这里的是public long cd;
            curView.data.progress_unit = (float)1 / curView.data.data.cd;
            curView.data.progress = (float)(curView.data.data.cd - curView.data.cd) * curView.data.progress_unit;
            levelpop.SetPrograss(curView.data.time, curView.data.progress);
            RestRoleAnim();
            //if (resp.cd > 0)
            //{
            //    levelTimer = new PassiveTimer(1);
            //}
            //else
            //{
            //    levelTimer = null;
            //    levelpop.battleBtn.isEnabled = true;
            //}
        }
    }

    public void BattleEnter()
    { 
        //新建请求
        C2sSprotoType.checkpoint_battle_enter.request obj = new C2sSprotoType.checkpoint_battle_enter.request();
        //初始化关卡数据
        obj.csv_id = GetLevelView(curFousLevel).data.data.csv_id;
        obj.checkpoint = GetLevelView(curFousLevel).data.data.checkpoint;
        obj.chapter = GetLevelView(curFousLevel).data.data.chapter;
        obj.type = (int)levelpop.curFousTab;
        
        NetworkManager.Instance.CheckPointBattleEnter(obj); 
    }

    /// <summary>
    /// 进入下一关
    /// </summary>
    public void NextLevel()
    {
        curFousLevel++;  
        if (curFousLevel > levelmax)
        {
            if (UserManager.Instance.level < curChapterData.level)
            {
                ToastManager.Instance.Show("等级不够");
            }
            else if (UserManager.Instance.GetPower() < curChapterData.power)
            {
                ToastManager.Instance.Show("战斗力不够");
            }
            else
            {
                if (levelpop.curFousTab == Def.levelType.Normal && curChapterData.typeMax[(int)Def.levelType.Hard] > 0)
                {
                    curChapterData.curLevel[(int)Def.levelType.Hard] = 1;
                }
                else if (levelpop.curFousTab == Def.levelType.Hard && curChapterData.typeMax[(int)Def.levelType.Hell] > 0)
                {
                    curChapterData.curLevel[(int)Def.levelType.Hell] = 1;
                } 

                if (UserManager.Instance.curUnLockChapter < GameShared.Instance.config.chapter_max-1)
                {
                    UserManager.Instance.curUnLockChapter++;
                    curChapterData = GameShared.Instance.GetChapterById(UserManager.Instance.curUnLockChapter);
                    curChapterData.curLevel[(int)Def.levelType.Normal] = 1;
                    chapterPop.InitChapter(curChapterData);
                    InitLevel(curChapterData);
                    curView = GetLevelView(curFousLevel);
                } 
            }
        }
        else//下一关
        { 
            curView = GetLevelView(curFousLevel);
            UserManager.Instance.curLeveldata = curView.data.data;
            curView.data.unlock = true;
            curView.SetIcon();
            SelectLevel(curView);
            
        }
    }

    public LevelView GetLevelView(int level)
    {
        return levelpop.levelViewList[level-1] as LevelView;
    }
 
    /// <summary>
    /// 选关卡
    /// </summary>
    /// <param name="View"></param>
    public void SelectLevel(LevelView View)
    {
        if (View.data.data.checkpoint <= curChapterData.curLevel[(int)this.curType] && View.data.unlock == true)
        {
            //if (View.data.data.csv_id != UserManager.Instance.curLeveldata.csv_id)
            //{
                curView = View;
                levelpop.SetInfo(curView);
                C2sSprotoType.checkpoint_hanging_choose.request obj = new C2sSprotoType.checkpoint_hanging_choose.request();
                obj.chapter = curView.data.data.chapter;
                obj.checkpoint = curView.data.data.checkpoint;
                obj.type = (int)curView.data.data.type;
                obj.csv_id = curView.data.data.csv_id;
                NetworkManager.Instance.CheckPointChose(obj);
            //}
        }
        //else if (View.data.data.checkpoint == curChapterData.curLevel[(int)this.curType] && View.data.unlock == true)
        //{
        //    curView = View;
        //    levelpop.SetInfo(curView);
        //    C2sSprotoType.checkpoint_hanging_choose.request obj = new C2sSprotoType.checkpoint_hanging_choose.request();
        //    obj.chapter = curView.data.data.chapter;
        //    obj.checkpoint = curView.data.data.checkpoint;
        //    obj.type = (int)curView.data.data.type;
        //    obj.csv_id = curView.data.data.csv_id;
        //    NetworkManager.Instance.CheckPointChose(obj);
        //}
    }
    /// <summary>
    /// 设置除cd 外所有信息
    /// </summary>
    /// <param name="resp"></param>
    public void SelectLevelCallback(C2sSprotoType.checkpoint_hanging_choose.response resp)
    {
        if (curView != null && resp.errorcode == 1)
        {  
            curFousLevel = curView.data.data.checkpoint;
            levelTimer = null;
            if (curChapterData.curLevel[(int)curType] > curFousLevel)
            {
                levelpop.prograss.gameObject.SetActive(false);
                levelpop.time.gameObject.SetActive(false);
            }
            else
            {
                levelpop.prograss.gameObject.SetActive(true);
                levelpop.time.gameObject.SetActive(true);
                BattleEnter(); 
            }
            levelpop.SetInfo(curView); 
            if (levelpop.prograss.value < 1)
            {
                levelpop.battleBtn.isEnabled = false;
            }
            else
            {
                levelpop.battleBtn.isEnabled = true;
            } 
            UserManager.Instance.curLeveldata = curView.data.data;
           
        }
    }
     
    public void RestRoleAnim()
    {
        moveMax = levelpop.LeftPos.transform.localPosition.x - levelpop.rightPos.transform.localPosition.x;
        moveMax = Math.Abs(moveMax);
        Vector3 vec = levelpop.role.transform.localPosition;
        vec.x = levelpop.LeftPos.transform.localPosition.x + (moveMax * levelpop.prograss.value);
        levelpop.role.transform.localPosition = vec;
    }

    public void Update()
    {
        if (levelTimer != null && levelTimer.Update(Time.deltaTime) > 0)
        {
            if (levelpop.prograss.value < 1)
            {
                levelpop.time.text = Comm.DateDiffHour(DateTime.Now, curView.data.time);
                levelpop.prograss.value += curView.data.progress_unit;
                Vector3 vec = levelpop.role.transform.localPosition;
                vec.x = levelpop.LeftPos.transform.localPosition.x + (moveMax * levelpop.prograss.value);
                levelpop.role.transform.localPosition = vec;
                levelpop.battleBtn.isEnabled = false; 
            }
            else
            {
                BattleEnter();
            }
        } 
    }
     
}
