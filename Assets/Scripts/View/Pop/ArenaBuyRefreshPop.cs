using UnityEngine;
using System.Collections;

public class ArenaBuyRefreshPop : SPSGame.Unity.UIWndBase
{

    public UILabel time;
    public UILabel diamond; 

    public void InitData(string dia,string t)
    {
        diamond.text = dia;
        time.text = t;
    }
 

    public void AccptClick()
    {
        ArenaMgr.Instance.RefreshList();
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.ArenaDiamond, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
