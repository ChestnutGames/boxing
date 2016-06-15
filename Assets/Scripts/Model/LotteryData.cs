using UnityEngine;
using System.Collections;
using System;

public class LotteryData
{ 
    public enum MoneyType
    {
        Diamond = 1,
        Coin = 2,
        Friend = 3
    } 
    public MoneyType type;
    public int money;
    public int id;
    public int cd;
    public int drawnum;

    public int lefttime;

    public bool isShowTime;

    public DateTime refresh_time;
    public bool isScale;

    public bool mian;

    public string time;
    public string name;
    public string desc;
    public string icon;
    public string money_icon;
    public string num;
    public string num_name;
    public string btnnor;
    public string btndis;
    public string kechou;
     
     
}
