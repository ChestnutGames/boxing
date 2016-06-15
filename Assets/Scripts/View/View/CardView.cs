using UnityEngine;
using System.Collections;

public class CardView : ItemView {
     
    public void CardClick()
    {
        if (isClick)
        {
            LiLianMgr.Instance.LiLianSend(data);
        }
    } 
}
