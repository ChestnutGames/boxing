using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementData {
    public int id;
    public int type;
    public string name;
    public int condition; 
    public string desc;
    public string icon;
    public string unlockId;
    public string reward;
    public int curStar;
    public List<ItemViewData> rewarData;

}
