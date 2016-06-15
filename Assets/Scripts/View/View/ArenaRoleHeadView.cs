using UnityEngine;
using System.Collections;

public class ArenaRoleHeadView : MonoBehaviour
{
    public UISprite bg;
    public UISprite icon;
    public UISprite kuang;
    public UIButton btn;
    public RoleData data;
    private bool isClick;

    public void InitData(RoleData d, bool click = true)
    {
        data = d;
        //isClick = click;
        //SetFous(false);
        //if (data != null)
        //{
        //    SetBgQuality(data.quality);
        //    icon.spriteName = data.icon;
        //    btn.normalSprite = data.icon;

        //}
        //else
        //{
        //    kuang.spriteName = "sdf";
        //    icon.spriteName = "";
        //    btn.normalSprite = "";
        //}
    }

    public void RestEmpty()
    {
        data = null;
        kuang.spriteName = "sdf";
        icon.spriteName = "";
        btn.normalSprite = "";
        SetFous(false);
    }

    public void RestItem(RoleData d)
    {
        data = d;
        SetFous(false);
        if (data != null)
        {
            SetBgQuality(data.quality);
            icon.spriteName = data.icon;
            btn.normalSprite = data.icon;
        }
        else
        {
            kuang.spriteName = "sdf";
            icon.spriteName = "";
            btn.normalSprite = "";
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
                kuang.spriteName = "sdf";
                break;
        }
    }

    public void SetFous(bool b)
    {
        if (b)
        {
            bg.gameObject.SetActive(true);
        }
        else
        {
            bg.gameObject.SetActive(false);
        }
    }

    void OnClick()
    { 
        ArenaMgr.Instance.ChooseBattleRole(data); 
    }
}
