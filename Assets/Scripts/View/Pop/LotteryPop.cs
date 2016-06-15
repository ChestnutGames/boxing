 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LotteryPop : MonoBehaviour 
{ 
    public GameObject itemPrefab;
    public GameObject bilingPrefab;
    public UIScrollView scroll;
    public UIGrid table; 

    public  List<LotteryView> viewList; 
     
    public LotteryView cur = null;
     
     
    public void InitData()
    {
       LotterMgr.Instance.OpenPop(this); 
    }
     

    public void SetList(List<LotteryData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new List<LotteryView>();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(true);
            LotteryView v = obj.GetComponent<LotteryView>();
            v.InitData(this, list[i], i);
            v.transform.parent = table.transform;
            v.transform.localPosition = Vector3.zero;
            v.transform.localScale = Vector3.one;
            viewList.Add(v);
        }  
        table.Reposition();
        scroll.ResetPosition();
    } 

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.Lottery, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
