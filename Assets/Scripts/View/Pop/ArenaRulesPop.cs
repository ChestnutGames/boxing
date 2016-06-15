using UnityEngine;
using System.Collections;

public class ArenaRulesPop : SPSGame.Unity.UIWndBase
{
    public UILabel title;
    public UILabel content;
    public GameObject close;

    public void InitData(object b)
    {

    }
	// Use this for initialization
	void Awake () { 
        ListenOnClick(close, CloseClick); 
	} 

    public void CloseClick(GameObject obj)
    { 
        UIManager.Instance.HideWindow<ArenaRulesPop>();
    }

    public void Destory()
    {
        UIManager.Instance.RemoveWindow<ArenaRulesPop>();
    }
     
}
