using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelData   {
    public int csv_id;
    public int chapter;
    public int combat;
    public int level;
    public string name;
    public int checkpoint;
    public Def.levelType type;
    public int cd;
    public int gain_gold;
    public int gain_exp;
    public int drop;
    public string reward;
    public int monster_csv_id1;
    public int monster_csv_id2;
    public int monster_csv_id3;



    public List<ItemViewData> rewardList;

    public RoleData enemy1;
    public RoleData enemy2;
    public RoleData enemy3;
}
