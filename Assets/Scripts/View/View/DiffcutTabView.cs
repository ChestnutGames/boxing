using UnityEngine;
using System.Collections;

public class DiffcutTabView : MonoBehaviour
{
    private Def.levelType id;
    public UISprite bg;
    public UISprite name;
    public UIButton btn;

    private LevelsPop pop;


    private string[] icons = {"普通", "困难", "炼狱"};

    public void InitData(LevelsPop m, Def.levelType i)
    {
        bg = this.GetComponent<UISprite>();
        btn = this.GetComponent<UIButton>();
        pop = m;
        id = i;
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
            name.spriteName = icons[(int)id] + "1";
        }
        else
        { 
            name.spriteName = icons[(int)id] + "2";
        } 
    }

    public void TabClick()
    {
        pop.TabChange(id);
    }
}
