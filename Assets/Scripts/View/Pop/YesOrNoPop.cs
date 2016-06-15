using UnityEngine;
using System.Collections;

public class YesOrNoPop : MonoBehaviour {
    public delegate void YesFun(object obj);
    public event YesFun YesCallBackEvent;

    public delegate void NoFun(object obj);
    public event NoFun NoCallBackEvent;

    public object obj;
    public UILabel content;

    public void InitData(object o, string c ="")
    {
        obj = o;
        content.text = c;
    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.YesOrNo, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);  
    }

    public void YesClick()
    {
        if (YesCallBackEvent!=null)
            YesCallBackEvent(obj);
        CloseClick();
    }

    public void NoClick()
    {
        if (NoCallBackEvent!=null)
            NoCallBackEvent(obj);
        CloseClick();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
