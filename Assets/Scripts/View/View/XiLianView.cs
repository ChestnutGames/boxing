using UnityEngine;
using System.Collections;

public class XiLianView : MonoBehaviour {

    public UILabel name;
    public UILabel num;
    public UIProgressBar progress; 
    public XiLianData data;

    public void InitData(XiLianData d)
    {
        data = d;
        name.text = d.name;
        num.text = d.num.ToString();
        float unit = (float)1/(d.max-d.min);
        progress.value = unit * d.num;
    }

    public void RestEmpty()
    {
        data = null;
        name.text = "";
        num.text = ""; 
        progress.value = 0;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
}
