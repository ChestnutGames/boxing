using UnityEngine;
using System.Collections;

public class TabView : MonoBehaviour { 
    //public delegate void ChangeFocus();
    //public event ChangeFocus ChangeFocusEvent; 
    private BagPop.TabType id; 
    public UISprite bg;
    public UISprite name;
    public UIButton btn;

    private BagPop pop;

     
    private string[] icons = {"全部","物品","材料","拳法","时装"};

    public void InitData(BagPop m, BagPop.TabType i)
    {
        bg = this.GetComponent<UISprite>();
        btn = this.GetComponent<UIButton>();
        pop = m;
        id = i; 
    } 
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetState(string name)
    {
        btn.normalSprite = name;
        btn.hoverSprite = name;
        bg.MakePixelPerfect();
    }



    public void SetState(bool b)
    {
        if (b)
        {
            if (id == 0)
            {
                btn.normalSprite = "标签左";
                btn.hoverSprite = "标签左";
            }
            else
            {
                btn.normalSprite = "标签中";
                btn.hoverSprite = "标签中";
            } 
            name.spriteName = icons[(int)id]+"1";
        }
        else
        {
            if (id == 0)
            {
                btn.normalSprite = "标签左暗";
                btn.hoverSprite = "标签左暗";
            }
            else
            {
                btn.normalSprite = "标签暗";
                btn.hoverSprite = "标签暗"; 
            }
            name.spriteName = icons[(int)id] + "2";
        }
        //name.MakePixelPerfect();
        bg.MakePixelPerfect();
    }

    public void TabClick()
    {
        pop.TabChange(id); 
    }
}
