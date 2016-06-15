using UnityEngine;
using System.Collections;

public class ArenaRewardView : SPSGame.Unity.UIObject
{

    public UILabel desc;
    public ItemView item1;
    public ItemView item2;
    // Use this for initialization
    public ArenaRewaredViewData data;

    void Start()
    {
        ListenOnClick(this.gameObject, ClickReward);
    }

    public void SetReward(bool b)
    {
        if (b)
        {
            item1.gameObject.SetActive(false);
            item2.gameObject.SetActive(false);
        }
        else
        {
            item1.gameObject.SetActive(true);
            item2.gameObject.SetActive(true);
        }
    }
    //遮罩不可点击
    public void SetMask(bool b)
    {
        if (b)
        { }
        else
        {
        }

    }

    public void InitData(ArenaRewaredViewData d)
    {
        data = d;
        if(data.item1!=null)
            item1.InitData(d.item1, 1, false);
        if (data.item2!= null)
            item1.InitData(d.item2, 1, false);
        desc.text = data.desc;
    }

    public void ClickReward(GameObject obj)
    {
        ArenaMgr.Instance.ArenaRewared();
    }

}
	
 
