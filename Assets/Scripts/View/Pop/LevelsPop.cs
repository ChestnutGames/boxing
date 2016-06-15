using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


//关卡界面
public class LevelsPop : MonoBehaviour {

    
    public UILabel name;
    public UILabel chapter;
    public UILabel coin;
    public UILabel exp;
    public UILabel time;
    public UISlider prograss;
    public UILabel guan; 

    public LevelView[] levelViewList;

    public DiffcutTabView[] tabList;
    public Def.levelType curFousTab;

    public GameObject role;
    public GameObject rightPos;
    public GameObject LeftPos;

    public UIButton battleBtn;

    public LevelView curLevelView; 

    public void TabChange(Def.levelType type)
    {
        if (type == curFousTab) return;
        curFousTab = type;
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].SetState(false);
            if (i == (int)type)
                tabList[(int)type].SetState(true);
        }
        LevelsMgr.Instance.SelectDifficulty(type);
    }

    public void SetTab()
    {
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].InitData(this, (Def.levelType)i);
            tabList[i].SetState(false);
        }
    }

    public void InitData(ChapterData d)
    {
        LevelsMgr.Instance.OpenLevel(this, d);
        
    }
    public void SetInfo(LevelView v)
    {
        curLevelView = v;
        name.text = v.data.data.name;
        chapter.text = "第" + v.data.data.chapter + "章";
        coin.text = v.data.data.gain_gold+ "/s";
        exp.text = v.data.data.gain_exp + "/s";
        
        guan.text = v.data.data.chapter + "-" + v.data.data.checkpoint; 
    }

    public void SetPrograss(DateTime t,float v)
    {
        time.text = Comm.DateDiffHour(DateTime.Now, t);
        prograss.value = v; 
    }


    public void SetLevelList(List<LevelViewData> list)
    {
        for (int i = 0; i < levelViewList.Length; i++)
        {
            if (i < list.Count)
            {
                levelViewList[i].InitData(list[i]);
            }
            else
            {
                levelViewList[i].RestEmpty();
            } 
        } 
    }

    public void BattletClick()
    {
        LevelsMgr.Instance.BattleStart();
    }
     
    public void ReturnClick()
    {
        LevelsMgr.Instance.CloseLevelPop();
        MainUI.Instance.SetPopState(MainUI.PopType.Level, false);
        this.gameObject.SetActive(false);
        Destroy(this); 
    }  
}
