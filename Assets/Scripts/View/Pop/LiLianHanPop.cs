using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiLianHanPop : MonoBehaviour {

    public UIScrollView scroll;
    public UITable table;
    public GameObject prefab;

    public LiLianHallData data;

    public void InitData(LiLianHallData d)
    {
        data = d;
        LiLianMgr.Instance.OpenHanPop(this);
    }

    public void InitData()
    { 
        LiLianMgr.Instance.OpenHanPop(this);
    }

    public void SetLiLianList(List<ItemViewData> list, bool click = true)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        } 
        for (int index = 0; index < list.Count; index++)
        {
            //设置格子
            GameObject obj = Instantiate(prefab); 
            CardView pop = obj.GetComponent<CardView>(); 
            pop.InitData(list[index],click); 
            pop.transform.parent = table.transform;
            pop.transform.localPosition = Vector3.zero;
            pop.transform.localScale = Vector3.one;
            pop.gameObject.SetActive(true);
        }
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true; 
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.LiLianHan, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    } 
}
