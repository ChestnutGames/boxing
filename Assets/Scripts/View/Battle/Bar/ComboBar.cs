





using UnityEngine;
using System.Collections;

public class ComboBar : MonoBehaviour
{
    [SerializeField]
    private UIProgressBar combobar;
    public int curCombo;
    private float unit;
    private float tarValue;
    UIEventListener list;
    public UISprite light;
    public int max_combo = 5;
    // Use this for initialization
    void Start()
    {
        //list = this.GetComponent<UIEventListener>();
        //list.onPress += OnPress;
    }

    public void Init()
    {
        combobar.value = 0;
        curCombo = 0;
        ChangeComboLabel(0);
        ChangeMaxCombo();
        //light.gameObject.SetActive(false);
    }

    public void ChangeMaxCombo()
    {
        unit = 1.0f / max_combo;
        ChangeCurCombo(curCombo);
    }

    public void ChangeCurCombo(int h)
    {
        curCombo = h;
        if (curCombo >= max_combo)
            //light.gameObject.SetActive(true);
        ChangeBooldBar();
    }

    public void ChangeComboLabel(int h)
    {
        if (h < max_combo)
        {
           // light.gameObject.SetActive(false);
        }
    }

    void ChangeBooldBar()
    {
        combobar.value = curCombo * unit;
    }

    void Update()
    {


    }

    void OnPress(bool pressed)
    {
        BattleManager.Instance.isTouchingSkill = true;
    }
}

