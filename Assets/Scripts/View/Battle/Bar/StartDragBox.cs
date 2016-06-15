using UnityEngine;
using System.Collections;

public class StartDragBox : MonoBehaviour {
    [SerializeField]
    private DragBox box;
    public bool isCanTouch;
    bool isStartBox;
	// Use this for initialization
	void Start () {
        isStartBox = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (BattleManager.Instance.isTouching && isCanTouch)
        {
            if (!box.haveStart)
            {
                tag = "start";
                box.haveStart = true; 
            }
            else
            {
                tag = "end";
            } 
        } 
	}

    public void Rest()
    { 

    }
    void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.tag == Def.Cours)
        {
            isCanTouch = true;
            if (!box.haveStart)
            {
                box.haveStart = true;
                isStartBox = true;
                BattleManager.Instance.pop.cursor.isStartCombo = true;
            }
            else
            {
                isStartBox = false;
                BattleManager.Instance.pop.cursor.isEndCombo = true;
            }
            //Debug.Log(tag + "OnTriggerEnter2D"
            //    + box.haveStart.ToString());
            //box.canTouch = true;
            ////开头 
            //if (!box.haveStart && BattleManager.Instance.isTouching)
            //{
            //    BattleManager.Instance.isTouchingPurpleBox = true;
            //    tag = "start";
            //    box.haveStart = true;
            //    //结尾
            //}else if(box.haveStart && BattleManager.Instance.isTouching)
            //{
            //    tag = "end";
            //} 
        }
    }
     

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == Def.Cours)
        {
            isCanTouch = false;
            if (isStartBox)
            {
                BattleManager.Instance.pop.cursor.isStartCombo = false;
            }
            else
            {
                box.haveStart = false;
                BattleManager.Instance.pop.cursor.isEndCombo = false;
            }
            
    //            Debug.Log(tag + "OnTriggerExit2D"+"/n"
    //+ box.haveStart.ToString() + "/n" + tag + "/n" + BattleManager.Instance.isTouching.ToString()
    //);
                //box.canTouch = false;
                //if (box.haveStart && BattleManager.Instance.isTouching && tag.Equals("end"))
                //{
                //    BattleManager.Instance.isTouchingPurpleBox = false;
                //    box.DragOver();
                //    tag = "Empty";
                //    box.haveStart = false;
                //}
                //else
                //{
                    
                //}
                 
        }
    }
}
