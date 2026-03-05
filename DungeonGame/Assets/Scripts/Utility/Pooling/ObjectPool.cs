using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Config")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int capacity;
    [SerializeField] private bool regrow;

    private ObjectPoolStorage pool;

    void Awake()
    {
        pool = new ObjectPoolStorage();
        pool.Init(prefab, capacity, regrow, this);
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
