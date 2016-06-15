using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmailContextPop : MonoBehaviour
{

    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private UILabel count;

    [SerializeField]
    private UILabel name;
    [SerializeField]
    private UILabel desc; 
    public UIButton receiveBtn;

    public EmailData data;
    public EmailView view;
     
  
    void Start()
    { 

    }
    public void InitData(EmailView v)
    {

        view = v; 
        data = view.data;
        if (data.isRead && data.itemList != null && data.itemList.Count<1)
        {
            EmailMgr.Instance.MailRead(v);
        }
        data.isRead = false;
        SetItemList();
        SetInfo();
        CheckReceive(); 
        v.RestView();
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.EmailContxt, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); 
    }

    public void SetInfo()
    {
        name.text = data.name;
        desc.text = data.desc;
        CheckReceive();
    }

    public void CheckReceive()
    {
        if (data.isRevice)
        {
            receiveBtn.enabled = true;
        }
        else
        {
            receiveBtn.enabled = false;
        }
    }

    public void RevcieClick()
    {
        EmailMgr.Instance.MailReceive(this.view,this);  
    }
     

    public void SetItemList()
    {
        if (data.itemList != null && data.isRevice)
        {
            receiveBtn.gameObject.SetActive(true);
            for (int i = 0; i < data.itemList.Count; i++)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.SetActive(true);
                RewardItem pop = obj.GetComponent<RewardItem>();
                if (data.itemList[i].data != null)
                {
                    pop.InitData(data.itemList[i], data.itemList[i].data.path, data.itemList[i].curCount);
                }
                pop.transform.parent = grid.transform;
                pop.transform.localScale = Vector3.one;
            }
        }
        else
        {
            grid.SetActive(false);
            receiveBtn.gameObject.SetActive(false);
        }

    }
 
}
