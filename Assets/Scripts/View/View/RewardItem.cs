using UnityEngine;
using System.Collections;

public class RewardItem : MonoBehaviour {

    public UISprite icon;
    public UISprite kuang;
    public UILabel count;
    public UISprite bg;
    public ItemViewData data;
	// Use this for initialization

    public void InitData(ItemViewData d, string name, int max, int cur)
    {
        data = d;
        icon.spriteName = name;
        count.text = cur.ToString() + "/" + max.ToString();
        SetBgQuality(data.data.quality);
    }

    public void InitDataHideBG(ItemViewData d, string name, int max, int cur,Def.CurrencyType type)
    {
        data = d;
        icon.spriteName = name;
        count.text = cur.ToString() + "/" + max.ToString();
        SetBgQuality(data.data.quality);
        SetIcon(type);
    }

    public void InitData(ItemViewData d,string name,int cur)
    {
        data = d;
        icon.spriteName = name;
        count.text = cur.ToString();
        SetBgQuality(data.data.quality);
    }

    public void InitData(ItemViewData d, string name)
    {
 
    }

    void SetIcon(Def.CurrencyType type)
    {
        kuang.gameObject.SetActive(false);
        bg.spriteName = Def.NoneSprite;
        switch (type)
        {
            case Def.CurrencyType.Diamond:
                icon.spriteName = Def.DiamondTex;
                break;
            case Def.CurrencyType.Gold:
                icon.spriteName = Def.CoinTex;
                break;
            case Def.CurrencyType.Heart:
                icon.spriteName = "blue";
                break; 
            case Def.CurrencyType.Exp:
                icon.spriteName = "blue";
                break; 
        } 
    }

    void SetBgQuality(ItemData.QualityType t)
    {
        switch (t)
        {
            case ItemData.QualityType.White:
                kuang.spriteName = "white";
                break;
            case ItemData.QualityType.Green:
                kuang.spriteName = "green";
                break;
            case ItemData.QualityType.Blue:
                kuang.spriteName = "blue";
                break;
            case ItemData.QualityType.Purple:
                kuang.spriteName = "purple";
                break;
            case ItemData.QualityType.Glod:
                kuang.spriteName = "orange";
                break;
            case ItemData.QualityType.Red:
                kuang.spriteName = "red";
                break;
            default:
                kuang.spriteName = "dfee";
                break;
        }
    }
	 
}
