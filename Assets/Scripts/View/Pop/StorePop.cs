using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorePop : MonoBehaviour {

    public UIScrollView scroll;
    public UITable table;

    public GameObject itemPrefab;
    public GameObject popPrefab;

    public List<StoreView> viewList;
    public int curFouse;
    public int tabid;
    void Start()
    {
        
    }

    public void InitData(int tab)
    {
        tabid = tab;
        StoreMgr.Instance.OpenPop(this);
        
    }

    public void NormalClick()
    {
        if(tabid!=1) 
            StoreMgr.Instance.SetListByTab(1);
    }

    public void PointClick()
    {
        if(tabid!=2)
            StoreMgr.Instance.SetListByTab(2);
    }


    public List<StoreView> SetList(List<ProductData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new List<StoreView>();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(true);
            StoreView v = obj.GetComponent<StoreView>();
            v.InitData(this, list[i], i);
            v.gameObject.name = list[i].csv_id.ToString();
            v.transform.parent = table.transform;
            v.transform.localPosition = Vector3.zero;
            v.transform.localScale = Vector3.one;
            viewList.Add(v); 
        } 
        //始终两行
        int col = viewList.Count / 2;
        if (viewList.Count % 2 > 0)
        {
            col++;
        }
        table.columns = col;
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true;
        return viewList;
    }

    public void ItemClick(int i)
    { 
        StoreMgr.Instance.OpenBuyPop(i); 
    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.Store, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
	
}
