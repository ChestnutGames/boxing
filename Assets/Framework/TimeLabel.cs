using UnityEngine;
using System.Collections;

namespace Framework
{
    [RequireComponent(typeof(UILabel))]
    public class TimeLabel : MonoBehaviour
    {
        private UILabel _label;

        // Use this for initialization
        void Awake()
        {
            _label = GetComponent<UILabel>();

            if (GameManager.Instance != null)
            {
               GameManager.Instance.TimeChangeEvent += HandleTimeChangeEvent;
            }
        }

        void OnEnable()
        {
            UpdateTime();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TimeChangeEvent -= HandleTimeChangeEvent;
            }
        }

        public void UpdateTime()
        {
            if (GameManager.Instance != null)
            {
                if (_label == null)
                {
                    Debug.LogError("Missing UILabel component. Oh, I didn't consider that!");
                    return;
                }

                _label.text = GameManager.Instance.GetTime().ToString();
            }
        }

        private void HandleTimeChangeEvent(int time,int inv)
        {
            UpdateTime();
        }
    }
}