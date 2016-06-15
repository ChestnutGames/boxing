using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class BagPop : MonoBehaviour {

    public enum TabType
    {
        All = 0,
        Good = 1,
        Material = 2, 
        Boxing = 3,
        Fashion = 4 
    }   

    private const int MAXROW = 5;
    private const int MAXCUL = 10;
    private const int MAXBLOCK = 50;

    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private UIScrollView scroll;
    [SerializeField]
    public UITable grid;
    [SerializeField]
    private UILabel desc;
    [SerializeField]
    private UILabel name;
    [SerializeField]
    private UILabel count;
    [SerializeField]
    private UILabel type;
    [SerializeField]
    private UISprite icon;
    public UISprite kuang;
    public GameObject usePrefab;
    public Hashtable viewList;

    public UIButton useBtn;
     

    public TabView[] tabList;
    private TabType curFousTab;
    

    private List<ItemViewData> allList;
     


	// Use this for initialization
	void Start () {
        InitData();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitData()
    {
        BagMgr.Instance.OpenPop(this);
        
     }  

    public void TabChange(TabType id)
    {
        if (id == curFousTab) return;
        curFousTab = id;
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].SetState(false);
            if (i == (int)id)
                tabList[(int)id].SetState(true);
        }
        SetContent(id);
    }
 
    public void SetList(List<ItemViewData> list)
    {
        allList = list;
        SetContent(TabType.All);
    }  

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.Bag, false);
        this.gameObject.SetActive(false);
        Destroy(this); 
    }

    public void UseItemClick()
    { 
       BagMgr.Instance.UseItem();  
    }
    
     

    public void ShowPathClick()
    {
        //todo 弹窗
        BagMgr.Instance.UseTest();
    } 
   

    public void SetTab()
    {
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].InitData(this, (BagPop.TabType)i);
        }
    }

    public void SetContent(TabType tabid)
    {
        if (allList!=null)
        {
            InitBag(allList, tabid);
        }
    }
    public void SetCount(int i)
    {
        count.text = i.ToString();
    }
    //tolua
    public void SetItemDesc(ItemView v)
    {
        if (v.data != null)
        {
            Debug.Log("fouse id" + v.data.data.id + "num" + v.data.curCount);
            v.SetFous(true);
            kuang.spriteName = v.data.kuang; 
            desc.text = v.data.data.desc;
            name.text = v.data.data.name;
            count.text = v.data.curCount.ToString();
            icon.spriteName = v.data.data.path; 
            useBtn.isEnabled = v.data.isUse;
        }
        else
        {
            kuang.spriteName = "ffe";
            desc.text = "";
            name.text = "";
            count.text = "";
            icon.spriteName = "ffe";
            type.text = "";
            useBtn.isEnabled = false;
        }
    }

    //tolua
    public void InitBag(List<ItemViewData> list,TabType type)
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        } 
        viewList = new Hashtable();
        for (int index = 0; index < MAXBLOCK; index++)
        { 
                //设置格子
                GameObject obj = Instantiate(itemPrefab);
                obj.SetActive(true);
                ItemView pop = obj.GetComponent<ItemView>(); 
                ItemViewData data = null;
                if (index < list.Count)
                { 
                        data = list[index];
                        if (data != null)
                        {
                            //如果指定类型就给添加到table
                            if ((type == TabType.All || type == (TabType)data.data.bagType) && data.data.isShow == 0)
                            {
                                obj.name = data.sort;
                                if (data.curCount < 1)
                                    data = null;
                            }
                            else
                            {
                                data = null;
                            }
                            if (data == null)
                                obj.name = "None"; 
                        } 
                }
                else
                {
                    obj.name = "None";
                } 
                pop.InitData(data, index);
                if(data!=null)
                    data.view = pop;
                pop.transform.parent = grid.transform;
                pop.transform.localScale = Vector3.one;
                viewList.Add(index,pop);
        }
        
        grid.Reposition();
        scroll.ResetPosition();
        grid.repositionNow = true;
        if (list != null && list.Count > 0)
        {  
            BagMgr.Instance.SelectItem(grid.GetChildList()[0].gameObject.GetComponent<ItemView>()); 
        }
        //MoveToLastVertical();
    }
     
    public void MoveToLastVertical()
    {
        Vector3[] corners = scroll.panel.worldCorners;
        Vector3 panelBottom = (corners[0] + corners[3]) * 0.5f;

        Transform panelTrans = scroll.panel.transform;

        Vector3 cp = panelTrans.InverseTransformPoint(grid.GetChildList()[0].transform.position);
        // UITable 中的条目根对象为 UISprite，居中对齐高度为 150，所以得到条目中心点在 Panel 中的坐标之后再加上其高度的一半，获得底端条目的底端坐标
        cp = new Vector3(cp.x, cp.y + 75, cp.z);
        Vector3 cc = panelTrans.InverseTransformPoint(panelBottom);
        Vector3 localOffset = cp - cc;

        // Offset shouldn't occur if blocked
        if (!scroll.canMoveHorizontally)
        {
            localOffset.x = 0f;
        }
        if (!scroll.canMoveVertically)
        {
            localOffset.y = 0f;
        }
        localOffset.z = 0f;

        //        SpringPanel.Begin (scrollView.panel.cachedGameObject,
        //            panelTrans.localPosition - localOffset, 13);
        scroll.DisableSpring();
        scroll.MoveRelative(new Vector3(localOffset.x, -localOffset.y, localOffset.z));
    }


    public void InitBag()
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        viewList = new Hashtable();
        for (int index = 0; index < MAXBLOCK; index++)
        {
            //设置格子
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(true);
            ItemView pop = obj.GetComponent<ItemView>();
            ItemViewData data = null; 
            obj.name = "None"; 
            pop.InitData(data, index);
            pop.transform.parent = grid.transform;
            pop.transform.localScale = Vector3.one;
            viewList.Add(index, pop);
        }
        grid.Reposition();
        grid.repositionNow = true;  
    } 

    public void AddItem(ItemViewData data) 
    { 
        for (int index = 0; index < MAXBLOCK; index++)
        { 
            ItemView iv = viewList[index] as ItemView;
            if (iv.name == "None")
            {
                //设置格子   
                if (data != null)
                { 
                    //如果指定类型就给添加到table
                    if ((curFousTab == (int)TabType.All || data.data.bagType == (int)curFousTab) && data.data.isShow == 0)
                    {
                        iv.name = data.sort;
                        iv.InitData(data, index);
                        if (data != null)
                            data.view = iv;
                    } 
                }
                break;
            }
        } 
        grid.Reposition();  
        grid.repositionNow = true; 
    }  
  
}
