using UnityEngine;
using System.Collections;

public class ClickMoveBox :  ClickBox  {


    public float speedScale;

    protected float maxSpeedX;
    protected float minSpeedX;

    protected float maxX;
    protected float minX;

    private Vector2 curSpeed;

    protected bool isBacking;

    public bool isAccdent;
    private Rigidbody2D rigidbody;

	// Use this for initialization
    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        clickDestoryCallEvent += MoveBox_DestoryEvent;
        clickOverAnimEvent += MoveBox_AnimPlayEvent; 
    }

    void MoveBox_AnimPlayEvent()
    {
        OverAnimPlay();
    }

    void MoveBox_DestoryEvent()
    {
        if (isNotAddCombo)
        {
            BattleManager.Instance.ClickBoxOver(this.gameObject, type,true);
        }
        else
        {
            BattleManager.Instance.ClickBoxOver(this.gameObject, type);
        }
    }

    public void InitMoveBox(float min,float max,bool isAcc = false,int click=0)
    {
        minX = BattleManager.Instance.pop.leftPos.transform.position.x;
        maxX = BattleManager.Instance.pop.rightPos.transform.position.x;
        rigidbody = GetComponent<Rigidbody2D>();
        maxSpeedX = max;
        minSpeedX = min;
        speedScale = 1;
        isBacking = false;
        isAccdent = isAcc;
        clickMax = click;
        MoveToLeft();
        
    } 

    // Update is called once per frame
    public void Update()
    {
        if (isAccdent)
        {
            speedScale += 0.00011f;
            if (speedScale > maxSpeedX)
                speedScale = maxSpeedX;
            
           
            Vector2 vec = rigidbody.velocity * speedScale;

            float temp = BattleManager.Instance.pop.spawner.staticMakeData.fastMaxSpeed;
            if (vec.x < 0)//往左
            {
                temp = -temp;
                if (vec.x > temp)
                    rigidbody.velocity = vec;
            }
            else//往右
            {
                if (vec.x < temp)
                    rigidbody.velocity = vec;
            }
             
        }
        if (transform.position.x-offsetX < minX)
        {
            MoveToRight();
        }
        else if (transform.position.x+offsetX > maxX)
        {
            if (isBacking)
            {
                BattleManager.Instance.RedRightOver(this.gameObject, this.type);
                //rigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                MoveToLeft();
            }
        }
    }

    public void ChangeSpeedScale(float scale)
    {
        speedScale = scale;
        Vector2 vec = rigidbody.velocity * speedScale;
        rigidbody.velocity = vec;
    }

    public void AttackRole()
    {
        BattleManager.Instance.MoveBoxOver(this.gameObject, type);  
    }


    public void MoveToRight()
    {
        AttackRole();
       // this.rigidbody2D.velocity = new Vector2(1, 0);
    }

    public void MoveToLeft()
    {
        curSpeed = new Vector2(speedScale * -minSpeedX, 0);
        rigidbody.velocity = curSpeed;
    }


    public void StopRed()
    { 
        rigidbody.velocity = new Vector2(0,0);
    }

    public void ResumeRed()
    {
        curSpeed = new Vector2(speedScale * minX, 0);
        rigidbody.velocity = curSpeed;
    }

    //击退
    public void GoRepel()
    {
        isBacking = true;
        rigidbody.velocity = new Vector2(0.5f, 0);
        StartCoroutine("Resume");
        if (isAccdent)
        {
            speedScale = minSpeedX; 
            Vector2 vec = rigidbody.velocity * speedScale;
            rigidbody.velocity = vec;
        }
    }
    public IEnumerator Resume()
    {
        yield return  new WaitForSeconds(0.8f);
        isBacking = false;
        if(this!=null)
            rigidbody.velocity = curSpeed;
    }
}
