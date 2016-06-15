using UnityEngine;
using System.Collections;

public class TagButton : MonoBehaviour 
{
	public string tag;
    // 注释了暂未使用的start和update函数--6.14

    // Use this for initialization
    //void Start () 
    //{
    //}

    //// Update is called once per frame
    //void Update () 
    //{
    //}

    public void OnButtonClicked()
	{
		GameObject go = GameObject.FindWithTag(tag);

		if(go != null)
		{
			NGUITools.SetActive(go, true);
		}
	}
}
