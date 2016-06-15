using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArenaPop : SPSGame.Unity.UIWndBase
{
    public UILabel battle;
    public UILabel rank;
    public UILabel battleNum;
    public UILabel time;

    public UIButton refreshBtn;

    public UIGrid grid;
    public UIScrollView scroll;

    public GameObject close;

    Dictionary<int, ArenaView> viewTable;

    private PassiveTimer timer;

    public GameObject prefab;

    private DateTime refreshTime;

    public void Awake()
    {
        ListenOnClick(close, CloseClick);
        EventManager.Register<EventArenaInfo>(SetList);
    }

    public void SetInfo()
    {
        battle.text = ArenaMgr.Instance.user_power.ToString();
        rank.text = ArenaMgr.Instance.user_rank.ToString();
        battleNum.text = (ArenaMgr.Instance.win_tms + ArenaMgr.Instance.lose_tms).ToString() + "/" + GameShared.Instance.config.arena_battle_max;
        refreshTime = DateTime.Now.AddSeconds(ArenaMgr.Instance.cd); 
        string str = Comm.DateDiffHour(DateTime.Now, refreshTime);
        this.time.text = str;
    }

    public void SetList(EventArenaInfo arg)
    {
        SetInfo();
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        if (arg.list != null)
        {
            viewTable = new Dictionary<int, ArenaView>();
            for (int i = 0; i < arg.list.Count; i++)
            {
                ArenaUserData r = arg.list[i];
                if (!viewTable.ContainsKey(r.csv_id))
                { 
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(true);
                    ArenaView pop = obj.GetComponent<ArenaView>();
                    pop.InitData(r);
                    pop.transform.parent = grid.transform;
                    pop.transform.position = Vector3.zero;
                    pop.transform.localScale = Vector3.one; 
                    viewTable.Add(r.csv_id, pop);
                }
            }
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
        }


    }

    public void InitData()
    {
        ArenaMgr.Instance.ArenaOpen(this);
        timer = new PassiveTimer(1);

    }

    public void RefreshClick()
    {
        if(time.text.Equals(""))
            ArenaMgr.Instance.RefreshList();
    }

    public void RankClick()
    {
        UIManager.Instance.ShowWindow<ArenaRankPop>();
    }

    public void StoreClick()
    {
        MainUI.Instance.StorePopClick(2);
    }

    public void SwaredClick()
    {
        UIManager.Instance.ShowWindow<ArenaRewardPop>();
    }

    public void PointClick()
    {
        UIManager.Instance.ShowWindow<ArenaPointPop>();
    } 

    public void CloseClick(GameObject obj)
    { 
        EventManager.Remove<EventArenaInfo>(SetList); 
        UIManager.Instance.HideWindow<ArenaPop>(); 
        NetworkManager.Instance.ArenaExit();
    }

	// Use this for initialization
	//void Start () {
	
	//}
	
	// Update is called once per frame
	void Update () {
        if (refreshTime != null && timer != null && timer.Update(Time.deltaTime) > 0)
        {
            string str = Comm.DateDiffHour(DateTime.Now, refreshTime);
            this.time.text = str;
        }
    }
}
