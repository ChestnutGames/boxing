using UnityEngine;
using System.Collections;

public class DailyData {

    public enum DailyType
    {
        Duan=1,
        Glod=2
    }

    public enum DailySub
    {
        Easy = 1,
        Normal = 2,
        Hard = 3,
    }

    public int id;
    public string refresh_time;
    public int level;
    public DailyType daily_type;
    public DailySub dialy_sub;
    public string name;
    public int diamond_count;
    public string icon;
    public string rewared;
    public int level_rewared;
    public int level_up;

    public int getNum;
}

public class QianDaoData
{
    public int id;
    public int month;
    public int num;
    public int item_id;
    public int item_count;
    public int diamond_count;
    public int vip_level;
    public int vip_item_id;
    public int vip_item_count;
    public int vip_diamond_count;

    public ItemData itemdata;
    public bool signed;
    public bool show;
    public int sort;
}
