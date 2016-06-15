using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipKitPop : MonoBehaviour {
    public UITable table;
    public UIScrollView scroll;
    public GameObject label;

    public void SetInfo(List<string> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(label);
            obj.SetActive(true);
            UILabel v = obj.GetComponent<UILabel>();
            v.text = list[i];
            obj.transform.parent = table.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.position = Vector3.zero;
        }
        table.Reposition();
        scroll.ResetPosition();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.EquipKit, false);
        this.gameObject.SetActive(false);
        Destroy(this);
    }
}
