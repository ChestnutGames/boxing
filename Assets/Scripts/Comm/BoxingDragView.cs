using System;
using UnityEngine;

/// <summary>
/// 第一个自己创建的拖拽功能
/// </summary>
public class BoxingDragView : UIDragDropItem
{

    private GameObject sourceParent;
    private BoxingView cur;
    public Vector3 oriPos; 
    /// <summary>
    /// 重写父类的拖拽开始函数
    /// </summary>
    protected override void OnDragDropStart()
    {
        //当拖拽开始时存储原始的父对象
        cur = this.GetComponent<BoxingView>();
        this.sourceParent = this.transform.parent.gameObject;
        //
        if (tag == Def.BoxingSave)
        {
            oriPos = new Vector3();
            oriPos.x = transform.localPosition.x;
            oriPos.y = transform.localPosition.y;
            oriPos.z = transform.localPosition.z;
        }
        if (cur != null && cur.data != null && cur.data.level > 0)
        { 
            this.transform.parent = BoxingMgr.Instance.GetPop().movePanel.transform;
            base.OnDragDropStart();
        } 
    }
    /// <summary>
    /// 重写父类的拖拽释放函数
    /// </summary>
    protected override void OnDragDropRelease(GameObject surface)
    {
        if (surface != null)
        {
            BoxingView tar = surface.GetComponent<BoxingView>();
            cur = this.GetComponent<BoxingView>();
            //是否相同类型技能

            bool flag = BoxingMgr.Instance.SwapBoxingList(ref cur, ref tar, sourceParent, oriPos);


            //最终调用父类的功能

            if (flag) //回归原位  OnDragDropRelease 会导致不能 改parent
            {  
                if (tag == Def.BoxingSave)
                {
                    this.transform.localPosition = oriPos;
                }
                BoxingMgr.Instance.RefreshTable();
            }
        }
        base.OnDragDropRelease(surface,sourceParent.transform,oriPos);
        if (tag == Def.BoxingSave)
        {
            this.transform.localPosition = oriPos;
        }
    }
}