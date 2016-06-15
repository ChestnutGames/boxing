using UnityEngine;
using System.Collections;

public class ClickBox : MonoBehaviour
{

    public int id;
    public BoxType type;

    public int blockIndex;

    public float widthX;


    public int clickCount;
    public int clickMax;

    public float offsetX;

    public UISprite sprite;

    public TweenScale tweenScale;

    public delegate void ClickDistoryCallback();
    public event ClickDistoryCallback clickDestoryCallEvent;

    public delegate void ClickOverAnimCallback();
    public event ClickOverAnimCallback clickOverAnimEvent;

    public bool isNotAddCombo;

    public const float AnimEndTime = 0.155f;
    public enum BoxType
    { 
        BlueBox ,
        YellowBox ,
        RedBox ,
        GroundBox,
        BombBox ,
        FastBox ,
        ComboBox ,
        ComboDragBox ,
        BossBox ,
        Skill ,
        None,
        Act
    }

    void Start()
    {
        clickDestoryCallEvent += DestoryBox;
        clickOverAnimEvent += OverAnimPlay;
    }

    public void Init(float offset,int depth)
    {
        offsetX = offset;
        clickCount = 0;
        sprite.depth = depth;
        id = depth;
        isNotAddCombo = false;
        
    } 
    public void EndDestoryAnim()
    {
        clickDestoryCallEvent();
    }

    public void OverAnimPlay()
    {
        if (tweenScale == null)
            tweenScale = this.GetComponent<TweenScale>();
        if (tweenScale != null)
            tweenScale.PlayReverse();
        Invoke("EndDestoryAnim", AnimEndTime);
    }

    public void Click()
    {
        clickCount++;
        clickOverAnimEvent();
    }

    public void Click(BoxType type)
    {
        clickCount++;
        this.type = type;
        clickOverAnimEvent(); 
    }

    public void NotAddComboClick()
    {
        clickCount++;
        this.type = type;
        clickOverAnimEvent();
        isNotAddCombo = true;
    }

    public void DestoryBox()
    {
        if (type == BoxType.GroundBox)
        {
            if (clickOverAnimEvent != null)
                clickDestoryCallEvent();
            isNotAddCombo = false;
        }
        else if (type == BoxType.BombBox)
        {
            clickDestoryCallEvent();

        }
        else if (type != BoxType.ComboBox && BattleManager.Instance.isTouchingComboBox != true)
        {
            BattleManager.Instance.ClickBoxOver(this.gameObject, type,this.isNotAddCombo); 
        } 
    } 
	 
}
