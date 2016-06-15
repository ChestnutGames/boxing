using UnityEngine;
using System.Collections;
using System;

public class BoxingLevelData : AttrsBase{

    public float[] equipAttrArr = new float[Enum.GetNames(typeof(Def.AttrType)).Length];
    public float[] equipAdditionArr = new float[Enum.GetNames(typeof(Def.AttrType)).Length];//增益

    public ItemData.QualityType quality;

    public int g_csv_id;
    public string name;
    public int csv_id;
    public int skill_level;
    public string skill_icon;
    public string skill_desc;
    public string skill_effect;
    public Def.SkillType skill_type;
    public Def.HurtType skill_hurt;
    public int trigger_pre;
    public int trigger_num;
    public Def.TriggerType trigger_type;
    public int trigger_arg;
    public Def.FormulaType formula_type;
    public int effect_pre;
    public Def.AddEffectType add_effect_type;
    public int add_state_pre;
    public int buff_id;
    public int item_id;
    public int item_num;
    public int coin_type;
    public int coin;
    public int equip_buff_id;

    public ItemViewData item_data;
}
