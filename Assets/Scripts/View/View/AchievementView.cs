using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementView : MonoBehaviour {
    [SerializeField]
    private UISprite[] starList;
    [SerializeField]
    private UISprite icon;
    [SerializeField]
    private UILabel name;
    [SerializeField]
    private UILabel condition;
    [SerializeField]  
    private UILabel progress; 
    public UIButton receiveBtn;
    [SerializeField]
    private UISlider slider;
    [SerializeField]
    private UILabel gift;
     

    public AchievementViewData data;

    public AchievementRewardItem[] rewardList;

    private float preCondition;

    private int index;

    public void InitData(AchievementViewData d,int i)
    {
        index = i;
        data = d;
        if (data.data != null)
        {
            SetStar();
            SetReward();
            SetInfo();
            CheckReceive();
        }
        data.view = this;
    }

    public void RestData(AchievementViewData d)
    {
        float a = preCondition /d.data.condition;
        data.curProgress =(int)( a * 100.0f);
        data = d;
        if (data.data != null)
        {
            SetStar();
            SetReward();
            SetInfo();
            CheckReceive();
        }
    }

    public void RestData(AchievementData d)
    {
        data.data = d;
        float a = preCondition / data.data.condition;
        data.curProgress = (int)(a * 100.0f); 

        if (data.data != null)
        {
            SetStar();
            SetReward();
            SetInfo(); 
            CheckReceive();
        }
    }

    public void SetStar()
    {
        if (data.data != null)
        {
            for (int i = 0; i < starList.Length; i++)
            {
                if (i < data.data.curStar)
                {
                    starList[i].spriteName = "星星";
                }
                else
                {
                    starList[i].spriteName = "星星空";
                }
            }
        }
    }


    public void SetReward()
    {
        if (data.isReceive == false && data.data.condition != 0)
        {
            gift.gameObject.SetActive(true); 
            if (rewardList != null && data.isReceive == false)
            {
                data.data.rewarData = new List<ItemViewData>();
                for (int i = 0; i < rewardList.Length; i++)
                {
                    List<GameShared.StrData> str = GameShared.Instance.GetStrData(data.data.reward);
                    if (i < str.Count && str[i] != null)
                    {
                        ItemData d = GameShared.Instance.GetItemData(str[i].id);
                        ItemViewData v = new ItemViewData();
                        v.data = d;
                        v.curCount = str[i].num;
                        data.data.rewarData.Add(v);
                        if (d != null)
                        {
                            rewardList[i].Init(d.path, str[i].num.ToString());
                        }
                        else
                        {
                            rewardList[i].gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        rewardList[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            gift.gameObject.SetActive(false);
        }
    }


    public void SetInfo()
    {
        icon.spriteName = data.data.icon;
        name.text = data.data.name;
        condition.text = data.data.desc;
        slider.value = data.curProgress * 0.01f;
        float temp = data.data.condition * (data.curProgress * 0.01f);
        progress.text = (int)temp + "/" + data.data.condition;
        preCondition = temp;
        if (data.data.condition == 0)
        {
            slider.gameObject.SetActive(false);
            progress.gameObject.SetActive(false); 
        }
        else
        {
            slider.gameObject.SetActive(true);
            progress.gameObject.SetActive(true); 
        }
    }
     

    public void CheckReceive()
    {
        if ((data.curProgress > 99 || data.data.condition==0) && data.isReceive == false && data.isUnlock == true)
        {
            receiveBtn.isEnabled = true;
        }
        else
        {
            receiveBtn.isEnabled = false;
        }
        if (data.data.condition == 0)
            receiveBtn.gameObject.SetActive(false);
    } 
    public void ReceiveClick()
    {
        if (data.data != null)
        {
            AchievementMgr.Instance.Revcie(this);
        }
    }
     
}
