using UnityEngine;
using System.Collections;

public class BooldBar : MonoBehaviour {

    public UISlider Booldbar;
    public UISlider Booldbgbar;

    public int maxHealth = 100;
    public int health;
    private float unit;

    private float booldbarCount;

    public UILabel fi;
    
	// Use this for initialization
	void Start () {
	
	}

    public void Init()
    {
        Booldbgbar.value = 1;
        Booldbar.value = 1;
        ChangeMaxHealth(maxHealth);
    }
     
    public void ChangeMaxHealth(int h)
    {
        
        maxHealth = h;
        unit = 1.0f/maxHealth;
        ChangeCurHealth(health);
    }

    public void ChangeCurHealth(int h)
    {
       fi.text = maxHealth + "/"+ h;
       health = h;
       ChangeBooldBar();
    }

    void ChangeBooldBar()
    {
        Booldbar.value = health * unit;
    }
	
	// Update is called once per frame
	void Update () {

        if (Booldbgbar.value < Booldbar.value)
        {
            Booldbgbar.value = Booldbar.value;
        }
        else if (Booldbgbar.value != Booldbar.value)
        {
            Booldbgbar.value -= 0.004f;
        }
        
	}
}
