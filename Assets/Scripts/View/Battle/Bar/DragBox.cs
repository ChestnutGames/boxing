using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragBox : ClickBox { 

    private List<GameObject> _destoryList = new List<GameObject>();

    private List<ClickBox> _groundList = new List<ClickBox>();
     
    //开始滑动
    public bool haveStart;
    //是否停止滑动
    public bool isTouching;


    public UIWidget left;
    public UIWidget right;
    public Collider2D childBox;

	// Use this for initialization
	void Start () {
        isTouching = false;
        haveStart = false;
        sprite = this.GetComponent<UISprite>();
        left.depth = sprite.depth + 1;
        right.depth = sprite.depth + 1;
        childBox = left.GetComponent<Collider2D>();
	}

    void Init()
    {
        sprite = this.GetComponent<UISprite>();
        left.depth = sprite.depth + 1;
        right.depth = sprite.depth + 1;
    }

    void Rest()
    {
        isTouching = false;
        haveStart = false;
      
    }
	
	// Update is called once per frame
	void Update () {
        //if (isTouching && BattleManager.Instance.isTouching && haveStart != true)
        //{
        //    haveStart = true;
        //    BattleManager.Instance.isTouchcount = 0;   
        //}
        //if (runing && BattleManager.Instance.isTouchcount > 0)
        //{
        //    Rest();
        //}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.tag != Def.Cours && haveStart)
        //{
        //    _comboList.Add(col.gameObject); 
        //} 
    } 
 

    //获得爆炸范围内box
    protected void GetContainStaticBox()
    {
        List<GameObject> list = BattleManager.Instance.pop.spawner._groundObjects;
        for (int i = 0; i < list.Count; i++)
        {
            if (this.GetComponent<Collider2D>().bounds.Intersects(list[i].GetComponent<Collider2D>().bounds)
                && list[i].tag != Def.DragBox)
                _groundList.Add(list[i].GetComponent<ClickBox>());
        }
        AddDestoryList(BattleManager.Instance.pop.spawner._blueObjects);
        AddDestoryList(BattleManager.Instance.pop.spawner._fastObjects);
        AddDestoryList(BattleManager.Instance.pop.spawner._redObjects);
        AddDestoryList(BattleManager.Instance.pop.spawner._yellowObjects);
        AddDestoryList(BattleManager.Instance.pop.spawner._bombObjects);
    }

    private void AddDestoryList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && this.GetComponent<Collider2D>().bounds.Intersects(list[i].GetComponent<Collider2D>().bounds)
                && list[i].tag != Def.DragBox)
                _destoryList.Add(list[i]);
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        //if (col.tag == Def.Cours && haveStart)
        //{
        //    haveStart = false;
        //    _comboList.Clear();
        //}  
    } 

    public void DragOver(bool b)
    { 
        if(BattleManager.Instance.isBattle == false)
        return;
            //中间没断开 
            if (b)
            {
                GetContainStaticBox();
                BattleManager.Instance.DragComboOver(this.gameObject, _destoryList); 
                _destoryList.Clear();
                _groundList.Clear();
                BattleManager.Instance.RoleChange(BoxType.ComboBox);
            }
            else
            {
                _destoryList.Clear();
                _groundList.Clear();
                BattleManager.Instance.ComboHitOver(BoxType.None);
            }
            BattleManager.Instance.pop.cursor.dragBox = null;
            haveStart = false; 
        } 
     
}
