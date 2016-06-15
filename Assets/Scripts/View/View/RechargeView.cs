using UnityEngine;
using System.Collections;

public class RechargeView : MonoBehaviour {
     
    public UILabel rmb;
    public UILabel get;
    public UILabel zeng;
    public UISprite icon;
    public UISprite once;

    public ReChargeData data;
    private int index;
    RechargePop pop;

	// Use this for initialization
	void Start () {
	
	}

    public void InitData(RechargePop p, ReChargeData d, int i)
    {
        pop = p; 
        data = d;
        index = i;
        SetInfo();
    }

    void SetInfo()
    { 
        rmb.text = data.rmb.ToString();
        if (data.first > 0)
        {
            zeng.text = data.first.ToString();
        }
        else
        {
            zeng.gameObject.SetActive(false);
        }
        get.text = data.diamond.ToString(); 
        icon.spriteName = data.icon;
        if (data.Once)
        {
            once.gameObject.SetActive(true);
            zeng.gameObject.SetActive(true); 
        }
        else
        {
            once.gameObject.SetActive(false);
            zeng.gameObject.SetActive(false); 
        }
    }


    public void Click()
    {
        pop.BuyClick(this);
    }
	// Update is called once per frame
	void Update () {
	
	}

    
}
