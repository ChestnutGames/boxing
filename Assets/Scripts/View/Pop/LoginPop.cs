using UnityEngine;
using System.Collections;

public class LoginPop : MonoBehaviour
{

    public UILabel name;
    public UILabel pwd;


    public UIButton loginBtn;
    public UIButton RegistBtn;

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        name.text = PlayerPrefs.GetString("defname");
        pwd.text = PlayerPrefs.GetString("defpwd");
#elif UNITY_ANDROID  
#endif
        LoginMgr.Instance.OpenPop(this);
    }

    public void LoginClick()
    {

        PlayerPrefs.SetString("defname", name.text);
        PlayerPrefs.SetString("defpwd", pwd.text);
        LoginMgr.Instance.Login(name.text, pwd.text);

    }

    public void RegistClick()
    {
        LoginMgr.Instance.Regist(name.text, pwd.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
