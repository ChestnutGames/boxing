using UnityEngine;
using System.Collections;

public class EmailView : MonoBehaviour
{
    [SerializeField]
    UISprite bg;
    [SerializeField]
    UISprite icon;
    [SerializeField]
    UILabel name;
    [SerializeField]
    UILabel from;
    [SerializeField]
    UILabel date;
    [SerializeField]
    UISprite state; 
    public EmailData data;

    private EmailPop pop;
    private int index;

    public void InitData(EmailPop p,EmailData d, int i)
    {
        index = i;
        pop = p;
        data = d;
        SetReadState(d.isRead); 
        icon.spriteName = data.icon;
        name.text = data.name;
        date.text = data.date;
        from.text = data.from;
        state.MakePixelPerfect();
    }

    public void SetReadState(bool b)
    {
        data.isRead = b;
        if (data.isRevice)
        {
            state.spriteName = "未读邮件";
        }
        else
        {
            state.spriteName = "已读邮件";
            
        }
    }

    public void RestView()
    {
        if (data.isRevice == false)
        {
            SetReadState(data.isRead);
        }
        icon.spriteName = data.icon;
        name.text = data.name;
        date.text = data.date;
        from.text = data.from;
        state.MakePixelPerfect();
    }

    public void ChangeBg(bool b)
    {
        if (!b)
        {
            bg.spriteName = "未选中底框";
        }
        else
        {
            bg.spriteName = "选中底框";
        }
    }

    public void EmailClick()
    {
        EmailMgr.Instance.OpenContext(this); 
    }
}
