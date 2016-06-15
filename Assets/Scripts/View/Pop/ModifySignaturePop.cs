using UnityEngine;
using System.Collections;

public class ModifySignaturePop : MonoBehaviour
{
    public UIInput name;
    public UIButton saveSignBtn;
    // Use this for initialization
    void Start()
    {

    }

    public void InitData(string name)
    {
       
    }
 

    public void SetName(string str)
    {
        name.label.text = str;
    } 

    public void ModifyNameClick()
    {
        UserInfoMgr.Instance.ModifySignature(this,name.value); 
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.UserSignModify, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
	 
}
