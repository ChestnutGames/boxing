using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargePop : MonoBehaviour
{
    public UISprite curVip;
    public UISprite nextVip;
    public UISprite nextVip1;
    public UILabel nextCount;
    public UISprite bg;

    public UILabel pre_diomand;

    public UIButton rewaredBtn;
    public UIButton vipBuyBtn;

    public GameObject itemPrefab;
 

    public UIScrollView scroll;
    public UITable table;

    public UISlider slider; 

    public ItemView[] mianItems;
    public ItemView[] mainVipItems;

    public UILabel curLevel;
    public UILabel[] nextLevel;

    public UILabel[] vipLevel; 

    private List<RechargeView> viewList;
    private List<ReChargeData> dataList;

    public Hashtable vipList;


    public UITable attrTable;
    public UIScrollView attrScroll;
    public GameObject attrLabelPrefab;

    public GameObject vip;
    public GameObject store;

    public VipData data;

    private int curChongZhi;

    public int curVipIndex;

    void Start()
    {
        UserManager.Instance.ChangeInfoEvent += SetInfo;
    } 
     
    public void InitData()
    {   
        RechargeMgr.Instance.OpenPop(this); 
    }

    public void CheckBtn()
    {
        if (!data.isRevice && data.vip_level <= UserManager.Instance.vip_level)
        {
            rewaredBtn.isEnabled = true;
        }
        else
        {
            rewaredBtn.isEnabled = false;
        }
        if (!data.isPurchased && data.vip_level <= UserManager.Instance.vip_level)
        {
            vipBuyBtn.isEnabled = true;
        }
        else
        {
            vipBuyBtn.isEnabled = false;
        }
    }

    void ChangeTab(bool b)
    {
        if (b)
        {
            bg.spriteName = "充值底板"; 
            vip.SetActive(false);
            store.SetActive(true);
        }
        else
        {
            bg.spriteName = "特权底板";
           
            vip.SetActive(true);
            store.SetActive(false);
        }
    }

    public void SetVipAttrDesc(string strs)
    {
        while (attrTable.transform.childCount > 0)
        {
            DestroyImmediate(attrTable.transform.GetChild(0).gameObject);
        }
        string[] str = strs.Split('*');
        if (str != null)
        {
            for (int i = 0; i < str.Length; i++)
            {
                GameObject obj = Instantiate(attrLabelPrefab);
                obj.SetActive(true);
                UILabel v = obj.GetComponent<UILabel>();
                v.text = str[i];
                obj.transform.parent = attrTable.transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.position = Vector3.zero;
            }
            attrTable.Reposition();
            attrScroll.ResetPosition();
        } 
    }

    public void SetInfo()
    {
        int cur_level_chong_min = UserManager.Instance.GetCurVipLevelRecharge(); 
        int level;
        if (UserManager.Instance.vip_level + 1 <= 6)
        {
           level= UserManager.Instance.vip_level + 1;
        }
        else
        {
            level =UserManager.Instance.vip_level;
        }
        int max = GameShared.Instance.GetVipByLevel(level).diamond_count;
         
        int chaju = max - UserManager.Instance.recharge_total;
        if (UserManager.Instance.vip_level >= GameShared.Instance.config.user_vip_max)
        {
            chaju = 0;
        }
        nextCount.text = (chaju).ToString();

        int n = max - cur_level_chong_min; //两级减差多少
        int dacheng = UserManager.Instance.recharge_total - cur_level_chong_min; //当前及达成多少
        float a = (float)dacheng / (float)n; 
        //float a = (float)curChongZhi / max; //float.Parse(UserManager.Instance.recharge_progress.ToString());
        float pr = a;//(float)UserManager.Instance.recharge_progress * 0.01f; // a * 0.01f; 
        if (float.IsNaN(pr))
        {
            slider.value = 0;
        }
        else
        {
            slider.value = pr;
        }  
        SetLevelInfo();
    }


    public void SetStore(List<ReChargeData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new List<RechargeView>();
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.SetActive(true);
                RechargeView v = obj.GetComponent<RechargeView>();
                v.InitData(this, list[i], i);
                v.gameObject.name = list[i].id.ToString();
                v.transform.parent = table.transform;
                v.transform.localPosition = Vector3.zero;
                v.transform.localScale = Vector3.one;
                viewList.Add(v);
            }
            //始终两行
            int col = viewList.Count / 2;
            if (viewList.Count % 2 > 0)
            {
                col++;
            }
            table.columns = col;
            table.Reposition();
            scroll.ResetPosition();
        }
    }

    public void SetVipList(Hashtable list)
    {
        vipList = list;
    }

    public void SetVipInfo(int i)
    {
        curVipIndex = i;
        if (curVipIndex < 1)
        {
            curVipIndex = 1;
        }
        SetInfoVip((VipData)vipList[curVipIndex]); 
    }

    public VipData GetVipInfo()
    {
        return vipList[curVipIndex] as VipData; 
    }

    public void SetLevelInfo()
    {
        
        int a = UserManager.Instance.vip_level;
        if(a<GameShared.Instance.config.user_vip_max)
        {
            a++;
        }
        for (int i = 0; i < nextLevel.Length; i++)
        {
            nextLevel[i].text = a.ToString();
        }
        curLevel.text = UserManager.Instance.vip_level.ToString();
    }

    

    public void SetInfoVip(VipData vi)
    {
        data = vi; 
        if(data == null)
            GameShared.Instance.GetVipByLevel(UserManager.Instance.vip_level);
        this.SetVipAttrDesc(vi.vip_attrdesc);
        CheckBtn();
        pre_diomand.text = vi.diamond_show.ToString();

        for (int i = 0; i < vipLevel.Length; i++)
        {
            vipLevel[i].text = data.vip_level.ToString();
        }
         
        if (data.giftList != null)
        {
            int temp = 3;
            if (data.giftList.Count < 3)
                temp = data.giftList.Count;
            for (int i = 0; i < 3; i++)
            {
                mainVipItems[i].SetFous(false);
            }
            for (int i = 0; i < temp; i++)
            {    
                mainVipItems[i].InitData(data.giftList[i], i,false);
                mainVipItems[i].SetFous(false);
            }
        }

        if (data.rewardList != null)
        {
            int temp = 3;
            if (data.rewardList.Count < 3)
                temp = data.rewardList.Count;
            for (int i = 0; i < 3; i++)
            {
                mianItems[i].SetFous(false);
            }
            for (int i = 0; i < temp; i++)
            {
                mianItems[i].InitData(data.rewardList[i], i, false);
                mianItems[i].SetFous(false);
            }
        } 

    }

    public void NextVipClick()
    {
        if (vipList == null) return;
        if (curVipIndex < vipList.Count)
        {
            curVipIndex++;
            SetInfoVip((VipData)vipList[curVipIndex]); 
        } 
    }

    public void PreVipClick()
    {
        if (vipList == null) return;
        if (curVipIndex > 1)
        {
            curVipIndex--; 
            SetInfoVip((VipData)vipList[curVipIndex]);
        }
    } 

    public void StoreClick()
    {
        ChangeTab(false);
    }

    public void VipCilck()
    {
        ChangeTab(true);
    } 

    public void VipBuyClick()
    {
        RechargeMgr.Instance.VipBuy(); 
    }

    public void RewaredClick()
    {
        if (!data.isRevice)
        {
            RechargeMgr.Instance.Rewared();
        }
    }

    public void BuyClick(RechargeView v)
    {
        RechargeMgr.Instance.RechargePurchase(v); 
    }

    public void CloseClick()
    { 
        UserManager.Instance.ChangeInfoEvent -= SetInfo;
        MainUI.Instance.SetPopState(MainUI.PopType.ReCharge, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
