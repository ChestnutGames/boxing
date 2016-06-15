using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainUI : UnitySingleton<MainUI>
{
    public enum PopType
    {
        RoleLevel=0,
        WakeUp=1, 
        Bag=2,
        EmailContxt,
        Email,
        Achevement,
        Friend,
        FriendDel,
        Store,
        StoreBuy,
        Daily,
        ReCharge,
        ReChargeBuy,
        Vip,
        Lottery,
        Biling,
        UserInfo,
        UserNameModify,
        UserSignModify,
        Roles,
        Boxing,
        Equip,
        EquipKit, 
        BagUse,
        DiomandTo,
        GetItem,
        YesOrNo,
        XiLian,
        LiLian,
        LiLianHan,
        Level,
        LevelResult,
        chapter,
        Arena,
        ArenaDiamond,
        ArenaRules,
        ArenaPoint,
        ArenaRank,
        ArenaReward
    }
    public GameObject RoleLevelPop;
    public GameObject RoleWakePop;
    public GameObject BagPop;
    public GameObject mailPop;
    public GameObject achievementPop;
    public GameObject friendPop;
    public GameObject storePop;
    public GameObject dailyPop;
    public GameObject rechargePop;
    public GameObject lotteryPop;

    public GameObject rolesPop;
    public GameObject userInfoPop;
    public GameObject boxingPop;
    public GameObject equipPop;
    public GameObject diomandToPop;
    public GameObject getItemPop;
    public GameObject xilianPop;

    public GameObject lilianPop;
    public GameObject lilianHanPop;
    public GameObject levelPop;
    public GameObject chapterPop;
    public GameObject levelResultPop;

    public GameObject arenaPop;
    public GameObject arenaDiamondPop; 

    public SkeletonAnimation effect;
    public SkeletonAnimation roleasset;


    public GameObject yesOrNoPop; 


    public UILabel level;
    public UILabel vip;
    public UILabel power;
    public UILabel coin;
    public UILabel exp;
    public UILabel diamond;




    private bool[] isShow;

    void Awake()
    {
        UIManager.Instance.uiRoot = GameObject.Find("MainUI"); 
    }

    void Start()
    {
        isShow = new bool[Enum.GetNames(typeof(PopType)).Length];
        for (int i = 0; i < isShow.Length; i++)
        {
            isShow[i] = false;
        } 
        UserManager.Instance.ChangeInfoEvent += SetInfo;
        SetInfo();
        //effect.state.End += (aa, bb) =>
        //    {
        //        effect.gameObject.SetActive(false); 
        //    };
    }

    public void LoginOutClick()
    {
        NetworkManager.Instance.LoginOut();
    }

    public void CloseEffectClick()
    {
        //effect.state.End(null,1);
        if (isShow[(int)PopType.Lottery] == true)
        {
            LotterMgr.Instance.AnimNext();
        }
        else
        {
            MainUI.Instance.effect.gameObject.SetActive(false);
        }
    }

    public void SetEffect(SkeletonDataAsset asset,Spine.AnimationState.StartEndDelegate action)
    {
        effect.gameObject.SetActive(true);
        roleasset.gameObject.SetActive(false);
        effect.skeletonDataAsset = asset;
        effect.ResetNew(); 
        effect.state.End += action;
    }

    public void SetEffect(SkeletonDataAsset asset,SkeletonDataAsset role,Spine.AnimationState.StartEndDelegate action)
    {
        effect.gameObject.SetActive(true); 
        effect.skeletonDataAsset = asset;
        effect.ResetNew(); 

        roleasset.gameObject.SetActive(true);
        roleasset.skeletonDataAsset = role;
        roleasset.loop = true;
        roleasset.ResetNew();
        roleasset.state.SetAnimation(0, Def.RoleStanby, true);  
        effect.state.End += action;
    }

    public void SetEffect(SkeletonDataAsset asset, SkeletonDataAsset role)
    {
        effect.gameObject.SetActive(true);
        effect.skeletonDataAsset = asset;
        effect.ResetNew(); 

        roleasset.gameObject.SetActive(true);
        roleasset.skeletonDataAsset = role;
        roleasset.loop = true;
        roleasset.ResetNew();
        roleasset.state.SetAnimation(0, Def.RoleStanby, true);
    } 

    public void SetInfo()
    {
        vip.text = UserManager.Instance.vip_level.ToString() ;
        level.text = UserManager.Instance.level.ToString();
        power.text = UserManager.Instance.GetPower().ToString();
        coin.text = UserManager.Instance.coin.ToString();
        exp.text = UserManager.Instance.exp.ToString();
        diamond.text = UserManager.Instance.diamond.ToString();
    }

    public void ChapterClick()
    {
        if (isShow[(int)PopType.chapter] != true)
        {
            GameObject obj = Instantiate(chapterPop);
            obj.SetActive(true);
            ChapterPop pop = obj.GetComponent<ChapterPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.chapter] = true;
        }
    }

    public void levelsClick(ChapterData d)
    {
        if (isShow[(int)PopType.Level] != true)
        {
            GameObject obj = Instantiate(levelPop);
            obj.SetActive(true);
            LevelsPop pop = obj.GetComponent<LevelsPop>();
            pop.InitData(d);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Level] = true;
        }
    }

    public void LilianPop()
    {
        
        if (isShow[(int)PopType.LiLian] != true)
        {
            GameObject obj = Instantiate(lilianPop);
            obj.SetActive(true);
            LiLianPop pop = obj.GetComponent<LiLianPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.LiLian] = true;
        } 
    }

    public void LilianHanPop(LiLianHallData d)
    {

        if (isShow[(int)PopType.LiLianHan] != true)
        {
            GameObject obj = Instantiate(lilianHanPop);
            obj.SetActive(true);
            LiLianHanPop pop = obj.GetComponent<LiLianHanPop>();
            pop.InitData(d);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.LiLianHan] = true;
        }
    }
    public void LilianHanPop()
    {

        if (isShow[(int)PopType.LiLianHan] != true)
        {
            GameObject obj = Instantiate(lilianHanPop);
            obj.SetActive(true);
            LiLianHanPop pop = obj.GetComponent<LiLianHanPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.LiLianHan] = true;
        }
    }

    public void BagClick()
    {
        if (isShow[(int)PopType.Bag] != true)
        {
            GameObject obj = Instantiate(BagPop);
            obj.SetActive(true);
            BagPop pop = obj.GetComponent<BagPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Bag] = true;
        }
    }

    public void LevelResultClick(List<LevelData> list)
    {
        if (isShow[(int)PopType.LevelResult] != true)
        {
            GameObject obj = Instantiate(levelResultPop);
            obj.SetActive(true);
            LevelResultPop pop = obj.GetComponent<LevelResultPop>();
            pop.InitData(list);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.LevelResult] = true;
        }
    }

    public void XiLianLClick(RoleData r)
    {
        if (isShow[(int)PopType.XiLian] != true)
        {
            GameObject obj = Instantiate(xilianPop);
            obj.SetActive(true);
            XiLianPop pop = obj.GetComponent<XiLianPop>();
            pop.InitData(r);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.XiLian] = true;
        }
    }

    public void DiomandToClick()
    {
        if (isShow[(int)PopType.DiomandTo] != true)
        {
            GameObject obj = Instantiate(diomandToPop);
            obj.SetActive(true);
            DiomandToPop pop = obj.GetComponent<DiomandToPop>(); 
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.DiomandTo] = true;
        }
    }

    public void GetItemClick(List<ItemViewData> list)
    {
        //if (isShow[(int)PopType.GetItem] != true)
        //{
        if (list != null && list.Count > 0)
        {
            GameObject obj = Instantiate(getItemPop);
            obj.SetActive(true);
            GetItemPop pop = obj.GetComponent<GetItemPop>();
            pop.InitData(list);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.GetItem] = true;
        }
        //}
    } 

    public void MailClick()
    { 
        if (isShow[(int)PopType.Email] != true)
        {
            GameObject obj = Instantiate(mailPop);
            obj.SetActive(true);
            EmailPop pop = obj.GetComponent<EmailPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Email] = true;
        }
    } 

    public void AchievementClick()
    {
        if (isShow[(int)PopType.Achevement] != true)
        {
            GameObject obj = Instantiate(achievementPop);
            obj.SetActive(true);
            AchievementPop pop = obj.GetComponent<AchievementPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Achevement] = true;
        }
    } 

    public void FriendPopClick()
    {
        if (isShow[(int)PopType.Friend] != true)
        {
            GameObject obj = Instantiate(friendPop);
            obj.SetActive(true);
            FriendPop pop = obj.GetComponent<FriendPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Friend] = true;
        }
    }

    public void DailyPopClick()
    {
        if (isShow[(int)PopType.Daily] != true)
        {
            GameObject obj = Instantiate(dailyPop);
            obj.SetActive(true);
            DailyPop pop = obj.GetComponent<DailyPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Daily] = true;
        }
    }

    public void StorePopClick(int i)
    {
        if (isShow[(int)PopType.Store] != true)
        {
            GameObject obj = Instantiate(storePop);
            obj.SetActive(true);
            StorePop pop = obj.GetComponent<StorePop>();
            pop.InitData(i);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Store] = true;
        }
    }

    public void ArenaPopClick()
    { 
            ArenaPop pop = UIManager.Instance.ShowWindow<ArenaPop>(); 
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Arena] = true; 
    }


    public void ArenaDiamondPopClick(string dia,string p)
    {
        if (isShow[(int)PopType.ArenaDiamond] != true)
        {
            GameObject obj = Instantiate(arenaDiamondPop);
            obj.SetActive(true);
            ArenaBuyRefreshPop pop = obj.GetComponent<ArenaBuyRefreshPop>();
            pop.InitData(dia,p);
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.ArenaDiamond] = true;
        }
    }  
    

    public void RechargePopClick()
    {
        if (isShow[(int)PopType.ReCharge] != true)
        {
            GameObject obj = Instantiate(rechargePop);
            obj.SetActive(true);
            RechargePop pop = obj.GetComponent<RechargePop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.ReCharge] = true;
        }
    }

    public void LotteryPopClick()
    {
        if (isShow[(int)PopType.Lottery] != true)
        {
            GameObject obj = Instantiate(lotteryPop);
            obj.SetActive(true);
            LotteryPop pop = obj.GetComponent<LotteryPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Lottery] = true;
        }
    }

    public void UserInfoPopClick()
    {
        if (isShow[(int)PopType.UserInfo] != true)
        {
            GameObject obj = Instantiate(userInfoPop);
            obj.SetActive(true);
            UserInfoPop pop = obj.GetComponent<UserInfoPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.UserInfo] = true;
        }
    }

    public void RolesPopClick()
    {
        if (isShow[(int)PopType.Roles] != true)
        {
            GameObject obj = Instantiate(rolesPop);
            obj.SetActive(true);
            RolesPop pop = obj.GetComponent<RolesPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Roles] = true;
        }
    }

    public void BoxingPopClick()
    {
        if (isShow[(int)PopType.Boxing] != true)
        {
            GameObject obj = Instantiate(boxingPop);
            obj.SetActive(true);
            BoxingPop pop = obj.GetComponent<BoxingPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Boxing] = true;
        }
    }

    public void EquipPopClick()
    {
        if (isShow[(int)PopType.Equip] != true)
        {
            GameObject obj = Instantiate(equipPop);
            obj.SetActive(true);
            EquipmentPop pop = obj.GetComponent<EquipmentPop>();
            pop.InitData();
            pop.transform.parent = this.transform;
            pop.transform.localScale = Vector3.one;
            isShow[(int)PopType.Equip] = true;
        }
    } 

    public bool GetPopState(PopType type)
    {
        return isShow[(int)type];
    }

    public void SetPopState(PopType type,bool b)
    {
        isShow[(int)type] = b;
        if (b == false)
        {
            //NetworkManager.Instance.UserInfo();
        }
    }


	 
}
