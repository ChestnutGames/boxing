using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmailData {
    public enum EmailType
    {
        ReadDel = 1,
        Read = 2, 
    }

    public long id;
    public string name;
    public string from;
    public string date;
    public string icon;
    public string desc;
    public EmailType type;
    public List<ItemViewData> itemList; //s 图片 和 数量
    public bool isRead;
    public bool isRevice;

    public string sort;
}
