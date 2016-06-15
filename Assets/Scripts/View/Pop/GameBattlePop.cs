using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using Framework;

public class GameBattlePop : MonoBehaviour {
       

    public BlockManager spawner;

    public TouchZone touchZone;

    public CameraManger camera;
    public BGManager bg;

 
    //公用数据
    public GameObject leftPos;
    public GameObject rightPos;
    public GameObject initMoveBoxPos;


    public ClickCursor cursor;
    public MonsterMgr role;
    public MonsterMgr emeny;

    public GameObject rolePos;
    public GameObject emenyPos;

    public BooldBar roleBoolbar;
    public BooldBar emenyBoolbar;
    public ComboBar comboBar;
    public GameObject comboBarAnim;

     

    public GameObject rolePrefab;
    public GameObject emenyPrefab;


    public UILabel huosheng;

    public CloudSpawner cloudCreator;


    //适配用
    public GameObject bottomBar;
    public GameObject bottomBarBg;


    public UILabel roletxt;
    public UILabel emenytxt;


    public UILabel rolefi;
    public UILabel emenyfi;

    public void Awake()
    {
        BattleManager.Instance.InitData(this);
        UIManager.Instance.battleRoot = this.gameObject;
    }

    void OnApplicationPause()
    {
       // if (BattleManager.Instance.isBattle )
 //           NetworkManager.Instance.AppUnFocusBattle();
    }

    void OnApplicationFocus()
    {
       // if (BattleManager.Instance.isBattle )
//            NetworkManager.Instance.AppFocusBattle();
    }

}
