using UnityEngine;
using System.Collections;

public class BagUsePop : MonoBehaviour {

    public UISprite icon;
    public UISprite kuang;
    public UILabel name;
    public UILabel num;
    public UIButton subBtn;
    public UIButton addBtn;
    public UISlider slider;

    private ItemViewData data;

    private int count;
    private float unit;
	// Use this for initialization
	void Start () {
	
	}

    public void InitData(ItemViewData d)
    {
        data = d;
        icon.spriteName = d.data.path;
        kuang.spriteName = data.kuang;
        count = 1;
        SetNum(count); 
        name.text = d.data.name;
        float a = 1.0f / (float)data.curCount;
        unit = a;
        slider.value = a;
        
    }

    public void SetNum(int c)
    {
        num.text = c + "/" + data.curCount;
    }

    public void SubClick()
    {
        count--;
        isClick = true;
        CheckBtn();
    }

    public void PlusClick()
    {
        count++;
        isClick = true;
        CheckBtn();
    }
    private bool isClick = false;
    public void SliderChange()
    {
        if (isClick)
        {
            isClick = false;
            return;
        }
        count =(int)(slider.value / unit);
        if (slider.value == 1.0f)
            count = data.curCount;
        SetNum(count);
        CheckBtn();
    }
    public void CheckBtn()
    {
        if (count < data.curCount)
        {
            addBtn.isEnabled = true;
        }
        else
        {
            addBtn.isEnabled = false;
        }
        if (count > 1)
        {
            subBtn.isEnabled = true;
        }
        else
        {
            subBtn.isEnabled = false;
        }
        slider.value = count * unit; 
    }

    public void SetSlider()
    {
        float a = (float)count / data.curCount;
        if (slider.value == 1.0f)
            count = data.curCount;
        slider.value = a;
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void UseClick()
    {
        count =(int)(slider.value / unit);
        if (slider.value == 1.0f)
            count = data.curCount;
        if (count > 0)
        {
            BagMgr.Instance.UseItem(count);
        }
    }

    public void CloseClick()
    {
        
        MainUI.Instance.SetPopState(MainUI.PopType.BagUse, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); 
    }
}
