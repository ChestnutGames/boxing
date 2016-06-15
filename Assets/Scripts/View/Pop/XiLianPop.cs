 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XiLianPop : MonoBehaviour {

    public RewardItem item;
    public RewardItem res;  
     

    public GameObject yesOrNo; 
    public UIButton xiBtn;


    public XiLianView[] dangQian;
    public XiLianView[] xiLianHou;
    public UIToggle[] unLock;

    public RewardItem[] condition;

    public RoleData data;
    public int lockCount;

	// Use this for initialization
	void Start () {
        //InitDataHideBG
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UnlockChange()
    {
        lockCount = 0;
        for (int i = 0; i < unLock.Length; i++)
        {
            if(unLock[i].value)
                lockCount++;
        }
        if (lockCount > 3)
        {
            for (int i = 0; i < unLock.Length; i++)
            {
                if (unLock[i].value == false)
                    unLock[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < unLock.Length; i++)
            { 
                    unLock[i].enabled = true;
            }
        }
        RolesMgr.Instance.SetCondition(lockCount);
    }

    public void InitData(RoleData r)
    {
        data = r;
        RolesMgr.Instance.OpenXiLianPop(this); 
    }

    public void InitLock()
    {
        for (int i = 0; i < unLock.Length; i++)
        {
            unLock[i].value = false;
        }  
    }

    public void SetDangQian(List<XiLianData> list)
    {
        for(int i=0;i<dangQian.Length;i++)
        {
            dangQian[i].InitData(list[i]);
        } 
    } 

    public void SetXiLianHou(List<XiLianData> list)
    {
        for (int i = 0; i < xiLianHou.Length; i++)
        {
            xiLianHou[i].InitData(list[i]);
        }
    }

    public void SetXiLianHoutEmpty()
    {
        for (int i = 0; i < xiLianHou.Length; i++)
        {
            xiLianHou[i].RestEmpty();
        }
    }
     

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.XiLian, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void CancelClick()
    {
        RolesMgr.Instance.XiLianCancel();
    }

    public void ApplyClick()
    {
        RolesMgr.Instance.XiLianOk();
    } 

    public void XiLianClick()
    {
        RolesMgr.Instance.XiLian();
    }
}
