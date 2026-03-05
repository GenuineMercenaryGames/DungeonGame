using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    [Header("Object Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialCapacity = 20;
    [SerializeField] private bool allowRegrow = true;
    [SerializeField] private int regrowFactor = 2;

    private ObjectPoolStorage pool;
    private bool initialized;

    void Start()
    {
        Init(prefab, initialCapacity, allowRegrow, regrowFactor);
    }

    public void Init(GameObject prefab, int initialCapacity = 20, bool allowRegrow = true, int regrowFactor = 2)
    {
        if (initialized)
            return;
        pool = new ObjectPoolStorage(transform, prefab, initialCapacity, allowRegrow, regrowFactor);
        initialized = true;
    }

    public GameObject Get()
    {
        return pool.Get();
    }

    public T Get<T>() where T : Component
    {
        return pool.Get<T>();
    }

    public void Return(GameObject obj)
    {
        pool.Return(obj);
    }

    public void Return<T>(T component) where T : Component
    {
        pool.Return(component);
    }
}
