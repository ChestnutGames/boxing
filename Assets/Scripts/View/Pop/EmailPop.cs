using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmailPop : MonoBehaviour {

    const int MaxEmailCount = 50;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    public GameObject popPrefab;
    [SerializeField]
    private UITable grid;
    [SerializeField]
    private UIScrollView scroll; 
    [SerializeField]
    private UILabel count;

    private int mailNum;
      
    public Hashtable viewList;

    public UIButton allRecBtn;

	void Start ()
    {
        
	}

    public EmailView GetView(int i)
    {
        return viewList[i] as EmailView;
    }

    public void InitData()
    {
        EmailMgr.Instance.OpenPop(this);
        EmailMgr.Instance.GetMailList(); 
    } 
 
    public void ReceiveList()
    { 
        IDictionaryEnumerator enumerator = viewList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            EmailView v = enumerator.Value as EmailView;
            if (v != null)
            {

                v.data.isRead = false;
                v.data.isRevice = false;
                this.ResultRestList(v);
                v.RestView();
            }
        } 
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.Email, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void ResultRestList(EmailView v)
    {
        EmailData.EmailType type = v.data.type;
        if (v != null)
        {
            switch (type)
            {
                case EmailData.EmailType.ReadDel:
                    v.data.isRead = false;
                    if (v.data.isRevice == false)
                    {
                        mailNum--;
                        Destroy(v.gameObject);
                        viewList.Remove(v);
                    }
                    break; 
                case EmailData.EmailType.Read: 
                    v.data.isRead = false;
                    if (v.data.isRevice == false)
                    {
                        v.SetReadState(false);
                    }
                    break; 
            }
        }
        if (grid != null)
        {
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
        }
        SetInfo();
    }


    public void ReceiveAllClick()
    {
        EmailMgr.Instance.MailReceiveAll(); 
    }
     

    public void SetList(List<EmailData> list)
    {
        mailNum = 0;
        if (list != null)
        {
            while (grid.transform.childCount > 0)
            {
                DestroyImmediate(grid.transform.GetChild(0).gameObject);
            }
            viewList = new Hashtable();
            for (int i = 0; i < list.Count; i++)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.SetActive(true);
                int index = 0;
                if (list[i].type == EmailData.EmailType.ReadDel)
                {
                    index = 100 + (int)list[i].type;
                }
                else
                {
                    index = 1000 + i;
                }
                obj.name = list[i].sort;
                EmailView v = obj.GetComponent<EmailView>();
                v.InitData(this, list[i], i);
                v.transform.parent = grid.transform;
                v.transform.localPosition = Vector3.zero;
                v.transform.localScale = Vector3.one;
                viewList.Add(i, v);
            }
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
            mailNum = viewList.Count;
        } 
    }

    public void SetInfo()
    { 
        count.text = mailNum + "/" + MaxEmailCount;
        
    }    
	
}
