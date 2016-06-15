using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementPop : MonoBehaviour
{

    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private UIGrid grid;
    [SerializeField]
    private UIScrollView scroll;
    private Dictionary<int,AchievementView> viewList; 

    void Start()
    {
        
    }  

    public void InitData()
    {
        AchievementMgr.Instance.OpenPop(this);
        AchievementMgr.Instance.GetAchievementList();
    }

    public void CloseClick()
    { 
        MainUI.Instance.SetPopState(MainUI.PopType.Achevement, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    } 

    public void SetItemList(List<AchievementViewData> list)
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        viewList = new Dictionary<int,AchievementView>();
        for (int i = 0; i < list.Count; i++) 
        {
            GameObject row = Instantiate(itemPrefab);
            row.SetActive(true);
            row.transform.parent = grid.transform;
            row.transform.localScale = Vector3.one;
            row.transform.position = Vector3.zero;
            AchievementView view = row.GetComponent<AchievementView>();
            view.InitData(list[i],i);
            viewList.Add(view.data.data.type,view);
        }
        grid.Reposition();
        scroll.ResetPosition();
        grid.repositionNow = true; 
    }

    public AchievementView GetViewByTyep(int type)
    {
        return viewList[type];
    } 

    public void RemoveItem(AchievementView v)
    {
        grid.RemoveChild(v.gameObject.transform);
    }

    public void AddNewItem(AchievementViewData d)
    {
         GameObject row = Instantiate(itemPrefab);
            row.SetActive(true);
            row.transform.parent = grid.transform;
            row.transform.localScale = Vector3.one;
            row.transform.position = Vector3.zero;
            AchievementView view = row.GetComponent<AchievementView>();
            view.InitData(d, viewList.Count);
            viewList.Add(view.data.data.type,view);
            grid.Reposition();
            scroll.ResetPosition();
            grid.repositionNow = true;
    }
     
     
}
