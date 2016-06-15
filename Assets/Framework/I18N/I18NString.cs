using UnityEngine;

namespace Framework
{
	[RequireComponent(typeof(UILabel))]
	public class I18NString : MonoBehaviour 
	{
		public string id;

		private UILabel _label;

		void Start () 
		{
			this._label = GetComponent<UILabel>();
			this._label.text = I18NManager.Instance.GetString(id);
		}
	}
}