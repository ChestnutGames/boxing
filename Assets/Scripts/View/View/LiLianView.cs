using UnityEngine;
using System.Collections;

public class LiLianView : MonoBehaviour {

    public UILabel name;

    public LiLianViewData data;
    public LiLianRunView run;

    public bool isSwared;
    public void InitData(LiLianViewData d)
    {
        data = d;
        data.view = this;
        name.text = data.data.csv_id.ToString();
        ChangeState(data.unlock);
        SetState(false);
    }

    
    public void SetState(bool b)
    {
        isSwared = b;
        if(b)
        {
            name.text = data.data.csv_id.ToString() + "+";
        }else
        {
            name.text = data.data.csv_id.ToString();
        }

    }

    public void RestView()
    { 
    }

    public void ChangeState(bool b)
    {
        data.unlock = b;  
    }

    public void LiLianClick()
    {
        if (isSwared)
        {
            LiLianMgr.Instance.GetLiLianSward(data);
        }
        else
        {
            LiLianMgr.Instance.LiLianOpen(data);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
