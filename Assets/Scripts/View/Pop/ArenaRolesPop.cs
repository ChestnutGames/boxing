using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRolesPop : SPSGame.Unity.UIWndBase
{ 
 
    public UIButton BattleBtn;
    public GameObject close;
     
    public UITable table;
    public UIScrollView scroll;

    public GameObject rolePrefabe;

    private Hashtable viewList;

    public HeadRole curView;

    public ArenaRoleView[] battleViewArr;

    public ArenaRoleView curRoleView;


    public HeadRole GetRoleView()
    {
        return curView;
    }

    void Start()
    {
        InitData();
    }

    public void InitData()
    {
        ArenaMgr.Instance.RoleChoosePop(this);
        ListenOnClick(close, CloseClick); 
        EventManager.Register<EventArenaBattleRoleList>(SetList);
        SetTable(ref UserManager.Instance.RoleTable);
        curRoleView = battleViewArr[0];
    }

    public bool CheckHasRole(RoleData d)
    {
        bool flag = false; 
        for (int i = 0; i < battleViewArr.Length; i++)
        {
            if (battleViewArr[i].data !=null && battleViewArr[i].data.csv_id == d.csv_id)
            {
                flag = true;
            } 
        }
        return flag;
    }

    public void SetBattleRole(RoleData d)
    {
        if(!CheckHasRole(d))
        { 
            if (d != null && curRoleView != null )
            {
                curRoleView.InitData(d); 
            }
            else if(d!=null)//之前一个都没选
            {
                battleViewArr[0].InitData(d);
            }
        }
    }

    public void SetList(EventArenaBattleRoleList arg)
    {
        if (arg.list != null)
        {
            for (int i = 0; i < battleViewArr.Length; i++)
            {
                RoleData d = UserManager.Instance.RoleTable[(int)arg.list[i]] as RoleData;
                battleViewArr[i].InitData(d);
            }
        }
        
    }

    public void SetRoleView(ArenaRoleView v)
    {
        curRoleView = v;

    }


    public void Battle()
    {
        ArenaMgr.Instance.BattleEnter();
    }
     

    public void SetTable(ref Hashtable list)
    { 
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new Hashtable(); 
        foreach (RoleData r in UserManager.Instance.RoleTable.Values)
        {
            //RoleData r = vv as RoleData;//UserManager.Instance.RoleTable[enumerator.Key] as RoleData; 
            if (r.is_possessed)
            {
                GameObject obj = Instantiate(rolePrefabe);
                obj.SetActive(true);
                ArenaRoleHeadView pop = obj.GetComponent<ArenaRoleHeadView>();
                obj.name = r.csv_id.ToString();
                pop.InitData(r);
                pop.transform.parent = table.transform;
                pop.transform.position = Vector3.zero;
                pop.transform.localScale = Vector3.one;
                viewList.Add(r.csv_id, pop);
            }
        }
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true;
    }

    public HeadRole GetCurView()
    {
        return curView;
    }

    public void SetCurView(HeadRole role)
    {
        curView = role;
        role.SetFous(true);
    }

    public HeadRole GetItemView(int i)
    {
        return viewList[i] as HeadRole;
    } 
 
     public void CloseClick(GameObject o)
    {
        EventManager.Remove<EventArenaBattleRoleList>(SetList);
        UIManager.Instance.HideWindow<ArenaRolesPop>();
    }
 
}
