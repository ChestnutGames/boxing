using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BomoBox : ClickMoveBox
{

    private List<ClickBox> _destoryList = new List<ClickBox>();

    private List<ClickBox> _groundList = new List<ClickBox>();

    public BoxCollider2D box;



    // Use this for initialization
    void Start()
    {
        clickDestoryCallEvent += BomoBox_DestoryEvent;
        clickOverAnimEvent += BomoBox_AnimPlayEvent; 
    }

    void BomoBox_AnimPlayEvent()
    {
        GetContainStaticBox(); 
        for (int i = 0; i < _destoryList.Count; i++)
        {
            _destoryList[i].NotAddComboClick();
        }
        for (int i = 0; i < _groundList.Count; i++)
        {
            _groundList[i].NotAddComboClick();
        }
        _destoryList.Clear();
        _groundList.Clear(); 
        OverAnimPlay();
    }

    void BomoBox_DestoryEvent()
    {
        BattleManager.Instance.ClickBoxOver(this.gameObject, type,false);  
    }
 

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - offsetX < minX)
        {
            BattleManager.Instance.MoveBoxOver(this.gameObject, type); 
        }
        else if (transform.position.x + offsetX > maxX)
        {
            if (isBacking)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            else
            {
                MoveToLeft();
            }
        } 
    }
     
    //获得爆炸范围内box
    protected void GetContainStaticBox()
    {
        List<GameObject> list = BattleManager.Instance.pop.spawner._groundObjects;
        for (int i = 0; i < list.Count; i++)
        {
            if (box.bounds.Intersects(list[i].GetComponent<Collider2D>().bounds)
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
            if (list[i] != null && box.bounds.Intersects(list[i].GetComponent<Collider2D>().bounds)
                && list[i].tag != Def.DragBox)
            {

                ClickBox b = list[i].GetComponent<ClickBox>(); 
                if(b.type!=ClickBox.BoxType.BombBox) 
                    _destoryList.Add(b);
            }
        } 
 
    } 
}
