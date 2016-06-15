using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class ClickCursor : MonoBehaviour {

    public GameObject leftPos;
    public GameObject rightPos;

    public GameObject iniPos;
     
    public float speedScale = 1;

    protected float maxX;
    protected float minX;

    private Vector2 curSpeed = new Vector2(1,0);
    private Vector2 initSpeed = new Vector2(1, 0);

    private List<GameObject> touchBox = new List<GameObject>();

    private List<GameObject> comboBox = new List<GameObject>();
    public DragBox dragBox;
    private bool isToRight; 

    public bool isStartCombo;
    public bool isEndCombo;

    private BoxCollider2D collider;

    private Rigidbody2D rigidbody;

    List<float> speedList;

	// Use this for initialization
	void Start () {
        
        minX = leftPos.transform.position.x;
        maxX = rightPos.transform.position.x;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>(); 
	}

    public void init(List<float> speed)
    {
        speedList = speed;
        collider = GetComponent<BoxCollider2D>();
        Rest();
        Begine();
    }

    public void RestPos()
    {
        Vector3 v = this.transform.position;
        v.x = minX;
        this.transform.position = v; 
    }

    void Rest()
    {
        RestPos();
        isStartCombo = false;
        isEndCombo = false;
        
        ChangeSpeedByLevel(0);
    }

    public void Stop()
    { 
        curSpeed = rigidbody.velocity;   
        rigidbody.velocity = new Vector2(0,0);
    }
     

    void Begine()
    {
        MoveToRight(); 
    } 
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < minX)
        {
            MoveToRight();
        }
        else if (transform.position.x > maxX)
        {
            MoveToLeft();
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("enter");
        if (col.tag == Def.Box || col.tag == Def.DragBox)
        {
            touchBox.Add(col.gameObject);
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log("exit");
        if (col.tag == Def.Box || col.tag == Def.DragBox)
        {
            touchBox.Remove(col.gameObject); 
        }  
    }

    public void StartCombo()
    {  

    }
    public void EndCombo()
    {
 
    }
     
    //返回是否是 combobox
    public bool Click()
    {
        if (BattleManager.Instance.isBattle == false)
            return false;
        ClickBox topbox = null; 
        //是否触碰到box
        if (touchBox.Count > 0)
        {
            //获得最上面的box
            for (int i = 0; i < touchBox.Count; i++)
            {
                if (topbox != null)
                {
                    if (touchBox[i] != null)
                    { 
                        ClickBox curbox = touchBox[i].GetComponent<ClickBox>(); 
                        if (topbox.sprite.depth < curbox.sprite.depth)
                        {
                            topbox = curbox;
                        }
                    }
                }
                else
                { 
                    
                    if (touchBox[i]!=null) 
                        topbox = touchBox[i].GetComponent<ClickBox>();
                    if (topbox == null && touchBox[i].gameObject!=null && touchBox[i].gameObject.transform.parent.gameObject != null)
                        topbox = touchBox[i].gameObject.transform.parent.gameObject.GetComponent<ClickBox>();
                }
            } 
            //处理click事件
            if (topbox != null && topbox.type != ClickBox.BoxType.ComboBox)
            { 
                touchBox.Remove(topbox.gameObject);
                topbox.Click(topbox.type);
            }
            else if (topbox != null && topbox.type == ClickBox.BoxType.ComboBox)
            {
                dragBox = topbox.GetComponent<DragBox>();
                return true;
            }
            else //有时候touchbox 没清干净
            {
                BattleManager.Instance.ClickBoxOver(ClickBox.BoxType.None);
            }
        }
        else  //什么都没点到
        {
            BattleManager.Instance.ClickBoxOver(ClickBox.BoxType.None); 
        }
        return false;
    }

    public void ChangeSpeedByLevel(int level)
    {
        if (level >= speedList.Count) return;
        Vector2 vec = curSpeed;
        if (isToRight)
        {
            vec.x = speedList[level];
        }
        else
        {
            vec.x = speedList[level] * (-1); 
        }
        if (level < speedList.Count)
        {  
            curSpeed = vec;
        }
        this.GetComponent<Rigidbody2D>().velocity = curSpeed; 
    }
     


    void MoveToRight()
    {
        isToRight = true;
        if (curSpeed.x <0)
        { 
            float x = curSpeed.x*(-1);
            curSpeed = new Vector2(x, 0);
            collider.offset = new Vector2(-1,0);
        }
        this.GetComponent<Rigidbody2D>().velocity = curSpeed; 
    }

    void MoveToLeft()
    {
        isToRight = false;
        if (curSpeed.x > 0)
        {
            float x = curSpeed.x * (-1);
            curSpeed = new Vector2(x, 0);
            collider.offset = new Vector2(1 , 0);
        }
        this.GetComponent<Rigidbody2D>().velocity = curSpeed;  
    }
}
