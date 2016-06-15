using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRewardPop : SPSGame.Unity.UIWndBase
{
    public UILabel rank;
    public ItemView v1;
    public ItemView v2;
    public ItemView v3;
    public UIScrollView scroll;
    public UIGrid grid;
    public GameObject close;
    public List<ArenaRewardView> list;
    void Awake()
    {
        ListenOnClick(close, CloseClick); 
        EventManager.Register<EventArenaRewardList>(SetList);
    }

	// Use this for initialization
	void Start () {
        InitData(); 
    }

    public void SetRewared()
    {
        if (ArenaMgr.Instance.rankRewardList == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetReward(ArenaMgr.Instance.rankRewardList[i]); 
            if (ArenaMgr.Instance.user_rank <= list[i].data.rank)
            {
                list[i].SetMask(true);
            }
            else
            {
                list[i].SetMask(false);
            }
        }
    }

    public void SetList(EventArenaRewardList arg)
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        if (arg.list != null)
        {
            list = new List<ArenaRewardView>();
            for (int i = 0; i < arg.list.Count; i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Ui/View/ArenaRewardView"));
                obj.SetActive(true);
                ArenaRewardView pop = obj.GetComponent<ArenaRewardView>();
                pop.InitData(arg.list[i]);
                pop.transform.parent = grid.transform;
                pop.transform.position = Vector3.zero;
                pop.transform.localScale = Vector3.one;
                list.Add(pop);
            }
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
        }
        SetRewared();
    } 

    public void Rewared()
    {
        NetworkManager.Instance.AraConvertPts(1);
    }

    public void InitData()
    {
        ActionParam param = new ActionParam();
        GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaSwaredList, param);
    } 

    public void CloseClick(GameObject obj)
    {
        EventManager.Remove<EventArenaRewardList>(SetList);
        UIManager.Instance.HideWindow<ArenaRewardPop>();
    }

    public void Destory()
    {
        UIManager.Instance.RemoveWindow<ArenaRewardPop>();
    }  
}
