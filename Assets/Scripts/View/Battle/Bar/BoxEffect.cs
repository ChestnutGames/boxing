using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class BoxEffect : MonoBehaviour
{
    public int renderQueue = 30000;
    public bool runOnlyOnce = false;

    public Renderer render { get; set; }
    void Start()
    {
        render = this.GetComponent<Renderer>(); 
        Update();
    }
    void Update()
    {
        if (render != null && render.sharedMaterial != null)
        {
            render.sharedMaterial.renderQueue = renderQueue;
        }
        if (runOnlyOnce && Application.isPlaying)
        {
            this.enabled = false;
        }
    }

    
}