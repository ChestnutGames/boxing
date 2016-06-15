using UnityEngine;
using System.Collections;

public class BoxingView : MonoBehaviour
{
    public UISprite bg;
    public UISprite focus;
    public UISprite icon; 
    public UISprite kuang;
    public BoxingViewData data;
    public BoxingEquipData equip;
    public Def.SkillType type;
    private bool isClick; 

    public void InitData(BoxingViewData d, bool click = true)
    { 
        data = d; 
        isClick = click;
        SetFous(false);
        data.view = this;
        if (data.data != null && data.data.levelData != null)
        {
            SetUnlock();
            this.name = d.sort.ToString();
            type = data.data.levelData.skill_type;
            SetBgQuality(data.data.levelData.quality);
            icon.spriteName = data.data.levelData.skill_icon.ToString();
            SetType(data.data.levelData.skill_type);
        }
        else
        {
            RestEmpty();
        } 
    }

    public void InitData(BoxingEquipData e, int p,bool click = true)
    {
        equip = e;
        data = e.viewdata; 
        isClick = click;
        SetFous(false); 
        if (data.data != null && data.data.levelData != null)
        {
            SetUnlock();
            this.name = data.sort.ToString();
            type = data.data.levelData.skill_type;
            SetBgQuality(data.data.levelData.quality);
            icon.spriteName = data.data.levelData.skill_icon.ToString();
            SetType(data.data.levelData.skill_type);
        }
        else
        {
            RestEmpty();
        } 
    }

    public void SetUnlock()
    {
        if (data.level == 0)
        {
            kuang.gameObject.SetActive(true);
        }
        else
        {
            kuang.gameObject.SetActive(false);
        }
    }

    public void SetType(Def.SkillType type)
    {
        switch (type)
        {
            case Def.SkillType.Active:
                bg.spriteName = "边框1";
                break;
            case Def.SkillType.Passive:
                bg.spriteName = "边框3";
                break; 
        }
    }

    public void RestEmpty()
    { 
        if(equip!=null)
            equip.viewdata = null;
        data = null; 
        icon.spriteName = "";
        kuang.spriteName = "";
        SetFous(false);
        this.name = "None";
    }

    public void RestEquipEmpty(int i)
    {
        equip = new BoxingEquipData();
        equip.position = i; 
        data = null;
        icon.spriteName = "";
        kuang.spriteName = "";
        SetFous(false);
        this.name = "None";
    } 


    public void RestItem(BoxingViewData d)
    {
        if(equip!=null)
            equip.viewdata = d;
        data = d;
        SetFous(false);
        if (data != null && data.data != null)
        {
            this.name = d.sort.ToString();
            SetBgQuality(data.data.levelData.quality);
            icon.spriteName = data.data.levelData.skill_icon.ToString();
        }
        else
        {
            RestEmpty();
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
                kuang.spriteName = "";
                break;
        }
    }

    public void SetFous(bool b)
    {
        if (b)
        {
            focus.gameObject.SetActive(true);
        }
        else
        {
            focus.gameObject.SetActive(false);
        }
    }

    void OnClick()
    {
        if (isClick)
        {
            BoxingMgr.Instance.SetBoxingFocus(this);
        }
    } 
}
