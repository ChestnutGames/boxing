using UnityEngine;
using System.Collections;

public class UserInfoPop : MonoBehaviour {

    public GameObject namePop;
    public GameObject signPop;

    public UISprite[] wakeSprite;

    public UILabel name;
    public UILabel level;
    public UILabel vip;
    public UILabel power;
    public UILabel crit;
    public UILabel defense;
    public UILabel pray;
    public UILabel rolePower;
    public UILabel roleCrit;

    public UILabel upPower;
    public UILabel upExp; 
    public UILabel sign;

    public UIButton levelUpBtn;

    public SkeletonAnimation anim;

    public UILabel talk;
    public UISprite talkBg;

    public bool talkShow = false; 

    public void Start()
    {


    }

    public void InitData()
    {
        UserInfoMgr.Instance.OpenPop(this); 
    }

    

    public void SetWakeLevel(int level)
    {
        for (int i = 0; i < wakeSprite.Length; i++)
        {
            wakeSprite[i].spriteName = "觉醒皇冠_2";
            if (i < level)
            {
                wakeSprite[i].spriteName = "觉醒皇冠_1";
            }
        }
    }

    public void SetName(string n)
    {
        name.text = n;
    }

    public void SetSign(string s)
    {
        sign.text = s;
    }
 

    public void SetLevelBtnInfo(bool level, int power, int exp)
    { 
        levelUpBtn.isEnabled = level;

        upPower.text = power.ToString();
        upExp.text =exp.ToString(); 
    }

    public void UpLevelClick()
    {
        UserInfoMgr.Instance.UpLevel(); 
    }

    public void NamePopClick()
    {
        UserInfoMgr.Instance.NameModifyPop(); 
    }

    public void SignPopClick()
    {
        UserInfoMgr.Instance.SignModifyPop();
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.UserInfo, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }


    public void SetTalk(string str)
    {
        talk.text = str;
    }


    public void TalkClick()
    {
        if (talkShow)
        {
            talkShow = false;
            talkBg.GetComponent<UISprite>().gameObject.SetActive(false);
        }
        else
        {
            talkShow = true;
            talkBg.GetComponent<UISprite>().gameObject.SetActive(true);
        }

    }
}
