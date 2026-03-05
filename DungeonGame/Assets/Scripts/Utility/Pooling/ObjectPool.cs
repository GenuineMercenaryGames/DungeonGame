using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    [Header("Pool Config")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialCapacity;
    [SerializeField] private bool allowRegrow;
    [SerializeField] private int regrowFactor;

    private ObjectPoolStorage pool;

    void Awake()
    {
        pool = new ObjectPoolStorage(transform, prefab, initialCapacity, allowRegrow, regrowFactor);
    }

    public GameObject Get()
    {
        return pool.Get();
    }

    public T Get<T>() where T : Component
    {
        var obj = pool.Get();
        var component = obj.GetComponent<T>();
        if (component == null)
        {
            pool.Return(obj);
            return null;
        }
        return component;
    }

    public void Return(GameObject obj)
    {
        pool.Return(obj);
    }

    public void Return<T>(T component) where T : Component
    {
        pool.Return(component.gameObject);
    }
}
