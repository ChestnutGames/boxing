using UnityEngine;
using System.Collections;

public class WakeUpLevelView : MonoBehaviour {
    public UISprite[] wakeSprite;
	// Use this for initialization
    private string on;
    private string off;

    public void InitData(string a,string b)
    {
        on = a;
        off = b;
 
    }

    public void SetWakeLevel(int level)
    {
        for (int i = 0; i < wakeSprite.Length; i++)
        {
            wakeSprite[i].spriteName = "觉醒皇冠_2";
            if (i < level)
            {
                wakeSprite[i].spriteName = "觉醒皇冠_1";
            }
        }
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
