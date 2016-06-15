using UnityEngine;
using System.Collections;

public class DelFriendPop : MonoBehaviour {

    public UISprite icon;
    public UILabel name;
    public UILabel level;
    public UILabel vip;
    public UILabel power;
    public UILabel time;
    public UILabel uid;
    public UILabel qian; 

    public int index;

    public FriendView view;
    public FriendData data;
	void Start () {
	
	}

    public void InitData(FriendView v)
    {
        view = v; 
        data = view.data; 
        name.text = data.name;
        level.text = data.level.ToString();
        vip.text = data.vip.ToString();
        power.text = data.power.ToString();
        time.text = data.time;
        qian.text = data.qian;
        uid.text = data.id.ToString();
    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.FriendDel, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void Del()
    {
        FriendMgr.Instance.OpenDelYesOrNoPop(this); 
    }
     

 
}
