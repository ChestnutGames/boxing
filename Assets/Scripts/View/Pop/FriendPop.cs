using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendPop : MonoBehaviour
{



    public enum FriendTab
    {
        Friend = 0,
        Add = 1,
        Apply = 2
    }

    [SerializeField]
    public GameObject itemPrefab; 
    public GameObject popPrefab;
    public UILabel uid;
    [SerializeField]
    public UIGrid table;
    [SerializeField]
    private UIScrollView scroll;
    [SerializeField]
    private UILabel liveNum;
    [SerializeField]
    public UILabel pointLab;
    [SerializeField]
    public UILabel searchTxt;
  

    public GameObject friend;
    public GameObject add;
    public GameObject apply;

    private FriendTab curTab;

    private List<FriendData> curList; 
    public Hashtable viewList; 
     
    private int livefr;

    public FriendTabView[] tabList;

    void Start()
    {  
         
    }

    public void SetPoint(int p)
    { 
        pointLab.text = p.ToString();
    }

    public FriendView GetView(int i)
    {
        return viewList[i] as FriendView;
    } 

    public void RestDelFriend(FriendView v)
    { 
        Destroy(v.gameObject); 
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true;
    } 

    public void InitData()
    {
        FriendMgr.Instance.OpenPop(this);
        SetList(curTab);
        SetTab();
        SetPoint(100);
        tabList[(int)FriendTab.Friend].SetState(true); 
        TabChange(FriendTab.Friend); 
        livefr = 0;
        liveNum.text = livefr.ToString();
        uid.text = UserManager.Instance.uid.ToString();
    }
      
    public void SetTab()
    {  
        for (int i = 0; i < tabList.Length; i++)
        {
            tabList[i].InitData(this, (FriendTab)i);
        } 
    }

    public void TabChange(FriendTab i)
    {
        if(i != curTab)
        {
            tabList[(int)curTab].SetState(false);
            curTab = i;
            tabList[(int)curTab].SetState(true); 
            SetList(curTab);
            SetInfo(curTab);
        } 
    }

    public void RemoveAllView()
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            FriendView v = GetView(i);
            if (v != null)
            {
                curList.Remove(GetView(i).data);
                Destroy(GetView(i).gameObject);
            }  
        }
    }

    public void SetLive(int i)
    {
        livefr += i;
        liveNum.text = livefr.ToString();
    }

    public void RemoveFriendView(FriendView v)
    {
        curList.Remove(v.data);
        Destroy(v.gameObject);
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true; 
    }

    public void SetList(FriendTab tab)
    {
        switch (tab)
        {
            case FriendTab.Friend:
                FriendMgr.Instance.GetFriendList();
                break;
            case FriendTab.Add:
                FriendMgr.Instance.GetFriendAddList();
                break;
            case FriendTab.Apply:
                FriendMgr.Instance.GetFriendApplyList();
                break;
        } 
    }

    public void SetList(List<FriendData> list)
    {
        curList = list;
        
        if (table != null)
        {
            while (table.transform.childCount > 0)
            {
                DestroyImmediate(table.transform.GetChild(0).gameObject);
            }
        }
        viewList = new Hashtable(); 
        if (curList != null)
        {
            if (curTab == FriendTab.Friend)
            {
                livefr = list.Count;
                liveNum.text = livefr.ToString();
            }
            for (int i = 0; i < curList.Count; i++)
            {
                //设置格子
                curList[i].time = Comm.NowDateDiff(curList[i].time); 
                GameObject obj = Instantiate(itemPrefab);
                obj.SetActive(true);
                FriendView pop = obj.GetComponent<FriendView>();
                pop.InitData(this, curList[i], i, curTab);
                pop.transform.parent = table.transform;
                pop.transform.position = Vector3.zero;
                pop.transform.localScale = Vector3.one; 
                viewList.Add(i, pop);
            }
            table.Reposition();
            scroll.ResetPosition();
        } 
    }

    public void SetInfo(FriendTab tab)
    {
        friend.SetActive(false);
        add.SetActive(false);
        apply.SetActive(false); 
        switch (tab)
        {
            case FriendTab.Friend:
                friend.SetActive(true);
                break;
            case FriendTab.Add:
                add.SetActive(true);
                break;
            case FriendTab.Apply:
                apply.SetActive(true);
                break;
        }
    } 


    public void CloseClick()
    {
         
        MainUI.Instance.SetPopState(MainUI.PopType.Friend, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); 
    }

    public void ReceiveAllClick()
    {
        FriendMgr.Instance.ReceiveAll(); 
    }

    public void GiftAllClick()
    {
        FriendMgr.Instance.SendAll(); 
       
    }

    public void ApplyAllClick()
    {
        FriendMgr.Instance.ApplyAll(); 
    }
     
    public void AcceptAllClick()
    {
        FriendMgr.Instance.AcceptAll(); 
    }

    public void RefuseAllClick()
    {
        FriendMgr.Instance.RefuseAll(); 
    }
    public void SwapClick()
    {
        FriendMgr.Instance.Swap();
    }
    public void SearchClick()
    {
        if (Comm.IsNumberic(searchTxt.text.ToString()))
        { 
            if (viewList != null && viewList.Count > 0)
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    Destroy(GetView(i).gameObject);
                }
                viewList.Clear();
            }
            FriendMgr.Instance.Search(searchTxt.text.ToString()); 
        }
    }  
     
}
