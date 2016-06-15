using UnityEngine;
using System.Collections;

public class EquipmentPop : MonoBehaviour {

    public GameObject kit;
    public UISprite itemView;
    public UILabel name;
    public UILabel level;

    public UILabel intensifylevel;
    public UILabel attr;
    public UILabel attrType;
    public UILabel use;
    public UILabel pre;

    public UILabel power;
    public UILabel defense;
    public UILabel crit;
    public UILabel pray;

    public UILabel tips;

    public UIButton intensifyBtn;

    public UISprite[] equipView;
    public UIButton[] equipViewBtn;

    public UILabel xilan;

     

    public void SetTotalNum(string p,string d, string c, string y)
    {
        power.text = "战斗力"+p;
        defense.text = "防御"+d;
        crit.text = "暴击"+c;
        pray.text = "祈祷"+y;
    }

    public void SetXiLianTips(string str)
    {
        xilan.text = str;
    }

    public void InitData()
    {
        EquipmentMgr.Instance.OpenPop(this); 
    }

    public void CloseClick()
    {
        EquipmentMgr.Instance.EquipPopClose(this.gameObject); 
    }

    public void IntensifyClick()
    {
        EquipmentMgr.Instance.Intensify();
    }

    public void SetEquipList(Hashtable table)
    { 
        equipView[0].spriteName = ((EquipViewData)table[(int)EquipData.EquipType.Boxing]).data.levelData.path;
        equipView[1].spriteName = ((EquipViewData)table[(int)EquipData.EquipType.Cloth]).data.levelData.path;
        equipView[2].spriteName = ((EquipViewData)table[(int)EquipData.EquipType.Shoe]).data.levelData.path;
        equipView[3].spriteName = ((EquipViewData)table[(int)EquipData.EquipType.Belt]).data.levelData.path; 

        equipViewBtn[0].normalSprite = ((EquipViewData)table[(int)EquipData.EquipType.Boxing]).data.levelData.path;
        equipViewBtn[1].normalSprite = ((EquipViewData)table[(int)EquipData.EquipType.Cloth]).data.levelData.path;
        equipViewBtn[2].normalSprite = ((EquipViewData)table[(int)EquipData.EquipType.Shoe]).data.levelData.path;
        equipViewBtn[3].normalSprite = ((EquipViewData)table[(int)EquipData.EquipType.Belt]).data.levelData.path;  
    }

    public void SelectBoxing()
    {
        Hashtable table = UserManager.Instance.equipTable;
        EquipmentMgr.Instance.SelectEquip(((EquipViewData)table[(int)EquipData.EquipType.Boxing])); 
    }
    public void SelectCloth()
    {
        Hashtable table = UserManager.Instance.equipTable;
        EquipmentMgr.Instance.SelectEquip(((EquipViewData)table[(int)EquipData.EquipType.Cloth]));
    }
    public void SelectShoe()
    {
        Hashtable table = UserManager.Instance.equipTable;
        EquipmentMgr.Instance.SelectEquip(((EquipViewData)table[(int)EquipData.EquipType.Shoe]));
    }
    public void Selectbelt()
    {
        Hashtable table = UserManager.Instance.equipTable;
        EquipmentMgr.Instance.SelectEquip(((EquipViewData)table[(int)EquipData.EquipType.Belt]));
    }

    public void KitClick()
    {
        EquipmentMgr.Instance.KitDescShow();
    }
}
