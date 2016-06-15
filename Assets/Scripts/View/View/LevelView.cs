using UnityEngine;
using System.Collections;

public class LevelView : MonoBehaviour {

    public LevelViewData data;
    public UISprite icon;
    public UIButton btn;

    public void InitData(LevelViewData d)
    {
        data = d;
        SetIcon();
    }

    public void RestEmpty()
    {
 
    }

    public void SetIcon()
    {
        if (data.unlock)
        {
            btn.normalSprite = "关卡1";  
        }
        else
        {
            btn.normalSprite = "关卡3"; 
        } 
    }

    public void LevelClick()
    {
        LevelsMgr.Instance.SelectLevel(this); 
    }
 
}
