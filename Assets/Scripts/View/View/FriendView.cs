using UnityEngine;
using System.Collections;

public class FriendView : MonoBehaviour {
     
    public UIButton giftBtn;
    public UIButton receiveBtn;
    public UIButton applyBtn;
    private int index;
    public FriendData data;
    private FriendPop pop;
    private FriendPop.FriendTab tab;

    public GameObject friend;
    public GameObject add;
    public GameObject apply;

    public UISprite icon;
    public UILabel name;
    public UILabel level;
    public UILabel vip;
    public UILabel power;
    public UILabel time; 

    public void InitData(FriendPop p, FriendData d,int i,FriendPop.FriendTab t)
    {
        pop = p;
        data = d;
        index = i; 
        tab = t;
        CheckBtn();
        SetInfo();
    }

    public void Rest()
    {
        CheckBtn();
        SetInfo();
    }

    public void CheckBtn()
    {
        if (data.isApply)
        {
            applyBtn.isEnabled = true; 
        }
        else
        { 
            applyBtn.isEnabled = false;
        }

        if (data.isHeart)
        {
            giftBtn.isEnabled = true;
            giftBtn.gameObject.SetActive(true);
        }
        else
        {
            giftBtn.isEnabled = false;
            giftBtn.gameObject.SetActive(true);
        }

        if (data.heartamount>0)
        {
            receiveBtn.isEnabled = true;
        }
        else
        {
            receiveBtn.isEnabled = false;
        } 
    }  

    public void SetInfo()
    {
        icon.spriteName = data.icon;
        giftBtn.enabled = data.isHeart;
        receiveBtn.enabled = data.isReceive;
        friend.SetActive(false);
        add.SetActive(false);
        apply.SetActive(false); 
        switch (tab)
        {
            case FriendPop.FriendTab.Friend:
                friend.SetActive(true); 
                break;
            case FriendPop.FriendTab.Add:
                add.SetActive(true);
                break;
            case FriendPop.FriendTab.Apply:
                apply.SetActive(true);
                break;
        }
        name.text = data.name;
        level.text = data.level.ToString();
        vip.text = data.vip.ToString();
        power.text = data.power.ToString();
        time.text = data.time; 
    }

    public void Del()
    {
        if (tab == FriendPop.FriendTab.Friend)
        {
            FriendMgr.Instance.OpenFriendDel(this); 
        }
    }

    public void GiftClick()
    {
        FriendMgr.Instance.Send(this);
    }

    public void ReceiveClick()
    {
        FriendMgr.Instance.Receive(this); 
    }

    public void ApplyClick()
    {
        FriendMgr.Instance.Apply(this); 
    }

    public void AcceptClick()
    {
        FriendMgr.Instance.Accept(this); 
    }


    public void RefuseClick()
    {
        FriendMgr.Instance.Refuse(this);  
    }
}
