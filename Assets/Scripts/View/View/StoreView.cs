using UnityEngine;
using System.Collections;

public class StoreView : MonoBehaviour {
    public UISprite kuang;
    public UISprite bg;
    public UILabel num;
    public UISprite icon;
    public UILabel name;
    public UISprite hot;
    public UISprite currencyIcon;
    private int index;
    public ProductData data;
    StorePop pop;
    public void InitData(StorePop p, ProductData d, int i)
    {
        data = d;
        index = i;
        pop = p;
        SetInfo();
    }

    public void SetInfo()
    {
        if (data != null)
        {
            name.text = data.data.name;
            num.text = data.currency_num.ToString();
            if (data.data!=null)
                icon.spriteName = data.data.path;
            if (data.isHot)
            {
                hot.gameObject.SetActive(true);
            }
            else
            {
                hot.gameObject.SetActive(false);
            }
            currencyIcon.spriteName = data.currency_icon; 
        } 
        ChangeFocus(false);
    }

    public void ChangeFocus(string b, string k)
    {
        bg.spriteName = b;
        this.GetComponent<UIButton>().normalSprite = b;
        kuang.spriteName = k;
    }

    public void ChangeFocus(bool b)
    {
        if (b)
        {
            bg.spriteName = "选中商品";
            this.GetComponent<UIButton>().normalSprite = "选中商品"; 
            kuang.spriteName = "选中icon框";
        }
        else
        {
            bg.spriteName = "未选中商品";
            this.GetComponent<UIButton>().normalSprite = "未选中商品"; 
            kuang.spriteName = "道具框";
        }
    }

    public void ItemClick()
    {
        pop.ItemClick(index);
    } 
}
