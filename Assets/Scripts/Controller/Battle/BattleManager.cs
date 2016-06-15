//中转类
#define Queue


using CodeStage.AntiCheat.ObscuredTypes;
using Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public partial class BattleManager : UnitySingleton<BattleManager>
{


    public List<RoleData> araneUserList;
    public List<RoleData> araneEmenyList;

    public int curUserRoleIndex;
    public int curEmenyRoleIndex;

    public RoleData roleData;
    public RoleData emenyData;

    public static int skillReleaseCount = 5;

    private PassiveTimer timer;    

    public ObscuredInt comboLevel;
    public int cameraLevel;  
    
    //combo处理
    public bool isTouchingSkill; 
    //处理连击块
    public int isTouchcount;
    public bool isTouching; 
    public bool isTouchingComboBox;

    public bool isBattle = false;

    private int roleWinCount;
    private int emenyWinCount;
    public int winCountMax = 2;
    private int battleCount = 0;
      
    public GameBattlePop pop;
    public bool isArane;


    public ActQueueData curAct;
    public ActQueueData bufAct;

    public void Init()
    {
        isBattle = false;
    }

    public void InitLevel(RoleData r, RoleData e, List<BoxingLevelData> rl, List<BoxingLevelData> el)
    {
        isArane = false;
        roleData = r;
        emenyData = e; 
    }
     


    public void InitData(GameBattlePop p)
    {
        pop = p;
        Resolution();
        boxingEffectQueue = new List<ActQueueData>();
        sendTimer = new PassiveTimer(Def.BattleSendTime);
        sendQueue = new List<C2sSprotoType.BattleListElem>();
        //wTime.timeScale = 0.5f;
        if (pop.touchZone != null)
        {
            pop.touchZone.TouchingEvent += touchZone_TouchingEvent;
            pop.touchZone.TouchDownEvent += touchZone_TouchDownEvent;
            pop.touchZone.TouchUpEvent += touchZone_TouchUpEvent;
            InitEffect();
            InitBattle();
            InitBoxRate();
            ComboLevelRest();
        }
        StartCoroutine("StartBattleSend");
    }

#if Queue
    IEnumerator StartBattleSend()
    {
        yield return new WaitForSeconds(2.0f);
        AutoRoleChange(AutoUpdate());
    }
    //主角动画控制和伤害
    public void RoleChange(ClickBox.BoxType type,bool isHit = true)
    {
        if (isBattle == false)
            return;
        //解决combo动画中断敌人还在播放伤害动画
        if(type!=ClickBox.BoxType.ComboBox && pop.role.isComboing)
        {
            StopCoroutine("ComboRunAnim");
            pop.emeny.ChangeState(MonsterMgr.RoleState.Idle);
        }
        int hit = 0;
        ActQueueData act;
        switch (type)
        {
            case ClickBox.BoxType.YellowBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("Attack2RunAnim");
                hit = (int)pop.role.data.GetDamage(pop.emeny.data.defenseRate);
                pop.emeny.Hit(hit, hit);
                act = ManaulAttack(hit); 
                break;
            case ClickBox.BoxType.BlueBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("Attack1RunAnim"); 
                hit = (int)pop.role.data.GetDamage(pop.emeny.data.defenseRate);
                pop.emeny.Hit(hit, hit);
                act = ManaulAttack(hit);
                
                break;
            case ClickBox.BoxType.ComboBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("ComboRunAnim");
                hit = pop.role.data.ComboDamage(comboLevel);
                pop.emeny.Hit(hit, hit);
                act = ManaulAttack(hit);
                 
                break;
            case ClickBox.BoxType.Skill:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("ComboRunAnim");
                hit = pop.role.data.ComboDamage(comboLevel);
                pop.emeny.Hit(hit, hit);
                this.ComboAttack(hit, comboLevel);
                
                break; 
            case ClickBox.BoxType.None:
                pop.emeny.ChangeSortLayer(Def.SortRole);
                pop.role.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("HitRoleRunAnim");
                hit = (int)pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                pop.role.Hit(hit, hit);
                act = ManaulAttack(hit, pop.emeny.data.csv_id);
                break;
            //case ClickBox.BoxType.BombBox:
            //        pop.emeny.ChangeSortLayer(Def.SortRole);
            //        pop.role.ChangeSortLayer(Def.SortEmeny);
            //        if (isHit)
            //        {
            //            StartCoroutine("HitRoleRunAnim");
            //            hit = (int)pop.emeny.data.GetDamage(pop.role.data.defenseRate);
            //            pop.role.Hit(hit, hit);
            //            act = ManaulAttack(hit);
                        
            //        }
            //        //pop.emeny.ChangeSortLayer(Def.SortRole);
            //        //pop.role.ChangeSortLayer(Def.SortEmeny);
            //        //if (isHit)
            //        //{ 
            //        //    StartCoroutine("HitRoleRunAnim");
            //        //    hit = pop.emeny.data.Damage2();
            //        //    pop.role.Hit(hit,hit);
            //        //    this.ManaulAttack(hit);
            //        //}
            //        //else
            //        //{

            //        //}
            //        break; 
            //case ClickBox.BoxType.RedBox:
            //    pop.emeny.ChangeSortLayer(Def.SortRole);
            //    pop.role.ChangeSortLayer(Def.SortEmeny);
            //    if (isHit)
            //    {
            //        StartCoroutine("HitRoleRunAnim");
            //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
            //        pop.role.Hit(hit, hit);
            //        act = ManaulAttack(hit);
                    
            //    }
            //    else
            //    {
            //        StartCoroutine("RedRunAnim");
            //    }
            //    break;
            //case ClickBox.BoxType.FastBox:
            //    pop.emeny.ChangeSortLayer(Def.SortRole);
            //    pop.role.ChangeSortLayer(Def.SortEmeny);
            //    if (isHit)
            //    {
            //        StartCoroutine("HitRoleRunAnim");
            //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
            //        pop.role.Hit(hit, hit);
            //        act = ManaulAttack(hit);
                    
            //    }
            //    else
            //    {
            //        StartCoroutine("FastRunAnim"); 
            //    }
            //    break;
            //case ClickBox.BoxType.GroundBox:
            //    pop.emeny.ChangeSortLayer(Def.SortRole);
            //    pop.role.ChangeSortLayer(Def.SortEmeny);
            //    if (isHit)
            //    {
            //        StartCoroutine("HitRoleRunAnim");
            //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
            //        pop.role.Hit(hit, hit);
            //        act = ManaulAttack(hit);
                    
            //    }
            //    else
            //    {
            //        StartCoroutine("GroundRunAnim"); 
            //    }
            //    break;
            //case ClickBox.BoxType.Act:
            //    pop.emeny.ChangeSortLayer(Def.SortRole);
            //    pop.role.ChangeSortLayer(Def.SortEmeny);
            //    if (isHit)
            //    {
            //        StartCoroutine("ActRunAnim");
            //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
            //        pop.role.Hit(hit, hit);
            //        act = ManaulAttack(hit);
                     
            //    }
            //    else
            //    {
            //        StartCoroutine("ActRunAnim");
            //    }
            //    break;
        } 
    }
    /// <summary>
    /// 自动攻击
    /// </summary>
    /// <param name="act"></param>
    public void AutoRoleChange(ActQueueData act)
    {
        if(isBattle)
        { 
            MonsterMgr ar;
            MonsterMgr dr;
            string callboxing = "AutoBoxing";
            string callnormal = "AutoNormal";
            if (act.who == pop.role.data.csv_id)
            {
                ar = pop.role;
                dr = pop.emeny;
                callboxing = "UserAutoBoxing";
                callnormal = "UserAutoBoxing";
            } else
            {
                ar = pop.emeny;
                dr = pop.role;
                callboxing = "EmenyAutoBoxing";
                callnormal = "EmenyAutoBoxing";
            }
            int hit = 0;
            //拳法
            if (act.box != null)
            { 
                ar.ChangeSortLayer(Def.SortRole);
                dr.ChangeSortLayer(Def.SortEmeny); 
                switch(act.box.formula_type)
                {
                    case Def.FormulaType.Normal:
                        hit = (int)ar.data.GetDamage(dr.data.defenseRate, (double)act.box.effect_pre / 100.0);
                        break;
                    case Def.FormulaType.Crit:
                        hit = (int)ar.data.GetCrit(dr.data.defenseRate,(double)act.box.effect_pre/100.0);
                        break;
                    case Def.FormulaType.Combo:
                        hit = (int)ar.data.ComboDamage(comboLevel);
                        break; 
                }

                dr.Hit(hit); 
                this.AutoAttack(ar,dr,hit,act); 
                StartCoroutine(callboxing);
            } else //普通攻击
            {
                ar.ChangeSortLayer(Def.SortRole);
                dr.ChangeSortLayer(Def.SortEmeny);
                hit = (int)ar.data.GetDamage(dr.data.defenseRate);
                Debug.Log("hit" + roleData.curFightPower);
                dr.Hit(hit);
                this.AutoAttack(act, hit); 
                StartCoroutine(callnormal);
            } 
        }
    }
     //动画控制
    //自动
    IEnumerator UserAutoBoxing()
    {
        if (pop != null)
        {
            pop.role.ChangeState(MonsterMgr.RoleState.Attack2);
            yield return new WaitForSeconds(0.15f);
            pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
            yield return new WaitForSeconds(0.2f);
            this.AutoRoleChange(AutoUpdate());
        }
    }
    IEnumerator EmenyAutoBoxing()
    {
        if(pop!=null)
        { 
            pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
            yield return new WaitForSeconds(0.15f);
            pop.role.ChangeState(MonsterMgr.RoleState.Hit);
            yield return new WaitForSeconds(0.2f);
            this.AutoRoleChange(AutoUpdate());
        }
    }
    IEnumerator UserAutoNormal()
    {
        pop.role.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.15f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.5f);
        this.AutoRoleChange(AutoUpdate());
    }
    IEnumerator EmenyAutoNormal()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.15f);
        pop.role.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.5f);
        this.AutoRoleChange(AutoUpdate());
    }
#else
    IEnumerator StartBattleSend()
    {
        yield return new WaitForSeconds(2.0f);
        GetActByNet();
    } 
    public void RoleChange(ClickBox.BoxType type, bool isHit = true)
    {
        ActQueueData d = new ActQueueData();
        d.kf_id = (int)Def.KFID.Normal;
        d.who = roleData.csv_id;
        d.fight_type = Def.FightType.Manually;
        d.kfType = (int)Def.BattleAttackType.Boxing;
        if (type == ClickBox.BoxType.ComboBox)
            d.comboCount = comboLevel;
        if(type == ClickBox.BoxType.None)
            d.who = emenyData.csv_id;
        Debug.Log("ManaulAttack kf_id: " + d.kf_id);
        NetworkManager.Instance.SingleBattleList(d, (session, o, ud) =>
        {
            var resp = (C2sSprotoType.TMP_GuanQiaBattleList.response)o;

            RoleChangeManaul(type, resp);
        });
    }

    public void RoleChangeManaul(ClickBox.BoxType type, C2sSprotoType.TMP_GuanQiaBattleList.response resp)
    {
        if (isBattle == false)
            return;
        //解决combo动画中断敌人还在播放伤害动画
        if (type != ClickBox.BoxType.ComboBox && pop.role.isComboing)
        {
            StopCoroutine("ComboRunAnim");
            pop.emeny.ChangeState(MonsterMgr.RoleState.Idle);
        }
        int hit = (int)resp.totalattack;
        switch (type)
        {
            case ClickBox.BoxType.YellowBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("Attack2RunAnim");
                pop.emeny.Hit(hit, hit);
                //act = ManaulAttack(hit);
                break;
            case ClickBox.BoxType.BlueBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("Attack1RunAnim");
                pop.emeny.Hit(hit, hit);
                //act = ManaulAttack(hit);

                break;
            case ClickBox.BoxType.ComboBox:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("ComboRunAnim");
                pop.emeny.Hit(hit, hit);
                //act = ManaulAttack(hit);

                break;
            case ClickBox.BoxType.Skill:
                pop.role.ChangeSortLayer(Def.SortRole);
                pop.emeny.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("ComboRunAnim");
                pop.emeny.Hit(hit, hit);
                //this.ComboAttack(hit, comboLevel);

                break;
            case ClickBox.BoxType.None:
                pop.emeny.ChangeSortLayer(Def.SortRole);
                pop.role.ChangeSortLayer(Def.SortEmeny);
                StartCoroutine("HitRoleRunAnim");
                pop.role.Hit(hit, hit);
                //act = ManaulAttack(hit, pop.emeny.data.csv_id);
                break;
                //case ClickBox.BoxType.BombBox:
                //        pop.emeny.ChangeSortLayer(Def.SortRole);
                //        pop.role.ChangeSortLayer(Def.SortEmeny);
                //        if (isHit)
                //        {
                //            StartCoroutine("HitRoleRunAnim");
                //            hit = (int)pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                //            pop.role.Hit(hit, hit);
                //            act = ManaulAttack(hit);

                //        }
                //        //pop.emeny.ChangeSortLayer(Def.SortRole);
                //        //pop.role.ChangeSortLayer(Def.SortEmeny);
                //        //if (isHit)
                //        //{ 
                //        //    StartCoroutine("HitRoleRunAnim");
                //        //    hit = pop.emeny.data.Damage2();
                //        //    pop.role.Hit(hit,hit);
                //        //    this.ManaulAttack(hit);
                //        //}
                //        //else
                //        //{

                //        //}
                //        break; 
                //case ClickBox.BoxType.RedBox:
                //    pop.emeny.ChangeSortLayer(Def.SortRole);
                //    pop.role.ChangeSortLayer(Def.SortEmeny);
                //    if (isHit)
                //    {
                //        StartCoroutine("HitRoleRunAnim");
                //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                //        pop.role.Hit(hit, hit);
                //        act = ManaulAttack(hit);

                //    }
                //    else
                //    {
                //        StartCoroutine("RedRunAnim");
                //    }
                //    break;
                //case ClickBox.BoxType.FastBox:
                //    pop.emeny.ChangeSortLayer(Def.SortRole);
                //    pop.role.ChangeSortLayer(Def.SortEmeny);
                //    if (isHit)
                //    {
                //        StartCoroutine("HitRoleRunAnim");
                //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                //        pop.role.Hit(hit, hit);
                //        act = ManaulAttack(hit);

                //    }
                //    else
                //    {
                //        StartCoroutine("FastRunAnim"); 
                //    }
                //    break;
                //case ClickBox.BoxType.GroundBox:
                //    pop.emeny.ChangeSortLayer(Def.SortRole);
                //    pop.role.ChangeSortLayer(Def.SortEmeny);
                //    if (isHit)
                //    {
                //        StartCoroutine("HitRoleRunAnim");
                //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                //        pop.role.Hit(hit, hit);
                //        act = ManaulAttack(hit);

                //    }
                //    else
                //    {
                //        StartCoroutine("GroundRunAnim"); 
                //    }
                //    break;
                //case ClickBox.BoxType.Act:
                //    pop.emeny.ChangeSortLayer(Def.SortRole);
                //    pop.role.ChangeSortLayer(Def.SortEmeny);
                //    if (isHit)
                //    {
                //        StartCoroutine("ActRunAnim");
                //        hit = pop.emeny.data.GetDamage(pop.role.data.defenseRate);
                //        pop.role.Hit(hit, hit);
                //        act = ManaulAttack(hit);

                //    }
                //    else
                //    {
                //        StartCoroutine("ActRunAnim");
                //    }
                //    break;
        }
    }

    /// <summary>
    /// 自动攻击
    /// </summary>
    /// <param name="act"></param>
    public void AutoRoleChange(ActQueueData act)
    {
        if (isBattle)
        {
            MonsterMgr ar;
            MonsterMgr dr;
            string callboxing = "AutoBoxing";
            string callnormal = "AutoNormal";
            if (act.who == roleData.csv_id)
            {
                ar = pop.role;
                dr = pop.emeny;
                callboxing = "UserAutoBoxing";
                callnormal = "UserAutoBoxing";
            }
            else
            {
                ar = pop.emeny;
                dr = pop.role;
                callboxing = "EmenyAutoBoxing";
                callnormal = "EmenyAutoBoxing";
            }
            int hit = act.hit;
            //拳法
            if (act.box != null)
            {
                ar.ChangeSortLayer(Def.SortRole);
                dr.ChangeSortLayer(Def.SortEmeny);

                dr.Hit(hit);
                //this.AutoAttack(ar, dr, hit, act);
                StartCoroutine(callboxing);
            }
            else //普通攻击
            {
                ar.ChangeSortLayer(Def.SortRole);
                dr.ChangeSortLayer(Def.SortEmeny);
                Debug.Log("hit" + roleData.curFightPower);
                dr.Hit(hit);
                //this.AutoAttack(act, hit);
                StartCoroutine(callnormal);
            } 
        }
    }

    /// <summary>
    /// 请求下次攻击伤害
    /// </summary>
    public void GetActByNet()
    {
        if (curAct.isDead != 0)
        {
            NetworkManager.Instance.SingleBattleList(bufAct, (session, o, ud) =>
            {
                var resp = (C2sSprotoType.TMP_GuanQiaBattleList.response)o;
                Debug.Log("LevelBattleList");

                BattleActCallback(resp);
            });
        }
        else
        {
            StartCoroutine("CheckOver");
        }
    }

    public void BattleActCallback(C2sSprotoType.TMP_GuanQiaBattleList.response resp)
    {
        curAct = bufAct;
        curAct.hit = (int)resp.totalattack; 
        curAct.effect = (int)resp.effect;
        curAct.isDead = (int)resp.loser;

        if(roleData.csv_id == curAct.who)


        bufAct = new ActQueueData();
        bufAct.kf_id = (int)resp.kf_id;
        if ((int)resp.kf_id == (int)Def.KFID.Normal)
        {
            bufAct.kfType = (int)Def.BattleAttackType.Normal;
        }
        else
        {
            bufAct.kfType = (int)Def.BattleAttackType.Boxing;
        }
        bufAct.fight_type = Def.FightType.Auto;
        if (roleData.csv_id == curAct.who)
        {
            bufAct.who = emenyData.csv_id;
        }
        else
        {
            bufAct.who = roleData.csv_id;
        }

        Debug.Log("kf_id ;" + bufAct.kf_id + "bufAct.kfType:" + bufAct.kfType + "bufAct.fight_type:" + bufAct.fight_type + "bufAct.who" + bufAct.who);
        AutoRoleChange(curAct);
    }

    //动画控制
    //自动
    IEnumerator UserAutoBoxing()
    {
        if (pop != null)
        {
            pop.role.ChangeState(MonsterMgr.RoleState.Attack2);
            yield return new WaitForSeconds(0.15f);
            pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
            yield return new WaitForSeconds(2f);
            GetActByNet();
        }
    }
    IEnumerator EmenyAutoBoxing()
    {
        if (pop != null)
        {
            pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
            yield return new WaitForSeconds(0.15f);
            pop.role.ChangeState(MonsterMgr.RoleState.Hit);
            yield return new WaitForSeconds(2f);
            GetActByNet();
        }
    }
    IEnumerator UserAutoNormal()
    {
        pop.role.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.15f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(2f);
        GetActByNet();
    }
    IEnumerator EmenyAutoNormal()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.15f);
        pop.role.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(2f);
        GetActByNet();
    }

    IEnumerator CheckOver()
    {
        yield return new WaitForSeconds(1f);
        if (roleData.curFightPower < 0)
        {
            BattleManager.Instance.BattleOver(roleData.csv_id); 
        }
        else if (emenyData.curFightPower < 0)
        {
            BattleManager.Instance.BattleOver(emenyData.csv_id); 
        } 
    }

#endif

    //适配
    public void Resolution()
    {
        float a = (float)Screen.width / Screen.height;

        float y = 15.0f * (1 - (a - 1.0f));
        Vector3 vecr = pop.rolePos.transform.localPosition;
        vecr.y = -85 - y;
        pop.rolePos.transform.localPosition = vecr;

        Vector3 vece = pop.emenyPos.transform.localPosition;
        vece.y = -85 - y;
        pop.emenyPos.transform.localPosition = vece;

        float scale = a / 1.775f;
        pop.bottomBar.transform.localScale = new Vector3(scale, scale, 1);
        pop.bottomBarBg.transform.localScale = new Vector3(1, scale + 0.05f, 1);
    }


    public void InitArane(List<RoleData> user, List<RoleData> emeny)
    {
        isArane = true;
        curUserRoleIndex = 0;
        curEmenyRoleIndex = 0;
        araneUserList = user;
        araneEmenyList = emeny;
        roleData = araneUserList[curUserRoleIndex];
        emenyData = araneEmenyList[curEmenyRoleIndex]; 
    } 
    /// <summary>
    ///  角色上阵
    /// </summary>
    /// <param name="win"></param>
    public void ChangebattleRole()
    {
        if (roleData.curFightPower <= 0)
        {
            curUserRoleIndex++;
            if (curUserRoleIndex < 3)
            {
                roleData = araneUserList[curUserRoleIndex];
                InitRole();
            } 
        } else
        {
            curEmenyRoleIndex++;
            if (curEmenyRoleIndex < 3)
            {
                emenyData = araneEmenyList[curEmenyRoleIndex];
                InitEmeny();
            }
        }  
    }

    //连击combo控制
#region 
    public void ComboLevelAdd()
    {
        comboLevel++;
        cameraLevel++;
        ComboLevelChange();
    } 

    public void GetRandComboLevel()
    {
        skillReleaseCount = Random.RandomRange(GameShared.Instance.config.comboMin, GameShared.Instance.config.comboMax);
    }

    public void ComboLevelRest()
    {
        comboLevel = 0;
        cameraLevel = 0;
        ComboLevelChange();
        pop.camera.Init();
        pop.comboBar.Init();
        ComboEffectEnable(false);
        GetRandComboLevel();
    }

    public void ComboLevelRestNotCamera()
    {
        comboLevel = 0;
        ComboLevelChange();
        pop.comboBar.Init();
        pop.spawner.RemoveAllRed();
        ComboEffectEnable(false);
    }

    public void ComboLevelChange()
    {
        if (comboLevel >= skillReleaseCount)
        {
            ComboEffectEnable(true);
        }
            
        if (comboLevel <= skillReleaseCount)
        {
            if (cameraLevel > 4)
                cameraLevel = 4;
            pop.comboBar.ChangeCurCombo(comboLevel);
            pop.camera.MoveLevel(cameraLevel);
            ChangeRolePos(cameraLevel);
            pop.cursor.ChangeSpeedByLevel(comboLevel); 
        }
        else
        {
            pop.comboBar.ChangeComboLabel(comboLevel);
        }
    } 

    public void InitRolePos()
    {
        pop.role.transform.localPosition = pop.rolePos.transform.localPosition;
        pop.emeny.transform.localPosition = pop.emeny.transform.localPosition;
    }

    public void ChangeRolePos(int level)
    {
        Vector3 vecr = pop.rolePos.transform.localPosition;
        vecr.x += 20 * level;
        pop.role.transform.localPosition = vecr;
        Vector3 vece = pop.emenyPos.transform.localPosition;
        vece.x -= 20 * level;
        pop.emeny.transform.localPosition = vece;
    } 
#endregion

    
    public void ComboEffectEnable(bool b)
    {
        pop.comboBarAnim.SetActive(b);  
    }



    //动画控制
     
     

    

    /// <summary>
    /// 处理伤害效果
    /// </summary>
    /// <param name="act"></param>
    /// <param name="def"></param>
    /// <param name="hit"></param>
    /// <param name="d"></param>
    public int EffectHit(MonsterMgr act, MonsterMgr def, int hit, ActQueueData d)
    {
        double temp = 0; 
            switch (d.box.add_effect_type)
                {
                    case Def.AddEffectType.AbsorbHit: 
                    temp = (double)hit * ((double)d.box.add_state_pre/100.0);
                        act.Abosrb((int)temp); 
                        break;
                    case Def.AddEffectType.AbsorbMeFight: 
                    temp = (double)act.data.fightPower * ((double)d.box.add_state_pre/100.0);
                        act.Abosrb((int)temp); 
                        break;
                    case Def.AddEffectType.HitMeFight: 
                        temp = def.data.fightPower - def.data.curFightPower;
                        temp = temp * (double)d.box.add_state_pre/100.0; 
                        def.Hit((int)temp);
                        break;
                    case Def.AddEffectType.AbsorbEmenyFight: 
                        temp = (double)def.data.curFightPower * ((double)d.box.add_state_pre/100.0);
                        act.Abosrb((int)temp);
                        def.Hit((int)temp);
                        break;
                    case Def.AddEffectType.Rebound:  
                        temp = (double)hit * ((double)d.box.add_state_pre / 100.0);
                        act.Hit((int)temp);
                        break;
                }   
        return (int)temp;
    }   

    public List<C2sSprotoType.BattleListElem> sendQueue; 
    public List<ActQueueData> boxingEffectQueue; 
    public PassiveTimer sendTimer;

    public List<BoxingLevelData> roleBoxList; 
    public List<BoxingLevelData> emenyBoxList;

    public List<BoxingQueueData> roleBoxQueue;
    public List<BoxingQueueData> emenyBoxQueue;


    public void InitBoxRate()
    { 
        roleData.boxingBattleList = roleData.boxingBattleList.OrderBy(s => s.trigger_pre).ToList<BoxingLevelData>();
        emenyData.boxingBattleList = emenyData.boxingBattleList.OrderBy(s => s.trigger_pre).ToList<BoxingLevelData>();
        roleBoxQueue = new List<BoxingQueueData>();
        for (int i = 0; i < roleData.boxingBattleList.Count; i++)
        {
            BoxingQueueData d = new BoxingQueueData();
            d.data = roleData.boxingBattleList[i];
            d.useNum = 0;
            d.isUse = true;
            d.oldPre = d.data.trigger_pre;
            roleBoxQueue.Add(d);
        }
        emenyBoxQueue = new List<BoxingQueueData>();
        for (int i = 0; i < emenyData.boxingBattleList.Count; i++)
        {
            BoxingQueueData d = new BoxingQueueData();
            d.data = emenyData.boxingBattleList[i]; 
            d.useNum = 0;
            d.isUse = true;
            d.oldPre = d.data.trigger_pre;
            emenyBoxQueue.Add(d);
        } 
        RestBoxingQueue(roleBoxQueue);
        RestBoxingQueue(emenyBoxQueue);
    }

    /// <summary>
    /// 生成拳法的随机值 
    /// </summary>
    /// <param name="list"></param>
    public void RestBoxingQueue(List<BoxingQueueData> list)
    {
        int cur_pre=0;
        list = list.OrderBy(s => s.data.trigger_pre).ToList<BoxingQueueData>();
         
        
        for (int i = 0; i < list.Count; i++)
        { 
            if (list[i].isUse == true)
            {
                if (cur_pre <= 100)
                {
                    cur_pre += list[i].data.trigger_pre;
                    list[i].usePre = cur_pre;
                }
                else
                {
                    list[i].isUse = false;
                } 
            }
        } 
    }



    /// <summary>
    /// 检测是否可用
    /// </summary>
    /// <param name="list"></param>
    /// <param name="data"></param>
    /// <returns></returns> 
    public void RestBoxingQueue(List<BoxingQueueData> list, RoleData data)
    {
        int cur_pre = 0;
        list = list.OrderBy(s => s.data.trigger_pre).ToList<BoxingQueueData>();

        RoleData ar;
        RoleData dr;
        if (data.csv_id == roleData.csv_id)
        {
            ar = roleData;
            dr = emenyData;
        }
        else
        {
            ar = emenyData;
            dr = roleData;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].data.trigger_num  == 0 || list[i].useNum < list[i].data.trigger_num)
            {
                switch (list[i].data.trigger_type)
                {
                    case Def.TriggerType.User:
                        if (ar.curFightPowerPre < list[i].data.trigger_arg)
                        {
                            list[i].isUse = true;
                        }
                        else
                        {
                            list[i].isUse = false;
                        }
                        break;
                    case Def.TriggerType.Emeny:
                        if (pop.emeny.data.curFightPowerPre < list[i].data.trigger_arg)
                        {
                            list[i].isUse = true;
                        }
                        else
                        {
                            list[i].isUse = false;
                        }
                        break;
                    case Def.TriggerType.Attack:
                        if (ar.attackCount == list[i].data.trigger_arg)
                        {
                            list[i].isUse = true;
                        }
                        else
                        {
                            list[i].isUse = false;
                        }
                        break;
                }
            }
            else
            {
                list[i].isUse = false;
            }
        }
        for (int i = 0; i < list.Count; i++)
        { 
            if (list[i].isUse == true)
            {
                if (cur_pre <= 100)
                {
                    cur_pre += list[i].data.trigger_pre;
                    list[i].usePre = cur_pre;
                }
                else
                {
                    list[i].isUse = false;
                }
            }
        }
    }
    /// <summary>
    /// 生成一个自动攻击
    /// </summary>
    /// <param name="list"></param>
    public ActQueueData CreateBox(List<BoxingQueueData> list,RoleData data)
    {
        BoxingQueueData b = GetRandBox(list);
        ActQueueData d = new ActQueueData(); 
        RestBoxingQueue(list,data);
        if (b.data != null)
        {
            attackNum++; 
            d.who = data.csv_id;
            d.box = b.data;
            d.fight_type = Def.FightType.Auto;
            d.pre = b.randPre;
            d.effect = GetBoxingTriggerEffect(d);
        }
        else
        { 
            d.fight_type = Def.FightType.Auto;
            d.pre = b.randPre; 
            d.type = 3;
            d.who = data.csv_id; 
        } 
        int removeid = 99;
        //封印触发概率>x%的敌方技能
        for (int i = 0; i < boxingEffectQueue.Count; i++)
        {
            if (boxingEffectQueue[i].box.add_effect_type == Def.AddEffectType.Seal)
            {
                if (boxingEffectQueue[i].who != data.csv_id && d.pre >= boxingEffectQueue[i].box.effect_pre)
                {
                    removeid = data.csv_id;
                }
            } 
        }
        if(removeid!=99)
            boxingEffectQueue.RemoveAt(removeid);
        return d;
    }
  

    public int attackNum;

    /// <summary>
    /// 随机生成一个自动攻击
    /// </summary>
    /// <param name="list"></param>
    public BoxingQueueData GetRandBox(List<BoxingQueueData> list)
    {
        BoxingQueueData box = new BoxingQueueData(); 
        int rand = Random.RandomRange(0, 100); 
        if (list!=null)
        { 
            for(int i=0;i<list.Count;i++)
            {
                if (list[i] != null && rand <= list[i].usePre)
                {
                    if (CheckUseBox(list, list[i])  && list[i].isUse != false)
                    {
                        box = list[i];
                        list[i].useNum++;
                        if (list[i].data.trigger_num!=0 && list[i].useNum >= list[i].data.trigger_num)
                        {
                            list[i].isUse = false;
                            //RestBoxingQueue(list);
                        } 
                        break;
                    } 
                } 
            }
        }
        box.randPre = rand;
        return box;
    }


     

    /// <summary>
    /// 获得攻击附加效果
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public int GetBoxingTriggerEffect(ActQueueData act)
    {
        int id = 0;
        if (act.box.add_effect_type != Def.AddEffectType.None)
        {
            boxingEffectQueue.Add(act);
            id = (int)act.box.add_effect_type;
        }
        return id;
    }
    /// <summary>
    /// 检测拳法检查是否能触发效果
    /// </summary>
    /// <param name="list"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public bool CheckUseBox(List<BoxingQueueData> list, BoxingQueueData d) 
    {
        bool flag = false;
        switch (d.data.trigger_type)
        {
            case Def.TriggerType.None:
                flag = true;
                break;
            case Def.TriggerType.User:
                if (pop.role.data.curFightPowerPre < d.data.trigger_arg)
                    flag = true;
                break;
            case Def.TriggerType.Emeny:
                if (pop.emeny.data.curFightPowerPre < d.data.trigger_arg)
                    flag = true;
                break;
            case Def.TriggerType.Attack:
                if (attackNum < d.data.trigger_arg)
                {
                    flag = true;
                }
                else
                {
                    d.isUse = false;
                }
                break;
        }  
        return flag;
    }

    /// <summary>
    /// 插入自动攻击
    /// </summary>
    /// <param name="b"></param>
    public void AutoAttack(ActQueueData d,int hit)
    { 
        d.kfType = (int)Def.BattleAttackType.Normal;
        d.kf_id = (int)Def.KFID.Normal;
        if (d.box != null)
        { 
            d.effect = GetBoxingTriggerEffect(d);
            d.kf_id = d.box.g_csv_id;
            d.kfType = (int)Def.BattleAttackType.Boxing;
        } 
        d.hit = hit;
        d.fight_type = Def.FightType.Auto; 
        AddQueue(d);
    }


    public void AutoAttack(MonsterMgr act, MonsterMgr def, int hit, ActQueueData d)
    { 
        d.attach_effect = EffectHit(act,def,hit,d); 
        d.kfType = (int)Def.BattleAttackType.Normal;
        d.kf_id = (int)Def.KFID.Normal;
        if (d.box != null)
        {
            d.effect = GetBoxingTriggerEffect(d);
            d.kf_id = d.box.g_csv_id;
            d.kfType = (int)Def.BattleAttackType.Boxing;
        }
        d.hit = hit;
        d.fight_type = Def.FightType.Auto; 
        AddQueue(d);
    }
    /// <summary>
    /// 插入手动攻击
    /// </summary>
    /// <param name="b"></param>
    public ActQueueData ManaulAttack(int hit,int id = 0)
    { 
        ActQueueData d = new ActQueueData();
        d.hit = hit;
        d.who = id;
        d.kf_id = (int)Def.KFID.Normal;
        if (id == 0)
           d.who = roleData.csv_id;
        d.fight_type = Def.FightType.Manually;
        d.kfType = (int)Def.BattleAttackType.Boxing;
        Debug.Log("ManaulAttack kf_id: " + d.kf_id);  
        AddQueue(d);
        return d;
    }

    public int CheckWhoDead()
    {
        int i = 0;
        if (roleData.curFightPower < 0)
        {
            i = 1;
        }
        else if (emenyData.curFightPower < 0)
        {
            i = 2;
        }
        return i;
    }
    /// <summary>
    /// 插入combo攻击
    /// </summary>
    public void ComboAttack(int hit ,int count)
    {
        ActQueueData d = new ActQueueData();
        d.fight_type = Def.FightType.Manually;
        d.kf_id = (int)Def.KFID.Combo;
        d.hit = hit;
        d.kfType = (int)Def.BattleAttackType.Combo;
        d.comboCount = count;
        AddQueue(d);
    }
    //添加 攻击队列
    public void AddQueue(ActQueueData d)
    { 
        if (isBattle)
        {  
            C2sSprotoType.BattleListElem o = new C2sSprotoType.BattleListElem();
            o.attack = d.hit;//攻击力
            o.isdead = CheckWhoDead(); //0没死 1 主角 2 敌人
            o.attach_effect = d.attach_effect;//攻击效果
            o.kf_prob = d.pre; // 自动攻击改率 
            o.attcktype = (int)d.fight_type; //自动还是手动  
            o.kf_type = d.kfType; //1拳法类型 3普通 攻击 2COMBO 
            if (d.kf_id == 12)
                d.kf_id = (int)Def.BattleAttackType.Normal;
            o.kf_id = d.kf_id;
            o.fighterid = d.who; //发动角色的id 
            o.random_combo_num = d.comboCount;
             
            Debug.Log("角色 : id " +roleData.csv_id + "血量 : " + roleData.curFightPower + "|"+ "角色 : id " + emenyData.csv_id + "血量 : " + +emenyData.curFightPower + 
               "|"+ "角色id: " + o.fighterid + "伤害:" + o.attack + "isdead: " + o.isdead + "效果: " + o.attach_effect + "百分比: " + o.kf_prob + "附加伤害 : "+ d.attach_effect + 
                "o.attcktype: " + o.attcktype + "o.kf_type: " + o.kf_type + " 拳法id: " + o.kf_id );
            Debug.Log("--------------------------------------------------");
            sendQueue.Add(o);
            if (CheckWhoDead() != 0)//如果有人死了立即调用发送list
            {
                isBattle = false;
                StopGame();
                SendQueue();
            }
        }
    } 
 
    /// <summary>
    /// 发送
    /// </summary>
    public void SendQueue()
    {
        if(sendQueue.Count>0)
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("sendQueue: count :" + sendQueue.Count);
            if (isArane)
            {
                NetworkManager.Instance.ArenaBattleList(sendQueue);
            }
            else
            {
                NetworkManager.Instance.LevelBattleList(sendQueue);
            }
            sendQueue.Clear();
        } 
    }

    public void BattleDataError()
    {
        Debug.Log("战斗数据不正确");
        ToastManager.Instance.Show("战斗数据不正确");

        UIManager.Instance.uiRoot.SetActive(true);
        Destroy(UIManager.Instance.battleRoot); 
    }

   
    private bool isCreateUser;


    public bool isOnly = false;
    public ActQueueData  AutoUpdate()
    {
        if (isCreateUser)
        {
            isCreateUser = false;
            return this.CreateBox(roleBoxQueue, roleData);
        }
        else
        {
            isCreateUser = true;
            return this.CreateBox(emenyBoxQueue, emenyData);
        }
    }

   
    //手动
    IEnumerator Attack2RunAnim()
    {
        pop.role.ChangeState(MonsterMgr.RoleState.Attack2);  
        yield return new WaitForSeconds(0.15f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
    }
    IEnumerator Attack1RunAnim()
    {
        pop.role.ChangeState(MonsterMgr.RoleState.Attack1);
        yield return new WaitForSeconds(0.03f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        
    } 
    IEnumerator HitRoleRunAnim()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack1);
        yield return new WaitForSeconds(0.04f);
        pop.role.ChangeState(MonsterMgr.RoleState.Hit); 
    }
    IEnumerator ComboRunAnim()
    {
        pop.role.ChangeState(MonsterMgr.RoleState.Combo);  
        yield return new WaitForSeconds(0.07f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.25f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.25f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.01f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit); 
        yield return new WaitForSeconds(0.33f);
        pop.emeny.ChangeState(MonsterMgr.RoleState.Hit);
        yield return new WaitForSeconds(0.25f);
        pop.role.isComboing = false;
            
        
    }
    IEnumerator RedRunAnim()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack1); 
        yield return new WaitForSeconds(0.07f);
        pop.role.ChangeState(MonsterMgr.RoleState.Defense);
         
    }
    IEnumerator FastRunAnim()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.07f);
        pop.role.ChangeState(MonsterMgr.RoleState.Defense);

    }
    IEnumerator GroundRunAnim()
    {
        pop.emeny.ChangeState(MonsterMgr.RoleState.Attack2);
        yield return new WaitForSeconds(0.07f);
        pop.role.ChangeState(MonsterMgr.RoleState.Defense);

    }
    //人物 和战斗控制
#region 

    public void ChangeWinCount(int count)
    {
        roleWinCount = count;
        pop.huosheng.text = roleWinCount.ToString() + "：" + emenyWinCount; 
    }

    public void InitBattle(bool isFrist = true)
    {
        if (pop.emeny != null)
        {
            pop.emeny.gameObject.SetActive(false);
            Destroy(pop.emeny.gameObject); 
        }
        if (pop.role != null)
        {
            pop.role.gameObject.SetActive(false);
            Destroy(pop.role.gameObject); 
        }
        if (isFrist)
        {
            emenyWinCount = 0;
            roleWinCount = 0;
            battleCount = 0;
            ChangeWinCount(0);
        }
        
        InitRole();
        InitEmeny();
        winCountMax = pop.emeny.data.endWinCount;
        isTouchingSkill = false; 
        BattleStart();
        
    } 

    public void InitRole()
    {
        if (pop.role != null)
        {
            pop.role.gameObject.SetActive(false);
            Destroy(pop.role.gameObject);
        }
        GameObject go = Instantiate(pop.rolePrefab);
        go.transform.parent = pop.rolePos.transform.parent;
        go.transform.localScale = pop.rolePos.transform.localScale;
        go.transform.position = pop.rolePos.transform.position;
        go.transform.localRotation = pop.rolePos.transform.localRotation;
        pop.role = go.GetComponent<MonsterMgr>();
        pop.role.boolbar = pop.roleBoolbar;
        pop.role.gameObject.SetActive(true);
        pop.role.InitUser(roleData); 
    }

    public void InitEmeny()
    { 
        if (pop.emeny != null)
        { 
            pop.emeny.gameObject.SetActive(false);
            Destroy(pop.emeny.gameObject);
        }
        GameObject go = Instantiate(pop.emenyPrefab);
        go.transform.parent = pop.emenyPos.transform.parent;
        go.transform.localScale = pop.emenyPos.transform.localScale;
        go.transform.position = pop.emenyPos.transform.position;
        go.transform.localRotation = pop.emenyPos.transform.localRotation;
        pop.emeny = go.GetComponent<MonsterMgr>();
        pop.emeny.boolbar = pop.emenyBoolbar;
        pop.emeny.gameObject.SetActive(true);
        pop.emeny.InitEmeny(emenyData);
    }

    public void BattleStart()
    {
        pop.cursor.gameObject.SetActive(true);
        isBattle = true;
        pop.camera.Init();
        pop.cursor.init(GameShared.Instance.cursorSpeedList);
        pop.comboBar.Init();
        InitRolePos();

        comboLevel = 0; 
        isTouchcount = 0;
        comboLevel = 0;
        isTouching = false;
        isTouchingComboBox = false; 
        this.isCreateUser = true;

        sendQueue = new List<C2sSprotoType.BattleListElem>();

        timer = new PassiveTimer(5.0f);
        pop.spawner.EnableSpawn(true);
        pop.spawner.Restart(pop.role.data, pop.emeny.data);

        

        pop.roletxt.text = "战斗力：" + pop.role.data.fightPower + "暴击:" + pop.role.data.crit + "技巧:" + pop.role.data.skill + "防御:" + pop.role.data.defense;

        pop.emenytxt.text = "战斗力：" + pop.emeny.data.fightPower + "暴击:" + pop.emeny.data.crit + "技巧:" + pop.emeny.data.skill + "防御:" + pop.emeny.data.defense;

    }

    public void StopRedBox()
    {
        pop.cursor.RestPos();
        pop.cursor.Stop();
        pop.spawner.StopRed();
        Invoke("ResumeRedBox", 1.0f);
    }

    public void ResumeRedBox()
    {
        pop.cursor.gameObject.SetActive(true);
        pop.cursor.init(pop.role.data.cursorSpeedList);
        //spawner.MoveBoxBackAction();
    }

    public void BattleOver(int id)
    {
        int dead = 0;
        if (id == roleData.csv_id)
        { 
            dead = 1;
        }
        else
        { 
            dead = 2;
        }
        //关卡结束
        if (isArane == false)
        {
            isBattle = false;
            this.sendTimer = null;
            pop = null;
            LevelOver(dead); 
        }
        else//竞技场结束
        {
            if (isBattle != false)
            {
                NetworkManager.Instance.NextRole();
                ChangebattleRole();
            }

            if(curUserRoleIndex>2 || curEmenyRoleIndex >2)
            {
                isBattle = false;
                this.sendTimer = null;
                pop = null;
                AraneOver(dead);
            }
        } 

        if (sendQueue.Count>0)
            sendQueue[sendQueue.Count - 1].isdead = dead;

        this.SendQueue();
        this.ClearData();
    }
    public void AraneOver(int dead)
    {
        if (dead == 1)
        {
            ToastManager.Instance.Show("玩家胜利");
            LevelsMgr.Instance.BattleOver(Def.BattleOverType.Success);
        }
        else
        {
            ToastManager.Instance.Show("敌人胜利");
            LevelsMgr.Instance.BattleOver(Def.BattleOverType.Fail);
        }  
    }
    public void LevelOver(int dead)
    {
        if (dead == 1)
        {
            ToastManager.Instance.Show("玩家胜利");
            LevelsMgr.Instance.BattleOver(Def.BattleOverType.Success);
        }
        else
        {
            ToastManager.Instance.Show("敌人胜利");
            LevelsMgr.Instance.BattleOver(Def.BattleOverType.Fail);
        }
    }
 

#endregion

    public void StopGame()
    {
        Time.timeScale = 0;
         
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; 
    }

    public void ClearData()
    {

    }

    void InitEffect()
    {
        pop.cloudCreator.EnableSpawn(true);
        pop.cloudCreator.SetOnSpawn(InitCloud);
        ComboEffectEnable(false); 
    } 

    void InitCloud(GameObject obj)
    {
        float speed = Random.Range(0.1f, 0.5f);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    } 

    void Update()
    {
        if (isBattle)
        {
            if (sendTimer!=null && sendTimer.Update(Time.deltaTime) > 0)
            {
                SendQueue();
            }
             
            if (timer != null && timer.Update(Time.deltaTime) > 0)
            {

            }
        }  
    } 
 
    void touchZone_TouchUpEvent(Vector3 worldPos)
    { 
        if (Time.timeScale != 0)
        {
            if (isBattle)
            {
                isTouching = false;
                if (isTouchcount == 0 && pop.cursor.isEndCombo)
                {
                    if (pop.cursor.dragBox != null)
                        pop.cursor.dragBox.DragOver(true);  
                }
                else
                {
                    if (pop.cursor.dragBox != null)
                    {
                        pop.cursor.dragBox.haveStart = false;
                        pop.cursor.dragBox.DragOver(false);
                    }
                }
                isTouchingComboBox = false;
            }
        }
        isTouchingSkill = false;  
    }
    
    void touchZone_TouchingEvent(Vector3 worldPos)
    {
         

    }

    void touchZone_TouchDownEvent(Vector3 worldPos)
    {  
        if (!isTouchingSkill)
        {  
                if (Time.timeScale != 0)
                {
                    if (isBattle)
                    {
                        isTouchcount++;
                        isTouching = true; 
                        if (!isTouchingComboBox)
                        {
                            bool iscombo = pop.cursor.Click();
                            if (iscombo && pop.cursor.isStartCombo)
                            {
                                isTouchcount = 0;
                                isTouchingComboBox = true;
                                ComboTouchStart(); 
                            }
                        }
                    } 
            }
        }
    }

    public void ComboTouchStart()
    {
        //spawner.RemoveAllStatic();
    }

    
    //块消除
#region
    //盾牌没被击破的点击处理
    public void ClickGroundBox(GameObject obj, ClickBox.BoxType type ,bool isNotAdd=false)
    {
        if (isNotAdd == false)
            ComboLevelAdd();
        RoleChange(type,false);
    }

    //静态块点和移动块点中
    public void ClickBoxOver(GameObject obj, ClickBox.BoxType type, bool isNotAdd = false)
    {
        if(isNotAdd == false)
            ComboLevelAdd();
        pop.spawner.Remove(obj); 
        RoleChange(type,false);
    }

    //静态块点和移动块点中
    public void ClickBoxOver(ClickBox.BoxType type, bool isNotAdd = false)
    {
        if (type == ClickBox.BoxType.None)
        {
            ComboLevelRest();
        }
        else
        {
            ComboLevelAdd(); 
        }
        RoleChange(type,false); 
    }
    //红块移动到最左
    public void MoveBoxOver(GameObject obj, ClickBox.BoxType type, bool isNotAdd = false)
    {
        ComboLevelRest();
        pop.spawner.Remove(obj); 
        RoleChange(type,true); 
    }
    //combo点击失败
    public void ComboHitOver(ClickBox.BoxType type, bool isNotAdd = false)
    {
        ComboLevelRest(); 
        RoleChange(type, true);
    }
    
    //拖动块的连续combo
    public void DragComboOver(GameObject obj, List<GameObject> list, bool isNotAdd = false)
    {  
        for(int i=0;i<list.Count;i++)
        {
            ComboLevelAdd();
            pop.spawner.Remove(list[i]);
        }
        pop.spawner.Remove(obj);
        pop.bg.DouDong(5); 
        RoleChange(ClickBox.BoxType.Skill);
        //spawner.ResumenCycle(BlockManager.StaticList);
    }

    //炸弹爆炸
    public void BombOver(GameObject obj, List<GameObject> list, bool isNotAdd = false)
    {
        ComboLevelAdd();
        for (int i = 0; i < list.Count; i++)
        {
            pop.spawner.Remove(list[i]);
        }
        pop.spawner.Remove(obj); 
        RoleChange(ClickBox.BoxType.BombBox,false);
    }
    //红块移动到最右边消失
    public void RedRightOver(GameObject obj, ClickBox.BoxType type, bool isNotAdd = false)
    {
        pop.spawner.Remove(obj); 
    }
 

    //combo释放
    public void ComboRelease()
    { 
        if (comboLevel >= skillReleaseCount && isBattle)
        {
            RoleChange(ClickBox.BoxType.Skill); 
            ComboLevelRestNotCamera(); 
            //spawner.StopCycle(BlockManager.StaticList);
            pop.bg.DouDong(5); 
        }
    }
#endregion
}
