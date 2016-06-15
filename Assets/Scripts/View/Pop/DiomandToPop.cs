using UnityEngine;
using System.Collections;

public class DiomandToPop : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.DiomandTo, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void RechagerClick()
    {
        CloseClick();
        MainUI.Instance.RechargePopClick(); 
    }
}
