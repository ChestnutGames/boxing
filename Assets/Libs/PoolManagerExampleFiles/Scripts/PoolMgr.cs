using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using SLCGame;

/// <summary>
/// 对unity 手动prefab管理
/// </summary>
namespace SLCGame.Unity
{
    public class PoolMgr 
    {
        public Dictionary<string, GameObject> pool;
        public void PoolInit()
        {

        }


        //public GameObject NewPrefabInst(string path)
        //{
        //    GameObject
        //    return
        //}

        //public bool Destroy(string poolName)
        //{
        //    // Use TryGetValue to avoid KeyNotFoundException.
        //    //   This is faster than Contains() and then accessing the dictionary
        //    SpawnPool spawnPool;
        //    if (!this._pools.TryGetValue(poolName, out spawnPool))
        //    {
        //        Debug.LogError(
        //            string.Format("PoolManager: Unable to destroy '{0}'. Not in PoolManager",
        //                          poolName));
        //        return false;
        //    }

        //    // The rest of the logic will be handled by OnDestroy() in SpawnPool
        //    UnityEngine.Object.Destroy(spawnPool.gameObject);

        //    // Remove it from the dict in case the user re-creates a SpawnPool of the 
        //    //  same name later
        //    //this._pools.Remove(spawnPool.poolName);

        //    return true;
        //}

        //public void DestroyAll()
        //{
        //    foreach (KeyValuePair<string, SpawnPool> pair in this.pool)
        //        UnityEngine.Object.Destroy(pair.Value);

        //    // Clear the dict in case the user re-creates a SpawnPool of the same name later
        //    this._pools.Clear();
        //}
    }  
}

