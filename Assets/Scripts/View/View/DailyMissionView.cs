using UnityEngine;
using System.Collections;

public class DailyMissionView : MonoBehaviour {

    public UIToggle toggle;
    public UILabel name;
    public UILabel level;
    public UILabel exp;
    public DailyData data;
    private DailyPop pop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitData(DailyPop p, DailyData d)
    {
        data = d;
        pop = p;
        SetInfo();
    }

    public void SetInfo()
    {
        name.text = data.name;
        exp.text = "+" + data.getNum;
        level.text = "+" + data.level_up.ToString();
    }

}
