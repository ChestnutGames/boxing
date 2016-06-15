using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

 
public class EventArenaRankList : EventArgs
{
    public List<ArenaUserData> list;
    public EventArenaRankList(List<ArenaUserData> l)
    {    
        list = l;
    }
}

public class EventArenaBattleRoleList : EventArgs
{
    public List<long> list;
    public EventArenaBattleRoleList(List<long> l)
    {
        list = l;
    }
}

public class EventArenaInfo : EventArgs
{
    public List<ArenaUserData> list;
    public EventArenaInfo(List<ArenaUserData> l)
    {
        list = l;
    }
}

public class EventPointRewardList : EventArgs
{
    public List<AranePointViewData> list;
    public EventPointRewardList(List<AranePointViewData> l)
    {
        list = l;
    }
}

public class EventRewardCollectedList : EventArgs
{
    public List<ArenaUserData> list;
    public EventRewardCollectedList(List<ArenaUserData> l)
    {
        list = l;
    }
}

public class EventArenaRewardList : EventArgs
{
    public List<ArenaRewaredViewData> list;
    public EventArenaRewardList(List<ArenaRewaredViewData> l)
    {
        list = l;
    }
}


public class ArenaRank100List : EventArgs
    {
        public List<ArenaUserData> list;
        public ArenaRank100List(List<ArenaUserData> l)
        {
            list = l;
        }
    }

public class EventLoadScaneFinish : EventArgs
{ 
    public EventLoadScaneFinish(string name)
    {
        Application.LoadLevel(name);
    }
}