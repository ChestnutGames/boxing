using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxingPop : MonoBehaviour {
    public UIScrollView roleScrollView;
    public UITable roleTable;
    public UIScrollView boxScrollView;
    public UITable boxTable;

    public UIPanel movePanel;

    public UISlider fraSilder;
    public UILabel fralable;
    public UIButton upLevelBtn;
    public UIButton saveBtn;

    public ItemView itemIcon;
    public UILabel itemName;
    public UILabel itemProcess;
    public UILabel[] attrs;

    public BoxingView boxingIcon;
    public UILabel name;
    public UILabel level;
    public UILabel desc;

    public Hashtable roleViewTable;
    public Hashtable boxViewTable;
    public HeadRole curRoleView;
    public BoxingView curBoxView;

 

    public GameObject rolePrefabe;
    public GameObject boxPrefabe;

    public BoxingView[] passiveArr;
    public BoxingView[] activeArr;

    public void Start()
    {

    }

    public void InitData()
    {
        BoxingMgr.Instance.OpenPop(this); 
    }

    public HeadRole GetRoleView(int id)
    {
        return roleViewTable[id] as HeadRole;
    }

    public BoxingView GetBoxView(int id)
    {
        return boxViewTable[id] as BoxingView;
    }

    public void SetFragNum(string s)
    {
        itemProcess.text = s;
    }
    public void SetBoxingInfo(BoxingViewData data, List<string> strs)
    {
        ItemViewData item = new ItemViewData();
        item.curCount =data.data.levelData.item_num;
        item.data = GameShared.Instance.GetItemData(data.data.levelData.item_id);
        Debug.Log("id" + data.data.csv_id + "fra" + data.fra_value);
        if (item != null)
        {
            itemIcon.InitData(item, false);
            itemName.text = item.data.name;
            itemProcess.text = data.frag_num + "/" + data.data.levelData.item_num;
            fraSilder.value = data.fra_value;
        }
        else
        {
            itemName.text = "";
            itemProcess.text = "";
            fraSilder.value = 0;
        }
        
        //upLevelBtn.isEnabled = data.canUpLevel;
         
        if (strs != null && strs.Count > 0)
        {
            for (int i = 0; i < strs.Count; i++)
            {
                if (i<attrs.Length)
                    attrs[i].text = strs[i];
            }
        }
        desc.text = data.data.levelData.skill_desc;
        name.text = data.data.name;
        level.text = data.data.levelData.skill_level.ToString();
        boxingIcon.InitData(data,false); 
    }

    public void SetRoleList(ref List<RoleData> list)
    {
        while (roleTable.transform.childCount > 0)
        {
            DestroyImmediate(roleTable.transform.GetChild(0).gameObject);
        } 
        if (list != null)
        {
                roleViewTable = new Hashtable();
                for (int i = 0; i < list.Count; i++)
                {
                    RoleData r = list[i] as RoleData;
                    GameObject obj = Instantiate(rolePrefabe);
                    obj.SetActive(true);
                    HeadRole pop = obj.GetComponent<HeadRole>();
                    pop.InitData(r, true);
                    obj.name = r.sort.ToString(); 
                    pop.transform.parent = roleTable.transform;
                    pop.transform.position = Vector3.zero;
                    pop.transform.localScale = Vector3.one;
                    roleViewTable.Add(r.csv_id, pop); 
                }
                roleTable.Reposition();
                roleScrollView.ResetPosition();
                roleScrollView.Scroll(0);
                roleTable.repositionNow = true;
        } 
    }

    public void SetBoxingList(Hashtable list)
    {
        while (boxTable.transform.childCount > 0)
        {
            DestroyImmediate(boxTable.transform.GetChild(0).gameObject);
        } 
        if (list != null)
        {
                boxViewTable = new Hashtable();
                System.Collections.IDictionaryEnumerator enumerator = UserManager.Instance.RoleTable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    //设置格子 roleTable
                    BoxingViewData r = list[enumerator.Key] as BoxingViewData;
                    if (r != null)
                    {
                        GameObject obj = Instantiate(boxPrefabe);
                        obj.SetActive(true);
                        BoxingView pop = obj.GetComponent<BoxingView>();
                        pop.InitData(r, true);
                        obj.name = r.sort.ToString();
                        pop.transform.parent = boxTable.transform;
                        pop.transform.position = Vector3.zero;
                        pop.transform.localScale = Vector3.one;
                        if (!boxViewTable.Contains(r.data.csv_id))
                        {
                            boxViewTable.Add(r.data.csv_id, pop);
                        }
                    }
                }
                boxScrollView.ResetPosition();
                boxTable.Reposition(); 
                boxTable.repositionNow = true;
        } 
    }
   
    public void BoxingUpLevelClick()
    {
        BoxingMgr.Instance.UpLevel(); 
    }

    public void BoxingSaveClick()
    {
        BoxingMgr.Instance.SaveBoxing();
    }

    public void RoleSelectClick(HeadRole h)
    {
        BoxingMgr.Instance.SetRoleFocus(h); 
    }

    public void CloseClick()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.Boxing, false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

	 
}
