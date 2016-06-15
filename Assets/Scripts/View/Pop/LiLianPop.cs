using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LiLianPop : MonoBehaviour {

    public GameObject lilianPrefabe;
    public GameObject runPrefabe;
    public UILabel level;
    public UILabel strength;
    public UILabel name;

    public UIProgressBar strengthBar;
    public UIProgressBar levelBar;
    public UILabel levelBarTxt;
    public UILabel strengthBarTxt;

    public UIScrollView scroll;
    public UITable table;

    public UIScrollView timeScroll;
    public UITable timeTable;

    public UIButton inviteBtn;
    public UIButton chargenBtn;

    public UILabel timeTxt;
    public UILabel totalTimeTxt;
    public UILabel NumTxt;

    public Dictionary<int, LiLianView> viewList;

    public LiLianRunView eventView;



	// Use this for initialization
	void Start () { 

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public LiLianView GetViewById(int id)
    {
       return viewList[id];
    }

    public void InitData()
    { 
        LiLianMgr.Instance.OpenPop(this); 
    }
 

    

    public void OpenChongZhi()
    {
        //todo
        LiLianMgr.Instance.OpenBuyStrength();
    }

    public void InviteClick()
    {
        LiLianMgr.Instance.OpenCard();
    }

    public void SetLiLianList(List<LiLianViewData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new Dictionary<int, LiLianView>();
        for (int index = 0; index < list.Count; index++)
        {
            //设置格子
            GameObject obj = Instantiate(lilianPrefabe);
            obj.SetActive(true);
            LiLianView pop = obj.GetComponent<LiLianView>(); 
            pop.InitData(list[index]); 
            pop.transform.parent = table.transform;
            pop.transform.localPosition = Vector3.zero;
            pop.transform.localScale = Vector3.one;
            viewList.Add(list[index].data.csv_id, pop);
        } 
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true; 
    }

    public void SetLiLianRun(List<LiLianRunData> list)
    {
        while (timeTable.transform.childCount > 0)
        {
            DestroyImmediate(timeTable.transform.GetChild(0).gameObject);
        }
        runViewList = new List<LiLianRunView>();
        for (int index = 0; index < list.Count; index++)
        {
            //设置格子
            GameObject obj = Instantiate(runPrefabe);
            obj.SetActive(true);
            LiLianRunView pop = obj.GetComponent<LiLianRunView>();
            pop.InitData(list[index]);
            list[index].view = obj.GetComponent<UILabel>();
            pop.time.text = Comm.DateDiffHour(DateTime.Now, list[index].time);
            pop.transform.parent = timeTable.transform;
            pop.transform.localScale = Vector3.one;
            runViewList.Add(pop); 
        }
        timeTable.Reposition();
        timeScroll.ResetPosition();
        timeTable.repositionNow = true; 
    }

    public void SetLiLianRunEvent(LiLianRunData r)
    {
        eventView.InitData(r);
        eventView.RestView(r);
    }

    public void AddLiLianRunView(LiLianRunData r)
    {
        r.state = 0;
        GameObject obj = Instantiate(runPrefabe);
        obj.SetActive(true);
        LiLianRunView pop = obj.GetComponent<LiLianRunView>();
        pop.InitData(r);
        r.view = obj.GetComponent<UILabel>();
        pop.time.text = Comm.DateDiffHour(DateTime.Now,r.time);
        pop.transform.parent = timeTable.transform;
        pop.transform.localScale = Vector3.one;
        runViewList.Add(pop);
        timeTable.Reposition(); 
        timeTable.repositionNow = true; 
    }

    public List<LiLianRunView> runViewList;
    public LiLianRunView GetLiLianRunView(int id)
    {
        return runViewList[id];
    }
     


    public void CloseClick()
    { 
        LiLianMgr.Instance.ClosePop();
        MainUI.Instance.SetPopState(MainUI.PopType.LiLian, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
