using UnityEngine;
using System.Collections;

public class ChapterPop : MonoBehaviour {
    public ChapterData data;

    public UISprite bg;

    public UIButton pre;
    public UIButton next;

    public UILabel name;


    public int index;

    public void InitData() 
    {
        LevelsMgr.Instance.OpenChapter(this); 
    }

    public void InitChapter(ChapterData cur)
    {
        if (cur.csv_id >UserManager.Instance.curUnLockChapter)
        {
            ToastManager.Instance.Show("未解锁");
        }else
        {
            name.text = cur.name;
            bg.spriteName = cur.bg;
            index = cur.csv_id;
            data = cur;
        }
    }

    public void NextClick()
    {
        if(index<GameShared.Instance.config.chapter_max)
        {
            index++;
            InitChapter(GameShared.Instance.GetChapterById(index));
        }
    }

    public void PreClick()
    {
        if (index > 1)
        {
            index--;
            InitChapter(GameShared.Instance.GetChapterById(index));
        }
    } 

    public void ToLevelClick()
    {
        LevelsMgr.Instance.CreateLevelPop(data);
    }

    public void Close()
    {
        MainUI.Instance.SetPopState(MainUI.PopType.chapter, false);
        this.gameObject.SetActive(false);
        Destroy(this);
    }  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
