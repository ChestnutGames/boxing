using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LiLianRunData  {
    /// <summary>
    /// state = 1 运行
    /// state = 2 结束
    /// state = 0 空闲
    /// </summary>
    public LiLianRunData()
    {
        state = 0;
    }

    public LiLianCardData card;
    public LiLianHallData hall;
    public LiLianEventData trigger;
    public LiLianViewData hallviewdata;

    public List<ItemViewData> swaredList; 

    public int triggr_id;
    public int quanguan_id;
    public int card_id;

    public int runid;

    public int state;

     
    public DateTime time;

    public PassiveTimer runTime;

    public UILabel view;

    public int if_lilian_reward;
    public int if_event_reward;
    public Def.LiLianType type;
}
