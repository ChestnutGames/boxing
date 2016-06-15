using UnityEngine;
using System.Collections;

public class BillingMgr : UnitySingleton<BillingMgr>
{
    //private List<AchievementView> viewList;
    //private List<AchievementViewData> dataList;
    //private AchievementView curCallBackView;
    //public AchievementPop pop;

    //private LuaState l;

    //private bool isOpen;

    //public bool IsOpen
    //{
    //    get { return isOpen; }
    //    set { isOpen = value; }
    //}

    //public void OpenPop(AchievementPop p)
    //{
    //    pop = p;
    //}

    //public void Start()
    //{
    //    InitLua();
    //}

    //public void InitLua()
    //{
    //    TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Achievement);
    //    LuaState l = new LuaState();
    //    l.DoString(scriptFile.text);

    //    LuaFunction f = l.GetFunction("InitLua");
    //    f.Call(this);
    //}

    //public void GetAchievementList()
    //{
    //    if (ClientSockt.Instance.connected)
    //    {
    //        NetworkManager.Instance.AchievementList();
    //    }
    //    else
    //    {
    //        SetPopList(dataList);
    //    }
    //}

    //public void SetPopList(List<AchievementViewData> list)
    //{
    //    viewList = pop.SetItemList(list);
    //}
}
