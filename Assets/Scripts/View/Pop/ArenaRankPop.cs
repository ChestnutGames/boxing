using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRankPop : SPSGame.Unity.UIWndBase {

    public UILabel rank;
    public UIScrollView scroll;
    public UIGrid grid;
    public GameObject close;
    Dictionary<int, ArenaRankView> viewTable;
    public void Awake()
    {
        ListenOnClick(close, CloseClick);
        EventManager.Register<ArenaRank100List>(ArenaRank100List);
         
    }

    public void Start()
    {
        NetworkManager.Instance.ArenaRankList();
    }

    public void SetInfo()
    {
        rank.text = ArenaMgr.Instance.user_rank.ToString();
    }

    public void ArenaRank100List(ArenaRank100List arg)
    {
        SetInfo();
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        if (arg.list != null)
        {
            viewTable = new Dictionary<int, ArenaRankView>();
            for (int i = 0; i < arg.list.Count; i++)
            { 
                GameObject obj = Instantiate(ResourceManager.Load<GameObject>("Prefabs/Ui/View/ArenaRankView"));
                obj.SetActive(true);
                ArenaRankView pop = obj.GetComponent<ArenaRankView>();
                pop.InitData(arg.list[i]);
                pop.transform.parent = grid.transform;
                pop.transform.position = Vector3.zero;
                pop.transform.localScale = Vector3.one;
                viewTable.Add(arg.list[i].ara_rnk, pop);
            }
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
        }
    }

	 
    public void CloseClick(GameObject obj)
    {
        EventManager.Remove<ArenaRank100List>(ArenaRank100List);
        UIManager.Instance.HideWindow<ArenaRankPop>();
    }

    public void Destory()
    {
        UIManager.Instance.RemoveWindow<ArenaRankPop>();
    } 
	 
}
