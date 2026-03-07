using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPoolController> objectPools = new();

    // TODO : Clean this up a bit, because pretty much both ensure pool and get pool do the same thing now.
    // The code is slowly evolving, we'll see what the requirements really are down the line, but this may have actually been overkill for this project...

    public ObjectPoolController EnsureObjectPool(GameObject prefab, int initialCount = 20)
    {
        ObjectPoolController pool;
        if (objectPools.ContainsKey(prefab))
        {
            // Ensure that the pool has at least as much capacity reserved as specified
            pool = objectPools[prefab];
            if (pool.Storage.Capacity < initialCount)
            {
                pool.Storage.Resize(initialCount);
            }
        }
        else
        {
            // Spawn pool parent game object
            var go = new GameObject($"ObjectPool {objectPools.Count} ({prefab.name})");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;

            // Add pool component
            pool = go.AddComponent<ObjectPoolController>();
            pool.Init(prefab, initialCount);
            objectPools.Add(prefab, pool);
        }
        return pool;
    }

    public ObjectPoolController GetObjectPool(GameObject prefab)
    {
        return EnsureObjectPool(prefab);
    }

}
