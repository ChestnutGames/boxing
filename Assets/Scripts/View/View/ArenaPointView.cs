using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPointView : SPSGame.Unity.UIObject
{
    public UILabel num;
    public UILabel state;
    public UIScrollView scroll;
    public UIGrid grid;
    public AranePointViewData data; 


    public void InitData(AranePointViewData d)
    {
        data = d;
        num.text = data.num.ToString();
        state.text = data.state;
        SetSwaredList(d.list); 
    }
    /// <summary>
    /// 设置未接就变黑的遮罩
    /// </summary>
    /// <param name="b"></param>
    public void SetMask(bool b)
    {
        if (b)
        {
            state.text = "可领取";
        }
        else
        {
            state.text = "尚未达成";
        }
    }



    public void SetReward(long b)
    {
        if (b >0 )
        {
            scroll.gameObject.SetActive(false); 
        }
        else
        {
            scroll.gameObject.SetActive(true);
        }
    }

    public void SetSwaredList(List<ItemViewData> list)
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        } 
            for (int i = 0; i < list.Count; i++)
            { 
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Ui/View/ItemView"));
                obj.SetActive(true);
                ItemView pop = obj.GetComponent<ItemView>();
                pop.InitData(list[i]);
                pop.transform.parent = grid.transform;
                pop.transform.position = Vector3.zero;
                pop.transform.localScale = Vector3.one; 
            }
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true; 
    }
}

