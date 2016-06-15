using UnityEngine;
using System.Collections;
using LuaInterface;

public class LuaMgr : UnitySingleton<LuaMgr> {

    public LuaState state;

    public void InitData()
    {
        if(state == null)
            state = new LuaState();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
