using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPointPop : SPSGame.Unity.UIWndBase
{
    public UIScrollView scroll;
    public UIGrid grid;
    public UILabel point;
    public GameObject btn;
    public GameObject close;
    public List<ArenaPointView> list;
	// Use this for initialization
    void Awake()
    {
        ListenOnClick(close, CloseClick);
        ListenOnClick(btn, ReviveAll); 
        EventManager.Register<EventPointRewardList>(SetList);
    }

    void Start()
    {
        InitData();
    }

    public void InitData()
    {
        ActionParam param = new ActionParam(); 
        GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaPointList, param); 
    }

    public void SetInfo()
    {
        point.text = ArenaMgr.Instance.integral.ToString();
    }

    public void SetRewared()
    {
        if (ArenaMgr.Instance.pointRewardList == null) return;
        for (int i = 0; i < ArenaMgr.Instance.pointRewardList.Count; i++)
        {
            list[i].SetReward(ArenaMgr.Instance.pointRewardList[i]);
            if (ArenaMgr.Instance.integral <= list[i].data.num)
            {
                list[i].SetMask(true);
            }
            else
            {
                list[i].SetMask(false);
            }
        }
    }
    public void SetList(EventPointRewardList arg)
    {
        SetInfo();
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        if (arg.list != null)
        {
            list = new List<ArenaPointView>();
            for (int i = 0; i < arg.list.Count; i++)
            { 
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Ui/View/ArenaPointView"));
                obj.SetActive(true);
                ArenaPointView pop = obj.GetComponent<ArenaPointView>();
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
        point.text = ArenaMgr.Instance.integral.ToString();
        SetRewared();
    }

    public void ReviveAll(GameObject obj)
    {
        NetworkManager.Instance.AraConvertPts(ArenaMgr.Instance.integral);
    }

    public void CloseClick(GameObject obj)
    {
        EventManager.Remove<EventPointRewardList>(SetList);
        UIManager.Instance.HideWindow<ArenaPointPop>();
    }

    public void Destory()
    {
        UIManager.Instance.RemoveWindow<ArenaPointPop>();
    }
}
 