using UnityEngine;
using System.Collections;
using System;

public class RoleStarData : AttrsBase{ 
	public int g_csv_id;
    public int star;
    public int us_prop_num;
    public int csv_id;
    public string name;
    public int us_prop_csv_id;
    public int star_init;
    public int sharp;
    public string anim;
    public int skill_csv_id;
    public int gather_buffer_id;
    public int battle_buffer_id;
    public string strs;

    //上阵加成
    public float[] battleAttr = new float[Enum.GetNames(typeof(Def.AttrType)).Length];
    public float[] battleAddition = new float[Enum.GetNames(typeof(Def.AttrType)).Length];//增益

}
