using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelResultPop : MonoBehaviour
{
     
    public UIScrollView scroll;
    public UITable table;

    public GameObject itemPrefab;


    public void InitData(List<LevelData> list)
    {
        SetList(list);
    }

    void SetList(List<LevelData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        } 
        for (int i = 0; i < list.Count; i++)
        {
            //设置格子
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(true);
            LevelResultView pop = obj.GetComponent<LevelResultView>();
             
            pop.InitData(list[i].name, list[i].rewardList[0]);
            pop.transform.parent = table.transform;
            pop.transform.localScale = Vector3.one; 
        }
        scroll.ResetPosition();
        table.Reposition();
        table.repositionNow = true;
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.LevelResult, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
