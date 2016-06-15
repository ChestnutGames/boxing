using UnityEngine;
using System.Collections;

public class FriendTabView : MonoBehaviour
{ 
    private FriendPop.FriendTab id;
    public UISprite bg;
    public UISprite name;
    public UIButton btn;

    private FriendPop pop;


    private string[] icons = { "好友列表","添加好友", "申请列表" };

    public void InitData(FriendPop m, FriendPop.FriendTab i)
    {
        bg = this.GetComponent<UISprite>();
        btn = this.GetComponent<UIButton>();
        pop = m;
        id = i;
        SetState(false);
    }

    public void SetState(string b)
    {
        btn.normalSprite = b;
        btn.hoverSprite = b;
        name.spriteName = icons[(int)id];
    }
     

    public void SetState(bool b)
    {
        if (!b)
        {
            btn.normalSprite = "标签";
            btn.hoverSprite = "标签"; 
            name.spriteName = icons[(int)id];
        }
        else
        { 
            btn.normalSprite = "标签选中";
            btn.hoverSprite = "标签选中"; 
            name.spriteName = icons[(int)id] + "2";
        } 
        //bg.MakePixelPerfect();
    }

    public void TabClick()
    {
        pop.TabChange(id);
    }
}
