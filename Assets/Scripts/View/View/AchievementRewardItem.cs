using UnityEngine;
using System.Collections;

public class AchievementRewardItem : MonoBehaviour
{
    public UISprite icon;
    public UILabel count;

    public void Init(string i, string c)
    {
        icon.spriteName = i;
        count.text = c;
    } 
}
