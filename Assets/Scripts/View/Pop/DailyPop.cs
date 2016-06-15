using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DailyPop : MonoBehaviour {

    const int REWAREDMAX = 3;
    const int DATEMAX = 30;  
    public enum DailyTab
    {
        Sign,
        DuanLian,
        Gold
    }

    public GameObject datePrefab;
    public GameObject rewaredPrefab;

    public UISprite bg; 

    public DailyTabView[] tabList;
    public GameObject sign;
    public GameObject duanlian;
    public GameObject gold;
    public UILabel qiandaoNum;

    public UIScrollView dateScroll;
    public UITable dateTable;
    public UIScrollView rewaredScroll;
    public UITable rewaredTable;

    public UIButton rewaredBtn;

    public UILabel duanLastTime;
    public UILabel duanlevel;
    public UIButton duanBtn;

    public UILabel goldLastTime;
    public UILabel goldlevel;
    public UIButton goldBtn;

    private Hashtable dateListView;

    public DailyMissionView[] duanlianCheckList;
    public DailyMissionView[] goldCheckList;

    private DailyTab curTab;
    private int curRewaredCount;

    private PassiveTimer timer;

    private DateTime duanTime;
    private DateTime goldTime;

    private bool isQianDao;

    void Start () { 

	}

    public void SetDuanTime(DateTime t)
    {
        duanTime = t;
    }

    public void SetGoldTime(DateTime t)
    {
        goldTime = t;
    }
  
    public void CheckBtnTime()
    {  
        string str = Comm.DateDiffHour(DateTime.Now, duanTime);
        if (str == "")
        {
            duanLastTime.gameObject.SetActive(false);
            duanBtn.gameObject.SetActive(true);
        }
        else
        {
            duanLastTime.gameObject.SetActive(true);
            duanBtn.gameObject.SetActive(false);
            duanLastTime.text = str;
        }
        string str2 = Comm.DateDiffHour(DateTime.Now, goldTime);
        if (str2 == "")
        {
            goldLastTime.gameObject.SetActive(false);
            goldBtn.gameObject.SetActive(true);
        }
        else
        {
            goldLastTime.gameObject.SetActive(true);
            goldBtn.gameObject.SetActive(false);
            goldLastTime.text = str2;
        } 
    }

    void SetReciveBtn(bool b)
    { 
        rewaredBtn.gameObject.SetActive(b);  
    }

    void Update()
    {
        if (timer!=null && timer.Update(Time.deltaTime) > 0)
        {
            CheckBtnTime();
        }
    }

    public void InitData()
    {
        duanTime = DateTime.Now;
        goldTime = DateTime.Now;
        timer = new PassiveTimer(1);
       
        DailyMgr.Instance.OpenPop(this);
     
    }
	 

    public void SetTab()
    {
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].InitData(this, (DailyTab)i);
        }
    }

    public void ReceiveClick()
    {
        DailyMgr.Instance.Receive(); 
    }

    public DailyView GetDailyView(int i)
    {
        return ((DailyView)dateListView[i + 1]);
    } 

    public void GoldClick()
    {

        DailyMgr.Instance.Gold();  
    }

    public void DuanLianClick()
    {
        
        DailyMgr.Instance.DuanLian();   
    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.Daily, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
    public void TabChange(DailyTab i)
    {
        if (i != curTab)
        {
            tabList[(int)curTab].SetState(false);
            curTab = i;
            tabList[(int)curTab].SetState(true); 
            if (curTab != DailyTab.Sign)
            {
                bg.spriteName = "面板2";
            }
            else
            {
                bg.spriteName = "面板1";
            }
            sign.SetActive(false);
            duanlian.SetActive(false);
            gold.SetActive(false); 
            switch(curTab)
            {
                case DailyTab.Sign:
                    sign.SetActive(true);
                    DailyMgr.Instance.DailyList();
                    break;
                case DailyTab.DuanLian:
                    duanlian.SetActive(true);
                    DailyMgr.Instance.SetDuanLian(); 
                    break;
                case DailyTab.Gold:
                    gold.SetActive(true);
                    DailyMgr.Instance.SetGold();  
                    break;
            }
        }
    } 

    public void SetQianDao(List<QianDaoData> list)
    { 
        while (dateTable.transform.childCount > 0)
        {
            DestroyImmediate(dateTable.transform.GetChild(0).gameObject);
        }
        dateListView = new Hashtable();
        dateListView.Clear();
        for (int index = 0; index< list.Count; index++)
        {  
                GameObject obj = Instantiate(datePrefab); 
                obj.SetActive(true); 
                DailyView pop = obj.GetComponent<DailyView>();
                pop.InitData(this, list[index]);
                obj.name = list[index].sort.ToString();
                pop.transform.parent = dateTable.transform;
                pop.transform.localPosition = Vector3.zero;
                pop.transform.localScale = Vector3.one;
                dateListView.Add(list[index].num, pop); 
        }  
        dateTable.Reposition();
        dateScroll.ResetPosition(); 
    } 
   
    public void SetRewared(List<ItemViewData> list)
    {
        while (rewaredTable.transform.childCount > 0)
        {
            DestroyImmediate(rewaredTable.transform.GetChild(0).gameObject);
        }  
        if (list != null)
        {
            for (int index = 0; index < list.Count; index++)
            {
                GameObject obj = Instantiate(rewaredPrefab);
                obj.SetActive(true);
                ItemView pop = obj.GetComponent<ItemView>();
                ItemViewData data = null;
                obj.name = index.ToString();
                if (index < list.Count)
                {
                    data = list[index];

                    if (data.curCount < 1)
                        data = null;
                }
                else
                {
                    obj.name = "None";
                }
                pop.InitData(data, index, false);
                pop.transform.parent = rewaredTable.transform;
                pop.transform.localPosition = Vector3.zero;
                pop.transform.localScale = Vector3.one;
            }
        }
        rewaredTable.Reposition();
        rewaredScroll.ResetPosition();
    }

    public void SetDuanLian(List<DailyData> list) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            duanlianCheckList[i].InitData(this, list[i]);
        }  
    }
    public void SetGold(List<DailyData> list)
    {
        for (int i = 0; i <list.Count; i++)
        {
            goldCheckList[i].InitData(this, list[i]);
        }  
    }
}
