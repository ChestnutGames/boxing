using UnityEngine;
using System.Collections;
using Spine;
using CodeStage.AntiCheat.ObscuredTypes; 

public class StaticBoxState : IState
{
    ClickBox.BoxType type;
    string[] strs;
    MonsterMgr role;
    public StaticBoxState(ClickBox.BoxType t,string[] s,MonsterMgr r)
    {
        type = t; 
        role = r;
        strs = s;
    }

    public void OnEnter(string prevState)
    {  
        role.ChangeSortLayer(Def.SortRole);
        int rand = Random.Range(1, 10);
        if (rand > 5 && strs.Length<=1)
        {
            role.animation.state.SetAnimation(0, strs[0], false);
            role.effect.state.SetAnimation(0, strs[0], false);
        }
        else
        {
            role.animation.state.SetAnimation(0, strs[1], false);
            role.effect.state.SetAnimation(0, strs[1], false);
        }
        role.animation.state.AddAnimation(0, "standby", true, 0);
    }

    public void OnExit(string nextState)
    { 
    }

    public void OnUpdate()
    { 

    } 
}

public class MoveBoxState : IState
{
    ClickBox.BoxType type;
    string[] strs;
    MonsterMgr role;
    public MoveBoxState(ClickBox.BoxType t,string[] s,MonsterMgr r)
    {
        type = t; 
        role = r;
        strs = s;
    }

    public void OnEnter(string prevState)
    {
        role.ChangeSortLayer(Def.SortEmeny);
        int rand = Random.Range(1, 10);
        if (rand > 5 && strs.Length <= 1)
        {
            role.animation.state.SetAnimation(0, strs[0], false);
            role.effect.state.SetAnimation(0, strs[0], false);
        }
        else
        {
            role.animation.state.SetAnimation(0, strs[1], false);
            role.effect.state.SetAnimation(0, strs[1], false);
        }
        role.animation.state.AddAnimation(0, "standby", true, 0); 
    }

    public void OnExit(string nextState)
    {
 	     
    }

    public void OnUpdate()
    {
 	     
    }
}

public class SceneState : IState
{ 
    string[] strs;
    MonsterMgr role;
    public SceneState(string[] s, MonsterMgr r)
    { 
        role = r;
        strs = s;
    }

    public void OnEnter(string prevState)
    {
        role.animation.state.SetAnimation(0, strs[0], false);
        role.effect.state.SetAnimation(0, strs[0], false); 
    }

    public void OnExit(string nextState)
    {

    }

    public void OnUpdate()
    {

    }

}

public class MonsterMgr : MonoBehaviour {
    [SerializeField]
    public SkeletonAnimation animation;
    public SkeletonAnimation effect; 
    public BooldBar boolbar;

    [SerializeField]
    private GameObject labelInitPos;
    [SerializeField]
    private GameObject labelTopRightPos;
    [SerializeField]
    private GameObject labelBottomLeftPos;
      

    public UIPanel hurtPanel;
    public GameObject hurt;

    public RoleData data; 

    private MonsterMgr emeny; 

    public bool isComboing;

    public FiniteStateMachine fsm;

      

    public enum RoleState
    {
        Attack1,
        Attack2,
        Idle,
        Hit,
        Defense, 
        Victory,
        Combo,
        Skill,
        Lose
    }

    public delegate void AnimEndCallBackEvent();
    public event AnimEndCallBackEvent AnimEndEvent;

	// Use this for initialization
	void Start () {
        Resolution();
        animation = this.GetComponent<SkeletonAnimation>();
         

       //SkeletonDataAsset data = Resources.Load<SkeletonDataAsset>("eluosi/New SkeletonData.asset");
       //animation.skeletonDataAsset = data;
       //data.Reset();
       //animation.Reset(); 
	}

    public void InitData()
    {
         
    }

    //适配
    public void Resolution()
    {
        float a = Screen.width / Screen.height;
        float z = 0.1f * (1 - (a - 1.0f)); 
        this.transform.localScale = new Vector3(0.4f+z,0.4f+z,0.4f+z); 
    }

    public void OnAnimEnd()
    {
        AnimEndEvent();
    }

    public void InitUser(RoleData d)
    {
        data = d;
        data.attackCount = 0;
        data.RestUserAttr();
        d.curFightPower = d.fightPower;
        boolbar.ChangeMaxHealth(d.fightPower);//tod pwoer
        boolbar.ChangeCurHealth(d.curFightPower); 
        isComboing = false; 
        ChangeState(RoleState.Idle);
        
    }

    public void InitEmeny(RoleData d)
    { 
        data = d;
        data.attackCount = 0;
        data.RestEmenyAttr();
        d.curFightPower = d.fightPower;
        boolbar.ChangeMaxHealth(d.fightPower);//tod pwoer
        boolbar.ChangeCurHealth(d.curFightPower);
        isComboing = false;
        ChangeState(RoleState.Idle);

    }

    public void InitFsm()
    {
        //初始化状态机
        fsm = new FiniteStateMachine(); 
        fsm.Register("punch", new StaticBoxState(ClickBox.BoxType.BlueBox,
            new string[] {"punch1","punch2" },this));
        fsm.Register("Hpunch", new StaticBoxState(ClickBox.BoxType.YellowBox,
            new string[] { "Hpunch1", "Hpunch2" }, this));
        fsm.Register("combo punch", new StaticBoxState(ClickBox.BoxType.ComboBox,
             new string[] { "combo punch1" }, this));
        fsm.Register("combo punch", new StaticBoxState(ClickBox.BoxType.Skill,
             new string[] { "combo punch2" }, this));

        fsm.Register("block", new MoveBoxState(ClickBox.BoxType.None,
            new string[] { "block" }, this));
        fsm.Register("injure", new MoveBoxState(ClickBox.BoxType.None,
            new string[] { "injure" }, this));

        fsm.Register("victory", new SceneState(new string[] { "victory" }, this));
        fsm.Register("defeat", new SceneState(new string[] { "defeat" }, this));
        fsm.Register("standby", new SceneState(new string[] { "standby" }, this));

        fsm.EntryPoint("standby");

        //fsm.State("injure").On("BombBox", delegate(int hitNum, bool isHit)
        //{ 
        //    if(isHit)
        //        BattleManager.Instance.role.Hit(emeny.attr.bomb_Min, emeny.attr.bomb_Max); 
        //});
        //fsm.State("injure").On("RedBox", delegate(int hitNum, bool isHit)
        //{
        //    if(isHit)
        //        BattleManager.Instance.role.Hit(emeny.attr.red_Min, emeny.attr.red_Max); 
        //});
        // fsm.State("injure").On("FastBox", delegate(int hitNum, bool isHit)
        //{ 
        //    if(isHit)
        //        BattleManager.Instance.role.Hit(emeny.attr.fast_Min, emeny.attr.fast_Max); 
        //});
        // fsm.State("injure").On("GroundBox", delegate(int hitNum, bool isHit)
        //{ 
        //    if(isHit)
        //        BattleManager.Instance.role.Hit(emeny.attr.ground_Min, emeny.attr.ground_Max); 
        //});    
    }

    public void ChangeSortLayer(string sort)
    {
        if(animation!= null && effect !=null)
        { 
            animation.GetComponent<Renderer>().sortingLayerName = sort;
            effect.GetComponent<Renderer>().sortingLayerName = sort;
        }
    }

    public void Hit(int act)
    { 
        data.curFightPower -= act; 
        boolbar.ChangeCurHealth(data.curFightPower); 
        //if (data.curFightPower < 0)
        //{ 
        //    BattleManager.Instance.isBattle = false;
        //    BattleManager.Instance.BattleOver(data.csv_id);
        //}
    }

    public void Abosrb(int act)
    { 
        data.curFightPower += act;
        if (data.curFightPower > data.fightPower)
            data.curFightPower = data.fightPower;
        boolbar.ChangeCurHealth(data.curFightPower);
    }

    public void Hit(int min,int max)
    {
        int act = Random.Range(min, max);  
        data.curFightPower -= act; 
        boolbar.ChangeCurHealth(data.curFightPower); 
        //if (data.curFightPower < 0)
        //{ 
        //    BattleManager.Instance.isBattle = false;
        //    BattleManager.Instance.BattleOver(data.csv_id);
        //}
        //if (labelInitPos != null)
        //{
        //    GameObject go = Instantiate(hurt) as GameObject;
        //    go.transform.position = labelInitPos.transform.position;
        //    go.transform.parent = hurtPanel.transform;
        //    go.transform.localScale = Vector3.one;

        //    HurtLabel label = go.GetComponent<HurtLabel>();
        //    label.Init(labelInitPos.transform.position, labelTopRightPos.transform.position,
        //        labelBottomLeftPos.transform.position, act - defense);
        //}
    }

    public void ChangeWeapon(AtlasAsset atlasAsset, SkeletonRenderer skeletonRenderer, string slotname, string spritename)
    {
        AtlasAttachmentLoader loader = new AtlasAttachmentLoader(atlasAsset.GetAtlas());
        float scaleMultiplier = skeletonRenderer.skeletonDataAsset.scale;
        var regionAttachment = loader.NewRegionAttachment(null, slotname, spritename);
        regionAttachment.Width = regionAttachment.RegionOriginalWidth * scaleMultiplier;
        regionAttachment.Height = regionAttachment.RegionOriginalHeight * scaleMultiplier;
        regionAttachment.SetColor(new Color(1, 1, 1, 1));
        regionAttachment.UpdateOffset();
        var slot = skeletonRenderer.skeleton.FindSlot(slotname);
        slot.Attachment = regionAttachment;

    } 

    public float ChangeState(RoleState state)
    {
        if (this == null)
            return 0;
        if (animation==null)
            animation = this.GetComponent<SkeletonAnimation>(); 
        int rand = Random.Range(1, 10);
        isComboing = false;
        TrackEntry track;
        switch (state)
        {
            case RoleState.Attack1:
                ChangeWear();
                if (rand > 5)
                {
                    track = animation.state.SetAnimation(0, "punch1", false);
                    effect.state.SetAnimation(0, "punch1", false);
                }
                else
                {
                    track = animation.state.SetAnimation(0, "punch2", false);
                    effect.state.SetAnimation(0, "punch2", false);
                } 
                animation.state.AddAnimation(0, "standby", true, 0); 
                break;
            case RoleState.Attack2:
                if (rand > 5)
                {
                    track = animation.state.SetAnimation(0, "Hpunch1", false);
                    effect.state.SetAnimation(0, "Hpunch1", false);
                }
                else
                {
                    track=animation.state.SetAnimation(0, "Hpunch2", false);
                    effect.state.SetAnimation(0, "Hpunch2", false);
                } 
                animation.state.AddAnimation(0, "standby", true, 0);
                break;
            case RoleState.Defense:
                track = animation.state.SetAnimation(0, "block", false);
                effect.state.SetAnimation(0, "block", false);
                animation.state.AddAnimation(0, "standby", true, 0);
                break;
            case RoleState.Combo:
                isComboing = true;
                track = animation.state.SetAnimation(0, "combo punch2", false);
                effect.state.SetAnimation(0, "combo punch2", false);
                animation.state.AddAnimation(0, "standby", true, 0);
                break;
            case RoleState.Skill:
                isComboing = true;
                track = animation.state.SetAnimation(0, "combo punch1", false);
                effect.state.SetAnimation(0, "combo punch1", false);  
                animation.state.AddAnimation(0, "standby", true, 0);
                break;
            case RoleState.Idle: 
                track = animation.state.SetAnimation(0, "standby", true);
                effect.state.SetAnimation(0, "standby", false);
                break; 
            case RoleState.Hit:
                track = animation.state.SetAnimation(0, "injure", false);
                effect.state.SetAnimation(0, "injure", false);
                animation.state.AddAnimation(0, "standby", true, 0); 
                break; 
            case RoleState.Victory:
                if (rand > 5)
                {
                    track = animation.state.SetAnimation(0, "victory1", false);
                    effect.state.SetAnimation(0, "victory1", false);
                }
                else
                {
                    track = animation.state.SetAnimation(0, "victory2", false);
                    effect.state.SetAnimation(0, "victory2", false);
                    //animation.state.AddAnimation(0, "idle", true, 0);
                }
                
                break;
            case RoleState.Lose: 
                effect.state.SetAnimation(0, "defeat", false);
                track = animation.state.SetAnimation(0, "defeat", false); 
                break;
        }
        return 1;
    } 

    void ChangeWear()
    {
        //animation.skeleton.SetAttachment("xianluo_r3_c12", "_r3_c7");
        //animation.skeleton.GetAttachment();
    }

    void BattleStart()
    { 
    }
	
	// Update is called once per frame
	void Update () {
       //fsm.Update();



	}
}
