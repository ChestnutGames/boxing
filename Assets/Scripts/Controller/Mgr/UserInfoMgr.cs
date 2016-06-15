using UnityEngine;
using System.Collections; 
using Assets.Scripts.Common;
using System.Collections.Generic;
using LuaInterface;
using System;

public class UserInfoMgr : UnitySingleton<UserInfoMgr>
{
    private UserInfoPop pop;
    private ModifyNamePop namePop;
    private ModifySignaturePop signPop;

    
     
    private LuaState l;
     
    public void OpenPop(UserInfoPop p)
    {
        pop = p; 
        UserManager.Instance.RestUserAttr(); 
        SetInfo();
        SetLevelBtnInfo();
    }

    public void Start()
    {
        //InitLua();  
    }



    public void InitLua()
    {
        TextAsset scriptFile = Resources.Load<TextAsset>(Def.Lua_UserInfo);
        l = new LuaState();
        l.DoString(scriptFile.text);

        LuaFunction f = l.GetFunction("InitLua");
        f.Call(this);
    }

    public void SetLevelBtnInfo()
    {
        bool level; int power; int exp;
        if (UserManager.Instance.level < GameShared.Instance.config.user_level_max)
        {
            level = true;
        }
        else
        {
            level = false;
        }
        power = UserInfoMgr.Instance.getUpPower();
        exp = UserInfoMgr.Instance.GetUpExp();
        pop.SetLevelBtnInfo(level, power, exp);
    }

    public void SetInfo()
    {
        UserAttrs attr = UserManager.Instance.userAttr;
        if (attr != null)
        {
            pop.SetName(UserManager.Instance.nickName);
            pop.SetSign(UserManager.Instance.signTxt);
            pop.level.text = UserManager.Instance.level.ToString();
            pop.vip.text = UserManager.Instance.vip_level.ToString();
            pop.power.text = ((int)attr.attrArr[(int)Def.AttrType.FightPower]).ToString();
            pop.crit.text = ((int)attr.attrArr[(int)Def.AttrType.Crit]).ToString();
            pop.defense.text = ((int)attr.attrArr[(int)Def.AttrType.Defense]).ToString();
            pop.pray.text = ((int)attr.attrArr[(int)Def.AttrType.Pray]).ToString();

            pop.sign.text = UserManager.Instance.signTxt;
            if (UserManager.Instance.GetCurRole() != null && UserManager.Instance.GetCurRole().starData != null)
            {
                pop.rolePower.text = (UserManager.Instance.GetCurRole().starData.battleAddition[(int)Def.AttrType.FightPower]).ToString()+"%";
                pop.roleCrit.text = (UserManager.Instance.GetCurRole().starData.battleAddition[(int)Def.AttrType.Crit]).ToString() + "%";
                pop.SetWakeLevel(UserManager.Instance.GetCurRole().wakeLevel);
                 
                pop.anim.skeletonDataAsset = GameShared.Instance.GetSkeletonAssetByPath(UserManager.Instance.GetCurRole().path); 
                pop.anim.Reset();
            }
            else
            {
                pop.rolePower.text = 0+"";
                pop.roleCrit.text = 0+"";
            }
        }
        string[] s = UserManager.Instance.GetCurRole().starData.strs.Split('*');
        pop.SetTalk(s[0]); 
    }

    public int GetUpExp()
    {
        //LuaFunction f = l.GetFunction("GetUpExp");
        //object[] obj = f.Call();
        //return Convert.ToInt32(obj[0]);
        int num = 0;
        UserLevelData d = GameShared.Instance.GetUserLevelByLevel(UserManager.Instance.level+1);
        if (d != null)
            num = d.exp;
        return num;
    }

    public int getUpPower()
    {
        //LuaFunction f = l.GetFunction("getUpPower");
        //object[] obj = f.Call(); 
        //return Convert.ToInt32(obj[0]);
        return (int)UserManager.Instance.userAttr.GetUpLevelPower(UserManager.Instance.level);
    }

    public void NameModifyPop()
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.UserNameModify) != true)
        {
            GameObject obj = Instantiate(pop.namePop);
            obj.SetActive(true);
            ModifyNamePop p = obj.GetComponent<ModifyNamePop>();
            p.InitData(UserManager.Instance.nickName); 
            p.transform.parent = pop.transform.parent;
            p.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.UserNameModify, true);
        } 
    }

    public void SignModifyPop()
    {
        if (MainUI.Instance.GetPopState(MainUI.PopType.UserNameModify) != true)
        {
            GameObject obj = Instantiate(pop.signPop);
            obj.SetActive(true);
            ModifySignaturePop p = obj.GetComponent<ModifySignaturePop>();
            p.InitData(UserManager.Instance.signTxt);
            p.SetName(UserManager.Instance.signTxt);
            p.transform.parent = pop.transform.parent;
            p.transform.localScale = Vector3.one;
            MainUI.Instance.SetPopState(MainUI.PopType.UserSignModify, true);
        }
    }   

    public void ModifySignature(ModifySignaturePop p, string str)
    {
        if (!str.Equals(""))
        {
            signPop = p;
            C2sSprotoType.user_sign.request obj = new C2sSprotoType.user_sign.request();
            obj.sign = str;
            signPop.saveSignBtn.isEnabled = false;
            NetworkManager.Instance.UserSignModify(obj);
        }
        else
        {
            ToastManager.Instance.Show("不能为空");
        }
    }

    public void UserSignModifyCallback(C2sSprotoType.user_sign.response resp)
    {
        signPop.saveSignBtn.isEnabled = true;
        if (signPop != null && resp.errorcode == 1)
        {
            //LuaFunction f = l.GetFunction("ModifySignatureCallBack");
            //f.Call(signPop);  
            UserManager.Instance.signTxt = signPop.name.label.text;
            pop.SetSign(UserManager.Instance.signTxt);
            signPop.CloseClick();
            
        }
        signPop = null;
    }

    public void ModifyName(ModifyNamePop p, string str)
    {
        if (str.Equals(""))
        {
            ToastManager.Instance.Show("不能为空");
        }
        else if (!UserManager.Instance.canModifyName && UserManager.Instance.diamond < Def.ModifyNameDimand && !UserManager.Instance.canModifyName)
        {
            MainUI.Instance.DiomandToClick();  
        }
        else
        {
            namePop = p;
            C2sSprotoType.user_modify_name.request obj = new C2sSprotoType.user_modify_name.request();
            obj.name = str;
            namePop.saveNameBtn.isEnabled = false;
            NetworkManager.Instance.UserNameModify(obj);

        }
    }

    public void UserModifyNameCallback(C2sSprotoType.user_modify_name.response resp)
    {
        namePop.saveNameBtn.isEnabled = true;
        if (namePop != null && resp.errorcode ==1)
        {
            //LuaFunction f = l.GetFunction("ModifyNameCallBack");
            //f.Call(namePop);  
            UserManager.Instance.nickName = namePop.name.label.text;
            pop.SetName(UserManager.Instance.nickName);
            if (!UserManager.Instance.canModifyName)
            {
                UserManager.Instance.SubDiamond(Def.ModifyNameDimand);
            }
            else
            {
                UserManager.Instance.canModifyName = false;
            }
            namePop.CloseClick();
        }
        namePop = null;
    }
    public void UpLevel()
    {
        if (UserManager.Instance.exp < GetUpExp())
        {
            ToastManager.Instance.Show("经验不够");
        }else
        {
            pop.levelUpBtn.isEnabled = false;
            NetworkManager.Instance.UserUpgrade(); 
        }
    }

    public void UserUpgradeCallback(C2sSprotoType.user_upgrade.response resp)
    { 
        //LuaFunction f = l.GetFunction("UpLevelCallBack");
        //f.Call(namePop);   
        pop.levelUpBtn.isEnabled = true;
        if (resp.errorcode == 1)
        {
            UserManager.Instance.SubExp(GetUpExp());
            UserManager.Instance.UpLevel();
            SetInfo(); 
            SetLevelBtnInfo();
            MainUI.Instance.SetEffect(GameShared.Instance.GetSkeletonAssetByPath(Def.UpgradeAmin),
                (aa, bb) =>
                {
                    MainUI.Instance.effect.gameObject.SetActive(false);
                });
        }
        
    }

    public void RandomName(ModifyNamePop p)
    {
        namePop = p;
        namePop.randomNameBtn.isEnabled = false;
        NetworkManager.Instance.UserRandomNameModify();
    }
    public void UserRandomNameCallback(C2sSprotoType.user_random_name.response resp)
    {
        namePop.randomNameBtn.isEnabled = true;
        if (resp.name != null && resp.errorcode ==1)
        {
            namePop.randomNameBtn.isEnabled = true;
            namePop.SetName(resp.name);
        }
    }

     
}
