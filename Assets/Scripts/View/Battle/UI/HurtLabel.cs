using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HurtLabel : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    public UILabel label;

    public void Init(Vector3 init,Vector3 topright, Vector3 bottomleft, int hurt)
    {
        path.Add(init);
        path.Add(new Vector3(topright.x,topright.y,0));
        path.Add(new Vector3(topright.x,bottomleft.y, 0));

        //键值对儿的形式保存iTween所用到的参数
		Hashtable args = new Hashtable();

        //移动的速度，
        args.Add("speed", 3f);
        //移动的整体时间。如果与speed共存那么优先speed
        args.Add("time", 4f);
        //延迟执行时间
        args.Add("delay", 0.1f);

        args.Add("path", path.ToArray());
        //移动结束时调用，参数和上面类似
        args.Add("oncomplete", "AnimationEnd");
        args.Add("oncompleteparams", "end");
        args.Add("oncompletetarget", gameObject); 

        //这里是设置类型，iTween的类型有很多种，在源码中的枚举EaseType中
        //例如移动的特效，先震动在移动、先后退在移动、先加速在变速、等等
        args.Add("easeType", iTween.EaseType.easeInOutCubic);

        iTween.MoveTo(this.gameObject,args);
        label.text = hurt.ToString();
    }

    //对象移动时调用
    void AnimationEnd(string f)
    {
        this.gameObject.SetActive(false);
        Destroy(this);
    }

    void Start()
    {
        label = this.GetComponent<UILabel>();

    }
    void OnDrawGizmos()
    {
        if (path.Count > 0)
        {
            iTween.DrawPath(path.ToArray());
        }
    }  
}