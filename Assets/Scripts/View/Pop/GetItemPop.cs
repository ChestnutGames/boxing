using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetItemPop : MonoBehaviour
{

    private List<ItemViewData> dataList;
    private List<ItemView> viewList;

    public UIScrollView scroll;
    public UITable table;

    public GameObject itemPrefab;


    public void InitData(List<ItemViewData> list)
    {
        SetList(list);
    }

    void SetList(List<ItemViewData> list)
    {
        while (table.transform.childCount > 0)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        viewList = new List<ItemView>();
        for (int index = 0; index < list.Count; index++)
        {
            //设置格子
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(true);
            ItemView pop = obj.GetComponent<ItemView>();
            ItemViewData data = null;
            if (index < list.Count)
            {
                data = list[index];
                if (data != null)
                {
                }
            }
            else
            {
                obj.name = "None";
            }
            pop.InitData(data, index);
            pop.transform.parent = table.transform;
            pop.transform.localScale = Vector3.one;
            viewList.Add(pop);
        }
        table.Reposition();
        table.repositionNow = true;
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.GetItem, false);
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
