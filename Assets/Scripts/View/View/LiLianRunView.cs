using UnityEngine;
using System.Collections;
using System;

public class LiLianRunView : MonoBehaviour {

    public LiLianRunData data;

    public UILabel time;
    public UILabel state;
    public void InitData(LiLianRunData d)
    {
        data = d;
        d.view = time;
        state.text = "空闲";
    }

    public void RestView(LiLianRunData r)
    {
        data = r;
        r.view = time;
        if (r.state == 0)
        {
            state.text = "空闲";
            time.text = "";
        }
        else
        {
            state.text = "正在进行";
            string str = Comm.DateDiffHour(DateTime.Now, data.time);
            time.text = str;
        }
 
    }

    public void LiLianRunClick()
    {
        if(data.state !=0)
            LiLianMgr.Instance.QuickLiLian(data);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
