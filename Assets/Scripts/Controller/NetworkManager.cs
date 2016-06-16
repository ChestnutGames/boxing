using UnityEngine;
using System.Collections;
using Framework;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.Globalization;
using Sproto;

public class NetworkManager : UnitySingleton<NetworkManager>
{
    public bool IsNetworkAvailable { get; private set; }

    public delegate void UserFun(C2sSprotoType.user.response resp);
    public event UserFun UserCallBackEvent;

    public delegate void LoginFun(C2sSprotoType.login.response resp);
    public event LoginFun LoginCallBackEvent;

    //推送
    public delegate void NewMailFun(EmailData data);
    public event NewMailFun NewMailCallBackEvent;

    public delegate void LiLianUpdateFun(EmailData data);
    public event LiLianUpdateFun LiLianUpdateCallBackEvent;

    //抽奖
    public delegate void LotteryListFun(List<LotteryData> list);
    public event LotteryListFun LotteryListCallBackEvent;
    public delegate void LotteryChouFun(List<ItemViewData> list);
    public event LotteryChouFun LotteryChouCallBackEvent;


    public bool isLogin;

    //登录
    #region
    void Awake()
    {
        //ClientSocket.Instance.DisconnectEvent += Disconnect;
        InitS2c();
    }

    void InitS2c()
    {
        //注册响应
        ClientSocket.Instance.RegisterResponse(S2cNewMailRequest, "S2cNewMailRequest", null);
        ClientSocket.Instance.RegisterResponse(S2cLiLianUpdate, "S2cLiLianUpdate", null);
    }


    public void S2cLiLianUpdate(uint session, SprotoTypeBase requestObj, SprotoRpc.ResponseFunction Response, object ud)
    {
        var req = (S2cSprotoType.lilian_update.response)requestObj;
        S2cSprotoType.lilian_update.response obj = new S2cSprotoType.lilian_update.response();
        obj.errorcode = 1;
        obj.msg = "yes";
        ClientSocket.Instance.Response(session, obj, Response, ud);

        LiLianMgr.Instance.LiLianInfo();
    }

    public void S2cNewMailRequest(uint session, SprotoTypeBase requestObj, SprotoRpc.ResponseFunction Response, object ud)
    {
        //var req = (S2cSprotoType.newemail.request)rinfo.requestObj;

        //NetworkManager.Instance.NewMailCallBack(req);

        //S2cSprotoType.finish_achi.response obj = new S2cSprotoType.finish_achi.response();
        //obj.errorcode = 0;
        //obj.msg = "yes";
        //byte[] resp = rinfo.Response(obj);
        //PSocket.Send(resp, 0, resp.Length);
    }


    //错误状态提示
    public bool Error(long l, string msg)
    {
        string s = "code:" + l + "msg:" + msg + "状态 ：";
        switch (l)
        {
            case 2:
                Debug.Log(s + "离线");
                break;
            case 3:
                Debug.Log(s + "道具数不够");
                break;
            case 400:
                Debug.Log(s + "挑战失败");
                break;
            case 401:
                Debug.Log(s + "未处理授权");
                break;
            case 403:
                Debug.Log(s + "登录处理失败");
                break;
            case 406:
                Debug.Log(s + "已经登录");
                break;
        }
        Debug.Log(s);
        bool flag = false;
        if (l == 1 || l == 81 || l == 85)
        {
            flag = true;
        }
        else
        {
            ToastManager.Instance.Show(msg);
        }
        return flag;
    }
    public void Disconnect(SocketError socketError, PackageSocketError packageSocketError)
    {
        ToastManager.Instance.Show("断开连接");
        Debug.Log("disconnect");
        //ClientLogin(UserManager.Instance.username, UserManager.Instance.pwd); 
    }

    public void Regist(C2sSprotoType.signup.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.signup>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.signup.response)o;
            isLogin = true;
            Debug.Log("Regist" + resp.errorcode);
            //LoginMgr.Instance.RegistCallback(resp);
            if (resp.errorcode == 1)
            {

            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        }, obj, "Regist", null);
    }


    //public void ClientLogin(string name, string pwd)
    //{
    //    if (isLogin ==false)
    //    { 
    //        string ip = "192.168.1.239";
    //        int port = 3002;

    //        C2sSprotoType.login.request obj = new C2sSprotoType.login.request();
    //        obj.account = name;
    //        obj.password = pwd; 
    //        ClientSocket.Instance.Resquest<C2sProtocol.login>((session, o, ud) =>
    //        {
    //            var resp = (C2sSprotoType.login.response)o;
    //            Debug.Log("LoginResponse" + resp.errorcode);
    //            //LoginMgr.Instance.LoginCallback(resp); 
    //            if (resp.errorcode == 1)
    //            {
    //                isLogin = true;
    //            }
    //            else
    //            {
    //                Error(resp.errorcode, resp.msg);
    //            }

    //        },obj, "ClientLogin", null); 
    //    }
    //}



    public void LoginOut()
    {
        if (isLogin == false)
        {
            ClientSocket.Instance.Resquest<C2sProtocol.logout>((session, o, ud) =>
            {
                var resp = (C2sSprotoType.logout.response)o;
                Debug.Log("LoginOutResponse");
                isLogin = true;
                //LoginMgr.Instance.LoginOutCallback(resp);
                if (resp.errorcode == 1)
                {
                    UserManager.Instance.EndGame();
                }
                else
                {
                    Error(resp.errorcode, resp.msg);
                }

            },
           null, "LoginOut", null);
        }
    }

    public void LoginUserInfo()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user>((session, o, ud) =>
        {
            Debug.Log("LoginUserInfo");
            var resp = (C2sSprotoType.user.response)o;
            if (resp.errorcode == 1)
            {
                UserCallBackEvent(resp);
                LoginMgr.Instance.EntryGame();
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        null, "UserInfo", null);
    }


    public void UserInfo()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user.response)o;
            Debug.Log("LoginInfoResponse");
            if (resp.errorcode == 1)
            {
                Debug.Log("用户信息成功");
                UserCallBackEvent(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        null, "UserInfo", null);
    }

    public void UserCanModify()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user_can_modify_name>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user_can_modify_name.response)o;
            Debug.Log("user_can_modify_name");
            if (resp.errorcode == 1)
            {
                UserManager.Instance.canModifyName = true;
            }
            else
            {
                UserManager.Instance.canModifyName = false;
            }

        },
        null, "UserCanModify", null);
    }
    public void UserNameModify(C2sSprotoType.user_modify_name.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user_modify_name>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user_modify_name.response)o;
            Debug.Log("user_modify_name");
            if (resp.errorcode == 1)
            {
                UserInfoMgr.Instance.UserModifyNameCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "UserNameModify", null);
    }

    public void UserUpgrade()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user_upgrade>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user_upgrade.response)o;
            Debug.Log("user_upgrade");
            if (resp.errorcode == 1)
            {
                UserInfoMgr.Instance.UserUpgradeCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "UserUpgrade", null);
    }

    public void UserRandomNameModify()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user_random_name>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user_random_name.response)o;
            Debug.Log("user_random_name");
            if (resp.errorcode == 1)
            {
                UserInfoMgr.Instance.UserRandomNameCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        null, "UserRandomNameModify", null);
    }

    public void UserSignModify(C2sSprotoType.user_sign.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.user_sign>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.user_sign.response)o;
            Debug.Log("user_modify_name");
            if (resp.errorcode == 1)
            {
                UserInfoMgr.Instance.UserSignModifyCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "UserSignModify", null);
    }

    #endregion
    //角色
    #region
    public void RoleWake(C2sSprotoType.role_upgrade_star.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.role_upgrade_star>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.role_upgrade_star.response)o;
            Debug.Log("RoleWake");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.WakeUpCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "RoleWake", null);
    }

    public void RoleAll()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.role_all>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.role_all.response)o;
            Debug.Log("RoleAll");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.RoleListCallback(resp);
                Debug.Log("角色列表");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
       null, "RoleAll", null);
    }
    //角色解锁
    public void RoleRecruit(C2sSprotoType.role_recruit.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.role_recruit>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.role_recruit.response)o;
            Debug.Log("RoleRecruit");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.RoleUnlockCallback(resp);
                Debug.Log("角色解锁成功");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "RoleRecruit", null);
    }

    public void RoleBattle(C2sSprotoType.role_battle.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.role_battle>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.role_battle.response)o;
            Debug.Log("RoleBattle");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.RoleBattleCallback(resp);
                Debug.Log("角色上阵成功");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "RoleBattle", null);
    }
    #endregion
    //背包
    #region
    public void BagList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.props>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.props.response)o;
            Debug.Log("背包列表");
            BagMgr.Instance.BagListCallBack(resp);
        },
        null, "BagList", null);
    }

    public void ItemUse(ItemData data, int num, int role)
    {
        List<C2sSprotoType.prop> list = new List<C2sSprotoType.prop>();
        C2sSprotoType.prop p = new C2sSprotoType.prop();
        p.csv_id = data.id;
        p.num = num;
        list.Add(p);

        C2sSprotoType.use_prop.request obj = new C2sSprotoType.use_prop.request();
        obj.props = list;
        obj.role_id = role;
        ClientSocket.Instance.Resquest<C2sProtocol.use_prop>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.use_prop.response)o;
            Debug.Log("ItemUse rec" + resp.errorcode);
            BagMgr.Instance.UseItemCallBack(resp);
            if (resp.errorcode == 1)
            {
                Debug.Log("道具使用成");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        obj, "ItemUse", null);
    }

    public void ItemUse(List<ItemViewData> data, int role)
    {
        List<C2sSprotoType.prop> list = new List<C2sSprotoType.prop>();
        for (int i = 0; i < data.Count; i++)
        {
            C2sSprotoType.prop p = new C2sSprotoType.prop();
            p.csv_id = data[i].data.id;
            p.num = data[i].curCount;
            list.Add(p);
        }
        C2sSprotoType.use_prop.request obj = new C2sSprotoType.use_prop.request();
        obj.props = list;
        obj.role_id = role;
        ClientSocket.Instance.Resquest<C2sProtocol.use_prop>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.use_prop.response)o;
            Debug.Log("ItemUse");
            BagMgr.Instance.UseItemCallBack(resp);
            if (resp.errorcode == 1)
            {
                Debug.Log("道具使用成");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        },
        obj, "ItemUse", null);
    }

    #endregion
    //邮件
    #region
    //public void NewMailCallBack(S2cSprotoType.newemail.request resq)
    //{ 
    //    EmailData d = new EmailData();
    //    if (NewMailCallBackEvent != null && resq != null && resq.newmail != null)
    //    {   
    //            d.date = "2011";//resp.mail_list[i].acctime;
    //            d.desc = resq.newmail.content;
    //            d.from = "系统";
    //            ItemData temp = GameShared.Instance.GetItemData((int)resq.newmail.iconid);
    //            if (temp != null)
    //            {
    //                d.icon = temp.path;
    //            }
    //            d.id = (int)resq.newmail.emailid;
    //            d.isRead = resq.newmail.isread;
    //            d.isRevice = resq.newmail.isreward;
    //            d.name = resq.newmail.title;
    //            d.type = (EmailData.EmailType)resq.newmail.type;
    //            if (resq.newmail.attachs != null)
    //            {
    //                List<ItemViewData> l = new List<ItemViewData>();
    //                for (int j = 0; j < resq.newmail.attachs.Count; j++)
    //                {
    //                    ItemViewData item = new ItemViewData();
    //                    item.curCount = (int)resq.newmail.attachs[j].itemnum;
    //                    ItemData a = new ItemData();
    //                    a = GameShared.Instance.GetItemData((int)resq.newmail.attachs[j].itemsn);
    //                    item.data = a;
    //                    l.Add(item);
    //                }
    //                d.itemList = l;
    //            }  
    //        NewMailCallBackEvent(d);
    //    }
    //    else
    //    {
    //        NewMailCallBackEvent(d);
    //    }
    //}

    public void MailList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.mails>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.mails.response)o;
            if (Error(resp.errorcode, resp.msg))
            {
                Debug.Log("MailList");
                EmailMgr.Instance.MailListCallBack(resp);
            }
        },
        null, "MailList", null);
    }

    public void MailRead(List<long> id)
    {
        List<C2sSprotoType.idlist> list = new List<C2sSprotoType.idlist>();
        for (int i = 0; i < id.Count; i++)
        {
            C2sSprotoType.idlist d = new C2sSprotoType.idlist();
            d.id = id[i];
            list.Add(d);
        }
        C2sSprotoType.mail_read.request obj = new C2sSprotoType.mail_read.request();
        obj.mail_id = list;
        ClientSocket.Instance.Resquest<C2sProtocol.mail_read>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.mail_read.response)o;
            if (Error(resp.errorcode, resp.msg))
            {
                Debug.Log("阅读邮件");
                EmailMgr.Instance.MailReadCallBack();
            }
        },
        obj, "MailRead", null);
    }

    public void MailReceive(List<long> id)
    {
        List<C2sSprotoType.idlist> list = new List<C2sSprotoType.idlist>();
        for (int i = 0; i < id.Count; i++)
        {
            C2sSprotoType.idlist d = new C2sSprotoType.idlist();
            d.id = id[i];
            list.Add(d);
        }
        C2sSprotoType.mail_getreward.request obj = new C2sSprotoType.mail_getreward.request();
        obj.mail_id = list;
        ClientSocket.Instance.Resquest<C2sProtocol.mail_getreward>((session, o, ud) =>
        {
            Debug.Log("MailReceive");
            var resp = (C2sSprotoType.mail_getreward.response)o;
            EmailMgr.Instance.MailReceiveCallBack(resp);
            Error(resp.errorcode, resp.msg);
        },
        obj, "MailReceive", null);
    }

    public void MailDelete(List<long> id)
    {
        List<C2sSprotoType.idlist> list = new List<C2sSprotoType.idlist>();
        for (int i = 0; i < id.Count; i++)
        {
            C2sSprotoType.idlist d = new C2sSprotoType.idlist();
            d.id = id[i];
            list.Add(d);
        }
        C2sSprotoType.mail_delete.request obj = new C2sSprotoType.mail_delete.request();
        obj.mail_id = list;
        ClientSocket.Instance.Resquest<C2sProtocol.mail_delete>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.mail_delete.response)o;
            if (Error(resp.errorcode, resp.msg))
            {
                Debug.Log("MailDel");
                EmailMgr.Instance.MailDelCallBack();
            }
        },
        obj, "MailDelete", null);
    }
    #endregion
    //成就
    #region
    public void AchievementList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.achievement>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.achievement.response)o;
            Debug.Log("AchievementListResponse" + resp.errorcode);
            if (resp.errorcode == 1)
            {
                AchievementMgr.Instance.AchievementListCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "AchievementList", null);
    }

    public void AchievementReceive(int id)
    {
        C2sSprotoType.achievement_reward_collect.request obj = new C2sSprotoType.achievement_reward_collect.request();
        obj.csv_id = id;
        ClientSocket.Instance.Resquest<C2sProtocol.achievement_reward_collect>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.achievement_reward_collect.response)o;
            Debug.Log("AchievementReceive");
            if (resp.errorcode == 1)
            {
                AchievementMgr.Instance.RevcieCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "AchievementReceive", null);
    }

    #endregion
    //好友
    #region
    public void FriendList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.friend_list>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.friend_list.response)o;
            Debug.Log("FriendList");
            if (resp.errorcode == 1)
            {
                FriendMgr.Instance.FriendListCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        },
        null, "FriendList", null);
    }

    public void FriendApplyList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.applied_list>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.applied_list.response)o;
            Debug.Log("FriendApplyList");
            if (resp.errorcode == 1)
            {
                FriendMgr.Instance.FriendApplyCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        },
        null, "FriendApplyList", null);
    }


    public void FriendAddList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.otherfriend_list>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.otherfriend_list.response)o;
            Debug.Log("FriendApplyList");
            if (resp.errorcode == 1)
            {
                FriendMgr.Instance.FriendAddCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
        null, "FriendAddList", null);
    }

    public void FriendFind(int id)
    {
        C2sSprotoType.findfriend.request obj = new C2sSprotoType.findfriend.request();
        obj.id = id;
        ClientSocket.Instance.Resquest<C2sProtocol.findfriend>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.findfriend.response)o;
            Debug.Log("FriendApplyList");
            if (resp.errorcode == 1 || resp.errorcode == Def.Er_NotFindFrind)
            {
                FriendMgr.Instance.FriendFindCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        },
       obj, "FriendFind", null);
    }

    public void FriendApply(C2sSprotoType.applyfriend.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.applyfriend>((session, o, ud) =>
        {
            //var resp = (C2sSprotoType.applyfriend.response)o;
            //if (resp.errorcode == 1)
            //{ 
            //}
            //else
            //{
            //    Error(resp.errorcode, resp.msg);
            //}
        },
       obj, "FriendApply", null);
    }

    public void FriendAccept(C2sSprotoType.recvfriend.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.recvfriend>((session, o, ud) =>
        {
            //var resp = (C2sSprotoType.recvfriend.response)o;
            Debug.Log("FriendAccept");
            //FriendMgr.Instance.FriendReceiveCallBack(resp);
        },
       obj, "FriendAccept", null);
    }
    public void FriendRefuse(C2sSprotoType.refusefriend.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.refusefriend>((session, o, ud) =>
        {
            //var resp = (C2sSprotoType.refusefriend.response)o;
            Debug.Log("FriendRefuse");
            //FriendMgr.Instance.FriendRefuseCallBack(resp);
        },
       obj, "FriendRefuse", null);
    }

    public void FriendDelete(int id)
    {
        C2sSprotoType.deletefriend.request obj = new C2sSprotoType.deletefriend.request();
        obj.friendid = id;
        obj.type = 0;
        obj.signtime = 0;
        ClientSocket.Instance.Resquest<C2sProtocol.deletefriend>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.deletefriend.response)o;
            Debug.Log("FriendDelete");
            if (resp.errorcode == 1)
            {
                Debug.Log("删除成");
                FriendMgr.Instance.FriendDelCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
            //FriendMgr.Instance.FriendDelCallBack(resp);
        },
       obj, "FriendDelete", null);
    }

    public void ReceviceHeart(C2sSprotoType.recvheart.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.recvheart>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recvheart.response)o;
            Debug.Log("FriendApplyList");
            if (resp.errorcode == 1)
            {
                Debug.Log("送心成");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        },
       obj, "ReceviceHeart", null);
    }

    public void SendHeart(C2sSprotoType.sendheart.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.sendheart>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.sendheart.response)o;
            Debug.Log("FriendApplyList");
            if (resp.errorcode == 1)
            {
                Debug.Log("接受心成");
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
            //FriendMgr.Instance.FriendSendCallBack(resp);
        },
       obj, "SendHeart", null);
    }
    #endregion
    //商店 
    #region
    public void ShopList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.shop_all>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.shop_all.response)o;
            Debug.Log("ShopList");
            if (resp.errorcode == 1)
            {
                StoreMgr.Instance.ShopListCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "ShopList", null);
    }

    public void ShopPurchase(List<ProductData> list)
    {
        C2sSprotoType.shop_purchase.request obj = new C2sSprotoType.shop_purchase.request();
        List<C2sSprotoType.goodsbuy> l = new List<C2sSprotoType.goodsbuy>();
        for (int i = 0; i < list.Count; i++)
        {
            C2sSprotoType.goodsbuy g = new C2sSprotoType.goodsbuy();
            g.goods_id = list[i].csv_id;
            g.goods_num = list[i].buy_count;
            l.Add(g);
            Debug.Log(" g.goods_id" + g.goods_id + "g.goods_num" + g.goods_num);
        }
        obj.g = l;
        ClientSocket.Instance.Resquest<C2sProtocol.shop_purchase>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.shop_purchase.response)o;
            Debug.Log("ShopPurchase");
            if (resp.errorcode == 1)
            {
                Debug.Log("购买成功");
                StoreMgr.Instance.PurchaseCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "ShopPurchase", null);
    }

    public void ShopRefresh(int id)
    {
        C2sSprotoType.shop_refresh.request obj = new C2sSprotoType.shop_refresh.request();
        obj.goods_id = id;
        ClientSocket.Instance.Resquest<C2sProtocol.shop_refresh>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.shop_refresh.response)o;
            Debug.Log("ShopRefresh");
            if (resp.errorcode == 1)
            {
                Debug.Log("刷新成功");
                StoreMgr.Instance.ShopRefreshCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "ShopRefresh", null);
    }
    #endregion
    //充值
    #region
    public void RechargeList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.recharge_all>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recharge_all.response)o;
            Debug.Log("RechargeList");
            RechargeMgr.Instance.RechargeListCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, null, "RechargeList", null);
    }

    public void RechargeSwaredList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.recharge_vip_reward_all>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recharge_vip_reward_all.response)o;
            Debug.Log("RechargeSwaredList");
            if (resp.errorcode == 1)
            {
                RechargeMgr.Instance.RewardListCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "RechargeSwaredList", null);
    }

    public void RechargeSwared(C2sSprotoType.recharge_vip_reward_collect.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.recharge_vip_reward_collect>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recharge_vip_reward_collect.response)o;
            Debug.Log("RechargeSwared");
            RechargeMgr.Instance.RewaredCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "RechargeSwared", null);
    }


    public void RechargePurchase(List<C2sSprotoType.recharge_buy> list)
    {
        C2sSprotoType.recharge_purchase.request obj = new C2sSprotoType.recharge_purchase.request();
        obj.g = list;
        ClientSocket.Instance.Resquest<C2sProtocol.recharge_purchase>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recharge_purchase.response)o;
            Debug.Log("RechargePurchase");
            if (resp.errorcode == 1)
            {
                Debug.Log("充值成功");
                RechargeMgr.Instance.RechargePurchaseCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "RechargePurchase", null);
    }
    public void RechargeVipPurchase(C2sSprotoType.recharge_vip_reward_purchase.request obj)
    {

        ClientSocket.Instance.Resquest<C2sProtocol.recharge_vip_reward_purchase>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.recharge_vip_reward_purchase.response)o;
            Debug.Log("recharge_vip_reward_purchase");
            RechargeMgr.Instance.RechargeVipPurchaseCallBack(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "RechargeVipPurchase", null);
    }
    #endregion
    //抽奖
    #region
    public void LotteryListBack(C2sSprotoType.draw.response resp)
    {
        List<LotteryData> list = new List<LotteryData>();
        for (int i = 0; i < resp.list.Count; i++)
        {
            LotteryData re = new LotteryData();
            if (resp.list[i].lefttime == 0)
            {
                re.isShowTime = false;
            }
            else
            {
                re.lefttime = (int)resp.list[i].lefttime;
                re.isShowTime = true;
                int a = (int)resp.list[i].lefttime;
                re.refresh_time = DateTime.Now.AddSeconds(a);
            }
            re.id = (int)resp.list[i].drawtype;
            re.drawnum = (int)resp.list[i].drawnum;
            list.Add(re);
        }
        LotterMgr.Instance.LotteryListCallback(list);
    }

    public void LotteryList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.draw>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.draw.response)o;
            Debug.Log("LotteryList");
            LotteryListBack(resp);
        },
       null, "LotteryList", null);
    }


    public void LotteryChou(int id, bool mian)
    {
        C2sSprotoType.applydraw.request obj = new C2sSprotoType.applydraw.request();
        obj.drawtype = id;
        obj.iffree = mian;
        ClientSocket.Instance.Resquest<C2sProtocol.applydraw>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.applydraw.response)o;
            Debug.Log("LotteryChou");
            LotterMgr.Instance.ChouCallback(resp);
            Error(resp.errorcode, resp.msg);
        },
       obj, "LotteryChou", null);
    }
    #endregion
    //用户信息
    #region
    #endregion
    //日常
    #region
    public void DailyList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkin>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkin.response)o;
            Debug.Log("DailyList");
            DailyMgr.Instance.DailyListCallBack(resp);


        }, null, "DailyList", null);
    }

    public void DailySign()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkin_aday>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkin_aday.response)o;
            Debug.Log("DailySign");
            if (resp.errorcode == 1)
            {
                DailyMgr.Instance.SignCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "DailySign", null);
    }

    public void DailyRecivec(C2sSprotoType.checkin_reward.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkin_reward>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkin_reward.response)o;
            Debug.Log("DailyRecivec");
            DailyMgr.Instance.ReceiveCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "DailyRecivec", null);
    }

    public void DuanList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.exercise>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.exercise.response)o;
            Debug.Log("DuanList");
            DailyMgr.Instance.DuanLianListCallback(resp);

        }, null, "DuanList", null);
    }

    public void Duan(C2sSprotoType.exercise_once.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.exercise_once>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.exercise_once.response)o;
            Debug.Log("Duan");
            DailyMgr.Instance.DuanLianCallback(resp);
            Error(resp.errorcode, resp.msg);

        }, obj, "Duan", null);
    }


    public void Gold(C2sSprotoType.c_gold_once.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.c_gold_once>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.c_gold_once.response)o;
            Debug.Log("Gold");
            DailyMgr.Instance.GoldCallback(resp);
            Error(resp.errorcode, resp.msg);

        }, obj, "Gold", null);
    }

    public void GoldList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.c_gold>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.c_gold.response)o;
            Debug.Log("GoldList");
            DailyMgr.Instance.GoldListCallback(resp);

        }, null, "GoldList", null);
    }
    #endregion
    //装备
    #region
    public void EquipList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.equipment_all>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.equipment_all.response)o;
            Debug.Log("EquipList");
            EquipmentMgr.Instance.EquipListCallback(resp);
        }, null, "EquipList", null);
    }
    public void EquipIntensify(C2sSprotoType.equipment_enhance.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.equipment_enhance>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.equipment_enhance.response)o;
            Debug.Log("EquipIntensify");
            EquipmentMgr.Instance.IntensifyCallback(resp);
            if (resp.errorcode == 1 || resp.errorcode == Def.Er_FailEquip)
            {

            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }
        }, obj, "EquipIntensify", null);
    }
    #endregion
    //拳法
    #region
    public void BoxingList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.kungfu>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.kungfu.response)o;
            Debug.Log("BoxingList");
            BoxingMgr.Instance.InitBoxingListCallback(resp);
            //Error(resp.errorcode, resp.msg);
        }, null, "BoxingList", null);
    }
    public void BoxingUp(C2sSprotoType.kungfu_levelup.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.kungfu_levelup>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.kungfu_levelup.response)o;
            Debug.Log("BoxingUp" + resp.errorcode);
            BoxingMgr.Instance.UpLevelCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "BoxingUp", null);
    }
    public void BoxingChose(C2sSprotoType.kungfu_chose.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.kungfu_chose>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.kungfu_chose.response)o;
            Debug.Log("BoxingChose");
            BoxingMgr.Instance.SaveBoxingCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "BoxingChose", null);
    }
    #endregion

    //洗练 
    #region
    public void XiLianList(C2sSprotoType.xilian.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.xilian>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.xilian.response)o;
            Debug.Log("BoxingList");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.XiLianCallBack(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "XiLianList", null);
    }
    public void XiLianSave(C2sSprotoType.xilian_ok.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.xilian_ok>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.xilian_ok.response)o;
            Debug.Log("BoxingList");
            if (resp.errorcode == 1)
            {
                RolesMgr.Instance.XiLianOkCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "XiLianSave", null);
    }
    #endregion

    //关卡
    #region
    public void CheckPointChose(C2sSprotoType.checkpoint_hanging_choose.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_hanging_choose>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkpoint_hanging_choose.response)o;
            Debug.Log("CheckPointChose");
            if (resp.errorcode == 1)
            {
                LevelsMgr.Instance.SelectLevelCallback(resp);
            }
            else
            {
                //                Error(resp.errorcode, resp.msg);
            }

        }, obj, "CheckPointChose", null);
    }

    public void CheckPointBattleExit(C2sSprotoType.checkpoint_battle_exit.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_battle_exit>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkpoint_battle_exit.response)o;
            Debug.Log("CheckPointBattleExit");
            if (resp.errorcode == 1)
            {
                LevelsMgr.Instance.BattleOverCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "CheckPointBattleExit", null);
    }

    public void CheckPointBattleEnter(C2sSprotoType.checkpoint_battle_enter.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_battle_enter>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkpoint_battle_enter.response)o;
            Debug.Log("CheckPointBattleEnter");
            if (resp.errorcode == 1)
            {
                LevelsMgr.Instance.BattleEnterCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, obj, "CheckPointBattleEnter", null);
    }

    public void CheckPointChapter()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_chapter>((session, o, ud) =>
        {
            Debug.Log("CheckPointChapter");
            var resp = (C2sSprotoType.checkpoint_chapter.response)o;
            if (resp.errorcode == 1)
            {
                LevelsMgr.Instance.InitChapterAndLevelData(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "CheckPointChapter", null);
    }

    public void CheckPointHanging()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_hanging>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkpoint_hanging.response)o;
            Debug.Log("CheckPointHanging");
            if (resp.errorcode == 1)
            {
                UserManager.Instance.CheckPointHangingCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "CheckPointHanging", null);
    }

    public void CheckPointExit()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.checkpoint_exit>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.checkpoint_exit.response)o;
            Debug.Log("CheckPointExit");
            if (resp.errorcode == 1)
            {
                //UserManager.Instance.CheckPointHangingCallback(resp);
            }
            else
            {
                Error(resp.errorcode, resp.msg);
            }

        }, null, "CheckPointExit", null);
    }
    #endregion

    //历练
    #region
    public void GetStrength()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_get_phy_power>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_get_phy_power.response)o;
            Debug.Log("GetStrength");
            LiLianMgr.Instance.StrengthCallback(resp);
            Error(resp.errorcode, resp.msg);

        }, null, "GetStrength", null);
    }

    public void LiLianRewardList(C2sSprotoType.lilian_get_reward_list.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_get_reward_list>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_get_reward_list.response)o;
            Debug.Log("LiLianRewardList" + resp.errorcode);
            LiLianMgr.Instance.GetLiLianSwardCallback(resp);
            //LevelsMgr.Instance.SelectLevelCallback(resp); 
            //Error(resp.errorcode, resp.msg); 
        }, obj, "LiLianRewardList", null);
    }

    public void LiLianSaveRewardList(C2sSprotoType.lilian_rewared_list.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_rewared_list>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_rewared_list.response)o;
            Debug.Log("LiLianSaveRewardList" + resp.errorcode);
            LiLianMgr.Instance.GetLiLianSaveSwardListCallback(resp);
            //LevelsMgr.Instance.SelectLevelCallback(resp); 
            //Error(resp.errorcode, resp.msg); 
        }, obj, "LiLianSaveRewardList", null);
    }

    public void LiLianInfo()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.get_lilian_info>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.get_lilian_info.response)o;
            Debug.Log("LiLianInfo");
            LiLianMgr.Instance.LiLianInfoCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, null, "LiLianInfo", null);
    }

    public void LiLianStart(C2sSprotoType.start_lilian.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.start_lilian>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.start_lilian.response)o;
            Debug.Log("LiLianStart");
            LiLianMgr.Instance.LiLianCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "LiLianStart", null);
    }

    public void BuyStrength()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_purch_phy_power>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_purch_phy_power.response)o;
            Debug.Log("BuyStrength");
            LiLianMgr.Instance.BuyCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, null, "BuyStrength", null);
    }

    public void QuickLiLian(C2sSprotoType.lilian_inc.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_inc>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_inc.response)o;
            Debug.Log("QuickLiLian");
            LiLianMgr.Instance.QuickLiLianCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "QuickLiLian", null);
    }

    public void RestHall(C2sSprotoType.lilian_reset_quanguan.request obj)
    {
        ClientSocket.Instance.Resquest<C2sProtocol.lilian_reset_quanguan>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.lilian_reset_quanguan.response)o;
            Debug.Log("RestHall");
            LiLianMgr.Instance.RestHallCallback(resp);
            Error(resp.errorcode, resp.msg);
        }, obj, "RestHall", null);
    }


    #endregion

    //竞技场
    #region
    /// <summary>
    /// 排行榜
    /// </summary>
    public void ArenaRankList()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_lp>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_lp.response)o;
            Debug.Log("ArenaRankList");
            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaRankList, param);
            Error(resp.errorcode, resp.msg);
        }, null, "ArenaRankList", null);
    }

    public void ArenaExit()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_lp>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_lp.response)o;
            Debug.Log("ArenaExit");
            Error(resp.errorcode, resp.msg);
        }, null, "ArenaExit", null);
    }
    /// <summary>
    /// 竞技场刷新
    /// </summary>
    /// 
    public void ArenaInfo()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_enter>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_enter.response)o;
            Debug.Log("ArenaInfo");
            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaEnter, param);
            Error(resp.errorcode, resp.msg);
        }, null, "ArenaInfo", null);
    }
    public void ArenaListRefresh()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_rfh>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_rfh.response)o;
            Debug.Log("ArenaListRefresh");
            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaListRefresh, param);
            Error(resp.errorcode, resp.msg);
        }, null, "ArenaListRefresh", null);
    }
    /// <summary>
    /// 竞技场购买刷新
    /// </summary>
    public void ArenaBuyRefrshCount()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_clg_tms_purchase>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_clg_tms_purchase.response)o;
            Debug.Log("ArenaBuyRefrshCount");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaBuyRefresh, param);

            Error(resp.errorcode, resp.msg);
        }, null, "ArenaBuyRefrshCount", null);
    }
    /// <summary>
    /// 竞技场领取奖励
    /// </summary>
    public void ArenaRewardCollected()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.ara_rnk_reward_collected>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_rnk_reward_collected.response)o;
            Debug.Log("ArenaBuyRefrshCount");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaRewardCollected, param);

            Error(resp.errorcode, resp.msg);
        }, null, "ArenaRewardCollected", null);
    }

    /// <summary>
    /// 竞技场膜拜
    /// </summary>
    public void Worship(int id)
    {
        C2sSprotoType.ara_worship.request obj = new C2sSprotoType.ara_worship.request();
        //obj.uid = id;
        ClientSocket.Instance.Resquest<C2sProtocol.ara_clg_tms_purchase>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_worship.response)o;
            Debug.Log("ArenaBuyRefrshCount");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.Worship, param);

            Error(resp.errorcode, resp.msg);
        }, obj, "Worship", null);
    }
    public void AraConvertPts(long l)
    {
        C2sSprotoType.ara_convert_pts.request obj = new C2sSprotoType.ara_convert_pts.request();
        obj.pts = l;
        ClientSocket.Instance.Resquest<C2sProtocol.ara_convert_pts>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_convert_pts.response)o;
            Debug.Log("AraConvertPts");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.AraConvertPts, param);

            Error(resp.errorcode, resp.msg);
        }, obj, "AraConvertPts", null);
    }

    public void AraRoleChooseEnter(long uid)
    {
        C2sSprotoType.ara_choose_role_enter.request obj = new C2sSprotoType.ara_choose_role_enter.request();
        obj.enemy_id = uid;
        ClientSocket.Instance.Resquest<C2sProtocol.ara_choose_role_enter>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_choose_role_enter.response)o;
            Debug.Log("AraRoleChooseEnter");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaBattleRoleList, param);

            Error(resp.errorcode, resp.msg);
        }, obj, "AraRoleChooseEnter", null);
    }

    public void AraRoleChoose(List<long> l)
    {
        C2sSprotoType.ara_choose_role.request obj = new C2sSprotoType.ara_choose_role.request();
        obj.bat_roleid = l;
        ClientSocket.Instance.Resquest<C2sProtocol.ara_choose_role>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ara_choose_role.response)o;
            Debug.Log("AraRoleChoose");

            //ActionParam param = new ActionParam();
            //param["resp"] = resp;
            //GameMain.Instance.CallLogicAction(Def.LogicActionDefine.Worship, param);

            Error(resp.errorcode, resp.msg);
        }, obj, "AraRoleChoose", null);
    }
    /// <summary>
    /// 有角色死亡调用
    /// </summary>
    public void NextRole()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.Arena_OnPrepareNextRole>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.Arena_OnPrepareNextRole.response)o;
            Debug.Log("AraRoleChoose");

            //ActionParam param = new ActionParam();
            //param["resp"] = resp;
            //GameMain.Instance.CallLogicAction(Def.LogicActionDefine.Worship, param);

            Error(resp.errorcode, resp.msg);
        }, null, "NextRole", null);
    }

    #endregion

    //核心战斗
    #region
    public void LevelBattleBegin(int roleid)
    {
        C2sSprotoType.BeginGUQNQIACoreFight.request obj = new C2sSprotoType.BeginGUQNQIACoreFight.request();
        obj.monsterid = roleid;
        ClientSocket.Instance.Resquest<C2sProtocol.BeginGUQNQIACoreFight>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.BeginGUQNQIACoreFight.response)o;
            Debug.Log("BattleBegin");
            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.LevelFightBegin, param);

            Error(resp.errorcode, "");
        }, obj, "LevelBattleBegin", null);
    }

    public void SingleLevelBattleBegin(int roleid)
    {
        C2sSprotoType.TMP_BeginGUQNQIACoreFight.request obj = new C2sSprotoType.TMP_BeginGUQNQIACoreFight.request();
        obj.monsterid = roleid;
        ClientSocket.Instance.Resquest<C2sProtocol.TMP_BeginGUQNQIACoreFight>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.TMP_BeginGUQNQIACoreFight.response)o;
            Debug.Log("BattleBegin");
            ActionParam param = new ActionParam();
            param["resp"] = resp;
            BattleManager.Instance.bufAct = new ActQueueData();
            BattleManager.Instance.bufAct.kf_id = (int)resp.kf_id;
            BattleManager.Instance.bufAct.who = (int)resp.firstfighter;
            BattleManager.Instance.bufAct.kfType = (int)Def.BattleAttackType.Normal;
            BattleManager.Instance.bufAct.fight_type = Def.FightType.Auto;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.LevelFightBegin, param);

            Error(resp.errorcode, "");
        }, obj, "LevelBattleBegin", null);
    }

    public void LevelBattleList(List<C2sSprotoType.BattleListElem> list)
    {
        C2sSprotoType.GuanQiaBattleList.request obj = new C2sSprotoType.GuanQiaBattleList.request();
        obj.fightlist = list;
        ClientSocket.Instance.Resquest<C2sProtocol.GuanQiaBattleList>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.GuanQiaBattleList.response)o;
            Debug.Log("LevelBattleList");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.LevelFightList, param);

            Error(resp.errorcode, "");
        }, obj, "LevelBattleList", null);
    }

    public void SingleBattleList(ActQueueData curAct, ClientSocket.RespCb resp)
    {
        C2sSprotoType.TMP_GuanQiaBattleList.request obj = new C2sSprotoType.TMP_GuanQiaBattleList.request();
        C2sSprotoType.BattleListElem e = new C2sSprotoType.BattleListElem();
        e.attcktype = (int)curAct.fight_type; //自动还是手动 
        e.kf_type = (int)curAct.kfType;
        e.kf_id = (int)curAct.kf_id;
        e.fighterid = curAct.who;
        e.random_combo_num = curAct.comboCount;
        obj.fightinfo = e;
        ClientSocket.Instance.Resquest<C2sProtocol.TMP_GuanQiaBattleList>(resp, obj, "SingleBattleList", null);
    }

    public void ArenaBattleList(List<C2sSprotoType.BattleListElem> list)
    {
        C2sSprotoType.ArenaBattleList.request obj = new C2sSprotoType.ArenaBattleList.request();
        obj.fightlist = list;
        ClientSocket.Instance.Resquest<C2sProtocol.ArenaBattleList>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.ArenaBattleList.response)o;
            Debug.Log("ArenaBattleList");

            ActionParam param = new ActionParam();
            param["resp"] = resp;
            GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaFightList, param);

            Error(resp.errorcode, "");
        }, obj, "ArenaBattleList", null);
    }
    public void ArenaBeginBattle(int id)
    {
        C2sSprotoType.BeginArenaCoreFight.request obj = new C2sSprotoType.BeginArenaCoreFight.request();
        obj.monsterid = id;
        ClientSocket.Instance.Resquest<C2sProtocol.BeginArenaCoreFight>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.BeginArenaCoreFight.response)o;
            Debug.Log("LevelBattleList");


            if (Error(resp.errorcode, ""))
            {
                ActionParam param = new ActionParam();
                param["resp"] = resp;
                GameMain.Instance.CallLogicAction(Def.LogicActionDefine.ArenaBattleEnter, param);
            }
        }, obj, "LevelBattleList", null);
    }




    public void AppUnFocusBattle()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.OnNormalExitCoreFight>((session, o, ud) =>
        {

        }, null, "AppUnFocusBattle", null);
    }
    public void AppFocusBattle()
    {
        ClientSocket.Instance.Resquest<C2sProtocol.OnReEnterCoreFight>((session, o, ud) =>
        {
            var resp = (C2sSprotoType.OnReEnterCoreFight.response)o;
            Error(resp.errorcode, "");
        }, null, "AppFocusBattle", null);
    }
    #endregion
}
