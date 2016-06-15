using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RolesPop : MonoBehaviour {
    public UILabel name;

    public UILabel power;
    public UILabel defense;
    public UILabel crit;
    public UILabel pray; 

    

    public UILabel curbattle;
    public UILabel nextbattle;
    public UILabel curcollect;
    public UILabel nextcollect;

    public UILabel denglu;

    public UIButton UpWakeBtn;
    public UIButton BattleBtn;

    public SkeletonAnimation anim;

    public WakeUpLevelView wakeView;

    public UISlider exp;
    public UILabel expLabel;

    public UITable table;
    public UIScrollView scroll;

    public GameObject rolePrefabe;

    private Hashtable viewList;

    public HeadRole curView;

    public UILabel talk;
    public UISprite talkBg;

    public bool talkShow = false;

    public UISprite arrow1;
    public UISprite arrow2;

    public UILabel collectDesc;

    public UIButton xilianBtn;

    public HeadRole GetRoleView()
    {
        return curView;
    }

    public void SetCollectDesc(string t)
    {
        collectDesc.text = t;
    }

    public void  InitData()
    {
        RolesMgr.Instance.OpenPop(this);
        
    }

    public void SetArrowShow(bool b)
    {
        arrow1.gameObject.SetActive(b);
        arrow2.gameObject.SetActive(b);
    }

    public void WakeUp()
    {
        RolesMgr.Instance.WakeUp(curView);
    }

    public void Battle()
    {
        RolesMgr.Instance.RoleBattle(curView);
    }

    public void SetBtn(bool wake,bool battle)
    {
        UpWakeBtn.isEnabled = wake;
        BattleBtn.isEnabled = battle;  
    } 

    public void SetTable(ref Hashtable list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new Hashtable();
        //System.Collections.IDictionaryEnumerator enumerator = list.GetEnumerator();
        //while (enumerator.MoveNext())
        //{
        foreach (RoleData r in UserManager.Instance.RoleTable.Values)
        {
            //RoleData r = vv as RoleData;//UserManager.Instance.RoleTable[enumerator.Key] as RoleData;
            GameObject obj = Instantiate(rolePrefabe);
            obj.SetActive(true);
            HeadRole pop = obj.GetComponent<HeadRole>();
            obj.name = r.sort.ToString();
            pop.InitData(r);
            pop.transform.parent = table.transform;
            pop.transform.localScale = Vector3.one;
            viewList.Add(r.sort, pop);
        }
        table.Reposition();
        scroll.ResetPosition();
        table.repositionNow = true; 
    }

    public HeadRole GetCurView()
    {
        return curView;
    }

    public void SetCurView(HeadRole role)
    {
        curView = role;
        role.SetFous(true);
    } 

    public HeadRole GetItemView(int i)
    {
        return viewList[i] as HeadRole; 
    }
    public void SetInfo(string p, string d, string c, string r, string t)
    {
        power.text = p;
        defense.text = d;
        crit.text = c;
        pray.text = r; 
    }

    public void XiLianClick()
    {
        RolesMgr.Instance.OpenXiLianPop(curView.data);
    }

    public void SetRoleInfo(RoleData d, string cb, string cc, string nb, string nc, float v, string e)
    {
        name.text = d.name;
        wakeView.SetWakeLevel(d.wakeLevel);

        if (d.path != null)
        {
            SkeletonDataAsset sk = GameShared.Instance.GetSkeletonAssetByPath(d.path);
            anim.skeletonDataAsset = sk;
            anim.Reset();
        }

        curbattle.text = cb;
        curcollect.text = cc; 
        nextbattle.text = nb;
        nextcollect.text = nc;
        exp.value = v;
        expLabel.text = e;
        if (UserManager.Instance.level >= GameShared.Instance.config.xilian_level_open && d.is_possessed)
        {
            xilianBtn.gameObject.SetActive(true);
        }
        else
        {
            xilianBtn.gameObject.SetActive(false);
        }

    }

    public void SetFragInfo(float v,string e)
    {
        exp.value = v;
        expLabel.text = e;
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.Roles, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); 
    }

    public void SetTalk(string str)
    {
        talk.text = str;
    }

    
    public void TalkClick()
    {
        if (talkShow)
        {
            talkShow = false;
            talkBg.GetComponent<UISprite>().gameObject.SetActive(false);
        }
        else
        {
            talkShow = true;
            talkBg.GetComponent<UISprite>().gameObject.SetActive(true);
        }
 
    }
}
