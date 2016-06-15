using UnityEngine;
using System.Collections;

public class HUDBlood : MonoBehaviour {


    UISlider mSlider;
    public UISprite mMid;
	// Use this for initialization
	void Awake () {
        mSlider = transform.GetComponent<UISlider>();

	}

    public void SetValue( float _value )
    {
        mSlider.value = _value;
    }
	
	// Update is called once per frame
	void Update () {
	
        if( null != mMid )
        {
            mMid.fillAmount = Mathf.Lerp(mMid.fillAmount, mSlider.value, 3f * Time.deltaTime);
            mMid.drawRegion = new Vector4(0f, 0f, mMid.fillAmount, 1f);
            if (mMid.fillAmount < 0.001f)
                mMid.enabled = false;
     
            
        }
	}
}
