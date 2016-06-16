using System;
using UnityEngine;
using System.Collections;
using System.Text;


public class LoginMgr : UnitySingleton<LoginMgr> {

    LoginPop pop;
    ClientLogin login;
    // Use this for initialization
    public bool isLogin;

	void Awake () {  
        login = GameObject.Find("Login").GetComponent<ClientLogin>();
        string name = string.Format(ActionFormat(), "erew");
        var type = System.Reflection.Assembly.GetCallingAssembly().GetType(name);
        Debug.Log(type);
        isLogin = false;

    }

    protected virtual string ActionFormat()
    {
        return "Action_{0}";
    }

    public void OpenPop(LoginPop p)
    {
        pop = p; 
    }
	 

    public void Login(string n,string p)
    {
        UserManager.Instance.username = n;
        UserManager.Instance.pwd = p;
        pop.loginBtn.isEnabled = false;

        string ip = Def.IP; 
        int port = 3002;
        login.Auth(ip, port, "sample", n, p, null, OnLoginCallback);
    } 

    public void OnLoginCallback(bool ok, object ud, byte[] secret, string dummy)
    { 
        if (ok)
        {
            int _1 = dummy.IndexOf(':');
            int _2 = dummy.IndexOf('@', _1);
            int _3 = dummy.IndexOf('#', _2);
            string uen = dummy.Substring(_1 + 1, _2 - _1 - 1);
            byte[] uid = Crypt.base64decode(Encoding.ASCII.GetBytes(uen));
            string sen = dummy.Substring(_2 + 1, _3 - _2 - 1);
            byte[] subid = Crypt.base64decode(Encoding.ASCII.GetBytes(sen));
            string gated = dummy.Substring(_3 + 1);

            Debug.Log(string.Format("{0},{1}", uid, subid));
            Debug.Log("login");
            var s = gated.Split(':');
  //         string ip = s[0];
  //         int port = int.Parse(s[1]);
            User u = new User();
            u.Server = "sample";
            u.Account = UserManager.Instance.username;
            u.Password = UserManager.Instance.pwd;
            u.Secret = secret;
            u.Uid = uid;
            UInt32 id = UInt32.Parse(Encoding.ASCII.GetString(uid));
            UserManager.Instance.uid = id;
            u.Subid = subid;
            Debug.Log("OnLoginCallback");
            ClientSocket.Instance.Auth(Def.IP, Def.port, u, null, OnAuthCallback);
        }
        else
        {
            Debug.Log("auth failture");  
        }
    }

    public void OnAuthCallback(bool ok, object ud, byte[] subid, byte[] secret)
    {
        if (ok)
        {
            ToastManager.Instance.Show("登录成功");
            Debug.Log("登录成功");
            UserManager.Instance.InitUserData(); 
        }
        else
        {

        }
    }

    //public void LoginCallback(C2sSprotoType.login.response resp)
    //{
    //    pop.loginBtn.isEnabled = true;
    //    if (resp.errorcode == 1)
    //    { 
    //        ToastManager.Instance.Show("登录成功");
    //        Debug.Log("登录成功");
    //        isLogin = true;
    //        UserManager.Instance.InitUserData(); 
    //    } 
    //}

    public void EntryGame()
    {
        Application.LoadLevelAsync("MainUI");
        BoxingMgr.Instance.InitBoxingList();
    }

    public void Regist(string n, string p)
    {
        C2sSprotoType.signup.request obj = new C2sSprotoType.signup.request(); 
        obj.password =p;
        pop.RegistBtn.isEnabled = true;

        string ip = Def.IP;
        int port = 3001;
        UserManager.Instance.username = n;
        UserManager.Instance.pwd = p;
        login.Auth(ip, port, "sample", UserManager.Instance.username, UserManager.Instance.pwd, null, OnSignupCallback); 
    } 

    public void OnSignupCallback(bool ok, object ud, byte[] secret, string dummy)
    {
        if (ok)
        {

            ToastManager.Instance.Show("注册成功");
            Debug.Log("注册成功");
        }
        else
        {
            ToastManager.Instance.Show("please enter your username."); 
        }
    }

    //public void RegistCallback(C2sSprotoType.signup.response resp)
    //{
    //    pop.RegistBtn.isEnabled = true;
    //    if (resp.errorcode == 1)
    //    {
    //        ToastManager.Instance.Show("注册成功");
    //        Debug.Log("注册成功");
    //    }
    //}

    //public void LoginOut()
    //{
    //    NetworkManager.Instance.LoginOut();
    //}

    //public void LoginOutCallback(C2sSprotoType.logout.response resp)
    //{
    //    ToastManager.Instance.Show("登出成功");
    //    UserManager.Instance.RestUserData();
    //    Application.LoadLevelAsync("Login");
    //}


}
