using UnityEngine;
using System.Collections;

public class ArenaRoleView : SPSGame.Unity.UIObject
{

    public RoleData data;
    public int index;
    public UISprite role;

    public void InitData(RoleData d)
    {
        role.spriteName = d.icon;
        data = d; 
        //ListenOnClick(this.gameObject, ClickRole);
    }

    public void ClickRole(GameObject obj)
    {
        ArenaMgr.Instance.ChooseRoleView(this);
    }
}
