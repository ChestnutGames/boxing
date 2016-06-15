using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipData {

    public int vip_level;
    public int diamond_count;
    public int coin_up;
    public int exp_up;
    public int coin_max;
    public int exp_max;
    public int equipment_enhance_success_rate_up_p;
    public int prop_refresh_reduction_p;
    public int arena_frozen_time_reduction_p;
    public int purchase_hp_count;
    public int SCHOOL_reset_count; 

    public string swared;
    public string gift_swared;
    public int diamond_show;
    public int diamond;
    public int store_refresh;

    public string vip_attrdesc;

    public bool isRevice;
    public bool isPurchased;
    public List<ItemViewData> rewardList;
    public List<ItemViewData> giftList;
}
