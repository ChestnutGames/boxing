using UnityEngine;
using System.Collections;

public class ArenaRankView : SPSGame.Unity.UIObject
{

    public UILabel num;
    public UILabel battle;
    public UILabel name;
    public UILabel vip;
    ArenaUserData data;
    public bool self;

    public void InitData(ArenaUserData v)
    {
        data = v;
        battle.text = v.total_combat.ToString();
        num.text = v.ara_rnk.ToString();
        name.text = v.uname.ToString();
        vip.text = v.vip.ToString();
    }
}
