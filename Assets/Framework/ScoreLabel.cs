using UnityEngine;
using System.Collections;

namespace Framework
{
	[RequireComponent(typeof(UILabel))]
	public class ScoreLabel : MonoBehaviour 
	{
		private UILabel _label;

		// Use this for initialization
		void Awake() 
		{
			_label = GetComponent<UILabel>();

			if(GameManager.Instance != null)
			{
				GameManager.Instance.ScoreChangeEvent += HandleScoreChangeEvent;
			}
		}

		void OnEnable()
		{
			UpdateScore ();
		}

		// Update is called once per frame
		void Update () 
		{
		}

		void OnDestroy()
		{
			if(GameManager.Instance != null)
			{
				GameManager.Instance.ScoreChangeEvent -= HandleScoreChangeEvent;
			}
		}

		public void UpdateScore()
		{
			if(GameManager.Instance != null)
			{
				if(_label == null)
				{
					Debug.LogError("Missing UILabel component. Oh, I didn't consider that!");
					return;
				}

				_label.text = GameManager.Instance.GetScore().ToString();
			}
		}

		private void HandleScoreChangeEvent (int score, int inc)
		{
			UpdateScore();
		}
	}
}