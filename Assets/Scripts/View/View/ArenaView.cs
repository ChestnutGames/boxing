using UnityEngine;
using System.Collections;

public class ArenaView : SPSGame.Unity.UIObject {

    public UILabel battle;
    public UILabel rank;
    public UILabel name;
    public SkeletonAnimation anim;
    public GameObject btn;
    public ArenaUserData data;
    public bool self;
    public void InitData(ArenaUserData v, bool s = false)
    {
        data = v;
        battle.text = v.total_combat.ToString();
        rank.text = v.ara_rnk.ToString();
        name.text = v.uname.ToString();
        self = s;
        //v.iconid
        ListenOnClick(btn, BattleClick);
    }

    public void SetIsBattle(bool b)
    { 
         btn.SetActive(b); 
    }

    public void BattleClick(GameObject obj)
    {
        if(!self)
            ArenaMgr.Instance.Battle(this);
    }
}
