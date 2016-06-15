using UnityEngine;
using System.Collections;

public class SimpleButton : MonoBehaviour
{
	public GameObject target;

	// 注释了暂未使用的start和update函数--6.14
	//void Start () 
	//{
	//}
	
	//// Update is called once per frame
	//void Update () 
	//{
	//}

	public void OnButtonClicked()
	{
		if(target != null && !NGUITools.GetActive(target))
		{
			NGUITools.SetActive(target, true);
		}
	}
}
