using UnityEngine;
using System.Collections;

public class ItemView : MonoBehaviour{
    public UISprite bg;
    public UISprite icon;
    public UILabel count;
    public UISprite kuang; 
    public ItemViewData data; 
    protected bool isClick;
    public int index;   

    public void InitData(ItemViewData d,int i,bool click = true)
    { 
        data = d;
        index = i;
        isClick = click;
        SetFous(false);
        if (data != null && data.data!=null)
        {
            kuang.spriteName = data.kuang;
            icon.spriteName = data.data.path;
            count.text = data.curCount.ToString();
        }
        else
        {
            kuang.spriteName = "48sdf";
            icon.spriteName = "48sdf";
            count.text = "";
        }
        //if (data == null || data.curCount == 0)
        //    count.gameObject.SetActive(false);
    }

    public void InitData(ItemViewData d, bool click = true)
    {
        data = d;
        isClick = click;
        SetFous(false);
        if (data != null && data.data != null)
        {
            kuang.spriteName = data.kuang;
            icon.spriteName = data.data.path;
            count.text = data.curCount.ToString();
        }
        else
        {
            kuang.spriteName = "48sdf";
            icon.spriteName = "48sdf";
            count.text = "";
        }
        //if (data == null || data.curCount == 0)
        //    count.gameObject.SetActive(false);
    }


    public void RestEmpty()
    {  
        count.text = "";
        icon.spriteName = "48sdf";
        kuang.spriteName = "48sdf";
        SetFous(false);
    }

    public void RestItem(ItemViewData d)
    { 
        data = d;
        SetFous(false);
        if (data != null && data.data!=null)
        {
            kuang.spriteName = data.kuang;
            icon.spriteName = data.data.path;
            count.text = data.curCount.ToString();
        }
        else
        {
            kuang.spriteName = "48sdf";
            icon.spriteName = "48sdf";
            count.text = "";
        }
        //if (data.curCount == 0)
        //    count.gameObject.SetActive(false);
    }

    public void SetCount(int i)
    {
        data.curCount = i;
        count.text = i.ToString(); 
    } 
    
    public void SetFous(bool b)
    {
        if (b)
        {
            bg.gameObject.SetActive(true);
        }
        else
        {
            bg.gameObject.SetActive(false);
        }  
    } 

    void OnClick()
    {
        if (isClick)
        {
            BagMgr.Instance.SelectItem(this);
        }
    }  
}
