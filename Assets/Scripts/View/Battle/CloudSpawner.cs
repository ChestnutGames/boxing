using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CloudSpawner : MonoBehaviour
{
    public enum CloudType
    {
        SUN,
        NIGHT,
        DESERT,
    }

    public CloudType type;
    private GameObject[] prefabs;

    public GameObject[] prefabsSun;
    public GameObject[] prefabsNight;
    public GameObject[] prefabsDesert;
    private int length;
    public float[] intervals;

    public float checkInterval = 0.5f;
    public float randomness = 0.1f;
    public int maxSpawns = 20;

    public bool IsSpawning { get { return _isSpawning; } }

    private BoxCollider _collider;
    private float _time;
    private bool _isSpawning;

    private List<GameObject> _spawnObjects = new List<GameObject>();
    private Action<GameObject> _onSpawnAction;

    public bool isYMove;

     
     


    // Use this for initialization
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        ChangeType(CloudType.DESERT);
    }


    public void Restart()
    {
        _time = 0;
        length = 0;
        RemoveAll();
    }

    public void ChangeType(CloudType t)
    {
        type = t;
        switch (type)
        { 
            case CloudType.SUN:
                prefabs = prefabsSun;
                length = prefabsSun.Length;
                break;
            case CloudType.NIGHT:
                prefabs = prefabsNight;
                length = prefabsNight.Length;
                break;
            case CloudType.DESERT:
                prefabs = prefabsDesert;
                length = prefabsDesert.Length;
                break;
        } 
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isSpawning)
        {
            return;
        } 

        float currTime = _time;
        float nextTime = _time + Time.deltaTime + (UnityEngine.Random.value * randomness);

        _time = nextTime;

        //if(Mathf.RoundToInt(nextTime / checkInterval) == Mathf.RoundToInt(currTime / checkInterval))
        //{
        //	return;
        //}

        if (_spawnObjects.Count < maxSpawns)
        { 
            for (int i = 0; i < length; i++)
            {
                float interval = intervals[i];

                int a = Mathf.RoundToInt(nextTime / interval);
                int b = Mathf.RoundToInt(currTime / interval);
                if (a > b)
                {
                    Spawn(prefabs[i]);
                }
            }
        }
    }

    public void EnableSpawn(bool enable)
    {
        _isSpawning = enable;
    }

    public void Spawn(GameObject prefab)
    {
        float min;
        float max;

        float x;
        float y;

        Vector3 pos = transform.position;

        if (isYMove)
        {
            min = _collider.bounds.min.x;
            max = _collider.bounds.max.x;
            x = Mathf.Lerp(min, max, UnityEngine.Random.value);
            y = pos.y;
        }
        else
        {
            min = _collider.bounds.min.y;
            max = _collider.bounds.max.y;
            y = Mathf.Lerp(min, max, UnityEngine.Random.value);
            x = pos.x;
        }
        if (type != CloudType.SUN)
        {
            y += 0.1f;
        }

        GameObject go = Instantiate(prefab) as GameObject;

        go.transform.parent = this.transform;
        go.transform.localScale = Vector3.one;
        go.transform.position = new Vector3(x, y, pos.z);

        _spawnObjects.Add(go);

        if (_onSpawnAction != null)
        {
            _onSpawnAction(go);
        }
    }

    public void SetOnSpawn(Action<GameObject> action)
    {
        _onSpawnAction = action;
    }

    public void ForEach(Action<GameObject> action)
    {
        foreach (GameObject go in _spawnObjects)
        {
            action(go);
        }
    }

    public void ForEach<T>(Action<T> action)
        where T : MonoBehaviour
    {
        foreach (GameObject go in _spawnObjects)
        {
            T t;
            if ((t = go.GetComponent<T>()) != null)
            {
                action(t);
            }
        }
    }

    public void Remove(GameObject go)
    {
        if (_spawnObjects.Contains(go))
        {
            _spawnObjects.Remove(go);
        }

        Destroy(go);
    }

    public void RemoveAll()
    {
        foreach (GameObject go in _spawnObjects)
        {
            Destroy(go);
        }
        _spawnObjects.Clear();
    }
}
