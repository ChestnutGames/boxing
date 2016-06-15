using UnityEngine;
using System.Collections;

public class GroundBox : ClickMoveBox
{
    string[] spriteList = { "groundred1", "groundred2", "groundred3" };
	// Use this for initialization 
    void Start()
    {
        clickDestoryCallEvent += GroundBox_DestoryEvent;
        clickOverAnimEvent += GroundBox_AnimPlayEvent; 
    } 

    void GroundBox_AnimPlayEvent()
    {  
        if((clickMax)==clickCount)
        {
            OverAnimPlay();
        }else
        {
            sprite.spriteName = spriteList[clickCount];
            BattleManager.Instance.ClickGroundBox(this.gameObject, type,this.isNotAddCombo); 
        }
    }

    void GroundBox_DestoryEvent()
    {
        BattleManager.Instance.ClickBoxOver(this.gameObject, type);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - offsetX < minX)
        {
            MoveToRight();
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
}
