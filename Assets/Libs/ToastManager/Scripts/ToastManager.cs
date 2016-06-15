using UnityEngine;
using System.Collections;
using Framework;

public class ToastManager : UnitySingleton<ToastManager>
{
    private ToastItem toastItem;
    
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void Show(string text)
    {
        Show(text, 1.0f);
    }

    public void Show(string text, float duration)
    {
        if(toastItem == null)
        {
            GameObject prefab = Resources.Load<GameObject>("ToastManager/ToastItem");
            toastItem = Instantiate(prefab).GetComponent<ToastItem>();
        }

        if (UIRoot.list.Count > 0)
        {
            Transform tran = toastItem.gameObject.transform;

            tran.parent = UIRoot.list[0].transform;
            tran.localPosition = Vector3.zero;
            tran.localScale = Vector3.one;
            tran.localRotation = Quaternion.identity;
        }

        toastItem.SetText(text);
        toastItem.SetDuration(duration);

        Invoke("InternalShow", 0.05f);
    }
    
    private void InternalShow()
    {
        toastItem.Show();
    }
}
