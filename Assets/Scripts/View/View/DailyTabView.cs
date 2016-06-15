using UnityEngine;
using System.Collections;

public class DailyTabView : MonoBehaviour
{
    private DailyPop.DailyTab id;
    public UISprite bg;
    public UISprite name;
    public UIButton btn;

    private DailyPop pop;


    private string[] icons = { "每日签到", "每日锻炼", "每日点金" };

    public void InitData(DailyPop m, DailyPop.DailyTab i)
    {
        bg = this.GetComponent<UISprite>();
        btn = this.GetComponent<UIButton>();
        pop = m;
        id = i;
        SetState(false);
    }

    public void SetState(string b,string k)
    {
        btn.normalSprite = b;
        btn.hoverSprite = b;
        name.spriteName = k;
    }


    public void SetState(bool b)
    {
        if (!b)
        {
            btn.normalSprite = "标签";
            btn.hoverSprite = "标签";
            name.spriteName = icons[(int)id] + "1";
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
