using UnityEngine;
using System.Collections;

public class ModifyNamePop : MonoBehaviour {
     
    public UIInput name;
    public UIButton randomNameBtn;
    public UIButton saveNameBtn;
	// Use this for initialization
	void Start () { 
	}

    public void InitData(string name)
    {
        SetName(name);
    } 

    public void SetName(string str)
    {
        name.label.text = str;
    }

    public void RandomNameClick()
    {
        UserInfoMgr.Instance.RandomName(this);
    }

    public void ModifyNameClick()
    {
        UserInfoMgr.Instance.ModifyName(this,name.value); 
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.UserNameModify, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
