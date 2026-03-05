using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPoolController> objectPools;

    public void EnsurePool(GameObject prefab)
    {
        if (objectPools.ContainsKey(prefab))
            return;

        var go = new GameObject($"ObjectPool {objectPools.Count} ({prefab.name})");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        var pool = go.AddComponent<ObjectPoolController>();
        pool.Init(prefab);
        objectPools.Add(prefab, pool);
    }

    public ObjectPoolController GetObjectPool(GameObject prefab)
    {
        EnsurePool(prefab);
        return objectPools[prefab];
    }

}
