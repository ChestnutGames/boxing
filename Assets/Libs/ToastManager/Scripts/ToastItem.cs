using UnityEngine;
using System.Collections;

public class ToastItem : MonoBehaviour
{
    [SerializeField]
    private UISprite _sprite;

    [SerializeField]
    private UILabel _label;

    private UITweener _tweenerIn;

    private UITweener _tweenerOut;

    private string _textSave;
    private float _durationSave;

    // Use this for initialization
    void Start ()
    {
        UITweener[] tweeners = GetComponents<UITweener>();

        foreach(UITweener tweener in tweeners)
        {
            if(tweener.tweenGroup == 0)
            {
                _tweenerIn = tweener;
            }
            else if (tweener.tweenGroup == 1)
            {
                _tweenerOut = tweener;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void SetText(string text)
    {
        _textSave = text;
    }

    public void SetDuration(float duration)
    {
        _durationSave = duration;
    }

    public void Show()
    {
        if (_label != null)
        {
            _label.text = _textSave;
            _tweenerOut.delay = _durationSave;

            _tweenerIn.ResetToBeginning();
            _tweenerOut.ResetToBeginning();

            _tweenerIn.PlayForward();
            _tweenerOut.PlayForward();
        }
    }
}
