using UnityEngine;
using System.Collections;

public class DailyView : MonoBehaviour {
    public UISprite bg;
    public UISprite icon;
    public UISprite gou;
    // Use this for initialization
    public QianDaoData data; 
    private DailyPop pop;
    public UISprite hot;
    public UILabel vip;

    public void InitData(DailyPop p, QianDaoData d)
    { 
        pop = p; 
        data = d; 
        if (data != null && data.itemdata!=null)
        { 
            icon.spriteName = data.itemdata.path; 
        }
        else
        {
            icon.spriteName = ""; 
        } 
        SetShow(d.show);
        SetReceive(d.signed);

        if (data.vip_level > 0)
        {
            vip.text = "v" + data.vip_level + "双倍";
        }
        else
        {
            hot.gameObject.SetActive(false);
        }
    }

    

    public void SetReceive(bool b)
    {
        data.signed = b;
        if (b)
        {
            gou.gameObject.SetActive(true); 
        }
        else
        {
            gou.gameObject.SetActive(false); 
        }
    }

     
    public void SetShow(bool b)
    {
        data.show = b;
        if (!b)
        { 
            bg.gameObject.SetActive(true);
        }
        else
        {
            bg.gameObject.SetActive(false);
        }
    }

    void OnClick()
    {
        if (data.show && data.signed == false)
        {
            DailyMgr.Instance.Sign(this);
        }
    }  
}
