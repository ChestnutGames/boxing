using UnityEngine;
using System.Collections;

public class LevelResultView : MonoBehaviour {

    public UILabel name;
    public UISprite icon;
    public UILabel num;
	// Use this for initialization

    public void InitData(string n,ItemViewData d)
    {
        name.text = n;
        icon.spriteName = d.data.path;
        num.text = d.curCount.ToString();
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
