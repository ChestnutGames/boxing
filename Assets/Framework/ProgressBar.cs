using System;
using System.Collections;

using UnityEngine;

public class ProgressBar : MonoBehaviour 
{
	public UISprite foreground;
	
	bool _isAnimating = false;
	
	string _progressAnimationName = "ProgressAnimation";
	
	public event Action<float> ValueChangedEvent;
	
	public float Value { get { return foreground.fillAmount; } }
	
	// Use this for initialization
	void Start () {
		
		//make sure the sprite is the type what this script wants!
		//foreground.type = UIBasicSprite.Type.Filled;
		//foreground.fillDirection = UIBasicSprite.FillDirection.Horizontal;
		//foreground.fillAmount = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ProgressAnimationUpdate(float theValue)
	{
		foreground.fillAmount = theValue;
		
		if(ValueChangedEvent != null)
		{
			ValueChangedEvent(theValue);
		}
	}
	
	void ProgressAnimationEnd()
	{
		_isAnimating = false;
	}
	
	public void SetProgress(float theProgress)
	{
		SetProgress(theProgress,true,0.5f);
	}
	
	public void SetProgress(float theProgress, bool theAnimated, double theAnimationTime = 0.5)
	{
		if (!theAnimated)
		{
			foreground.fillAmount = theProgress;
		}
		else // animated
		{
			if (_isAnimating)
			{
				iTween.StopByName(_progressAnimationName);
			}
			else
			{
				_isAnimating = true;
			}
			
			float oldValue = foreground.fillAmount;
			float newValue = theProgress;
			
			iTween.ValueTo(this.gameObject, iTween.Hash("name",_progressAnimationName,
											            "from", oldValue,
			                                            "to",newValue,
											            "time", theAnimationTime,
											            "onupdatetarget", this.gameObject,
											            "onupdate", "ProgressAnimationUpdate",
											            "easetype", iTween.EaseType.easeOutQuart,
											            "oncompletetarget", this.gameObject,
											            "oncomplete", "ProgressAnimationEnd"));
		}
		
	}
}