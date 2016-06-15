using UnityEngine;
using System.Collections;
using LuaInterface;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class EmailMgr : UnitySingleton<EmailMgr>
{ 
    private List<EmailView> viewList;
    private List<EmailData> dataList;
    private EmailView curCallBackView;
    public UIButton reciveBtn;
    public EmailPop pop; 

    private LuaState l; 

    public void OpenPop(EmailPop p)
    {
        pop = p;
    }

    public void OpenContext(EmailView view)
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.EmailContxt) != true)
        {
            GameObject obj = Instantiate(pop.popPrefab);
            obj.SetActive(true);
            EmailContextPop p = obj.GetComponent<EmailContextPop>();
            p.InitData(view);
            p.transform.parent = pop.transform.parent;
            p.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.EmailContxt, true);
        } 
 
    }

    public void Start()
    {
        //InitLua();  
    }

    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_Email); 
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void GetMailList()
    { 
            NetworkManager.Instance.MailList();
        
    } 

    public void SetPopList(List<EmailData> list)
    {
       pop.SetList(list);
       pop.SetInfo();
    }
    EmailContextPop mailpop = null;
    public void MailReceive(EmailView view,EmailContextPop p)
    {
        curCallBackView = view;
        mailpop = p;
        if (view.data.isRevice)
        {
            List<long> list = new List<long>();
            list.Add(curCallBackView.data.id);
            mailpop.receiveBtn.isEnabled = false;
            NetworkManager.Instance.MailReceive(list);  
         }
    }

    List<ItemViewData> allrecList = null;
    public void MailReceiveAll()
    {
        if (pop.viewList != null && pop.viewList.Count>0)
        {
            List<long> list = new List<long>();
            allrecList = new List<ItemViewData>();
            IDictionaryEnumerator enumerator = pop.viewList.GetEnumerator(); 
            while (enumerator.MoveNext())
            {
                EmailView v = enumerator.Value as EmailView;
                if (v != null)
                {
                
                    //无附件
                    if (v.data.isRevice)
                    {
                        //有附件
                        if (v.data.itemList != null && v.data.itemList.Count > 0)
                        {
                            list.Add(v.data.id);
                            for (int i = 0; i < v.data.itemList.Count; i++)
                            {
                                allrecList.Add(v.data.itemList[i]);
                            }
                        }
                    }
                    else
                    {
                        list.Add(v.data.id);
                    }
                }
            }
            pop.allRecBtn.isEnabled = false;
            NetworkManager.Instance.MailRead(list);
            NetworkManager.Instance.MailReceive(list); 
        } 
    }

    public string GetSort(bool r, bool s, long i)
    {
        int d2 = 1;
        if (!r)
            d2 = 2;

        int d1 = 1;
        if (!s)
            d1 = 2; 
            
        return d1.ToString()+ d2.ToString() + (i*100).ToString();
    }

    public void MailReceiveCallBack(C2sSprotoType.mail_getreward.response resp)
    {
        if (mailpop != null)
        {
            mailpop.receiveBtn.isEnabled = true;
            if (resp.errorcode == 1 && curCallBackView != null && mailpop != null)
            { 
                curCallBackView.data.isRead = false;
                curCallBackView.data.isRevice = false;
                curCallBackView.RestView();
                curCallBackView.name = GetSort(curCallBackView.data.isRead, curCallBackView.data.isRevice, curCallBackView.data.id);
                pop.SetInfo();
                if (mailpop != null)
                {
                    mailpop.SetItemList();
                }
                //NetworkManager.Instance.UserInfo();
                Debug.Log("领取成功");
                pop.ResultRestList(curCallBackView); 
                for (int i = 0; i < curCallBackView.data.itemList.Count; i++)
                {
                    BagMgr.Instance.AddItemNumById(curCallBackView.data.itemList[i].data.id,
                        curCallBackView.data.itemList[i].curCount);
                }
                MainUI.Instance.GetItemClick(curCallBackView.data.itemList); 
            }
        }
        else if (allrecList != null) //全部领取
        {
            pop.allRecBtn.isEnabled = true;
            if (resp.errorcode == 1)
            {  
                for (int aa = 0; aa < allrecList.Count; aa++)
                {
                    BagMgr.Instance.AddItemNumById(allrecList[aa].data.id, allrecList[aa].curCount);
                }
                pop.ReceiveList();
                MainUI.Instance.GetItemClick(allrecList); 
            }
        } 
        curCallBackView = null;
        mailpop = null; 
        allrecList = null;
    }

    public void MailRead(EmailView view)
    {

        List<long> list = new List<long>();
        list.Add(view.data.id);
        NetworkManager.Instance.MailRead(list);
        curCallBackView = view;
    }

    public void MailReadCallBack()
    {
        if (curCallBackView != null)
        {
            curCallBackView.data.isRead = false;
            curCallBackView.name = GetSort(curCallBackView.data.isRead, curCallBackView.data.isRevice, curCallBackView.data.id);
            pop.ResultRestList(curCallBackView);
            curCallBackView = null;
        }
    }


    public void MailDel(EmailView view)
    {
        curCallBackView = view;
        List<long> list = new List<long>();
        list.Add(view.data.id);
        NetworkManager.Instance.MailDelete(list); 
    }

    public void MailDelCallBack()
    { 
        pop.ResultRestList(curCallBackView);
    }
     
    public void MailListCallBack(C2sSprotoType.mails.response resp)
    {
        List<EmailData> list = new List<EmailData>();
        if (resp != null && resp.mail_list != null)
        {
            for (int i = 0; i < resp.mail_list.Count; i++)
            {
                EmailData d = new EmailData();
                d.date = resp.mail_list[i].acctime;
                d.desc = resp.mail_list[i].content;
                d.from = "系统";
                ItemData temp = GameShared.Instance.GetItemData((int)resp.mail_list[i].iconid);
                if (temp != null)
                {
                    d.icon = temp.path;
                }
                
                d.id = resp.mail_list[i].emailid;
                d.isRead = resp.mail_list[i].isread;//是否可读
                d.isRevice = resp.mail_list[i].isreward;//是否以领取
                d.name = resp.mail_list[i].title;
                d.type = (EmailData.EmailType)resp.mail_list[i].type;
                d.sort = GetSort(d.isRead, d.isRevice, d.id);
                if (resp.mail_list[i].attachs != null)
                {
                    List<ItemViewData> l = new List<ItemViewData>();
                    for (int j = 0; j < resp.mail_list[i].attachs.Count; j++)
                    {
                        ItemViewData item = new ItemViewData();
                        item.curCount = (int)resp.mail_list[i].attachs[j].itemnum;
                        ItemData a = new ItemData();
                        
                        a = GameShared.Instance.GetItemData((int)resp.mail_list[i].attachs[j].itemsn);
                        item.data = a;
                        l.Add(item);
                    }
                    d.itemList = l;
                }
                list.Add(d);
            } 
        }
        else
        { 
        }
        this.SetPopList(list);
    }  
 


}
